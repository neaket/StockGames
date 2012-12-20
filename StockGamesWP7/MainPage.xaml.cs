﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Xml;
using System.Text;
using System.IO;
using StockGames.CommunicationModule;

namespace StockGames
{
    public partial class MainPage : PhoneApplicationPage
    {
        ServerCommunication com;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            com = ServerCommunication.GetInstance;
        }

        private void ViewStocks_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ListStocksView.xaml", UriKind.Relative));
            com.StartSimulation();
        }
    }
}