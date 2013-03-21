using System;
using GalaSoft.MvvmLight.Messaging;
using Microsoft.Phone.Controls;

namespace StockGames.Views
{
    /// <summary>   The main menu view is the first screen that is displayed on the GUI. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    public partial class MainMenuView : PhoneApplicationPage
    {
        /// <summary>   Initializes a new instance of the MainMenuView class. </summary>
        public MainMenuView()
        {
            InitializeComponent();

            // Necessary for Page Navigation from the ViewModel.
            Messenger.Default.Register<Uri>(this, "Navigate",
                 uri => NavigationService.Navigate(uri));

            // Necessary for Back - Page Navigation from the ViewModel
            Messenger.Default.Register<object>(this, "NavigateBack",
                obj => NavigationService.GoBack());
        }
    }
}