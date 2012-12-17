using System;
using StockGames.Controllers;
using StockGames.Models;
using StockGames.Persistance.V1.Services;

namespace StockGames.ViewModels
{
    public class StockViewModel
    {
        public StockEntity Stock { get; set; }

        public StockViewModel(string stockIndex)
        {
            Stock = StocksManager.Instance.GetStock(stockIndex);
        }

        public void Update()
        {
            CommandInvoker.Instance.FetchCommand(CommandInvoker.REQUEST_UPDATE_STOCK, Stock);
        }
    }
}
