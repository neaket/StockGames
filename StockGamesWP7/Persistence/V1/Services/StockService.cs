﻿using System;
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

                var stockEntity = new StockEntity(stock.StockIndex, stock.CompanyName)
                    {
                        CurrentPrice = stock.CurrentPrice,
                        PreviousPrice = stock.PreviousPrice
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
            
            // TODO This method needs to be optimized in the future
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                var stocks = (from s in context.Stocks select s).ToList();
                IList<StockEntity> stockEntities = new List<StockEntity>(stocks.Count);

                foreach (var stock in stocks)
                {
                    stockEntities.Add(new StockEntity(stock.StockIndex, stock.CompanyName)
                    {
                        CurrentPrice = stock.CurrentPrice,
                        PreviousPrice = stock.PreviousPrice
                    });
                }

                return stockEntities;
            }
        }

        /// <summary> Adds a stock. </summary>
        /// <param name="stockEntity">  The stock entity. </param>
        public void AddStock(StockEntity stockEntity) 
        {
            Debug.Assert(stockEntity.CurrentPrice > 0);
            Debug.Assert(stockEntity.PreviousPrice > 0);

            using (var context = StockGamesDataContext.GetReadWrite())
            {
                // TODO ensure no duplicates
                var stock = new StockDataModel {StockIndex = stockEntity.StockIndex, CompanyName = stockEntity.CompanyName, CurrentPrice = stockEntity.CurrentPrice, PreviousPrice = stockEntity.PreviousPrice};
                var current = DateTime.Now; // TODO
                var previous = new DateTime(current.Year, current.Month, current.Day); // TODO
                var prevStockSnapshot = new StockSnapshotDataModel
                    {
                        Stock = stock,
                        Tombstone = previous,
                        Price = stockEntity.PreviousPrice
                    };
                context.StockSnapshots.InsertOnSubmit(prevStockSnapshot);

                var currentStockSnapshot = new StockSnapshotDataModel
                    {
                        Stock = stock,
                        Tombstone = current,
                        Price = stockEntity.CurrentPrice
                    };
                context.StockSnapshots.InsertOnSubmit(currentStockSnapshot);
                
                context.SubmitChanges();
            }
        }

        /// <summary> Adds a stock snapshot to a specific stock. </summary>
        /// <param name="stockIndex">   Index of the stock. </param>
        /// <param name="price">        The snapshot price. </param>
        public void AddStockSnapshot(string stockIndex, decimal price)
        {
            Debug.Assert(price > 0);
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var current = DateTime.Now; // TODO
                var stock = (from s in context.Stocks where s.StockIndex == stockIndex select s).Single();
                var stockSnapshot = new StockSnapshotDataModel
                {
                    Stock = stock,
                    Tombstone = current,
                    Price = price
                };
                stock.PreviousPrice = stock.CurrentPrice;
                stock.CurrentPrice = price;
                
                context.StockSnapshots.InsertOnSubmit(stockSnapshot);
                context.SubmitChanges();
            }
        }
    }
}
