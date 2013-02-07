using System;
using System.Diagnostics;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Controllers;
using StockGames.Entities;

namespace StockGames.ViewModels
{
    public class StockViewModel : ViewModelBase
    {
        public ICommand UpdateCommand { get; private set; }
        public ICommand NewTradeCommand { get; private set; }
        public ICommand LoadStockCommand { get; private set; }

        public StockEntity Stock { get; private set; }

        public StockViewModel()
        {
            LoadStockCommand = new RelayCommand<string>(LoadStock);
            UpdateCommand = new RelayCommand(Update);
            NewTradeCommand = new RelayCommand(NewTrade);
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

        private void LoadStock(string stockIndex)
        {
            Stock = StockManager.Instance.GetStock(stockIndex);
        }
    }
}
