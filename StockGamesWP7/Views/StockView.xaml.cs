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
using System.Windows.Navigation;
using StockGames.Models;
using StockGames.ViewModels;

namespace StockGames.Views
{
    public partial class StockView : PhoneApplicationPage
    {
        public StockView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            IDictionary<string, string> parameters = this.NavigationContext.QueryString;

            StockViewModel viewModel = new StockViewModel(parameters["StockIndex"]);

            DataContext = viewModel;
        }
    }
}