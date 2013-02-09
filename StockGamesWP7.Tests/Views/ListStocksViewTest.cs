using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Views;

namespace StockGames.Tests.Views
{
    [TestClass]
    [Tag("Views")]
    public class ListStocksViewTest
    {
        [TestInitialize]
        public void Initialize()
        {
            using (StockGamesDataContext context = StockGamesDataContext.GetReadWrite())
            {
                if (!context.DatabaseExists())
                {
                    context.CreateDatabase();
                }
            }
        }

        [TestMethod]
        public void TestCreateListStocksView()
        {
            var view = new ListStocksView(); 
        }
    }
}
