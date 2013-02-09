﻿using System.Data.Linq;
using Microsoft.Phone.Data.Linq;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.DataModel;
using StockGames.Persistence.V1.Services;
using System;
using StockGames.Entities;

namespace StockGames.Persistence.V1.Migrations
{
    public class InitialCreate
    {
        public const int Version = 1;

        public static void Update()
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {               

                var updater = context.CreateDatabaseSchemaUpdater();
                if (updater.DatabaseSchemaVersion >= Version) {
                    return;
                }

                PopulateFirstTimeStocks();
                CreateFirstPortfolio();

                updater.DatabaseSchemaVersion = Version;
                updater.Execute();
            }
        }

        private static void PopulateFirstTimeStocks()
        {
            var ss = StockService.Instance;
            DateTime prev = DateTime.Today;
            DateTime now = DateTime.Now;
            ss.AddStock("ABC", "ABC Company");
            ss.AddStockSnapshot("ABC", 58.8M, prev);
            ss.AddStockSnapshot("ABC", 58.17M, now);
            ss.AddStock("ONE", "One Company");
            ss.AddStockSnapshot("ONE", 51.6M, prev);
            ss.AddStockSnapshot("ONE", 51.8M, now);
            ss.AddStock("NINJ", "Ninja Corp");
            ss.AddStockSnapshot("NINJ", 7.1M, prev);
            ss.AddStockSnapshot("NINJ", 6M, now);
            ss.AddStock("BRAI", "Zombie Software");
            ss.AddStockSnapshot("BRAI", 121M, prev);
            ss.AddStockSnapshot("BRAI", 82M, now);
            ss.AddStock("SWRD", "Sword Construction Company");
            ss.AddStockSnapshot("SWRD", 1234M, prev);
            ss.AddStockSnapshot("SWRD", 1254M, now);
            ss.AddStock("FARM", "Family Farms");
            ss.AddStockSnapshot("FARM", 400M, prev);
            ss.AddStockSnapshot("FARM", 391M, now);
            ss.AddStock("PICK", "Pickles To Go");
            ss.AddStockSnapshot("PICK", 132M, prev);
            ss.AddStockSnapshot("PICK", 156M, now);
            ss.AddStock("DELI", "Delicious Soft Drinks Company");
            ss.AddStockSnapshot("DELI", 101M, prev);
            ss.AddStockSnapshot("DELI", 99M, now);
            ss.AddStock("SURV", "Survival Weapons");
            ss.AddStockSnapshot("SURV", 51.1M, prev);
            ss.AddStockSnapshot("SURV", 50.1M, now);
            ss.AddStock("NEWS", "News For You");
            ss.AddStockSnapshot("NEWS", 123.1M, prev);
            ss.AddStockSnapshot("NEWS", 123.5M, now);
            ss.AddStock("RED", "Planet Red");
            ss.AddStockSnapshot("RED", 82.23M, prev);
            ss.AddStockSnapshot("RED", 85.1M, now);
        }

        private static void CreateFirstPortfolio()
        {
            var portfolio = PortfolioService.Instance.AddPortfolio("Practice");

            PortfolioService.Instance.AddTransaction(portfolio.PortfolioId, 10000);
            PortfolioService.Instance.AddTrade(portfolio.PortfolioId, "NINJ", TradeType.Buy, 17);
            PortfolioService.Instance.AddTrade(portfolio.PortfolioId, "ABC", TradeType.Buy, 3);

            GameState.Instance.MainPortfolioId = portfolio.PortfolioId;
        }
    }
}
