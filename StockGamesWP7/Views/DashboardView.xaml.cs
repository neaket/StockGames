﻿using System;
using GalaSoft.MvvmLight.Command;
using Microsoft.Phone.Controls;
using StockGames.ViewModels;

namespace StockGames.Views
{
    public partial class DashboardView : PhoneApplicationPage
    {
        public DashboardView()
        {
            InitializeComponent();

            DataContext = new DashboardViewModel(); 
        }
    }
}