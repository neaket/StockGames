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
using System.Collections;
using StockGames.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using StockGames.Controllers;
using StockGames.Persistance.V1.Services;

namespace StockGames.ViewModels
{
    public class ListStocksViewModel
    {
        public ObservableCollection<StockEntity> Stocks { get; set; }

        public ListStocksViewModel()
        {
            Stocks = new ObservableCollection<StockEntity>();

            foreach (StockEntity stock in StocksManager.Instance.GetStocks())
            {
                Stocks.Add(stock);
            }
        }
    }
}
