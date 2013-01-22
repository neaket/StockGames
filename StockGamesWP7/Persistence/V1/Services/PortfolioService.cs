﻿using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.DataModel;
using System.Linq;
using System.Collections.Generic;
using System;
using StockGames.Models;

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

        public void AddTrade(int portfolioId, string stockIndex, TradeType tradeType, int quantity)
        {            
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var snapshot = (from s in context.StockSnapshots where s.StockIndex == stockIndex orderby s.Tombstone descending select s).First();

                PortfolioTradeDataModel trade = new PortfolioTradeDataModel() { 
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

                    var tradeEntity = new TradeEntity()
                    {
                        StockIndex = trade.StockSnapshot.StockIndex,
                        Quantity = trade.Quantity
                    };

                }
                return trades;
            }
        }
    }
}
