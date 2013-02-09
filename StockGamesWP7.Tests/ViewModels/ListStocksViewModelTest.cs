using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.Migrations;
using StockGames.ViewModels;

namespace StockGames.Tests.ViewModels
{
    [TestClass]
    public class ListStocksViewModelTest
    {

        [TestInitialize]
        public void Initialize()
        {
            MigrationManager.IfExistsRemoveDataContext();
            MigrationManager.InitializeDataContext();
        }

        [TestMethod]
        public void TestViewModelSetup()
        {
            ListStocksViewModel viewModel = new ListStocksViewModel();
            Assert.IsTrue(viewModel.Stocks != null);

            foreach (var stock in viewModel.Stocks)
            {
                Assert.IsTrue(!String.IsNullOrWhiteSpace(stock.StockIndex));
                
                Assert.IsTrue(!String.IsNullOrWhiteSpace(stock.CompanyName));
                Assert.IsTrue(stock.CurrentPrice > 0);
                Assert.IsTrue(stock.PreviousPrice > 0);
            }
        }
    }
}
