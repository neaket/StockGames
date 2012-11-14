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
    public class ClientMessage : IMessage
    {

        public int EventTime
        {
            get;
            set;
        }

        public int StockValue
        {
            get;
            private set;
        }

        public int EventReference
        {
            get;
            set;
        }

        public ClientMessage(int time, int stockValue)
        {
            StockValue = stockValue;
            EventTime = time;
        }       
    }
}
