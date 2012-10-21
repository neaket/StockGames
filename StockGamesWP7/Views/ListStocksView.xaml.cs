using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using StockGames.ViewModels;
using System.Windows.Navigation;

namespace StockGames.Views
{
    public partial class ListStocksView : PhoneApplicationPage
    {
        private ListStocksViewModel viewModel;
        public ListStocksView()
        {
            InitializeComponent();

            viewModel = new ListStocksViewModel();
            // temp
            // TODO 
          //  LayoutRoot.DataContext = new StockViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = viewModel;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/StockView.xaml", UriKind.Relative));
            
        }
    }
}