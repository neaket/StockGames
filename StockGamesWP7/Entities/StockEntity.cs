using System;
using System.ComponentModel;
using System.Data.Linq.Mapping;

namespace StockGames.Entities
{
    public class StockEntity :INotifyPropertyChanged
    {
        #region Private Variables
       
        private string _stockIndex;
        private string _companyName;
        private decimal _currentPrice;
        private decimal _previousPrice;

        #endregion

        //List of string constants used for notifying subscribers
        private const string CurrentPricePropertyName = "CurrentPrice";
        private const string PreviousPricePropertyName = "PreviousPrice";

        #region Public Properties and Manipulators
        public string StockIndex
        {
            get
            {
                return _stockIndex;
            }
            private set 
            {
                _stockIndex = value;
            }

        }
        public string CompanyName
        {
            get
            {
                return _companyName;
            }
            private set
            {
                _companyName = value;
            }
        }
        public decimal CurrentPrice
        {
            get
            {
                return _currentPrice;
            }
            set
            {
                _currentPrice = value;
                NotifyPropertyChanged(CurrentPricePropertyName);
            }
            
        }
        public decimal PreviousPrice
        {
            get
            {
                return _previousPrice;
            }
            set
            {
                _previousPrice = value;
                NotifyPropertyChanged(PreviousPricePropertyName);
            }
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
                if (PreviousPrice <= 0)
                {
                    return 0;
                }
                return DailyChange / PreviousPrice; 
            }
        }
        #endregion

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

        public override bool Equals(object obj)
        {
            var other = obj as StockEntity;
            if (other == null) return false;

            return other.StockIndex == StockIndex;
        }

        public override int GetHashCode()
        {
            return StockIndex.GetHashCode();  
        }
    }
}
