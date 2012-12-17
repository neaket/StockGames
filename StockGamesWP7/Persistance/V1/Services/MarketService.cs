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

        private static readonly MarketService instance = new MarketService();
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

        public MarketModel GetMarket(string marketId)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var query = from m in context.Markets where m.MarketId == marketId select m;
                return query.Single();
            }
        }
    }
}
