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

        public int StockReference
        {
            get;
            private set;
        }

        public int StockValue
        {
            get;
            private set;
        }

        public int EventReference
        {
            get;
            private set;
        }

        public ClientMessage(int stockReference, int stockValue, int eventReference)
        {
            StockReference = stockReference;
            StockValue = stockValue;
            EventReference = eventReference;
        }       
    }
}
