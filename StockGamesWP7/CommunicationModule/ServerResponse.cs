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
        public int Time
        {
            get;
            set;
        }

        public int StockPrice
        {
            get;
            set;
        }

        public ServerResponse(int outTime, int outPrice)
        {
            Time = outTime;
            StockPrice = outPrice;
        }
    }
}
