using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using StockGames.MVVM;

namespace StockGames.ViewModels
{
    public class DashboardViewModel
    {
        public RelayCommand ViewMarketCommand { get; private set; }

        public DashboardViewModel(RelayCommand viewMarketCommand)
        {
            ViewMarketCommand = viewMarketCommand;
        }

        
    }
}
