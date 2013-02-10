using System;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using StockGames.Messaging;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.Services;
using StockGames.Entities;

namespace StockGames.Tests.Persistence.V1.Services
{
    [TestClass]
    [Tag("Persistence")]
    public class StockServiceTests : WorkItemTest
    {
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
        public void StockWithZeroSnapshots()
        {
            int prevCount = StockService.Instance.GetStocks().Count();
            StockService.Instance.AddStock("TZERO", "Test Company Test");

            Assert.AreEqual(prevCount + 1, StockService.Instance.GetStocks().Count());

            StockEntity persisted = StockService.Instance.GetStock("TZERO");

            Assert.AreEqual("TZERO", persisted.StockIndex);
            Assert.AreEqual("Test Company Test", persisted.CompanyName);
            Assert.AreEqual(0, persisted.CurrentPrice);
            Assert.AreEqual(0, persisted.PreviousPrice);
        }

        [TestMethod]
        public void StockWithOneSnapshot()
        {
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

            string index = "TONE";
            string companyName = "Some Name";
            decimal price0 = 10;
            DateTime tombstone0 = now;


            StockService.Instance.AddStock(index, companyName);
            StockService.Instance.AddStockSnapshot(index, price0, tombstone0);

            var persisted = StockService.Instance.GetStock(index);

            Assert.AreEqual(index, persisted.StockIndex);
            Assert.AreEqual(companyName, persisted.CompanyName);
            Assert.AreEqual(price0, persisted.CurrentPrice);
            Assert.AreEqual(0, persisted.PreviousPrice);

            Assert.AreEqual(1, persisted.Snapshots.Count);
            Assert.AreEqual(price0, persisted.Snapshots[0].Price);
            Assert.AreEqual(tombstone0, persisted.Snapshots[0].Tombstone);

            var persistedList = StockService.Instance.GetStocks();
            Assert.AreEqual(1, persistedList.Length);

            persisted = persistedList[0];
            Assert.AreEqual(index, persisted.StockIndex);
            Assert.AreEqual(companyName, persisted.CompanyName);
            Assert.AreEqual(price0, persisted.CurrentPrice);
            Assert.AreEqual(0, persisted.PreviousPrice);

            Assert.AreEqual(1, persisted.Snapshots.Count);
            Assert.AreEqual(price0, persisted.Snapshots[0].Price);
            Assert.AreEqual(tombstone0, persisted.Snapshots[0].Tombstone);
        }

        [TestMethod]
        public void StockWithTwoSnapshots()
        {
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

            string index = "TTWO";
            string companyName = "Some Name";
            decimal price0 = 10;
            DateTime tombstone0 = now.AddHours(-3);
            decimal price1 = 20;
            DateTime tombstone1 = now.AddHours(-2);


            StockService.Instance.AddStock(index, companyName);
            StockService.Instance.AddStockSnapshot(index, price0, tombstone0);
            StockService.Instance.AddStockSnapshot(index, price1, tombstone1);

            var persisted = StockService.Instance.GetStock(index);

            Assert.AreEqual(index, persisted.StockIndex);
            Assert.AreEqual(companyName, persisted.CompanyName);
            Assert.AreEqual(price1, persisted.CurrentPrice);
            Assert.AreEqual(price0, persisted.PreviousPrice);

            Assert.AreEqual(2, persisted.Snapshots.Count);
            Assert.AreEqual(price0, persisted.Snapshots[1].Price);
            Assert.AreEqual(tombstone0, persisted.Snapshots[1].Tombstone);
            Assert.AreEqual(price1, persisted.Snapshots[0].Price);
            Assert.AreEqual(tombstone1, persisted.Snapshots[0].Tombstone);


            var persistedList = StockService.Instance.GetStocks();
            Assert.AreEqual(1, persistedList.Length);

            persisted = persistedList[0];
            Assert.AreEqual(index, persisted.StockIndex);
            Assert.AreEqual(companyName, persisted.CompanyName);
            Assert.AreEqual(price1, persisted.CurrentPrice);
            Assert.AreEqual(price0, persisted.PreviousPrice);

            Assert.AreEqual(2, persisted.Snapshots.Count);
            Assert.AreEqual(price0, persisted.Snapshots[1].Price);
            Assert.AreEqual(tombstone0, persisted.Snapshots[1].Tombstone);
            Assert.AreEqual(price1, persisted.Snapshots[0].Price);
            Assert.AreEqual(tombstone1, persisted.Snapshots[0].Tombstone);
        }

        [TestMethod]
        public void StockWithThreeSnapshots()
        {
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);

            string index = "TTHREE";
            string companyName = "Some Name";
            decimal price0 = 10;
            DateTime tombstone0 = now.AddHours(-3);
            decimal price1 = 20;
            DateTime tombstone1 = now.AddHours(-2);
            decimal price2 = 30;
            DateTime tombstone2 = now.AddHours(-1);


            StockService.Instance.AddStock(index, companyName);
            StockService.Instance.AddStockSnapshot(index, price0, tombstone0);
            StockService.Instance.AddStockSnapshot(index, price1, tombstone1);
            StockService.Instance.AddStockSnapshot(index, price2, tombstone2);

            var persisted = StockService.Instance.GetStock(index);

            Assert.AreEqual(index, persisted.StockIndex);
            Assert.AreEqual(companyName, persisted.CompanyName);
            Assert.AreEqual(price2, persisted.CurrentPrice);
            Assert.AreEqual(price1, persisted.PreviousPrice);

            Assert.AreEqual(3, persisted.Snapshots.Count);
            Assert.AreEqual(price0, persisted.Snapshots[2].Price);
            Assert.AreEqual(tombstone0, persisted.Snapshots[2].Tombstone);
            Assert.AreEqual(price1, persisted.Snapshots[1].Price);
            Assert.AreEqual(tombstone1, persisted.Snapshots[1].Tombstone);
            Assert.AreEqual(price2, persisted.Snapshots[0].Price);
            Assert.AreEqual(tombstone2, persisted.Snapshots[0].Tombstone);


            var persistedList = StockService.Instance.GetStocks();
            Assert.AreEqual(1, persistedList.Length);

            persisted = persistedList[0];
            Assert.AreEqual(index, persisted.StockIndex);
            Assert.AreEqual(companyName, persisted.CompanyName);
            Assert.AreEqual(price2, persisted.CurrentPrice);
            Assert.AreEqual(price1, persisted.PreviousPrice);

            Assert.AreEqual(2, persisted.Snapshots.Count);
            Assert.AreEqual(price1, persisted.Snapshots[1].Price);
            Assert.AreEqual(tombstone1, persisted.Snapshots[1].Tombstone);
            Assert.AreEqual(price2, persisted.Snapshots[0].Price);
            Assert.AreEqual(tombstone2, persisted.Snapshots[0].Tombstone);
        }

        [TestMethod]
        [Asynchronous]
        [Timeout(2000)]
        public void StockUpdateMessageTest()
        {
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            String stockIndex = "MSG";
            string companyName = "Some Name";
            decimal price0 = 10;
            DateTime tombstone0 = now.AddHours(-3);

            Messenger.Default.Register<StockUpdatedMessageType>(this,
                message => EnqueueCallback(() =>
                    {
                        Assert.AreEqual(stockIndex, message.StockIndex);

                        Messenger.Default.Unregister<StockUpdatedMessageType>(this);
                        TestComplete();
                    }
            ));

            StockService.Instance.AddStock(stockIndex, companyName);
            StockService.Instance.AddStockSnapshot(stockIndex, price0, tombstone0);
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
