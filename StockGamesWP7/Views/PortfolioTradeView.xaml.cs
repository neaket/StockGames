using System.Diagnostics;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using StockGames.ViewModels;

namespace StockGames.Views
{
    /// <summary>
    /// The portfolio trade view is used to display the portfolio trades on the GUI.
    /// </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    public partial class PortfolioTradeView : PhoneApplicationPage
    {
        /// <summary>   Initializes a new instance of the PortfolioTradeView class. </summary>
        public PortfolioTradeView()
        {
            InitializeComponent();
        }

        /// <summary>   Called when a page becomes the active page in a frame. </summary>
        ///
        /// <param name="e">    An object that contains the event data. </param>
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