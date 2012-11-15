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
using System.Linq;
using System.Collections.Generic;
using StockGames.Persistance.DataContexts;

namespace StockGames.Persistance.Services
{
    public class StockService
    {
        private static StockService instance;
        public static StockService Instance {
            get {
                if (instance == null) {
                    instance = new StockService();
                }
                return instance;
            }
        }

        public StockEntity Get(string stockIndex)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var query = from s in context.Stocks where s.StockIndex == stockIndex select s;
                return query.Single();
            }
        }

        public List<StockEntity> GetStocks()
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var query = from s in context.Stocks select s;
                return query.ToList();
            }
        }

    }
}
