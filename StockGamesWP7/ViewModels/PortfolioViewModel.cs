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
using StockGames.Views;

namespace StockGames.ViewModels
{
    /// <summary>   The PortfolioViewModel is used by the <see cref="PortfolioView" />. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    public class PortfolioViewModel : ViewModelBase
    {
        /// <summary>
        /// Loads the Portfolio into the ViewModel when executed. Can be used to refresh the data.
        /// </summary>
        ///
        /// <value> The load portfolio command. </value>
        public ICommand LoadPortfolioCommand { get; private set; }

        /// <summary>   Gets the name of the portfolio. </summary>
        ///
        /// <value> The name of the portfolio. </value>
        public string PortfolioName { get; private set; }

        /// <summary>   Gets the portfolio balance. </summary>
        ///
        /// <value> The portfolio balance. </value>
        public decimal PortfolioBalance { get; private set; }

        /// <summary>   Gets or sets the trades. </summary>
        ///
        /// <value> The trades. </value>
        public ObservableCollection<TradeEntity> Trades { get; private set; }

        private TradeEntity _selectedTrade;

        /// <summary>
        /// Gets or sets the currently selected trade. When this property changed the ViewStock view is
        /// displayed.
        /// </summary>
        ///
        /// <value> The selected trade. </value>
        public TradeEntity SelectedTrade 
        {
            get { return _selectedTrade; } 
            set
            {
                _selectedTrade = value;
                if (_selectedTrade != null) ViewStock();
            } 
        }

        /// <summary>   Default constructor. </summary>
        public PortfolioViewModel()
        {
            LoadPortfolioCommand = new RelayCommand(LoadPortfolio);
            Trades = new ObservableCollection<TradeEntity>();

            Messenger.Default.Register <PortfolioUpdatedMessageType>(this, message => LoadPortfolio());
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
