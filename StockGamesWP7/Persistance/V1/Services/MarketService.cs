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
using StockGames.Persistance.V1.DataModel;
using StockGames.Persistance.V1.DataContexts;
using System.Linq;

namespace StockGames.Persistance.V1.Services
{
    public class MarketService
    {
        public static MarketModel TestMarket
        {
            get
            {
                using (var context = StockGamesDataContext.GetReadOnly())
                {
                    return context.Markets.First();
                }
            }
        }

        private static MarketService instance = new MarketService();
        public static MarketService Instance
        {
            get
            {
                return instance;
            }
        }

        private MarketService() { }

        public void AddMarket(MarketModel market)
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                context.Markets.InsertOnSubmit(market);
                context.SubmitChanges();
            }
        }

        public MarketModel GetMarket(string marketID)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var query = from m in context.Markets where m.MarketID == marketID select m;
                return query.Single();
            }
        }
    }
}
