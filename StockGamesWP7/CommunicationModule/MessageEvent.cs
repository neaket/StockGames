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
    public class MessageEvent
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

        public bool IsSent
        {
            get;
            set;
        }

        public bool IsAnswered
        {
            get;
            private set;
        }

        public int EventNumber
        {
            get;
            set;
        }

        public MessageEvent(int stockReference, int stockValue) 
        {
            StockReference = stockReference;
            StockValue = stockValue;
        }
    }
}
