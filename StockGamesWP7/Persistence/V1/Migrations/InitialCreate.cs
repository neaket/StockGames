using Microsoft.Phone.Data.Linq;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.Services;
using System;

namespace StockGames.Persistence.V1.Migrations
{
    /// <summary>
    /// The InitialCreate migration is used to create the initial data inside of the persisted
    /// DataContext.
    /// </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class InitialCreate
    {
        /// <summary>
        /// The version is used to ensure that this migration is not run twice.  Each migration must
        /// have a unique version.
        /// </summary>
        public const int Version = 1;

        /// <summary>
        /// This method is called by the <see cref="MigrationManager"/> when initializing the database.
        /// </summary>
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
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            DateTime twoHoursAgo = now.AddHours(-2);
            DateTime anHourAgo = now.AddHours(-1);
            ss.AddStock("ABC", "ABC Company");
            ss.AddStockSnapshot("ABC", 58.8M, twoHoursAgo);
            ss.AddStockSnapshot("ABC", 58.17M, anHourAgo);
            ss.AddStock("ONE", "One Company");
            ss.AddStockSnapshot("ONE", 51.6M, twoHoursAgo);
            ss.AddStockSnapshot("ONE", 51.8M, anHourAgo);
            ss.AddStock("NINJ", "Ninja Corp");
            ss.AddStockSnapshot("NINJ", 7.1M, twoHoursAgo);
            ss.AddStockSnapshot("NINJ", 6M, anHourAgo);
            ss.AddStock("BRAI", "Zombie Software");
            ss.AddStockSnapshot("BRAI", 121M, twoHoursAgo);
            ss.AddStockSnapshot("BRAI", 115, anHourAgo);
            ss.AddStock("SWRD", "Sword Construction Company");
            ss.AddStockSnapshot("SWRD", 81, twoHoursAgo);
            ss.AddStockSnapshot("SWRD", 84, anHourAgo);
            ss.AddStock("FARM", "Family Farms");
            ss.AddStockSnapshot("FARM", 112M, twoHoursAgo);
            ss.AddStockSnapshot("FARM", 114M, anHourAgo);
            ss.AddStock("PICK", "Pickles To Go");
            ss.AddStockSnapshot("PICK", 132M, twoHoursAgo);
            ss.AddStockSnapshot("PICK", 134M, anHourAgo);
            ss.AddStock("DELI", "Delicious Soft Drinks Company");
            ss.AddStockSnapshot("DELI", 101M, twoHoursAgo);
            ss.AddStockSnapshot("DELI", 99M, anHourAgo);
            ss.AddStock("SURV", "Survival Weapons");
            ss.AddStockSnapshot("SURV", 51.1M, twoHoursAgo);
            ss.AddStockSnapshot("SURV", 50.1M, anHourAgo);
            ss.AddStock("NEWS", "News For You");
            ss.AddStockSnapshot("NEWS", 123.1M, twoHoursAgo);
            ss.AddStockSnapshot("NEWS", 123.5M, anHourAgo);
            ss.AddStock("RED", "Planet Red");
            ss.AddStockSnapshot("RED", 82.23M, twoHoursAgo);
            ss.AddStockSnapshot("RED", 85.1M, anHourAgo);
        }

        private static void CreateFirstPortfolio()
        {
            var portfolioId = PortfolioService.Instance.AddPortfolio("Portfolio");

            PortfolioService.Instance.AddTransaction(portfolioId, 10000, DateTime.Today);

            GameState.Instance.MainPortfolioId = portfolioId;
        }
    }
}
