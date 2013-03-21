using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using StockGames.Persistence.V1;
using StockGames.Persistence.V1.DataModel;
using GalaSoft.MvvmLight;
using StockGames.Persistence.V1.Services;
using StockGames.Views;

namespace StockGames.ViewModels
{
    /// <summary>   The PortfolioTradeViewModel is used by the <see cref="PortfolioTradeView" />. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class PortfolioTradeViewModel : ViewModelBase
    {
        /// <summary>   When LoadStockCommand is executed with [string stockIndex] as the parameter, the stock is loaded into this ViewModel. </summary>
        ///
        /// <value> The load stock command. </value>
        public ICommand LoadStockCommand { get; private set; }

        /// <summary>
        /// When the MakeTradeCommand is executed, the system will attempt to complete the trade.  If
        /// successful the GUI will go to the previous screen.
        /// </summary>
        ///
        /// <value> The make trade command. </value>
        public ICommand MakeTradeCommand { get; private set; }

        /// <summary>   Gets a list of trade types for the GUI dropdown. </summary>
        ///
        /// <value> A list of trade types. </value>
        public IEnumerable<TradeTypeWrapper> TradeTypes { get; private set; }

        /// <summary>   Gets the current price of the Stock on the Market. </summary>
        ///
        /// <value> The current price. </value>
        public decimal CurrentPrice { private set; get; }

        /// <summary>   Gets the amount, used to tell the user how much the trade will cost. </summary>
        ///
        /// <value> The amount. </value>
        public decimal Amount
        {
            get { return CurrentPrice*_quantity; }
        }

        private int _quantity;

        /// <summary>   Gets or sets the quantity of stocks to be traded. </summary>
        ///
        /// <value> The quantity. </value>
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                RaisePropertyChanged("Quantity");
                RaisePropertyChanged("Amount");
            }
        }

        private int _maximumQuantity;

        /// <summary>
        /// Gets or sets the maximum quantity of stocks that can be traded. Used for setting the maximum
        /// value of the Slider on the GUI.
        /// </summary>
        ///
        /// <value> The maximum quantity. </value>
        public int MaximumQuantity
        {
            private set { 
                _maximumQuantity = value;
                RaisePropertyChanged("MaximumQuantity"); 
            }
            get 
            { 
                return _maximumQuantity;
            }
        }

        /// <summary>   The stock index is used to identify a particular stock in the stock market. </summary>
        ///
        /// <value> The stock index. </value>
        public string StockIndex { get; private set; }

        private TradeTypeWrapper _selectedTradeType;

        /// <summary>
        /// Gets or sets the selected trade type.  When updated the MaximumQuantity is updated to reflect
        /// the selected Type.  (If sell is selected, the user cannot sell more stocks then they own.)
        /// </summary>
        ///
        /// <value> The type of the selected trade. </value>
        public TradeTypeWrapper SelectedTradeType
        {
            get { return _selectedTradeType; }
            set
            {
                _selectedTradeType = value;
                if (_selectedTradeType.TradeType == TradeType.Buy)
                {
                    MaximumQuantity = 10;
                }
                else if (_selectedTradeType.TradeType == TradeType.Sell)
                {
                    MaximumQuantity = PortfolioService.Instance.GetTradeQuantity(GameState.Instance.MainPortfolioId, StockIndex);
                }
            }
        }

        /// <summary>   Initializes a new instance of the PortfolioTradeViewModel class. </summary>
        public PortfolioTradeViewModel()
        {
            LoadStockCommand = new RelayCommand<string>(LoadStock);
            MakeTradeCommand = new RelayCommand(MakeTrade);
            StockIndex = "INDX";

            // Setup the trade types drop list, with a string representation
            var tradeTypes = new List<TradeTypeWrapper>
                {
                    new TradeTypeWrapper(TradeType.Buy, "Buy"),
                    new TradeTypeWrapper(TradeType.Sell, "Sell"),
                    //new TradeTypeWrapper(TradeType.Short, "Short"),
                    //new TradeTypeWrapper(TradeType.Cover, "Cover")
                };
            SelectedTradeType = tradeTypes[0]; // Set the initial tradetype to the first one

            TradeTypes = tradeTypes;
        }

        private void LoadStock(string stockIndex)
        {
            var stock = StockService.Instance.GetStock(stockIndex);
            StockIndex = stockIndex;
            CurrentPrice = stock.CurrentPrice;

        }

        private void MakeTrade()
        {
            if (Quantity <= 0)
            {
                MessageBox.Show("You must trade at least 1 stock.", "Error", MessageBoxButton.OK);
                return;
            }
            PortfolioService.Instance.AddTrade(GameState.Instance.MainPortfolioId, StockIndex, SelectedTradeType.TradeType, Quantity, GameState.Instance.GameTime);
            MessengerInstance.Send<object>(null, "NavigateBack");
        }

        /// <summary>   The TradeTypeWrapper is used to create a friendly representation of a TradeType for the GUI. </summary>
        ///
        /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
        public class TradeTypeWrapper
        {
            /// <summary>   The TradeType. </summary>
            public readonly TradeType TradeType;
            private readonly string _displayName;

            /// <summary>   Initializes a new instance of the TradeTypeWrapper class. </summary>
            ///
            /// <param name="tradeType">    Type of the trade. </param>
            /// <param name="displayName">  Friendly display name for the GUI. </param>
            public TradeTypeWrapper(TradeType tradeType, string displayName)
            {
                TradeType = tradeType;
                _displayName = displayName;
            }

            /// <summary>   ListPicker uses ToString to render an Item. </summary>
            ///
            /// <returns>   This TradeType as a friendly display dame. </returns>
            public override string ToString()
            {
                return _displayName;
            }
        }
    }
}
