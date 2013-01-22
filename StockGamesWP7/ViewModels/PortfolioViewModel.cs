using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Persistence.V1.DataModel;
using StockGames.Persistence.V1.Services;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using StockGames.Persistence.V1;
using StockGames.Entities;

namespace StockGames.ViewModels
{
    public class PortfolioViewModel : ViewModelBase
    {
        public string PortfolioName { get; private set; }
        public decimal PortfolioBalance { get; private set; }
        
        public ObservableCollection<TradeEntity> Trades { get; set; }

        private TradeEntity _selectedTrade;
        public TradeEntity SelectedTrade 
        {
            get { return _selectedTrade; } 
            set
            {
                _selectedTrade = value;
                if (_selectedTrade != null) ViewStock();
            } 
        }

        public PortfolioViewModel()
        {
            int portfolioId = GameState.Instance.MainPortfolioId;
            
            var portfolio = PortfolioService.Instance.GetPortfolio(portfolioId);
            PortfolioName = portfolio.Name;
            PortfolioBalance = portfolio.Balance;

            Trades = new ObservableCollection<TradeEntity>(PortfolioService.Instance.GetTrades(portfolioId));
        }

        private void ViewStock()
        {
            var uri = new Uri("/Views/StockView.xaml?StockIndex=" + SelectedTrade.StockIndex, UriKind.Relative);
            Messenger.Default.Send(uri, "Navigate");
            SelectedTrade = null;
            RaisePropertyChanged("SelectedTrade");
        }
    }
}
