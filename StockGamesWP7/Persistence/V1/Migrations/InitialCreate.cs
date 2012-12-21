using Microsoft.Phone.Data.Linq;
using StockGames.Models;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.DataModel;
using StockGames.Persistence.V1.Services;

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
            ss.AddStock(new StockEntity("ABC", "ABC Company")
            {
                CurrentPrice = 58.8M,
                PreviousPrice = 58.17M
            });
            ss.AddStock(new StockEntity("ONE", "One Company")
            {
                CurrentPrice = 51.6M,
                PreviousPrice = 51.8M
            });
            ss.AddStock(new StockEntity("NINJ", "Ninja Corp")
            {
                CurrentPrice = 7.1M,
                PreviousPrice = 6M
            });
            ss.AddStock(new StockEntity("BRAI", "Zombie Software")
            {
                CurrentPrice = 121M,
                PreviousPrice = 82M
            });
            ss.AddStock(new StockEntity("SWRD", "Sword Construction Company")
            {
                CurrentPrice = 1234M,
                PreviousPrice = 1231M
            });
            ss.AddStock(new StockEntity("FARM", "Family Farms")
            {
                CurrentPrice = 400M,
                PreviousPrice = 391M
            });
            ss.AddStock(new StockEntity("PICK", "Pickles To Go")
            {
                CurrentPrice = 132M,
                PreviousPrice = 156M
            });
            ss.AddStock(new StockEntity("DELI", "Delicious Soft Drinks Company")
            {
                CurrentPrice = 101M,
                PreviousPrice = 99M
            });
            ss.AddStock(new StockEntity("SURV", "Survival Weapons")
            {
                CurrentPrice = 51.1M,
                PreviousPrice = 50.1M
            });
            ss.AddStock(new StockEntity("NEWS", "News For You")
            {
                CurrentPrice = 123.1M,
                PreviousPrice = 123.5M
            });
            ss.AddStock(new StockEntity("RED", "Planet Red")
            {
                CurrentPrice = 82.23M,
                PreviousPrice = 85.1M
            });


        }

        private static void CreateFirstPortfolio()
        {
            var portfolio = new PortfolioModel {Name = "Practice"};

            PortfolioService.Instance.AddPortfolio(portfolio);
        }
    }
}
