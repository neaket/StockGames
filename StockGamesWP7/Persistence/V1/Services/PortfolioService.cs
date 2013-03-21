using GalaSoft.MvvmLight.Messaging;
using StockGames.Messaging;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.DataModel;
using System.Linq;
using System.Collections.Generic;
using System;
using StockGames.Entities;

namespace StockGames.Persistence.V1.Services
{
    public class PortfolioService
    {
        #region instance
        
        private static readonly PortfolioService instance = new PortfolioService();
        public static PortfolioService Instance {
            get 
            {
                return instance;
            }
        }

        private PortfolioService() { }

        #endregion

        /// <summary> Adds a portfolio. </summary>
        /// <param name="name"> The name of the portfolio. </param>
        /// <returns> The portfolio Id. </returns>
        public int AddPortfolio(string name)
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var portfolio = new PortfolioDataModel { Name = name };
                context.Portfolios.InsertOnSubmit(portfolio);
                context.SubmitChanges();
                return portfolio.PortfolioId;
            }
        }

        /// <summary> Adds a transaction to a portfolio. </summary>
        /// <param name="portfolioId">  Identifier for the portfolio. </param>
        /// <param name="amount">       The amount. </param>
        /// <param name="tombstone">    The (Date/Time) tombstone. </param>
        public void AddTransaction(int portfolioId, decimal amount, DateTime tombstone)
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var transaction = new PortfolioTransactionDataModel
                {
                    Amount = amount,
                    Tombstone = tombstone
                };

                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);
                context.PortfolioEntries.InsertOnSubmit(transaction);
                AddEntryHelper(portfolio, transaction);

                context.SubmitChanges();
            }

            MessengerWrapper.Send(new PortfolioUpdatedMessageType(portfolioId));
        }

        /// <summary> Adds a trade. </summary>
        /// <param name="portfolioId">  Identifier for the portfolio. </param>
        /// <param name="stockIndex">   Index of the stock. </param>
        /// <param name="tradeType">    Type of the trade. </param>
        /// <param name="quantity">     The quantity. </param>
        /// <param name="tombstone">    The (Date/Time) tombstone. </param>
        public void AddTrade(int portfolioId, string stockIndex, TradeType tradeType, int quantity, DateTime tombstone)
        {            
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var snapshot = (from s in context.StockSnapshots where s.StockIndex == stockIndex && s.Tombstone <= GameState.Instance.GameTime orderby s.Tombstone descending select s).FirstOrDefault();

                if (snapshot == null)
                {
                    throw new ArgumentException(String.Format("A snapshot with stockIndex '{0}' and before GameTime does not exist.", stockIndex));
                }

                decimal amount;
                int relativeQuanity;

                if (tradeType == TradeType.Buy)
                {
                    amount = -snapshot.Price*quantity;
                    relativeQuanity = quantity;
                } else
                {
                    amount = snapshot.Price*quantity;
                    relativeQuanity = -quantity;
                }
                var trade = new PortfolioTradeDataModel
                { 
                    Amount = amount,
                    Quantity = relativeQuanity, 
                    Tombstone = tombstone, 
                    TradeType = tradeType,
                    StockSnapshot = snapshot
                };

                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);
                context.PortfolioEntries.InsertOnSubmit(trade);
                AddEntryHelper(portfolio, trade);

                context.SubmitChanges();
            }

            MessengerWrapper.Send(new PortfolioTradeAddedMessageType(portfolioId, stockIndex, tradeType));
        }

        private void AddEntryHelper(PortfolioDataModel portfolio, PortfolioEntryDataModel entry)
        {
            portfolio.Balance += entry.Amount;
            entry.Portfolio = portfolio;
            portfolio.Entries.Add(entry);
        }

        /// <summary> Gets a portfolio. </summary>
        /// <param name="portfolioId">  Identifier for the portfolio. </param>
        /// <returns> The portfolio. </returns>
        public PortfolioDataModel GetPortfolio(int portfolioId)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);               
                return portfolio;
            }
        }


        /// <summary> Gets the entries of a specific portfolio. </summary>
        /// <param name="portfolioId">  Identifier for the portfolio. </param>
        /// <returns>
        /// The Entries.
        /// </returns>
        public PortfolioEntryDataModel[] GetEntries(int portfolioId)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);

                return portfolio.Entries.ToArray();
            }
        }

        public IEnumerable<TradeEntity> GetGroupedTrades(int portfolioId)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var groupedTrades = new List<TradeEntity>();

                
                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);

                var trades = from t in from e in portfolio.Entries
                             where e is PortfolioTradeDataModel
                             select e as PortfolioTradeDataModel
                             group t by t.StockSnapshot.StockIndex into g
                             where g.Sum(x => x.Quantity) > 0
                             select new 
                                 {
                                     Quantity = g.Sum(x => x.Quantity),
                                     Average = Math.Round(g.Sum(x => x.StockSnapshot.Price * x.Quantity) / g.Sum(x => x.Quantity), 2),
                                     StockIndex = g.Key

                                 };
                foreach (var trade in trades.ToArray())
                {
                    // Get latest snapshot
                    // TODO optimize
                    var latestSnapshotPriceQuery = from s in context.StockSnapshots 
                                                   where s.StockIndex == trade.StockIndex
                                                   && s.Tombstone <= GameState.Instance.GameTime
                                                   orderby s.Tombstone descending
                                                   select s.Price;
                    decimal latestSnapshotPrice = latestSnapshotPriceQuery.First();

                    var tradeEntity = new TradeEntity
                    {
                        StockIndex = trade.StockIndex,
                        Quantity = trade.Quantity,
                        AveragePurchasedPrice = trade.Average,
                        CurrentPrice = latestSnapshotPrice
                    };

                    groupedTrades.Add(tradeEntity);
                }

                return groupedTrades;
            }
        }

        public int GetTradeQuantity(int portfolioId, string stockIndex)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);

                // return the sum of all trade quantities for the given stockIndex
                var quantities =  (from t in
                       from e in portfolio.Entries
                       where e is PortfolioTradeDataModel
                       select e as PortfolioTradeDataModel
                       where t.StockSnapshot.StockIndex == stockIndex
                       group t by t.StockSnapshot.StockIndex into g
                       select g.Sum(x => x.Quantity)).ToArray();

                if (quantities.Length == 0)
                {
                    return 0;
                }
                else if (quantities.Length == 1)
                {
                    return quantities[0];
                }

                throw new Exception("The length of quanities should never exceed 1.");
            }
        }
    }
}
