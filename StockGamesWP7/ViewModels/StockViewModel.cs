using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Controllers;
using StockGames.Entities;
using StockGames.Messaging;
using StockGames.Persistence.V1.Services;
using StockGames.Views;

namespace StockGames.ViewModels
{
    /// <summary>   The StockViewModel is used by the <see cref="StockView" />. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    public class StockViewModel : ViewModelBase
    {
        /// <summary>   When the NewTradeCommand is executed, the <see cref="PortfolioTradeView"/> is displayed. </summary>
        ///
        /// <value> The new trade command. </value>
        public ICommand NewTradeCommand { get; private set; }

        /// <summary>
        /// When the LoadStockCommand is executed with [string stockIndex] as a parameter, the stock
        /// Stock data is loaded into this ViewModel.
        /// </summary>
        ///
        /// <value> The load stock command. </value>
        public ICommand LoadStockCommand { get; private set; }

        /// <summary>   Gets the stock. </summary>
        ///
        /// <value> The stock. </value>
        public StockEntity Stock { get; private set; }

        private PathGeometry _stockChartData = new PathGeometry();

        /// <summary>   Gets a Geometry object that is used to draw a broken line graph, the stock chart. </summary>
        ///
        /// <value> Information describing the broken line graph of the stock chart. </value>
        public Geometry StockChartData
        {
            get { return _stockChartData; }
        }

        private decimal _stockChartMax;

        /// <summary>   Gets the stock chart maximum cost. </summary>
        ///
        /// <value> The stock chart maximum. </value>
        public decimal StockChartMax { get { return _stockChartMax; } }
        private decimal _stockChartMiddle;

        /// <summary>   Gets the stock chart middle cost. </summary>
        ///
        /// <value> The stock chart middle cost. </value>
        public decimal StockChartMiddle { get { return _stockChartMiddle; } }
        private decimal _stockChartMin;

        /// <summary>   Gets the stock chart minimum cost. </summary>
        ///
        /// <value> The stock chart minimum cost. </value>
        public decimal StockChartMin { get { return _stockChartMin; } }

        /// <summary>   Initializes a new instance of the StockViewModel class. </summary>
        public StockViewModel()
        {
            LoadStockCommand = new RelayCommand<string>(LoadStock);
            NewTradeCommand = new RelayCommand(NewTrade);

            Messenger.Default.Register<StockUpdatedMessageType>(this, StockUpdated);
        }

        private void NewTrade()
        {
            var uri = new Uri("/Views/PortfolioTradeView.xaml?StockIndex=" + Stock.StockIndex, UriKind.Relative);
            Messenger.Default.Send(uri, "Navigate");
        }

        private void StockUpdated(StockUpdatedMessageType message)
        {
            if (Stock == null) return;
            if (message.StockIndex == Stock.StockIndex)
            {
                LoadStock(Stock.StockIndex);
            }
        }

        private void LoadStock(string stockIndex)
        {
            Stock = StockService.Instance.GetStock(stockIndex);
            
            var figure = new PathFigure();

            // TODO display the tombstone on the horizontal axis
            decimal startPrice;
            if (Stock.Snapshots.Count > 0)
            {
                startPrice = Stock.Snapshots[Stock.Snapshots.Count - 1].Price;
            }
            else
            {
                startPrice = 0;
            }
            
            Point start = new Point(0, -(double)startPrice);
            figure.StartPoint = start;
            _stockChartMax = startPrice;
            _stockChartMin = startPrice;

            for (int i = 1; i < Stock.Snapshots.Count; i++)
            {
                var segment = new LineSegment();
                int index = Stock.Snapshots.Count - 1 - i;
                var price = Stock.Snapshots[index].Price;

                if (_stockChartMax < price) _stockChartMax = price;
                if (_stockChartMin > price) _stockChartMin = price;

                segment.Point = new Point(i, -(double)price);
                figure.Segments.Add(segment);
            }

            _stockChartMiddle = (_stockChartMax + _stockChartMin) / 2;

            _stockChartData = new PathGeometry();
            _stockChartData.Figures.Clear();
            _stockChartData.Figures.Add(figure);

            RaisePropertyChanged("Stock");
            RaisePropertyChanged("StockChartMax");
            RaisePropertyChanged("StockChartMiddle");
            RaisePropertyChanged("StockChartMin");
            RaisePropertyChanged("StockChartData"); // NOTE: this property is throwing exceptions for multiple views, see TODO in ViewModelLocator.Stock
        }
    }
}
