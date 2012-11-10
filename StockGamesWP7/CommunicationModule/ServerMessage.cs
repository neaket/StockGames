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

namespace StockGames.CommunicationModule
{
    public class ServerMessage : IMessage
    {
        private int eventNumber;

        public ServerMessage(string message, int eventReference)
        {
            Message = message;
            eventNumber = eventReference;
        }

        public string Message
        {
            get;
            private set;
        }
    }
}
