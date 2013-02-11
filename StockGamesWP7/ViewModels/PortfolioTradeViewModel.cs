using System;
using System.Collections.Generic;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using StockGames.Controllers;
using StockGames.Persistence.V1;
using StockGames.Persistence.V1.DataModel;
using GalaSoft.MvvmLight;
using StockGames.Persistence.V1.Services;
using StockGames.Entities;

namespace StockGames.ViewModels
{
    public class PortfolioTradeViewModel : ViewModelBase
    {
        public ICommand LoadStockCommand { get; private set; }
        public ICommand MakeTradeCommand { get; private set; }
        public IEnumerable<TradeTypeWrapper> TradeTypes { get; private set; }
        public decimal CurrentPrice { private set; get; }
        public decimal Cost
        {
            get { return CurrentPrice*_quantity; }
        }

        private int _quantity;
        public int Quantity
        {
            get { return _quantity; }
            set
            {
                _quantity = value;
                RaisePropertyChanged("Quantity");
                RaisePropertyChanged("Cost");
            }
        }

        private int _maximumQuantity;
        public int MaximumQuantity
        {
            set { 
                _maximumQuantity = value;
                RaisePropertyChanged("MaximumQuantity"); 
            }
            get 
            { 
                return _maximumQuantity;
            }
        }

        public string StockIndex { get; private set; }

        private TradeTypeWrapper _selectedTradeType;
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
                else
                {
                    MaximumQuantity = 8;
                }
            }
        }

        

        public PortfolioTradeViewModel()
        {
            LoadStockCommand = new RelayCommand<string>(LoadStock);
            MakeTradeCommand = new RelayCommand(MakeTrade);
            StockIndex = "INDX";

            // Setup the trade types drop list, with a string representation
            var tradeTypes = new List<TradeTypeWrapper>
                {
                    new TradeTypeWrapper(TradeType.Buy, "Buy"),
                    //new TradeTypeWrapper(TradeType.Sell, "Sell"),
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
            PortfolioService.Instance.AddTrade(GameState.Instance.MainPortfolioId, StockIndex, SelectedTradeType.TradeType, Quantity, GameState.Instance.GameTime);
            MessengerInstance.Send<object>(null, "NavigateBack");
        }

        public class TradeTypeWrapper
        {
            public readonly TradeType TradeType;
            private readonly string _displayName;

            public TradeTypeWrapper(TradeType tradeType, string displayName)
            {
                TradeType = tradeType;
                _displayName = displayName;
            }

            // ListPicker uses ToString to render an Item
            public override string ToString()
            {
                return _displayName;
            }
        }
    }
}
