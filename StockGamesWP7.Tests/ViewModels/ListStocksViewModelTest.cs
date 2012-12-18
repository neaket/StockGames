﻿using System;
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
using StockGames;
using StockGames.Persistance.V1.DataContexts;
using StockGames.ViewModels;

namespace StockGames.Tests.ViewModels
{
    [TestClass]
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
            ListStocksViewModel viewModel = new ListStocksViewModel(null);
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
