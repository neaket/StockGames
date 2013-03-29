using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using System.Collections.ObjectModel;
using StockGames.Entities;
using StockGames.Messaging;
using StockGames.Persistence.V1.Services;
using StockGames.Views;

namespace StockGames.ViewModels
{
    /// <summary>   The ListStocksViewModel is used by the <see cref="ListStocksView" />. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class ListStocksViewModel : ViewModelBase
    {
        /// <summary>   Gets collection of StockEntities. </summary>
        ///
        /// <value> The StockEntities. </value>
        public ObservableCollection<StockEntity> Stocks { get; private set; }

        private StockEntity _selectedStock;

        /// <summary>
        /// Gets or sets the selected stock.  When the selected stock is changed, the
        /// <see cref="StockView"/> is displayed on the GUI.
        /// </summary>
        ///
        /// <value> The selected stock. </value>
        public StockEntity SelectedStock 
        { 
            get { return _selectedStock; } 
            set
            {
                _selectedStock = value;
                if (_selectedStock != null) ViewStock();
            } 
        }

        /// <summary>   Initializes a new instance of the ListStocksViewModel class. </summary>
        public ListStocksViewModel()
        {
            Messenger.Default.Register<GameTimeUpdatedMessageType>(this, GameTimeUpdated);
            Messenger.Default.Register<StockUpdatedMessageType>(this, StockUpdated);
            
            Stocks = new ObservableCollection<StockEntity>();
            LoadStocks();
        }

        // Reloads the data in the ViewModel when the GameTime is updated.
        private void GameTimeUpdated(GameTimeUpdatedMessageType message)
        {
            LoadStocks();
        }

        // Reloads the data in the ViewModel when a stock is updated
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
