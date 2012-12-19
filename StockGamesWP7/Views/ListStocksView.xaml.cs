using System;
using System.Diagnostics;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using Microsoft.Phone.Controls;
using StockGames.MVVM;
using StockGames.ViewModels;
using System.Windows.Navigation;
using StockGames.Models;

namespace StockGames.Views
{
    public partial class ListStocksView : PhoneApplicationPage
    {
        private readonly ListStocksViewModel _viewModel;

        public ListStocksView()
        {
            InitializeComponent();

            _viewModel = new ListStocksViewModel(new RelayCommand(param =>
                {
                    Debug.Assert(param is StockEntity);
                    ViewStock(param as StockEntity);
                }));

            DataContext = _viewModel;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            _viewModel.RefreshCommand.Execute(null);
            StockListBox.Visibility = Visibility.Visible;
            StockListBox.SelectedItem = _viewModel.SelectedStock;
        }

        private void ViewStock(StockEntity stockEntity)
        {
            NavigationService.Navigate(new Uri("/Views/StockView.xaml?StockIndex=" + stockEntity.StockIndex,
                                               UriKind.Relative));
        }
    }

    public class PositiveToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value is decimal);
            decimal from = (decimal) value;

            if (from > 0)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
    public class NegativeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            Debug.Assert(value is decimal);
            decimal from = (decimal)value;

            if (from < 0)
            {
                return Visibility.Visible;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    
}