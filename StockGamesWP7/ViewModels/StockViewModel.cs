using System.Diagnostics;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using StockGames.Controllers;
using StockGames.Models;

namespace StockGames.ViewModels
{
    public class StockViewModel : ViewModelBase
    {
        public ICommand UpdateCommand { get; private set; }
        public ICommand LoadStockCommand { get; private set; }

        public StockEntity Stock { get; private set; }

        public StockViewModel()
        {
            LoadStockCommand = new RelayCommand<string>(LoadStock);
            UpdateCommand = new RelayCommand(Update);
        }

        private void Update()
        {
            CommandInvoker.Instance.FetchCommand(CommandInvoker.REQUEST_UPDATE_STOCK, Stock);
        }

        private void LoadStock(string stockIndex)
        {
            Stock = StockManager.Instance.GetStock(stockIndex);
        }
    }
}
