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
                var query = from s in context.StockSnapshots where stockIndex == s.Stock.StockIndex orderby s.Tombstone select s;
                
                var newestTwoStockSnapshots = query.Take(2).ToList();

                var currentStockSnapshot = newestTwoStockSnapshots.First();
                var prevStockSnapshot = newestTwoStockSnapshots.ElementAt(1);

                StockEntity stock = new StockEntity(currentStockSnapshot.Stock.StockIndex, currentStockSnapshot.Stock.CompanyName);
                stock.CurrentPrice = currentStockSnapshot.Price;
                stock.PreviousPrice = prevStockSnapshot.Price;

                return stock;
            }
        }

        /// <summary>
        /// Note: This method needs to be optimized in the future
        /// </summary> 
        /// <returns></returns>
        public IEnumerable<StockEntity> GetStocks()
        {
            List<StockEntity> stocks = new List<StockEntity>();
            List<string> stockIndexes;
            using (var context = StockGamesDataContext.GetReadOnly())
            {

                stockIndexes = (from s in context.Stocks select s.StockIndex).ToList();
                var count = (from s in context.Stocks select s).Count();
            }


            

            foreach (var stockIndex in stockIndexes)
            {
                stocks.Add(GetStock(stockIndex));
            }

            return stocks;
        }

        public void AddStock(StockEntity stockEntity) 
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                StockModel stock;

                var existingQuery = from s in context.Stocks where s.StockIndex == stockEntity.StockIndex select s;
                stock = existingQuery.SingleOrDefault();
                if (stock == null)
                {
                    stock = stock = new StockModel();
                    stock.StockIndex = stockEntity.StockIndex;
                    stock.CompanyName = stockEntity.CompanyName;
                }

                DateTime current = DateTime.Now;
                DateTime previous = new DateTime(current.Year, current.Month, current.Day);
                StockSnapshotModel prevStockSnapshot = new StockSnapshotModel();
                prevStockSnapshot.Stock = stock;
                prevStockSnapshot.Market = MarketService.TestMarket;
                prevStockSnapshot.Tombstone = previous;
                prevStockSnapshot.Price = stockEntity.PreviousPrice;
                context.StockSnapshots.InsertOnSubmit(prevStockSnapshot);

                StockSnapshotModel currentStockSnapshot = new StockSnapshotModel();
                currentStockSnapshot.Stock = stock;
                currentStockSnapshot.Market = MarketService.TestMarket;
                currentStockSnapshot.Tombstone = current;
                currentStockSnapshot.Price = stockEntity.CurrentPrice;
                context.StockSnapshots.InsertOnSubmit(currentStockSnapshot);


                context.SubmitChanges();

                                
            }
        }


        

    }
}
