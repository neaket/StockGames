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
        //Private Variables
        private string _StockIndex;
        private string _CompanyName;
        private decimal _CurrentPrice;
        private decimal _PreviousPrice = 0;

        //List of string constants used for notifying subscribers
        public static string CURRENT_PRICE = "current price";
        public static string PREVIOUS_PRICE = "previous price";

        //Public variables and Manipulators
        public string StockIndex
        {
            get
            {
                return _StockIndex;
            }
            private set 
            {
                _StockIndex = value;
            }

        }
        public string CompanyName
        {
            get
            {
                return _CompanyName;
            }
            private set
            {
                _CompanyName = value;
            }
        }
        public decimal CurrentPrice
        {
            get
            {
                return _CurrentPrice;
            }
            set
            {
                _CurrentPrice = value;
                NotifyPropertyChanged(CURRENT_PRICE);
            }
            
        }
        public decimal PreviousPrice
        {
            get
            {
                return _PreviousPrice;
            }
            set
            {
                _PreviousPrice = value;
                NotifyPropertyChanged(PREVIOUS_PRICE);
            }
        }

        //Class Methods

        /// <summary>
        /// Constructor for Class, creates a stock with a name and index
        /// </summary>
        /// <param name="stockIndex">
        /// Index or symbol that represents the stock
        /// </param>
        /// <param name="companyName">
        /// The full Name of the company
        /// </param>
        public StockEntity(string stockIndex, string companyName)
        {
            StockIndex = stockIndex;
            CompanyName = companyName;
        }

        //INotifyPropertyChanged Interface Implmentation

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(String propertyName = "")
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }
    }
}
