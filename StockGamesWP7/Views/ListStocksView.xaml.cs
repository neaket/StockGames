using System;
using System.Diagnostics;
using System.Windows.Controls;
using Microsoft.Phone.Controls;
using StockGames.ViewModels;
using System.Windows.Navigation;
using StockGames.Models;

namespace StockGames.Views
{
    public partial class ListStocksView : PhoneApplicationPage
    {
        private ListStocksViewModel _viewModel;

        public ListStocksView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            _viewModel = new ListStocksViewModel();
            DataContext = _viewModel;
            StockListBox.SelectedItem = null; // clear the current selection
        }

        private void StockListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.AddedItems.Count <= 0) return;
            var selected = e.AddedItems[0] as StockEntity;

            Debug.Assert(selected != null, "A stock must be selected");

            NavigationService.Navigate(new Uri("/Views/StockView.xaml?StockIndex=" + selected.StockIndex, UriKind.Relative));
        }
    }
}