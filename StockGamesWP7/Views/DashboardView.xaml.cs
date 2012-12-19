using System;
using Microsoft.Phone.Controls;
using StockGames.MVVM;
using StockGames.ViewModels;

namespace StockGames.Views
{
    public partial class DashboardView : PhoneApplicationPage
    {
        public DashboardView()
        {
            InitializeComponent();

            var viewMarketCommand = new RelayCommand(param => ViewMarket());

            var viewModel = new DashboardViewModel(viewMarketCommand);
            DataContext = viewModel;
        }

        private void ViewMarket()
        {
            NavigationService.Navigate(new Uri("/Views/ListStocksView.xaml", UriKind.Relative));  
        }
    }
}