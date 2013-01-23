using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Persistence.V1.Services;
using StockGames.Persistence.V1;
using StockGames.Entities;

namespace StockGames.ViewModels
{
    public class PortfolioViewModel : ViewModelBase
    {
        public string PortfolioName { get; private set; }
        public decimal PortfolioBalance { get; private set; }
        
        public ObservableCollection<TradeEntity> Trades { get; private set; }

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
