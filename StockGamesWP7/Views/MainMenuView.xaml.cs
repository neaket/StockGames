using System;
using System.ComponentModel;
using System.Windows;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using StockGames.Persistance.V1;
using StockGames.Persistance.V1.Migrations;
using StockGames.ViewModels;

namespace StockGames.Views
{
    public partial class MainMenuView : PhoneApplicationPage
    {
        public MainMenuView()
        {
            InitializeComponent();

            DataContext = new MainMenuViewModel();

            // Necessary for Page Navigation from the ViewModel.
            Messenger.Default.Register<Uri>(this, "Navigate",
                (uri) => NavigationService.Navigate(uri));
        }
    }
}