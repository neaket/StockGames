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
using StockGames.Stubs;
using StockGames.Models;
using StockGames.Controllers;
using StockGames.CommunicationModule;

namespace StockGames.Commands
{
    public class RequestStockUpdateCmd : IStockCommand
    {
        //Private Variables
        private StocksManager _stocksMnger;
        private string _cmdName = CommandInvoker.REQUEST_UPDATE_STOCK;
        
        public RequestStockUpdateCmd(MessageHandler msgHandler, StocksManager stocksMnger)
        {
            _stocksMnger = stocksMnger;
        }

        //ICommand Interface implementation
        public void Execute(Object o)
        {
            if (!(o is StockEntity))
            {
                throw new ArgumentException("Object is not a stock Entity");
            }
        }

        public void Execute(StockEntity stock)
        {
            ServerCommunication ServerComm = ServerCommunication.GetInstance;
            ServerComm.StartSimulation(stock);
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
