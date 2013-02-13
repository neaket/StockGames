using System;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;
using StockGames.ViewModels;

namespace StockGames.Views
{
    public partial class MainMenuView : PhoneApplicationPage
    {
        public MainMenuView()
        {
            InitializeComponent();

            // Necessary for Page Navigation from the ViewModel.
            Messenger.Default.Register<Uri>(this, "Navigate",
                (uri) => NavigationService.Navigate(uri));

            // Necessary for Back - Page Navigation from the ViewModel
            Messenger.Default.Register<object>(this, "NavigateBack",
                (obj) => NavigationService.GoBack());
        }
    }
}