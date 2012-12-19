using System;
using System.ComponentModel;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Persistance.V1;
using StockGames.Persistance.V1.Migrations;

namespace StockGames.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        public Visibility ContinueVisiblity { get; private set; }
        public ICommand ContinueGameCommand { get; private set; }
        public ICommand NewGameCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        private bool showProgressBar;
        public bool ShowProgressBar { 
            get
            {
                return showProgressBar;
            } 
            set { 
                showProgressBar = value;
                RaisePropertyChanged("ShowProgressBar");
            }
        }

        public MainMenuViewModel()
        {
            ContinueVisiblity = GameSettings.Instance.ExistingGame ? Visibility.Visible : Visibility.Collapsed;

            ContinueGameCommand = new RelayCommand(ViewDashboard);
            NewGameCommand = new RelayCommand(NewGame);
            AboutCommand = new RelayCommand(ViewAbout);
            ShowProgressBar = false;
        }

        private void ViewDashboard()
        {
            Messenger.Default.Send<Uri>(new Uri("/Views/DashboardView.xaml", UriKind.Relative), "Navigate");
        }

        private void ViewAbout()
        {
            Messenger.Default.Send<Uri>(new Uri("/Views/AboutView.xaml", UriKind.Relative), "Navigate");
        }

        private void NewGame()
        {
            if (GameSettings.Instance.ExistingGame)
            {
                var result =
                    MessageBox.Show("Are you sure you want to start a new game?  Your existing game will be deleted.", "Confirm", MessageBoxButton.OKCancel);
                if (result != MessageBoxResult.OK) return;
            }
            ShowProgressBar = true;

            var newGameWorker = new BackgroundWorker();

            newGameWorker.DoWork += NewGameWorker_DoWork;
            newGameWorker.RunWorkerAsync();
            newGameWorker.RunWorkerCompleted += NewGameWorker_RunWorkerCompleted;
        }

        private void NewGameWorker_DoWork(object sender, DoWorkEventArgs doWorkEventArgs)
        {
            MigrationManager.IfExistsRemoveDatabase();
            MigrationManager.InitializeDatabase();

            GameSettings.Instance.ExistingGame = true;
            GameSettings.Instance.Save();
        }

        void NewGameWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ShowProgressBar = false;
            ViewDashboard();
        }
    }
}
