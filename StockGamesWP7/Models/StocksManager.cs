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
using System.Collections.Generic;
using System.Collections;

namespace StockGames.Models
{
    public class StocksManager : IEnumerable
    {
        //Private Variables
        private List<StockEntity> _Stocks;

        private static StocksManager _Instance = null;
        public static StocksManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new StocksManager();
                }
                return _Instance;
            }
        }

        private StocksManager()
        {
            _Stocks = new List<StockEntity>();
            _PopulateStocks();
        }

        public void AddStock(StockEntity stock)
        {
            _Stocks.Add(stock);
        }

        public void RemoveStock(StockEntity stock)
        {
            _Stocks.Remove(stock);
        }

        
        public StockEntity FindStock(String stockIndex)
        {
            foreach (StockEntity stock in this)
            {
                if (stock.StockIndex.Equals(stockIndex))
                    return stock;
            }

            throw new ArgumentException("Stock Index not in list");
        }

        //IENumerable interface implementation
        public IEnumerator GetEnumerator()
        {
            return _Stocks.GetEnumerator();
        }

        private void _PopulateStocks()
        {
            AddStock(new StockEntity("ABC", "ABC Company")
            {
                CurrentPrice = 58.8M,
                PreviousPrice = 58.17M
            });
            AddStock(new StockEntity("ONE", "One Company")
            {
                CurrentPrice = 51.6M,
                PreviousPrice = 51.8M
            });
            AddStock(new StockEntity("NINJ", "Ninja Corp")
            {
                CurrentPrice = 7.1M,
                PreviousPrice = 6M
            });
            AddStock(new StockEntity("BRAI", "Zombie Software")
            {
                CurrentPrice = 121M,
                PreviousPrice = 82M
            });
            AddStock(new StockEntity("SWRD", "Sword Construction Company")
            {
                CurrentPrice = 1234M,
                PreviousPrice = 1231M
            });
            AddStock(new StockEntity("FARM", "Family Farms")
            {
                CurrentPrice = 400M,
                PreviousPrice = 391M
            });
            AddStock(new StockEntity("PICK", "Pickles To Go")
            {
                CurrentPrice = 132M,
                PreviousPrice = 156M
            });
            AddStock(new StockEntity("DELI", "Delicious Soft Drinks Company")
            {
                CurrentPrice = 101M,
                PreviousPrice = 99M
            });
            AddStock(new StockEntity("SURV", "Survival Weapons")
            {
                CurrentPrice = 51.1M,
                PreviousPrice = 50.1M
            });
            AddStock(new StockEntity("NEWS", "News For You")
            {
                CurrentPrice = 123.1M,
                PreviousPrice = 123.5M
            });
            AddStock(new StockEntity("RED", "Planet Red")
            {
                CurrentPrice = 82.23M,
                PreviousPrice = 85.1M
            });
        }
    }
}
