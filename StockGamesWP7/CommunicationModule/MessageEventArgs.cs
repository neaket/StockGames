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
    public class MessageEventArgs :EventArgs
    {
        private int currentEventTime;
        private int inputStockValue;
        private int eventReference;

        public MessageEventArgs(int eventTime, int stockValue)
        {
            currentEventTime = eventTime;
            inputStockValue = stockValue;
        }

        public int EventTime
        {
            get { return currentEventTime; }
            set { currentEventTime = value; }
        }

        public int StockInputValue
        {
            get { return inputStockValue; }
            set { inputStockValue = value; }
        }

        public int EventIdentifier
        {
            get { return eventReference; }
            set { eventReference = value; }
        }

        public bool IsSent
        {
            get;
            set;
        }

        public bool IsSimulated
        {
            get;
            set;
        }

        public bool IsFinished
        {
            get;
            set;
        }
    }
}
