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

        public void AddStock(StockEntity stockEntity) 
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                // TODO ensure no duplicates
                var stock = new StockModel {StockIndex = stockEntity.StockIndex, CompanyName = stockEntity.CompanyName, CurrentPrice = stockEntity.CurrentPrice, PreviousPrice = stockEntity.PreviousPrice};
                var current = DateTime.Now; // TODO
                var previous = new DateTime(current.Year, current.Month, current.Day); // TODO
                var prevStockSnapshot = new StockSnapshotModel
                    {
                        Stock = stock,
                        Tombstone = previous,
                        Price = stockEntity.PreviousPrice
                    };
                context.StockSnapshots.InsertOnSubmit(prevStockSnapshot);

                var currentStockSnapshot = new StockSnapshotModel
                    {
                        Stock = stock,
                        Tombstone = current,
                        Price = stockEntity.CurrentPrice
                    };
                context.StockSnapshots.InsertOnSubmit(currentStockSnapshot);
                
                context.SubmitChanges();
            }
        }


        public void AddStockSnapshot(StockEntity stockEntity)
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                var current = DateTime.Now; // TODO
                var stock = (from s in context.Stocks where s.StockIndex == stockEntity.StockIndex select s).Single();
                var stockSnapshot = new StockSnapshotModel
                {
                    Stock = stock,
                    Tombstone = current,
                    Price = stockEntity.CurrentPrice
                };

                context.StockSnapshots.InsertOnSubmit(stockSnapshot);
                context.SubmitChanges();
            }
        }
    }
}
