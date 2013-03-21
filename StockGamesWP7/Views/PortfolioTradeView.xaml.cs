using System.Diagnostics;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using StockGames.ViewModels;

namespace StockGames.Views
{
    public partial class PortfolioTradeView : PhoneApplicationPage
    {
        public PortfolioTradeView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameters = NavigationContext.QueryString;
            var stockIndex = parameters["StockIndex"];

            var vm = DataContext as PortfolioTradeViewModel;
            Debug.Assert(vm != null, "View Model Must Be Set");

            vm.LoadStockCommand.Execute(stockIndex);
        }
    }
}