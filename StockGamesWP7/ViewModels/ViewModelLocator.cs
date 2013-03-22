/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:StockGames"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;

namespace StockGames.ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<MainMenuViewModel>();
            SimpleIoc.Default.Register<DashboardViewModel>();
            SimpleIoc.Default.Register<StockViewModel>();
            SimpleIoc.Default.Register<ListStocksViewModel>();
            SimpleIoc.Default.Register<PortfolioViewModel>();
            //  SimpleIoc.Default.Register<PortfolioTradeViewModel>();
            SimpleIoc.Default.Register<ListMissionsViewModel>();
            SimpleIoc.Default.Register<MissionViewModel>();
        }

        /// <summary>   Gets an instance of the MainMenuViewModel. </summary>
        ///
        /// <value> The MainMenuViewModel. </value>
        public MainMenuViewModel MainMenu
        {
            get
            {
                return ServiceLocator.Current.GetInstance<MainMenuViewModel>();
            }
        }

        /// <summary>   Gets an instance of the DashboardViewModel. </summary>
        ///
        /// <value> The DashboardViewModel. </value>
        public DashboardViewModel Dashboard
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DashboardViewModel>();
            }
        }

        /// <summary>   Gets an instance of the StockViewModel. </summary>
        ///
        /// <value> The StockViewModel. </value>
        public StockViewModel Stock
        {
            get
            {
                // Temporary fix for RaisePropertyChanged("StockChartData"); not working for two separate views
                // TODO revert back
                //return ServiceLocator.Current.GetInstance<StockViewModel>();
                return new StockViewModel();
            }
        }

        /// <summary>   Gets an instance of the ListStocksViewModel. </summary>
        ///
        /// <value> The ListStocksViewModel. </value>
        public ListStocksViewModel ListStocks
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ListStocksViewModel>();
            }
        }

        /// <summary>   Gets an instance of the PortfolioViewModel. </summary>
        ///
        /// <value> The PortfolioViewModel. </value>
        public PortfolioViewModel Portfolio
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PortfolioViewModel>();
            }
        }

        /// <summary>    Gets an instance of the PortfolioTradeViewModel. </summary>
        ///
        /// <value> The PortfolioTradeViewModel. </value>
        public PortfolioTradeViewModel PortfolioTrade
        {
            get 
            { 
                return new PortfolioTradeViewModel();
            }
        }

        /// <summary>    Gets an instance of the ListMissionsViewModel. </summary>
        ///
        /// <value> The ListMissionsViewModel. </value>
        public ListMissionsViewModel ListMissions
        {
            get { return ServiceLocator.Current.GetInstance<ListMissionsViewModel>(); }
        }

        /// <summary>   Gets an instance of the MissionViewModel. </summary>
        ///
        /// <value> The MissionViewModel. </value>
        public MissionViewModel Mission
        {
            get { return ServiceLocator.Current.GetInstance<MissionViewModel>(); }
        }
    }
}