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
    public class MessageEvent
    {
        private static bool isSent = false;
        private static bool isAnswered = false;

        private int stockReference;
        private int stockValue;

        private int eventNumber;
        
        public MessageEvent(int reference, int value) 
        {
            stockReference = reference;
            stockValue = value;
        }

        public int GetStockReference()
        {
            return stockReference;
        }

        public int GetStockValue()
        {
            return stockValue;
        }
        
        public bool IsSent()
        {
            return isSent;
        }

        public void WasSent()
        {
            isSent = true;
        }

        public bool IsAnswered()
        {
            return isAnswered;
        }

        public void WasAnswered()
        {
            isAnswered = true;
        }

        public void SetEventNumber(int eventNum)
        {
            eventNumber = eventNum;
        }

        public int GetEventNumber()
        {
            return eventNumber;
        }
    }
}
