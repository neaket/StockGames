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

            ss.AddStock("ABC", "ABC Company");
            ss.AddStock("ONE", "One Company");
            ss.AddStock("NINJ", "Ninja Corp");
            ss.AddStock("BRAI", "Zombie Software");
            ss.AddStock("SWRD", "Sword Construction Company");
            ss.AddStock("FARM", "Family Farms");
            ss.AddStock("PICK", "Pickles To Go");
            ss.AddStock("DELI", "Delicious Soft Drinks Company");
            ss.AddStock("SURV", "Survival Weapons");
            ss.AddStock("NEWS", "News For You");
            ss.AddStock("RED", "Planet Red");
        }

        private static void CreateFirstPortfolio()
        {
            var portfolioId = PortfolioService.Instance.AddPortfolio("Practice");

            PortfolioService.Instance.AddTransaction(portfolioId, 10000, DateTime.Today);

            GameState.Instance.MainPortfolioId = portfolioId;
        }
    }
}
