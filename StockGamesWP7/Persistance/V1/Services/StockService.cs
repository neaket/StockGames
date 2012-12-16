using System;
using StockGames.Models;
using System.Linq;
using System.Collections.Generic;
using StockGames.Persistance.V1.DataContexts;
using StockGames.Persistance.V1.DataModel;

namespace StockGames.Persistance.V1.Services
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
                // create query to get stocksnapshots 
                var query = from s in context.StockSnapshots where stockIndex == s.Stock.StockIndex orderby s.Tombstone descending select s;
                
                var newestTwoStockSnapshots = query.Take(2).ToList();

                var currentStockSnapshot = newestTwoStockSnapshots.First();
                var prevStockSnapshot = newestTwoStockSnapshots.ElementAt(1);

                var stock = new StockEntity(currentStockSnapshot.Stock.StockIndex, currentStockSnapshot.Stock.CompanyName)
                    {
                        CurrentPrice = currentStockSnapshot.Price,
                        PreviousPrice = prevStockSnapshot.Price
                    };

                return stock;
            }
        }

        public IEnumerable<StockEntity> GetStocks()
        {
            // TODO This method needs to be optimized in the future
            List<string> stockIndexes;
            using (var context = StockGamesDataContext.GetReadOnly())
            {
                stockIndexes = (from s in context.Stocks select s.StockIndex).ToList();
            }

            return stockIndexes.Select(GetStock).ToList();
        }

        public void AddStock(StockEntity stockEntity) 
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                // TODO ensure no duplicates
                var stock = new StockModel {StockIndex = stockEntity.StockIndex, CompanyName = stockEntity.CompanyName};

                var current = DateTime.Now;
                var previous = new DateTime(current.Year, current.Month, current.Day);
                var prevStockSnapshot = new StockSnapshotModel
                    {
                        Stock = stock,
                        Market = MarketService.TestMarket,
                        Tombstone = previous,
                        Price = stockEntity.PreviousPrice
                    };
                context.StockSnapshots.InsertOnSubmit(prevStockSnapshot);

                var currentStockSnapshot = new StockSnapshotModel
                    {
                        Stock = stock,
                        Market = MarketService.TestMarket,
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
                var current = DateTime.Now;
                var stock = (from s in context.Stocks where s.StockIndex == stockEntity.StockIndex select s).Single();

                var stockSnapshot = new StockSnapshotModel
                {
                    Stock = stock,
                    Market = MarketService.TestMarket, // TODO use the actual market
                    Tombstone = current,
                    Price = stockEntity.CurrentPrice
                };

                context.StockSnapshots.InsertOnSubmit(stockSnapshot);
                context.SubmitChanges();
            }
        }
    }
}
