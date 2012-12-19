using System;
using System.Windows;
using Microsoft.Phone.Controls;
using StockGames.Persistance.V1.Migrations;

namespace StockGames
{
    public partial class MainPage : PhoneApplicationPage
    {
        private static bool _existingGame = false; // TODO this needs to be persisted

        // Constructor
        public MainPage()
        {
            InitializeComponent();            
        }

        protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            ContinueBtn.Visibility = _existingGame ? Visibility.Visible : Visibility.Collapsed;
            base.OnNavigatedTo(e);
        }

        private void About_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/AboutView.xaml", UriKind.Relative));
        }

        private void NewGame_Click(object sender, RoutedEventArgs e)
        {
            if (_existingGame) { 
                var result =
                    MessageBox.Show("Are you sure you want to start a new game?  Your existing game will be deleted.", "Confirm", MessageBoxButton.OKCancel );
                if (result != MessageBoxResult.OK) return;
            }
            MigrationManager.IfExistsRemoveDatabase();
            MigrationManager.InitializeDatabase();

            _existingGame = true;
            ViewDashboard();
        }

        private void ContinueGame_Click(object sender, RoutedEventArgs e)
        {
            ViewDashboard();             
        }

        private void ViewDashboard()
        {
            NavigationService.Navigate(new Uri("/Views/DashboardView.xaml", UriKind.Relative));             
        }
    }
}