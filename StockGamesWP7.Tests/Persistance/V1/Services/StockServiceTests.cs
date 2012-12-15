using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockGames.Persistance.V1.DataContexts;
using StockGames.Persistance.V1.Services;
using StockGames.Models;
using System.Linq;
using StockGames.Persistance.V1.DataModel;

namespace StockGames.Tests.Persistance.V1.Services
{
    [TestClass]
    [Tag("Persistance")]
    public class StockServiceTests
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

            // TODO remove me
            MarketModel market = new MarketModel() { MarketID = "ONE", MarketName = "Initial Market" };
            MarketService.Instance.AddMarket(market);
        }
        
        [TestMethod]
        public void TestAddAndGetStockEntity()
        {
            StockEntity stockEntity = new StockEntity("ABC", "ABC Company Test");

            StockService.Instance.AddStock(stockEntity);

            Assert.AreEqual(1, StockService.Instance.GetStocks().Count());

            StockEntity persisted = StockService.Instance.GetStock("ABC");

            Assert.AreEqual(stockEntity, persisted);
        }

        [TestCleanup]
        public void Cleanup()
        {
            using (StockGamesDataContext context = StockGamesDataContext.GetReadWrite())
            {
                context.DeleteDatabase();
            }
        }
    }
}
