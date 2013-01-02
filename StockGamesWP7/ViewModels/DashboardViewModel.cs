using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace StockGames.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public ICommand ViewMarketCommand { get; private set; }
        public ICommand ViewPortfolioCommand { get; private set; }

        public DashboardViewModel()
        {
            ViewMarketCommand = new RelayCommand(ViewMarket);
            ViewPortfolioCommand = new RelayCommand(ViewPortfolio);
        }

        private void ViewMarket()
        {
            Messenger.Default.Send(new Uri("/Views/ListStocksView.xaml", UriKind.Relative), "Navigate");
        }

        private void ViewPortfolio()
        {
            Messenger.Default.Send(new Uri("/Views/PortfolioView.xaml", UriKind.Relative), "Navigate");
        }
    }
}
