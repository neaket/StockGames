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
        private static readonly PortfolioService instance = new PortfolioService();
        public static PortfolioService Instance {
            get 
            {
                return instance;
            }
        }

        private PortfolioService() { }

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

        public void AddTransaction(int portfolioId, decimal amount)
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                PortfolioTransactionDataModel transaction = new PortfolioTransactionDataModel()
                {
                    Amount = amount,
                    Tombstone = DateTime.Now
                };

                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);
                context.PortfolioEntries.InsertOnSubmit(transaction);
                portfolio.AddEntry(transaction);

                context.SubmitChanges();
            }
        }

        public void AddTrade(int portfolioId, string stockIndex, TradeType tradeType, int quantity)
        {            
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var snapshot = (from s in context.StockSnapshots where s.StockIndex == stockIndex orderby s.Tombstone descending select s).First();

                PortfolioTradeDataModel trade = new PortfolioTradeDataModel() { 
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
        }

        public PortfolioDataModel GetPortfolio(int portfolioId)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);               
                return portfolio;
            }
        }

        public IEnumerable<TradeEntity> GetTrades(int portfolioId)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                IList<TradeEntity> trades = new List<TradeEntity>();
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

                    var tradeEntity = new TradeEntity()
                    {
                        StockIndex = trade.StockSnapshot.StockIndex,
                        Quantity = trade.Quantity,
                        PurchasedPrice = trade.Amount,
                        CurrentPrice = latestSnapshotPrice
                    };

                    trades.Add(tradeEntity);
                }
                return trades;
            }
        }
    }
}
