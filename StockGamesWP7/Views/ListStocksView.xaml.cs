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
using StockGames.Models;

namespace StockGames.Views
{
    public partial class ListStocksView : PhoneApplicationPage
    {
        private ListStocksViewModel viewModel;
        public ListStocksView()
        {
            InitializeComponent();

            viewModel = new ListStocksViewModel();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            DataContext = viewModel;
            StockListBox.SelectedItem = null; // clear the current selection
        }

        private void StockListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count > 0)
            {
                StockEntity selected = e.AddedItems[0] as StockEntity;
                
                NavigationService.Navigate(new Uri("/Views/StockView.xaml?StockIndex=" + selected.StockIndex, UriKind.Relative));                
            }           
        }
    }
}