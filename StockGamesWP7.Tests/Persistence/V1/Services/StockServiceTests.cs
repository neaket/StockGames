using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.Services;
using StockGames.Entities;

namespace StockGames.Tests.Persistence.V1.Services
{
    [TestClass]
    [Tag("Persistence")]
    public class StockServiceTests
    {
        [TestInitialize]
        public void Initialize()
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                
                if (!context.DatabaseExists())
                {
                    context.CreateDatabase();     
                }
            }
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
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                context.DeleteDatabase();
            }
        }
    }
}
