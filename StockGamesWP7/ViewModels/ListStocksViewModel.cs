using System;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using StockGames.Models;
using System.Collections.ObjectModel;

namespace StockGames.ViewModels
{
    public class ListStocksViewModel
    {
        public ObservableCollection<StockEntity> Stocks { get; set; }
        public ICommand RefreshCommand { get; private set; }
        private ICommand ViewStockCommand { get; set; }

        public StockEntity SelectedStock { get { return null; } set { if (value != null) ViewStockCommand.Execute(value); } }

        public ListStocksViewModel(ICommand viewStockCommand)
        {
            if (viewStockCommand == null) 
                throw new ArgumentNullException("viewStockCommand");

            RefreshCommand = new RelayCommand(LoadStocks);
            ViewStockCommand = viewStockCommand;
            Stocks = new ObservableCollection<StockEntity>();
            LoadStocks();
        }

        private void LoadStocks()
        {
            Stocks.Clear();

            foreach (StockEntity stock in StocksManager.Instance.GetStocks())
            {
                Stocks.Add(stock);
            }
        }
    }
}
