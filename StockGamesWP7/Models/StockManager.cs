﻿using System;
﻿using System.Collections.Generic;
﻿using System.Linq;
﻿using StockGames.Persistance.V1.Services;

namespace StockGames.Models
{
    public class StocksManager
    {
        private readonly Dictionary<string, WeakReference> _stocks = new Dictionary<string, WeakReference>();

        private static readonly StocksManager instance = new StocksManager();
        public static StocksManager Instance
        {
            get
            {
                return instance;
            }
        }

        private StocksManager()
        {
        }

        public IEnumerable<StockEntity> GetStocks()
        {
            // TODO optimize (should use a filter and should not need two lists;
            var stocks = StockService.Instance.GetStocks().ToArray();

            lock (_stocks)
            {
                for (int i = 0; i < stocks.Length; i++)
                {
                    if (!_stocks.ContainsKey(stocks[i].StockIndex)) continue;
                    WeakReference tempStock;
                    StockEntity stockEntity = null;
                    var result = _stocks.TryGetValue(stocks[i].StockIndex, out tempStock);

                    if (result)
                    {
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
