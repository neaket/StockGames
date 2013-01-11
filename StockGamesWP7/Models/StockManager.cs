﻿using System;
﻿using System.Collections.Generic;
﻿using System.Linq;
﻿using StockGames.Persistence.V1.Services;

namespace StockGames.Models
{
    /// <summary>
    /// The StockManager is used to perform all Stock specific operations.  The StockManager also keeps a WeakReference to
    /// all currently used StockEntities.  Which allows multiple ViewModel's to share the same data.  If a StockEntity is not
    /// referenced by other code, then it will be properly garbage collected.   
    /// </summary>
    public class StockManager
    {
        private readonly Dictionary<string, WeakReference> _stocks = new Dictionary<string, WeakReference>();

        private static readonly StockManager instance = new StockManager();
        public static StockManager Instance
        {
            get
            {
                return instance;
            }
        }

        private StockManager()
        {
        }


        /// <returns>A IEnumerable of All StockEntities currently persisted in the App.</returns>
        public IEnumerable<StockEntity> GetStocks()
        {
            // TODO optimize (should use a filter and should not need two lists;
            var stocks = StockService.Instance.GetStocks().ToArray();

            lock (_stocks)
            {
                for (int i = 0; i < stocks.Length; i++)
                {
                    StockEntity stockEntity = null;
                    WeakReference tempStock;
                        
                    var result = _stocks.TryGetValue(stocks[i].StockIndex, out tempStock);

                    if (result)
                    {
                        // If a weakreference exists, attempt to get a strong reference to the StockEntity
                        // Note: the garbage collector can run at any time collecting the StockEntity
                        stockEntity = tempStock.Target as StockEntity;
                    }

                    if (stockEntity == null)
                    {
                        stockEntity = stocks[i];

                        if (stockEntity != null)
                        {
                            var weakStock = new WeakReference(stockEntity);
                            _stocks.Add(stocks[i].StockIndex, weakStock);
                        }
                    }
                    else
                    {
                        stocks[i] = stockEntity;
                    }
                }
            }

            return stocks;
        }

        public StockEntity GetStock(String stockIndex)
        {
            StockEntity stockEntity = null;

            lock (_stocks)
            {
                WeakReference tempStock;
                var result = _stocks.TryGetValue(stockIndex, out tempStock);
                
                if (result)
                {
                    // If a weakreference exists, attempt to get a strong reference to the StockEntity
                    // Note: the garbage collector can run at any time collecting the StockEntity
                    stockEntity = tempStock.Target as StockEntity;
                }

                if (stockEntity == null)
                {
                    stockEntity = StockService.Instance.GetStock(stockIndex);

                    if (stockEntity != null)
                    {
                        var weakStock = new WeakReference(stockEntity);
                        _stocks.Add(stockIndex, weakStock);
                    }
                }
            }

            if (stockEntity != null)
            {
                return stockEntity;
            }

            throw new ArgumentException(String.Format("Stock with Index of {0} does not exist.", stockIndex));
        }
    }
}