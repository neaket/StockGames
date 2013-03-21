using System.Diagnostics;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using StockGames.ViewModels;

namespace StockGames.Views
{
    /// <summary>   The portfolio view is used to display a portfolio on the GUI. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    public partial class PortfolioView : PhoneApplicationPage
    {
        /// <summary>   Initializes a new instance of the PortfolioView class. </summary>
        public PortfolioView()
        {
            InitializeComponent();
        }

        /// <summary>   Called when a page becomes the active page in a frame. </summary>
        ///
        /// <param name="e">    An object that contains the event data. </param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var vm = DataContext as PortfolioViewModel;
            Debug.Assert(vm != null, "View Model Must Be Set");

            vm.LoadPortfolioCommand.Execute(null);
        }
    }
}