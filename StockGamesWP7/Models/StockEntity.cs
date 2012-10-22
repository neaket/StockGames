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
using System.ComponentModel;

namespace StockGames.Models
{
    public class StockEntity :INotifyPropertyChanged
    {
        public string StockIndex
        {
            get;
            set;
        }
        public string CompanyName
        {
            get;
            set;
        }
        public decimal CurrentPrice
        {
            get;
            set;
        }
        public decimal PreviousPrice
        {
            get;
            set;
        }

        public decimal DailyChange
        {
            get
            {
                return CurrentPrice - PreviousPrice;
            }
        }

        public decimal ProfitAndLoss
        {
            get
            {
                return DailyChange / PreviousPrice;
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;
    }
}
