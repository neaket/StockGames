using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.Migrations;
using StockGames.Views;

namespace StockGames.Tests.Views
{
    [TestClass]
    [Tag("Views")]
    public class InitializeViewsTest
    {
        [TestInitialize]
        public void Initialize()
        {
            MigrationManager.IfExistsRemoveDataContext();
            MigrationManager.InitializeDataContext();
        }

        [TestMethod]
        public void AboutView()
        {
            var view = new DashboardView();
        }

        [TestMethod]
        public void DashboardView()
        {
            var view = new DashboardView();
        }
        
        [TestMethod]
        public void ListStocksView()
        {
            var view = new ListStocksView(); 
        }

        [TestMethod]
        public void MainMenuView()
        {
            var view = new MainMenuView();
        }

        [TestMethod]
        public void PortfolioTradeView()
        {
            var view = new PortfolioTradeView();
        }

        [TestMethod]
        public void PortfolioView()
        {
            var view = new PortfolioView();
        }

        [TestMethod]
        public void StockView()
        {
            var view = new StockView();
        }

    }
}
