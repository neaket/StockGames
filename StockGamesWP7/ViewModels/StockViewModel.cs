﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using StockGames.Models;

namespace StockGames.ViewModels
{
    public class StockViewModel
    {
        public StockEntity Stock { get; set; }

        public StockViewModel(string stockIndex)
        {
            Stock = StocksManager.Instance.FindStock(stockIndex);
        }
    }
}