using System;
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
using System.Collections.Generic;
using StockGames.Persistance.DataContexts;
using System.Linq;

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
                    populateFirstTimeStocks();
                }
            }
        }

        private void populateFirstTimeStocks()
        {
            using (StockGamesDataContext db = StockGamesDataContext.GetReadWrite())
            {
                var stocks = new List<StockEntity>();
                stocks.Add(new StockEntity("ABC", "ABC Company")
                {
                    CurrentPrice = 58.8M,
                    PreviousPrice = 58.17M
                });
                stocks.Add(new StockEntity("ONE", "One Company")
                {
                    CurrentPrice = 51.6M,
                    PreviousPrice = 51.8M
                });
                stocks.Add(new StockEntity("NINJ", "Ninja Corp")
                {
                    CurrentPrice = 7.1M,
                    PreviousPrice = 6M
                });
                stocks.Add(new StockEntity("BRAI", "Zombie Software")
                {
                    CurrentPrice = 121M,
                    PreviousPrice = 82M
                });
                stocks.Add(new StockEntity("SWRD", "Sword Construction Company")
                {
                    CurrentPrice = 1234M,
                    PreviousPrice = 1231M
                });
                stocks.Add(new StockEntity("FARM", "Family Farms")
                {
                    CurrentPrice = 400M,
                    PreviousPrice = 391M
                });
                stocks.Add(new StockEntity("PICK", "Pickles To Go")
                {
                    CurrentPrice = 132M,
                    PreviousPrice = 156M
                });
                stocks.Add(new StockEntity("DELI", "Delicious Soft Drinks Company")
                {
                    CurrentPrice = 101M,
                    PreviousPrice = 99M
                });
                stocks.Add(new StockEntity("SURV", "Survival Weapons")
                {
                    CurrentPrice = 51.1M,
                    PreviousPrice = 50.1M
                });
                stocks.Add(new StockEntity("NEWS", "News For You")
                {
                    CurrentPrice = 123.1M,
                    PreviousPrice = 123.5M
                });
                stocks.Add(new StockEntity("RED", "Planet Red")
                {
                    CurrentPrice = 82.23M,
                    PreviousPrice = 85.1M
                });

                db.Stocks.InsertAllOnSubmit(stocks);
                db.SubmitChanges();
            }
        }

        public List<StockEntity> GetAllStocks()
        {
            using (var db = StockGamesDataContext.GetReadOnly())
            {
                var query = from s in db.Stocks select s;
                return query.ToList();

            }
        }
    }
}
