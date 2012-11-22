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
using StockGames.Persistance.V1.Services;

namespace StockGames.Models
{
    public class StocksManager
    {

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
        }

        public void AddStock(StockEntity stock)
        {
            StockService.Instance.AddStock(stock);
        }
        
        public StockEntity FindStock(String stockIndex)
        {
            return StockService.Instance.GetStock(stockIndex);
        }

        //IENumerable interface implementation
        public IEnumerable<StockEntity> GetStocks()
        {
            return StockService.Instance.GetStocks();
        }

        private void _PopulateStocks()
        {
            //_Stocks = PersistanceController.Instance.GetAllStocks();
        }
    }
}
