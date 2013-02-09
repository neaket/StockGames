﻿using System;
using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockGames.Persistence.V1.DataContexts;
using StockGames.ViewModels;

namespace StockGames.Tests.ViewModels
{
    [TestClass]
    [Tag("ViewModels")]
    public class ListStocksViewModelTest
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
