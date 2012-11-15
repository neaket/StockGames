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
using StockGames.Controllers;

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
            _Stocks = PersistanceController.Instance.GetAllStocks();
        }
    }
}
