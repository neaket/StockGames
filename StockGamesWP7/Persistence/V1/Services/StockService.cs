using System;
using System.Linq;
using System.Collections.Generic;
using StockGames.Messaging;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.DataModel;
using StockGames.Entities;
using System.Diagnostics;

namespace StockGames.Persistence.V1.Services
{
    /// <summary>
    /// The StockService a singleton and is used to access or manipulate stock related persisted data for the
    /// application.
    /// </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class StockService
    {
        #region instance
        
        private static readonly StockService instance = new StockService();

        /// <summary>   Gets the singleton instance of this class. </summary>
        ///
        /// <value> The singleton instance. </value>
        public static StockService Instance {
            get 
            {
                return instance;
            }
        }

        private StockService() { }

        #endregion

        /// <summary> Gets a stock. </summary>
        /// <param name="stockIndex">   Index of the stock. </param>
        /// <returns> The stock. </returns>
        public StockEntity GetStock(string stockIndex)
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var stock = context.Stocks.Single(s => s.StockIndex == stockIndex);
                var snapshots = (from snapshot in context.StockSnapshots
                                        where snapshot.StockIndex == stockIndex &&
                                        snapshot.Tombstone <= GameState.Instance.GameTime
                                        orderby snapshot.Tombstone descending
                                        select new StockSnapshotEntity(snapshot.Price, snapshot.Tombstone)).ToArray();  // TODO limit amount of snapshots loaded .Take(...)
                
                var stockEntity = new StockEntity(stock.StockIndex, stock.CompanyName, snapshots);

                return stockEntity;
            }
        }

        /// <summary> ReturnsaAll stocks that are persisted. </summary>
        /// <returns>
        /// An Enumerator of all stocks.
        /// </returns>
        public StockEntity[] GetStocks()
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var stocks = (from stock in context.Stocks
                             orderby stock.StockIndex ascending
                              select new StockEntity(
                                  stock.StockIndex, 
                                  stock.CompanyName, 
                                  (from ss in stock.Snapshots where ss.Tombstone <= GameState.Instance.GameTime orderby ss.Tombstone descending select new StockSnapshotEntity(ss.Price, ss.Tombstone)).Take(2).ToList())
                             ).ToArray();
                return stocks;
            }
        }

        /// <summary> Adds a stock. </summary>
        /// <param name="stockIndex">       Index of the stock. </param>
        /// <param name="companyName">      Name of the company. </param>
        public void AddStock(string stockIndex, string companyName) 
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                // TODO ensure no duplicates
                var stock = new StockDataModel {StockIndex = stockIndex, CompanyName = companyName};
                context.Stocks.InsertOnSubmit(stock);
                
                context.SubmitChanges();
            }
        }

        /// <summary> Adds a stock snapshot to a specific stock. </summary>
        /// <param name="stockIndex">   Index of the stock. </param>
        /// <param name="price">        The snapshot price. </param>
        /// <param name="tombstone">    Date/Time tombstone of the snapshot. </param>
        /// <remarks>Not to be used for adding multiple snapshots.</remarks>
        public void AddStockSnapshot(string stockIndex, decimal price, DateTime tombstone)
        {
            Debug.Assert(price > 0);

            using (var context = StockGamesDataContext.GetReadWrite())
            { 
                var stock = (from s in context.Stocks where s.StockIndex == stockIndex select s).Single();
                var stockSnapshot = new StockSnapshotDataModel
                {
                    Stock = stock,
                    Tombstone = tombstone,
                    Price = price
                };
                
                context.StockSnapshots.InsertOnSubmit(stockSnapshot);
                context.SubmitChanges();
            }

            MessengerWrapper.Send(new StockUpdatedMessageType(stockIndex));
        }

        /// <summary>   Add multiple stock snapshots to a specific stock. </summary>
        ///
        /// <param name="stockIndex">   Index of the stock. </param>
        /// <param name="prices">       The prices, note must be 1-to-1 mapped with the tombstones. </param>
        /// <param name="tombstones">   The tombstones, note must be 1-to-1 mapped with the prices. </param>
        public void AddStockSnapshots(string stockIndex, IList<decimal> prices, IList<DateTime> tombstones)
        {
            int count = prices.Count();
            Debug.Assert(count == tombstones.Count(), "Each price must be 1-to-1 mapped with a tombstone");

            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var stock = (from s in context.Stocks where s.StockIndex == stockIndex select s).Single();

                for (int i = 0; i < count; i++)
                {
                    var stockSnapshot = new StockSnapshotDataModel
                    {
                        Stock = stock,
                        Tombstone = tombstones.ElementAt(i),
                        Price = prices.ElementAt(i)
                    };
                    context.StockSnapshots.InsertOnSubmit(stockSnapshot);
                }
                context.SubmitChanges();
            }

            MessengerWrapper.Send(new StockUpdatedMessageType(stockIndex));
        }
    }
}
