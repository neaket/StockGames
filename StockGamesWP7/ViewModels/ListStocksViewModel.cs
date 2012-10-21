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

namespace StockGames.ViewModels
{
    public class ListStocksViewModel
    {
        public ObservableCollection<StockEntity> Stocks { get; set; }

        public ListStocksViewModel()
        {
            // TEMP
            // TODO integrate code
            Stocks = new ObservableCollection<StockEntity>();

            Stocks.Add(new StockEntity()
            {
                CompanyName = "ABC Company",
                StockIndex = "ABC",
                CurrentPrice = 56M,
                PreviousPrice = 58M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "ABC Company",
                StockIndex = "ABC",
                CurrentPrice = 56M,
                PreviousPrice = 58M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "ABC Company",
                StockIndex = "ABC",
                CurrentPrice = 56M,
                PreviousPrice = 58M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "ABC Company",
                StockIndex = "ABC",
                CurrentPrice = 56M,
                PreviousPrice = 58M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "ABC Company",
                StockIndex = "ABC",
                CurrentPrice = 56M,
                PreviousPrice = 58M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "ABC Company",
                StockIndex = "ABC",
                CurrentPrice = 56M,
                PreviousPrice = 58M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "ABC Company",
                StockIndex = "ABC",
                CurrentPrice = 56M,
                PreviousPrice = 58M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "ABC Company",
                StockIndex = "ABC",
                CurrentPrice = 56M,
                PreviousPrice = 58M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "ABC Company",
                StockIndex = "ABC",
                CurrentPrice = 56M,
                PreviousPrice = 58M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "ABC Company",
                StockIndex = "ABC",
                CurrentPrice = 56M,
                PreviousPrice = 58M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "ABC Company",
                StockIndex = "ABC",
                CurrentPrice = 56M,
                PreviousPrice = 58M
            });

        }

    }
}
