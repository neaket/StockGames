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
using StockGames.Persistence.V1.Services;
using StockGames.Stubs;
using StockGames.Entities;
using StockGames.Controllers;
using StockGames.CommunicationModule;

namespace StockGames.Commands
{
    public class RequestStockUpdateCmd : IStockCommand
    {
        //Private Variables
        private string _cmdName = CommandInvoker.REQUEST_UPDATE_STOCK;
        
        public RequestStockUpdateCmd(MessageHandler msgHandler)
        {
        }

        //ICommand Interface implementation
        public void Execute(Object o)
        {
            var stockIndex = o as string;
            if (stockIndex == null)
            {
                throw new ArgumentException("Object is not a stock Entity");
            }

            CommunicationManager ServerComm = CommunicationManager.GetInstance;
            ServerComm.requestStockUpdate(stockIndex);    
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;

        //IStockCommand interface implementation

        public string CommandName
        {
            get
            {
                return _cmdName;
            }
        }
    }
}
