using System;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;

namespace StockGames.ViewModels
{
    public class DashboardViewModel : ViewModelBase
    {
        public RelayCommand ViewMarketCommand { get; private set; }

        public DashboardViewModel()
        {
            ViewMarketCommand = new RelayCommand(ViewMarket);
        }

        private void ViewMarket()
        {
            Messenger.Default.Send<Uri>(new Uri("/Views/ListStocksView.xaml", UriKind.Relative), "Navigate");
        }
    }
}
