using System;
using System.Windows.Input;
using StockGames.Controllers;
using StockGames.MVVM;
using StockGames.Models;
using StockGames.Persistance.V1.Services;

namespace StockGames.ViewModels
{
    public class StockViewModel
    {
        public ICommand UpdateCommand { get; private set; }

        public StockEntity Stock { get; set; }

        public StockViewModel(string stockIndex)
        {

            Stock = StocksManager.Instance.GetStock(stockIndex);
            UpdateCommand = new RelayCommand(param => Update());
        }

        private void Update()
        {
            CommandInvoker.Instance.FetchCommand(CommandInvoker.REQUEST_UPDATE_STOCK, Stock);
        }

        
    }
}
