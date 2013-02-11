 using System;
 using System.Collections.Generic;
 using System.ComponentModel;
using System.Data.Linq.Mapping;
using System.Collections.ObjectModel;

namespace StockGames.Entities
{
    public class StockEntity :INotifyPropertyChanged
    {
        #region Private Variables
       
        private string _stockIndex;
        private string _companyName;
        /// <summary> The snapshots, sorted in reverse order by tombstone. </summary>
        private IList<StockSnapshotEntity> _snapshots;

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
                if (_snapshots.Count < 1)
                    return 0;
                return _snapshots[0].Price;
            }
            
        }
        public decimal PreviousPrice
        {
            get
            {
                if (_snapshots.Count < 2)
                    return 0;
                return _snapshots[1].Price;
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

        /// <summary> Gets the snapshots sorted in reverse order by tombstone. </summary>
        /// <value> The snapshots. </value>
        public ReadOnlyCollection<StockSnapshotEntity> Snapshots
        {
            get { return new ReadOnlyCollection<StockSnapshotEntity>(_snapshots); }
        }
        #endregion

        /// <summary> Constructor for Class, creates a stock with a name and index. </summary>
        /// <param name="stockIndex">   Index or symbol that represents the stock. </param>
        /// <param name="companyName">  The full Name of the company. </param>
        /// <param name="snapshots">    The Stock Snapshots. </param>
        public StockEntity(string stockIndex, string companyName, IList<StockSnapshotEntity> snapshots)
        {
            StockIndex = stockIndex;
            CompanyName = companyName;
            _snapshots = snapshots;
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

    public class StockSnapshotEntity
    {
        public decimal Price { get; private set; }
        public DateTime Tombstone { get; private set; }

        public StockSnapshotEntity(decimal price, DateTime tombstone)
        {
            Price = price;
            Tombstone = tombstone;
        }
    }
}
