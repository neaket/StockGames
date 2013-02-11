using System;
using System.Linq;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Messaging;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.DataModel;
using StockGames.Entities;
using System.Diagnostics;

namespace StockGames.Persistence.V1.Services
{
    public class StockService
    {
        #region instance
        
        private static readonly StockService instance = new StockService();
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

            Messenger.Default.Send(new StockUpdatedMessageType(stockIndex));
        }
    }
}
