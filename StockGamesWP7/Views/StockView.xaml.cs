using System;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using StockGames.ViewModels;

namespace StockGames.Views
{
    public partial class StockView : PhoneApplicationPage
    {
        StockViewModel _viewModel;

        public StockView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameters = NavigationContext.QueryString;
                        
            _viewModel = new StockViewModel(parameters["StockIndex"]);

            DataContext = _viewModel;
        }

        private void Update_Click(object sender, EventArgs e)
        {
            Debug.Assert(_viewModel != null, "View Model Must Be Set");

            _viewModel.UpdateCommand.Execute(null);
        }
    }
}