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
                CurrentPrice = 58.8M,
                PreviousPrice = 58.17M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "One Company",
                StockIndex = "ONE",
                CurrentPrice = 51.6M,
                PreviousPrice = 51.8M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "Ninja Corp",
                StockIndex = "NINJ",
                CurrentPrice = 7.1M,
                PreviousPrice = 6M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "Zombie Software",
                StockIndex = "BRAI",
                CurrentPrice = 121M,
                PreviousPrice = 82M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "Sword Construction Company",
                StockIndex = "SWRD",
                CurrentPrice = 1234M,
                PreviousPrice = 1231M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "Family Farms",
                StockIndex = "FARM",
                CurrentPrice = 400M,
                PreviousPrice = 391M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "Pickles To Go",
                StockIndex = "PICK",
                CurrentPrice = 132M,
                PreviousPrice = 156M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "Delicious Soft Drinks Company",
                StockIndex = "DELI",
                CurrentPrice = 101M,
                PreviousPrice = 99M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "Survival Weapons",
                StockIndex = "SURV",
                CurrentPrice = 51.1M,
                PreviousPrice = 50.1M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "News For You",
                StockIndex = "NEWS",
                CurrentPrice = 123.1M,
                PreviousPrice = 123.5M
            });
            Stocks.Add(new StockEntity()
            {
                CompanyName = "Planet Red",
                StockIndex = "RED",
                CurrentPrice = 82.23M,
                PreviousPrice = 85.1M
            });

        }

    }
}
