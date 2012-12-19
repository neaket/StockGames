using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using StockGames.Controllers;
using StockGames.Models;

namespace StockGames.ViewModels
{
    public class StockViewModel
    {
        public ICommand UpdateCommand { get; private set; }

        public StockEntity Stock { get; set; }

        public StockViewModel(string stockIndex)
        {

            Stock = StocksManager.Instance.GetStock(stockIndex);
            UpdateCommand = new RelayCommand(Update);
        }

        private void Update()
        {
            CommandInvoker.Instance.FetchCommand(CommandInvoker.REQUEST_UPDATE_STOCK, Stock);
        }

        
    }
}
