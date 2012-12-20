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
    public class ServerResponse
    {
        private int time;
        private int stockPrice;

        public ServerResponse(int outTime, int outPrice)
        {
            time = outTime;
            stockPrice = outPrice;
        }

        public int Time
        {
            get { return time; }
            set { time = value; }
        }

        public int StockPrice
        {
            get { return stockPrice; }
            set { stockPrice = value; }
        }
    }
}
