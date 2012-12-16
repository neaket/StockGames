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
using StockGames.Persistance.V1.DataModel;
using System.Linq;
using StockGames.Persistance.V1.DataContexts;
using StockGames.Persistance.V1.Services;
using Microsoft.Silverlight.Testing;

namespace StockGames.Tests.Persistance.V1.Services
{
    [TestClass]
    [Tag("Persistance")]
    public class MarketServiceTests
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
        public void TestAddMarket()
        {
            MarketModel market = new MarketModel() { MarketId = "ONE", MarketName = "Initial Market" };
            MarketService.Instance.AddMarket(market);

            MarketModel persisted = MarketService.Instance.GetMarket("ONE");

            Assert.Equals(market, persisted);            
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
