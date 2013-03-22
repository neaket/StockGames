using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Persistence.V1;
using StockGames.Persistence.V1.Migrations;
using StockGames.Controllers;
using StockGames.Views;

namespace StockGames.ViewModels
{
    /// <summary>   The MainMenuViewModel is used by the <see cref="MainMenuView" />. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class MainMenuViewModel : ViewModelBase
    {
        private Visibility _continueVisibility;

        /// <summary>
        /// Gets the Visibility indicating whether a Continue Button should be visible on the GUI.
        /// </summary>
        ///
        /// <value> The continue visibility. </value>
        public Visibility ContinueVisibility { 
            get { return _continueVisibility; }
            private set 
            { 
                _continueVisibility = value;
                RaisePropertyChanged("ContinueVisibility");
            } 
        }

        /// <summary>
        /// When the ContinueGameCommand is executed, the game is continued and the
        /// <see cref="DashboardView"/> is displayed on the GUI.
        /// </summary>
        ///
        /// <value> The continue game command. </value>
        public ICommand ContinueGameCommand { get; private set; }

        /// <summary>
        /// When the NewGameCommand is executed, a new game is created and the
        /// <see cref="DashboardView"/> is displayed on the GUI.
        /// </summary>
        ///
        /// <value> The new game command. </value>
        public ICommand NewGameCommand { get; private set; }

        /// <summary>
        /// When the AboutCommand is executed, the
        /// <see cref="AboutView"/> is displayed on the GUI.
        /// </summary>
        ///
        /// <value> The about command. </value>
        public ICommand AboutCommand { get; private set; }
        private bool _showProgressBar;

        /// <summary>   Gets a boolean value indicating whether the progress bar is displayed on the GUI. </summary>
        ///
        /// <value> true if the progress bar should be visible, false if not. </value>
        public bool ShowProgressBar { 
            get
            {
                return _showProgressBar;
            } 
            set { 
                _showProgressBar = value;
                RaisePropertyChanged("ShowProgressBar");
            }
        }

        /// <summary>   Initializes a new instance of the MainMenuViewModel class. </summary>
        ///
        /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
        public MainMenuViewModel()
        {
            ContinueVisibility = GameState.Instance.ExistingGame ? Visibility.Visible : Visibility.Collapsed;
            ShowProgressBar = false;
            ContinueGameCommand = new RelayCommand(ViewDashboard);
            NewGameCommand = new RelayCommand(NewGame);
            AboutCommand = new RelayCommand(ViewAbout);
        }

        private void ViewDashboard()
        {
            Messenger.Default.Send(new Uri("/Views/DashboardView.xaml", UriKind.Relative), "Navigate");
        }

        private void ViewAbout()
        {
            Messenger.Default.Send(new Uri("/Views/AboutView.xaml", UriKind.Relative), "Navigate");
        }

        // Creates a new game, enables the progress bar. Displays the DashboardView when completed.
        private void NewGame()
        {
            if (GameState.Instance.ExistingGame)
            {
                var result =
                    MessageBox.Show("Are you sure you want to start a new game?  Your existing game will be deleted.", "Confirm", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK) return;
            }
            ShowProgressBar = true;

            //needs to run on UI thread
            var now = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Hour, 0, 0);
            GameState.Instance.GameDataExpiryTime = now.AddHours(-1);
            GameState.Instance.GameTime = now;

            var newGameWorker = new BackgroundWorker();

            newGameWorker.DoWork += NewGameWorker_DoWork;
            newGameWorker.RunWorkerAsync();
            newGameWorker.RunWorkerCompleted += NewGameWorker_RunWorkerCompleted;
        }

        // Completes computational intensive work on a background worke.
        private void NewGameWorker_DoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            MigrationManager.IfExistsRemoveDataContext();
            MigrationManager.InitializeDataContext();

            GameState.Instance.ExistingGame = true;
        }

        // Display the DashBoard when the background worker is completed.
        void NewGameWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ShowProgressBar = false;
            ContinueVisibility = Visibility.Visible;

            MissionController mc = MissionController.Instance;

            ViewDashboard();
        }
    }
}
