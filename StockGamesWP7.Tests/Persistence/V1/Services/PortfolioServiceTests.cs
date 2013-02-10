using System;
using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockGames.Entities;
using StockGames.Messaging;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.DataModel;
using StockGames.Persistence.V1.Services;

namespace StockGames.Tests.Persistence.V1.Services
{
    [TestClass]
    [Tag("Persistence")]
    public class PortfolioServiceTests : WorkItemTest
    {

        DateTime now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

        [TestInitialize]
        public void Initialize()
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                if (context.DatabaseExists())
                {
                    context.DeleteDatabase();
                }

                context.CreateDatabase();
            }
        }

        [TestMethod]
        public void CreatePortfolio()
        {
            string portfolioName = "UnitTestPortfolio";

            var portfolioId = PortfolioService.Instance.AddPortfolio(portfolioName);

            var persisted = PortfolioService.Instance.GetPortfolio(portfolioId);

            Assert.AreEqual(portfolioName, persisted.Name);
            Assert.AreEqual(0, persisted.Balance);
        }

        [TestMethod]
        public void AddTransaction()
        {
            string portfolioName = "UnitTestPortfolio";
            int portfolioId = PortfolioService.Instance.AddPortfolio(portfolioName);

            decimal amount = 50;
            DateTime tombstone = now;

            PortfolioService.Instance.AddTransaction(portfolioId, amount, tombstone);

            var portfolio = PortfolioService.Instance.GetPortfolio(portfolioId);
            Assert.AreEqual(amount, portfolio.Balance);

            var entries = PortfolioService.Instance.GetEntries(portfolioId);
            Assert.AreEqual(1, entries.Count());
            Assert.AreEqual(amount, entries.ElementAt(0).Amount);
            Assert.AreEqual(PortfolioEntryDataModel.EntryCode.Transaction, entries.ElementAt(0).Code);
        }

        [TestMethod]
        public void AddTradeBuy()
        {
            // Create stock with one snapshot
            string index = "TONE";
            string companyName = "Some Name";
            decimal price0 = 10;
            DateTime tombstone0 = now.AddHours(-1);


            StockService.Instance.AddStock(index, companyName);
            StockService.Instance.AddStockSnapshot(index, price0, tombstone0);
            //----------------

            string portfolioName = "UnitTestPortfolio";
            int portfolioId = PortfolioService.Instance.AddPortfolio(portfolioName);

            decimal initialAmount = 5000;
            int quantity0 = 5;

            PortfolioDataModel portfolio;

            try
            {
                PortfolioService.Instance.AddTrade(portfolioId, index, TradeType.Buy, quantity0, tombstone0);
                Assert.Fail("If Code reaches here, Portfolio Balance should now be negative, and should not be allowed.");

            }
            catch (ArgumentException)
            {
                portfolio = PortfolioService.Instance.GetPortfolio(portfolioId);
                Assert.AreEqual(0, portfolio.Balance);
            }

            // Add a transaction to give the portfolio a positive balance
            PortfolioService.Instance.AddTransaction(portfolioId, initialAmount, tombstone0);

            // Trade 1
            PortfolioService.Instance.AddTrade(portfolioId, index, TradeType.Buy, quantity0, tombstone0);

            portfolio = PortfolioService.Instance.GetPortfolio(portfolioId);
            Assert.AreEqual(initialAmount - quantity0 * price0, portfolio.Balance);

            var entries = PortfolioService.Instance.GetEntries(portfolioId);
            Assert.AreEqual(2, entries.Count());
            Assert.AreEqual(-quantity0 * price0, entries.ElementAt(1).Amount);
            Assert.AreEqual(PortfolioEntryDataModel.EntryCode.Trade, entries.ElementAt(1).Code);

            var trade = entries.ElementAt(1) as PortfolioTradeDataModel;
            Assert.AreEqual(quantity0, trade.Quantity);
            Assert.AreEqual(TradeType.Buy, trade.TradeType);
            Assert.AreEqual(tombstone0, trade.Tombstone);
            //-----------------

            // Add new snapshot
            decimal price1 = 5;
            DateTime tombstone1 = now;

            StockService.Instance.AddStockSnapshot(index, price1, tombstone1);
            // ------------
            
            // Trade 2
            int quantity1 = 7;
            PortfolioService.Instance.AddTrade(portfolioId, index, TradeType.Buy, quantity1, tombstone1);

            portfolio = PortfolioService.Instance.GetPortfolio(portfolioId);
            Assert.AreEqual(initialAmount - quantity0 * price0 - quantity1 * price1, portfolio.Balance);

            entries = PortfolioService.Instance.GetEntries(portfolioId);
            Assert.AreEqual(3, entries.Count());
            Assert.AreEqual(-quantity1 * price1, entries.ElementAt(2).Amount);
            Assert.AreEqual(PortfolioEntryDataModel.EntryCode.Trade, entries.ElementAt(2).Code);

            trade = entries.ElementAt(2) as PortfolioTradeDataModel;
            Assert.AreEqual(quantity1, trade.Quantity);
            Assert.AreEqual(TradeType.Buy, trade.TradeType);
            Assert.AreEqual(tombstone1, trade.Tombstone);
            // --------------
            
        }

        [TestMethod]
        public void GetGroupedTrades()
        {
            // Create stock with one snapshot
            string index = "TONE";
            string companyName = "Some Name";
            decimal price0 = 10;
            DateTime tombstone0 = now.AddHours(-1);


            StockService.Instance.AddStock(index, companyName);
            StockService.Instance.AddStockSnapshot(index, price0, tombstone0);
            //----------------

            string portfolioName = "UnitTestPortfolio";
            int portfolioId = PortfolioService.Instance.AddPortfolio(portfolioName);

            decimal initialAmount = 5000;
            int quantity0 = 5;

            // Add a transaction to give the portfolio a positive balance
            PortfolioService.Instance.AddTransaction(portfolioId, initialAmount, tombstone0);

            // Trade 1
            PortfolioService.Instance.AddTrade(portfolioId, index, TradeType.Buy, quantity0, tombstone0);
            //-----------------

            // Add new snapshot
            decimal price1 = 5;
            DateTime tombstone1 = now;

            StockService.Instance.AddStockSnapshot(index, price1, tombstone1);
            // ------------

            // Trade 2
            int quantity1 = 7;
            PortfolioService.Instance.AddTrade(portfolioId, index, TradeType.Buy, quantity1, tombstone1);
            // --------------

            var groupedTrades = PortfolioService.Instance.GetGroupedTrades(portfolioId);
            Assert.AreEqual(1, groupedTrades.Count());

            decimal average = Math.Round((quantity0*price0 + quantity1*price1)/ (quantity0 + quantity1), 2);
            var group0 = groupedTrades.ElementAt(0);
            Assert.AreEqual(price1, group0.CurrentPrice);
            Assert.AreEqual(quantity0 + quantity1, group0.Quantity);
            Assert.AreEqual(average, group0.AveragePurchasedPrice);
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(2000)]
        public void PortfolioUpdateMessageTest()
        {
            
            // Create stock with one snapshot
            string index = "TONE";
            string companyName = "Some Name";
            decimal price0 = 10;
            DateTime tombstone0 = now;


            StockService.Instance.AddStock(index, companyName);
            StockService.Instance.AddStockSnapshot(index, price0, tombstone0);
            //----------------
            
            int portfolioId = PortfolioService.Instance.AddPortfolio("UnitTestPortfolio");

            decimal amount = 100;
            DateTime tombstone = now;

            int count = 0;
            Messenger.Default.Register<PortfolioUpdatedMessageType>(this,
                message => EnqueueCallback(() =>
                    {
                        count++;
                        Assert.AreEqual(portfolioId, message.PortfolioId);

                        Messenger.Default.Unregister<StockUpdatedMessageType>(this);
                        if (count == 2)
                        {
                            TestComplete();
                        }
                    }
            ));

            PortfolioService.Instance.AddTransaction(portfolioId, amount, tombstone);
            PortfolioService.Instance.AddTrade(portfolioId, index, TradeType.Buy, 5, tombstone);
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
