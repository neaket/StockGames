using System;
using System.Diagnostics;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using StockGames.ViewModels;

namespace StockGames.Views
{
    /// <summary>   A GUI view that displays detailed information of a stock </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    public partial class StockView : PhoneApplicationPage
    {
        /// <summary>   Initializes a new instance of the StockView class. </summary>
        ///
        /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
        public StockView()
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

            var vm = DataContext as StockViewModel;
            Debug.Assert(vm != null, "View Model Must Be Set");

            vm.LoadStockCommand.Execute(stockIndex);
        }

        /// <summary>   Event handler. Called by Update for click events. </summary>
        ///
        /// <param name="sender">   Source of the event. </param>
        /// <param name="e">        An object that contains the event data. </param>
        private void Update_Click(object sender, EventArgs e)
        {
            var vm = DataContext as StockViewModel;
            Debug.Assert(vm != null, "View Model Must Be Set");

            vm.UpdateCommand.Execute(null);
        }

        private void NewTrade_Click(object sender, EventArgs e)
        {
            var vm = DataContext as StockViewModel;
            Debug.Assert(vm != null, "View Model Must Be Set");

            vm.NewTradeCommand.Execute(null);
        }
    }
}