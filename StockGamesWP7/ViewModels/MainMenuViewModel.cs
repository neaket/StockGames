﻿using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Persistence.V1;
using StockGames.Persistence.V1.Migrations;

namespace StockGames.ViewModels
{
    public class MainMenuViewModel : ViewModelBase
    {
        private Visibility _continueVisibility;
        public Visibility ContinueVisibility { 
            get { return _continueVisibility; }
            private set 
            { 
                _continueVisibility = value;
                RaisePropertyChanged("ContinueVisibility");
            } 
        }
        public ICommand ContinueGameCommand { get; private set; }
        public ICommand NewGameCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        private bool _showProgressBar;
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


        public MainMenuViewModel()
        {
            ContinueVisibility = GameSettings.Instance.ExistingGame ? Visibility.Visible : Visibility.Collapsed;
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
            ContinueVisibility = Visibility.Visible;
            ViewDashboard();
        }
    }
}