using System;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using StockGames.ViewModels;

namespace StockGames.Views
{
    public partial class StockView : PhoneApplicationPage
    {
        public StockView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameters = NavigationContext.QueryString;
            var stockIndex = parameters["StockIndex"];

            var vm = DataContext as StockViewModel;
            Debug.Assert(vm != null, "View Model Must Be Set");

            vm.LoadStockCommand.Execute(stockIndex);
        }

        private void Update_Click(object sender, EventArgs e)
        {
            var vm = DataContext as StockViewModel;
            Debug.Assert(vm != null, "View Model Must Be Set");

            vm.UpdateCommand.Execute(null);
        }
    }
}