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

namespace StockGames.CommunicationProtocol
{
    public class ClientMessage : Message
    {
        private int stockReference;
        private int stockValue;
        private int eventReference;

        public ClientMessage(int reference, int value, int eventReference)
        {
            stockReference = reference;
            stockValue = value;
            eventReference = eventReference;
        }

        public int GetStockReference()
        {
            return stockReference;
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
