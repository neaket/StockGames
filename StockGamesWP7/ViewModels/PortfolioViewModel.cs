using System;
using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Messaging;
using StockGames.Persistence.V1.Services;
using StockGames.Persistence.V1;
using StockGames.Entities;
using System.Windows.Input;

namespace StockGames.ViewModels
{
    public class PortfolioViewModel : ViewModelBase
    {
        public ICommand ViewPortfolioCommand { get; private set; }
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
            ViewPortfolioCommand = new RelayCommand(LoadPortfolio);
            Trades = new ObservableCollection<TradeEntity>();

            Messenger.Default.Register <PortfolioUpdatedMessageType>(this, (message) => LoadPortfolio());
        }

        private void LoadPortfolio()
        {
            int portfolioId = GameState.Instance.MainPortfolioId;
            var portfolio = PortfolioService.Instance.GetPortfolio(portfolioId);
            PortfolioName = portfolio.Name;
            PortfolioBalance = portfolio.Balance;
            RaisePropertyChanged("PortfolioBalance");

            Trades.Clear();
            foreach (var trade in PortfolioService.Instance.GetGroupedTrades(portfolioId))
            {
                Trades.Add(trade);
            }
            
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
