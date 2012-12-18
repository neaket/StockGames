using System;
using System.Diagnostics;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections;
using StockGames.MVVM;
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
        public ICommand RefreshCommand { get; private set; }
        private ICommand ViewStockCommand { get; set; }

        public StockEntity SelectedStock { get { return null; } set { if (value != null) ViewStockCommand.Execute(value); } }

        public ListStocksViewModel(ICommand viewStockCommand)
        {
            if (viewStockCommand == null) 
                throw new ArgumentNullException("viewStockCommand");

            RefreshCommand = new RelayCommand(param => LoadStocks());
            ViewStockCommand = viewStockCommand;
            Stocks = new ObservableCollection<StockEntity>();
            LoadStocks();
        }

        private void LoadStocks()
        {
            Stocks.Clear();

            foreach (StockEntity stock in StocksManager.Instance.GetStocks())
            {
                Stocks.Add(stock);
            }
        }
    }
}
