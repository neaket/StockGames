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
using StockGames.Controllers;
using StockGames.Entities;
using StockGames.Persistence.V1.Services;

namespace StockGames.Commands
{
    public class PushStockChangesCmd : IStockCommand
    {
        //Private Variables
        private string _cmdname = CommandInvoker.CHANGE_STOCK_DATA;

        //IStockCommand Interface Implementation
        public string CommandName
        {
            get
            {
                return _cmdname;
            }
        }

        //ICommand Interface implemetation
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            throw new NotImplementedException();
        }

        //Override of the inherited Execute() method
        public void Execute(StockEntity stock)
        {
            StockEntity targetStock;
            try
            {
                targetStock = StockService.Instance.GetStock(stock.StockIndex);
            }
            catch (ArgumentException)
            {
                //stock not found, do not execture further
                return;
            }

            //targetStock.PreviousPrice = targetStock.CurrentPrice;
            //targetStock.CurrentPrice = stock.CurrentPrice;
        }
    }
}
