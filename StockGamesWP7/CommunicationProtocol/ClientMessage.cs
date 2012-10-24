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

        public ClientMessage(int n, int m)
        {
            stockReference = n;
            stockValue = m;
        }

        public int getStockReference()
        {
            return stockReference;
        }

        public int getStockValue()
        {
            return stockValue;
        }

        private String StockName()
        {
            return "";
        }
    }
}
