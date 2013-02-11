using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using StockGames.Entities;
using StockGames.Messaging;
using StockGames.Persistence.V1.Services;

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
            Messenger.Default.Register<GameTimeUpdatedMessageType>(this, GameTimeUpdated);
            Messenger.Default.Register<StockUpdatedMessageType>(this, StockUpdated);
            
            Stocks = new ObservableCollection<StockEntity>();
            LoadStocks();
        }

        private void GameTimeUpdated(GameTimeUpdatedMessageType message)
        {
            LoadStocks();
        }

        private void StockUpdated(StockUpdatedMessageType message)
        {
            // TODO check the message.StockIndex for optimizations
            LoadStocks();
        }

        private void LoadStocks()
        {
            Stocks.Clear();
            foreach (var stock in StockService.Instance.GetStocks())
            {
                Stocks.Add(stock);
            }
           
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
