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
using StockGames.Models;
using StockGames.Persistence.V1.Services;
using System.Windows.Threading;
using GalaSoft.MvvmLight.Threading;

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
            var stock = parameter as StockEntity;
            if (stock == null)
            {
                throw new ArgumentException("Object is not a stock Entity");
            }
            
            DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    var guiStock = StockManager.Instance.GetStock(stock.StockIndex);
                    guiStock.CurrentPrice = stock.CurrentPrice;
                    guiStock.PreviousPrice = stock.PreviousPrice;
                    StockService.Instance.AddStockSnapshot(stock);
                }
            );                        
        }
    }
}
