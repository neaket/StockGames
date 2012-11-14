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
    public class MessageEventArgs : EventArgs
    {
        private int currentEventTime;
        private int inputStockValue;

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

        public bool EventSent
        {
            get;
            set;
        }

        public bool SimulationStarted
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
