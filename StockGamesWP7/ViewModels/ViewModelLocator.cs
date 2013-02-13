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

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}

            SimpleIoc.Default.Register<DashboardViewModel>();
            SimpleIoc.Default.Register<StockViewModel>();
            SimpleIoc.Default.Register<ListStocksViewModel>();
            SimpleIoc.Default.Register<PortfolioViewModel>();
            SimpleIoc.Default.Register<PortfolioTradeViewModel>();
            SimpleIoc.Default.Register<ListMissionsViewModel>();
            SimpleIoc.Default.Register<MissionViewModel>();
        }

        public DashboardViewModel Dashboard
        {
            get
            {
                return ServiceLocator.Current.GetInstance<DashboardViewModel>();
            }
        }

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

        public ListStocksViewModel ListStocks
        {
            get
            {
                return ServiceLocator.Current.GetInstance<ListStocksViewModel>();
            }
        }

        public PortfolioViewModel Portfolio
        {
            get
            {
                return ServiceLocator.Current.GetInstance<PortfolioViewModel>();
            }
        }

        public PortfolioTradeViewModel PortfolioTrade
        {
            get 
            { 
                return ServiceLocator.Current.GetInstance<PortfolioTradeViewModel>();
            }
        }

        public ListMissionsViewModel ListMissions
        {
            get { return ServiceLocator.Current.GetInstance<ListMissionsViewModel>(); }
        }

        public MissionViewModel Mission
        {
            get { return ServiceLocator.Current.GetInstance<MissionViewModel>(); }
        }

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}