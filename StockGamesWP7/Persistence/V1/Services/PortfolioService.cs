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
        /// <returns> The portfolio. </returns>
        public PortfolioDataModel AddPortfolio(string name)
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var portfolio = new PortfolioDataModel { Name = name };
                context.Portfolios.InsertOnSubmit(portfolio);
                context.SubmitChanges();
                return portfolio;
            }
        }

        /// <summary> Adds a transaction to a portfolio. </summary>
        /// <param name="portfolioId">  Identifier for the portfolio. </param>
        /// <param name="amount">       The amount. </param>
        public void AddTransaction(int portfolioId, decimal amount)
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var transaction = new PortfolioTransactionDataModel
                {
                    Amount = amount,
                    Tombstone = DateTime.Now
                };

                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);
                context.PortfolioEntries.InsertOnSubmit(transaction);
                portfolio.AddEntry(transaction);

                context.SubmitChanges();
            }

            Messenger.Default.Send(new PortfolioUpdatedMessageType(portfolioId));
        }

        /// <summary> Adds a trade. </summary>
        /// <param name="portfolioId">  Identifier for the portfolio. </param>
        /// <param name="stockIndex">   Index of the stock. </param>
        /// <param name="tradeType">    Type of the trade. </param>
        /// <param name="quantity">     The quantity. </param>
        public void AddTrade(int portfolioId, string stockIndex, TradeType tradeType, int quantity)
        {            
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var snapshot = (from s in context.StockSnapshots where s.StockIndex == stockIndex orderby s.Tombstone descending select s).First();

                var trade = new PortfolioTradeDataModel
                { 
                    Amount = snapshot.Price, 
                    Quantity = quantity, 
                    Tombstone = DateTime.Now, 
                    TradeType = tradeType,
                    StockSnapshot = snapshot
                };

                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);
                context.PortfolioEntries.InsertOnSubmit(trade);
                portfolio.AddEntry(trade);

                context.SubmitChanges();
            }

            Messenger.Default.Send(new PortfolioUpdatedMessageType(portfolioId));
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

        /// <summary> Gets the trades of a specific portfolio. </summary>
        /// <param name="portfolioId">  Identifier for the portfolio. </param>
        /// <returns>
        /// The Trades.
        /// </returns>
        public IEnumerable<TradeEntity> GetTrades(int portfolioId)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var trades = new List<TradeEntity>();
                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);
                
                foreach (var entry in portfolio.Entries)
                {
                    var trade = entry as PortfolioTradeDataModel;
                    if (trade == null)
                        continue;

                    // Get latest snapshot
                    // TODO optimize
                    var latestSnapshotPriceQuery = from s in context.StockSnapshots where s.StockIndex == trade.StockSnapshot.StockIndex orderby s.Tombstone descending select s.Price;
                    decimal latestSnapshotPrice = latestSnapshotPriceQuery.First();

                    var tradeEntity = new TradeEntity
                    {
                        StockIndex = trade.StockSnapshot.StockIndex,
                        Quantity = trade.Quantity,
                        AveragePurchasedPrice = trade.Amount,
                        CurrentPrice = latestSnapshotPrice
                    };

                    trades.Add(tradeEntity);
                }
                return trades;
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
                             select new 
                                 {
                                     Quantity = g.Sum(x => x.Quantity),
                                     Average = g.Average(x => x.Amount * x.Quantity) / g.Sum(x => x.Quantity), // TODO optimize
                                     StockIndex = g.Key

                                 };
                foreach (var trade in trades.ToArray())
                {
                    // Get latest snapshot
                    // TODO optimize
                    var latestSnapshotPriceQuery = from s in context.StockSnapshots 
                                                   where s.StockIndex == trade.StockIndex 
                                                   orderby s.Tombstone descending select s.Price;
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
    }
}
