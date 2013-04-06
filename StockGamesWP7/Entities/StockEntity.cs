using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace StockGames.Entities
{
    /// <summary>
    /// A StockEntity is used to compliment the GUI ViewModels to display a Stock.  Most
    /// <see cref="T:StockGames.Persistence.V1.Services.StockService" /> methods relating to Stocks return a
    /// StockEntity.
    /// </summary>
    ///
    /// <remarks>   Andrew Jeffery &amp; Nick Eaket, 3/20/2013. </remarks>
    public class StockEntity
    {
        /// <summary> The snapshots, sorted in reverse order by tombstone. </summary>
        private readonly IList<StockSnapshotEntity> _snapshots;

        /// <summary>   The stock index is used to identify a particular stock in the stock market. </summary>
        ///
        /// <value> The stock index. </value>
        public string StockIndex { get; private set; }

        /// <summary>   The name of the company that a stock represents. </summary>
        ///
        /// <value> The name of the company. </value>
        public string CompanyName { get; private set; }

        /// <summary>   Gets the current market price on a stock. </summary>
        ///
        /// <value> The current price. </value>
        public decimal CurrentPrice
        {
            get
            {
                if (_snapshots.Count < 1)
                    return 0;
                return _snapshots[0].Price;
            }

        }

        /// <summary>   Gets the integer index of the stock index for a stock. </summary>
        ///
        /// <value> The current price. </value>
        public int StockIntegerIndex
        {
            get
            {
                var letters= StockIndex.ToCharArray();
                int index=0;
                foreach (char letter in letters)
                {
                    index *= 100;
                    index += Convert.ToInt32(letter);
                }
                return index;
            }
        }
        /// <summary>   Gets the market's previous stock price. </summary>
        ///
        /// <value> The previous price. </value>
        public decimal PreviousPrice
        {
            get
            {
                if (_snapshots.Count < 2)
                    return 0;
                return _snapshots[1].Price;
            }
        }

        /// <summary>   The delta change of the current price and the previous price.</summary>
        ///
        /// <value> The delta change. </value>
        public decimal DeltaChange
        {
            get
            {
                return CurrentPrice - PreviousPrice;
            }
        }

        /// <summary>
        /// Calculates a ratio of the DailyChange / PreviousPrice.  Commonly refered on a stock market as
        /// Profit &amp; Loss.
        /// </summary>
        ///
        /// <value> The profit and loss. </value>
        public decimal ProfitAndLoss
        {
            get
            {
                if (PreviousPrice <= 0)
                {
                    return 0;
                }
                return DeltaChange / PreviousPrice;
            }
        }

        /// <summary> Gets the snapshots sorted in reverse order by tombstone. </summary>
        /// <value> The snapshots. </value>
        public ReadOnlyCollection<StockSnapshotEntity> Snapshots
        {
            get { return new ReadOnlyCollection<StockSnapshotEntity>(_snapshots); }
        }

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

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object" /> is equal to the current
        /// <see cref="T:System.Object" />.
        /// </summary>
        ///
        /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
        ///
        /// <param name="obj">  The <see cref="T:System.Object" /> to compare with the current
        ///                     <see cref="T:System.Object" />. </param>
        ///
        /// <returns>
        /// true if the specified <see cref="T:System.Object" /> is a StockEntity and its StockIndex is
        /// equal to this StockEntity's StockIndex, otherwise, false.
        /// </returns>
        public override bool Equals(object obj)
        {
            var other = obj as StockEntity;
            if (other == null) return false;

            return other.StockIndex == StockIndex;
        }

        /// <summary>   Serves as a hash function for a <see cref="T:StockGames.Entities.StockEntity" />. </summary>
        ///
        /// <returns>   A hash code for the current <see cref="T:StockGames.Entities.StockEntity" />. </returns>
        public override int GetHashCode()
        {
            return StockIndex.GetHashCode();
        }
    }

    /// <summary>
    /// Stock snapshot entity.  Used to hold a stock snapshot containing the price and a tombstone.
    /// </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    public class StockSnapshotEntity
    {
        /// <summary>   The snapshot's Price </summary>
        ///
        /// <value> The price. </value>
        public decimal Price { get; private set; }

        /// <summary>   The Date/Time of the snapshot's tombstone. </summary>
        ///
        /// <value> The tombstone. </value>
        public DateTime Tombstone { get; private set; }

        /// <summary>   Constructor. </summary>
        ///
        /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
        ///
        /// <param name="price">        The snapshot's price. </param>
        /// <param name="tombstone">    The snapshot's tombstone. </param>
        public StockSnapshotEntity(decimal price, DateTime tombstone)
        {
            Price = price;
            Tombstone = tombstone;
        }
    }
}