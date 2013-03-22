using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Controllers;
using StockGames.Messaging;
using StockGames.Persistence.V1;
using StockGames.Views;

namespace StockGames.ViewModels
{
    /// <summary>   The DashboardViewModel is used by the <see cref="DashboardView" />. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    public class DashboardViewModel : ViewModelBase
    {
        /// <summary>   When the ViewMarketCommand is executed, the <see cref="ListStocksView"/> is displayed on the GUI.  </summary>
        ///
        /// <value> The view market command. </value>
        public ICommand ViewMarketCommand { get; private set; }

        /// <summary>   When the ViewPortfolioCommand is executed, the <see cref="PortfolioView" /> is displayed on the GUI. </summary>
        ///
        /// <value> The view portfolio command. </value>
        public ICommand ViewPortfolioCommand { get; private set; }

        /// <summary>   When the AdvanceTimeByHourCommand is executed, the game time is advanced by one hour. </summary>
        ///
        /// <value> The advance time by hour command. </value>
        public ICommand AdvanceTimeByHourCommand { get; private set; }

        /// <summary>   When the ViewPortfolioCommand is executed, the <see cref="ListMissionsView" /> is displayed on the GUI. </summary>
        ///
        /// <value> The view missions command. </value>
        public ICommand ViewMissionsCommand { get; private set; }

        /// <summary>   Gets the current game time. </summary>
        ///
        /// <value> The time of the game. </value>
        public DateTime GameTime { get; private set; }

        /// <summary>   Initializes a new instance of the DashboardViewModel class. </summary>
        ///
        /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
        public DashboardViewModel()
        {
            ViewMarketCommand = new RelayCommand(ViewMarket);
            ViewPortfolioCommand = new RelayCommand(ViewPortfolio);
            AdvanceTimeByHourCommand = new RelayCommand(AdvanceTimeByHour);
            ViewMissionsCommand = new RelayCommand(ViewMissions);
            GameTime = GameState.Instance.GameTime;

            Messenger.Default.Register<GameTimeUpdatedMessageType>(this, message =>
                {
                    GameTime = message.GameTime;
                    RaisePropertyChanged("GameTime");
                });
        }

        private void ViewMarket()
        {
            Messenger.Default.Send(new Uri("/Views/ListStocksView.xaml", UriKind.Relative), "Navigate");
        }

        private void ViewPortfolio()
        {
            Messenger.Default.Send(new Uri("/Views/PortfolioView.xaml", UriKind.Relative), "Navigate");
        }

        private void AdvanceTimeByHour()
        {
            TimeController.Instance.AdvanceTimeByHour();
        }

        private void ViewMissions()
        {
            Messenger.Default.Send(new Uri("/Views/ListMissionsView.xaml", UriKind.Relative), "Navigate");
        }
    }
}
