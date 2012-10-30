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
using StockGames.Models;
using StockGames.Stubs;

namespace StockGames.Controllers
{
    public class ClientController
    {
        //Private variables
        private StocksManager _StockDataManager;
        private CommandInvoker _CmdInvoker;
        private MessageHandler _MsgHandler;

        public ClientController()
        {
            _StockDataManager = new StocksManager();
            _MsgHandler = new MessageHandler();
            _CmdInvoker = new CommandInvoker(_StockDataManager, _MsgHandler);
        }
    }
}
