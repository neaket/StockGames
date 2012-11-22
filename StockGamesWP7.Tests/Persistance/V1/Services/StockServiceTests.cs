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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockGames.Persistance.V1.DataContexts;
using StockGames.Persistance.V1.Services;
using StockGames.Models;
using System.Linq;
using Microsoft.Silverlight.Testing;
using StockGames.Persistance.V1.DataModel;

namespace StockGames.Tests.Persistance.V1.Services
{
    [TestClass]
    [Tag("p")]
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
        public void testAddAndGetStockEntity()
        {
            StockEntity stockEntity = new StockEntity("ABC", "ABC Company Test");

            StockService.Instance.AddStock(stockEntity);

            Assert.Equals(1, StockService.Instance.GetStocks().Count());

            StockEntity persisted = StockService.Instance.GetStock("ABC");

            Assert.Equals(stockEntity, persisted);
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
