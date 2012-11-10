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
    public class ClientMessage : Message
    {
        private int stockValue;
        private int eventReference;

        public ClientMessage(int reference, int value, int eventRef)
        {
            StockReference = reference;
            stockValue = value;
            eventReference = eventRef;
        }

        public int StockReference
        {
            get;
            private set;
        }

        public int GetStockValue()
        {
            return stockValue;
        }

        public int GetEventReference()
        {
            return eventReference;
        }

        private String StockName()
        {
            return "";
        }
    }
}
