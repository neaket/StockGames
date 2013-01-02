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
using StockGames.Models;
using StockGames.Persistence.V1.DataModel;
using StockGames.Persistence.V1.Services;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using StockGames.Persistence.V1;

namespace StockGames.ViewModels
{
    public class PortfolioViewModel : ViewModelBase
    {
        public PortfolioModel Portfolio { get; set; } // TODO avoid accessing the service layer
        
        public ObservableCollection<PortfolioTradeModel> Trades { get; set; }

        private StockEntity _selectedTrade;
        public StockEntity SelectedTrade 
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
            Portfolio = PortfolioService.Instance.GetPortfolio(portfolioId);
            Trades = new ObservableCollection<PortfolioTradeModel>(PortfolioService.Instance.GetEntries(portfolioId).Cast<PortfolioTradeModel>());
        }

        private void ViewStock()
        {
            var uri = new Uri("/Views/StockView.xaml?EntryId=" + SelectedTrade.StockIndex, UriKind.Relative);
            Messenger.Default.Send(uri, "Navigate");
            SelectedTrade = null;
            RaisePropertyChanged("SelectedTrade");
        }
    }
}
