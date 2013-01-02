using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.DataModel;
using System.Linq;
using System.Collections.Generic;
using System;

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

        public PortfolioModel AddPortfolio(string name)
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var portfolio = new PortfolioModel { Name = name };
                context.Portfolios.InsertOnSubmit(portfolio);
                context.SubmitChanges();
                return portfolio;
            }
        }

        public void AddTrade(int portfolioId, string stockIndex, TradeType tradeType, int quantity)
        {            
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var snapshot = (from s in context.StockSnapshots where s.StockIndex == stockIndex orderby s.Tombstone descending select s).First();

                PortfolioTradeModel trade = new PortfolioTradeModel() { 
                    Amount = snapshot.Price, 
                    Quantity = 17, 
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

        public PortfolioModel GetPortfolio(int portfolioId)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);
                return portfolio;
            }
        }

        public IEnumerable<PortfolioEntryModel> GetEntries(int portfolioId)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var portfolio = context.Portfolios.Single(p => p.PortfolioId == portfolioId);
                
                return portfolio.Entries.ToArray();
            }
        }
    }
}
