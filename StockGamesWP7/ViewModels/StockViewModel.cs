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

namespace StockGames.ViewModels
{
    public class StockViewModel : ViewModelBase
    {
        public ICommand UpdateCommand { get; private set; }
        public ICommand NewTradeCommand { get; private set; }
        public ICommand LoadStockCommand { get; private set; }

        public StockEntity Stock { get; private set; }

        private PathGeometry _stockChartData;
        public Geometry StockChartData
        {
            get { return _stockChartData; }
        }

        private decimal _stockChartMax;
        public decimal StockChartMax { get { return _stockChartMax; } }
        private decimal _stockChartMiddle;
        public decimal StockChartMiddle { get { return _stockChartMiddle; } }
        private decimal _stockChartMin;
        public decimal StockChartMin { get { return _stockChartMin; } }
                   

        public StockViewModel()
        {
            LoadStockCommand = new RelayCommand<string>(LoadStock);
            UpdateCommand = new RelayCommand(Update);
            NewTradeCommand = new RelayCommand(NewTrade);

            Messenger.Default.Register<StockUpdatedMessageType>(this, StockUpdated);
        }

        private void Update()
        {
            CommandInvoker.Instance.FetchCommand(CommandInvoker.REQUEST_UPDATE_STOCK, Stock);
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

            // TODO use the tombstone...
            var startPrice = Stock.Snapshots[Stock.Snapshots.Count - 1].Price;
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
            _stockChartData.Figures.Add(figure);

            RaisePropertyChanged("Stock");
            RaisePropertyChanged("StockChartMax");
            RaisePropertyChanged("StockChartMiddle");
            RaisePropertyChanged("StockChartMin");
            RaisePropertyChanged("StockChartData");

        }
    }
}
