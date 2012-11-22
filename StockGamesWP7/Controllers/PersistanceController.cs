using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using StockGames.Models;
using StockGames.Persistance.V1.DataContexts;
using StockGames.Persistance.V1.DataModel;
using StockGames.Persistance.V1.Services;
using StockGames.Persistance.V1.Migrations;

namespace StockGames.Controllers
{
    public class PersistanceController
    {
        
        
        private static PersistanceController instance;
        public static PersistanceController Instance 
        {
            get { 
                if (instance == null ) {
                    instance = new PersistanceController();
                }
                return instance;
            }                
        }

        private PersistanceController()
        {
            initDatabase();
        }

        private void initDatabase()
        {
            using (StockGamesDataContext dataContext = StockGamesDataContext.GetReadWrite())
            {
                if (!dataContext.DatabaseExists())
                {
                    dataContext.CreateDatabase();
                    //InitialCreate.Update();
                    populateFirstTimeStocks();
                }
            }
        }

        private void populateFirstTimeStocks()
        {
            MarketModel market = new MarketModel() { MarketID = "ONE", MarketName = "Initial Market" };
            MarketService.Instance.AddMarket(market);

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
    }
}
