using Microsoft.Phone.Testing;
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
        public void TestAddAndGetStock()
        {
            int prevCount = StockService.Instance.GetStocks().Count();
            StockService.Instance.AddStock("TEST", "Test Company Test");

            Assert.AreEqual(prevCount + 1, StockService.Instance.GetStocks().Count());

            StockEntity persisted = StockService.Instance.GetStock("TEST");

            Assert.AreEqual("TEST", persisted.StockIndex);
            Assert.AreEqual("Test Company Test", persisted.CompanyName);
            Assert.AreEqual(0, persisted.CurrentPrice);
            Assert.AreEqual(0, persisted.PreviousPrice);
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
