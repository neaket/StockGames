using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Models;
using System.Collections.ObjectModel;

namespace StockGames.ViewModels
{
    public class ListStocksViewModel : ViewModelBase
    {
        public ObservableCollection<StockEntity> Stocks { get; set; }

        private StockEntity _selectedStock;
        public StockEntity SelectedStock 
        { 
            get { return _selectedStock; } 
            set
            {
                _selectedStock = value; 
                if (_selectedStock != null) ViewStock();
                
            } 
        }

        public ListStocksViewModel()
        {
            
            LoadStocks();
        }

        private void LoadStocks()
        {
            Stocks = new ObservableCollection<StockEntity>(StocksManager.Instance.GetStocks());
        }

        private void ViewStock()
        {
            var uri = new Uri("/Views/StockView.xaml?StockIndex=" + SelectedStock.StockIndex, UriKind.Relative);
            Messenger.Default.Send(uri, "Navigate");
            SelectedStock = null;
            RaisePropertyChanged("SelectedStock");
            
        }
    }
}
