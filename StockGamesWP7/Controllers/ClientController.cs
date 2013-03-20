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
using StockGames.CommunicationModule;

namespace StockGames.Controllers
{
    public class ClientController
    {
        //Private variables
        private CommandInvoker _CmdInvoker;
        private CommunicationManager _ServerComm;


        private static ClientController _Instance;
        public static ClientController Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new ClientController();
                }
                return _Instance;
            }
        }

        private ClientController()
        {
            _ServerComm = CommunicationManager.GetInstance; // TODO
            _CmdInvoker = CommandInvoker.Instance;
        }
    }
}
