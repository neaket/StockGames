using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.DataModel;

namespace StockGames.Persistence.V1.Services
{
    public class PortfolioService
    {
        private static readonly PortfolioService instance = new PortfolioService();
        public static PortfolioService Instance {
            get 
            {
                return instance;
            }
        }

        private PortfolioService() { }

        public void AddPortfolio(PortfolioModel portfolio)
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                context.Portfolios.InsertOnSubmit(portfolio);
                context.SubmitChanges();
            }
        }
    }
}
