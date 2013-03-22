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
            var portfolioId = PortfolioService.Instance.AddPortfolio("Practice");

            PortfolioService.Instance.AddTransaction(portfolioId, 10000, DateTime.Today);

            GameState.Instance.MainPortfolioId = portfolioId;
        }
    }
}
