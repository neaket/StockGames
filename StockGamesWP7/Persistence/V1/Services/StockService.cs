using System;
using System.Linq;
using System.Collections.Generic;
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
                var lastTwoSnapshots = (from snapshot in context.StockSnapshots
                                        where snapshot.StockIndex == stockIndex &&
                                        snapshot.Tombstone <= GameState.Instance.GameTime
                                        orderby snapshot.Tombstone descending
                                        select snapshot.Price).Take(2).ToArray();
                decimal currentPrice = 0;
                decimal previousPrice = 0;

                if (lastTwoSnapshots.Length == 2)
                    previousPrice = lastTwoSnapshots[1];

                if (lastTwoSnapshots.Length >= 1)
                    currentPrice = lastTwoSnapshots[0];

                var stockEntity = new StockEntity(stock.StockIndex, stock.CompanyName)
                    {
                        CurrentPrice = currentPrice,
                        PreviousPrice = previousPrice
                    };

                return stockEntity;
            }
        }

        /// <summary> ReturnsaAll stocks that are persisted. </summary>
        /// <returns>
        /// An Enumerator of all stocks.
        /// </returns>
        public IEnumerable<StockEntity> GetStocks()
        {
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var stocks = (from stock in context.Stocks
                             orderby stock.StockIndex ascending 
                             select new StockEntity(stock.StockIndex, stock.CompanyName) 
                             {
                                 CurrentPrice = NullableToZeroConvert((from ss in stock.Snapshots where ss.Tombstone <= GameState.Instance.GameTime orderby ss.Tombstone descending select ss).First()),
                                 PreviousPrice = NullableToZeroConvert((from ss in stock.Snapshots where ss.Tombstone < GameState.Instance.GameTime orderby ss.Tombstone descending select ss).Distinct().OrderByDescending(ss => ss.Tombstone).Skip(1).First())
                             }).ToArray();
                return stocks;
            }
        }

        private decimal NullableToZeroConvert(StockSnapshotDataModel nullable)
        {
            if (nullable == null)
                return 0;
            return nullable.Price;
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
        }
    }
}
