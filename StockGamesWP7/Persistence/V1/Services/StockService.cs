using System;
using StockGames.Models;
using System.Linq;
using System.Collections.Generic;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.DataModel;

namespace StockGames.Persistence.V1.Services
{
    public class StockService
    {
        private static readonly StockService instance = new StockService();
        public static StockService Instance {
            get 
            {
                return instance;
            }
        }

        private StockService() { }

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
