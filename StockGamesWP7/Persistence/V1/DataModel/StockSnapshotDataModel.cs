using System;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace StockGames.Persistence.V1.DataModel
{
    /// <summary>
    /// The StockSnapshotDataModel is used to persist stock snapshots. A stock snapshot is
    /// essentially a price with a tombstone.  A stock snapshot has a many-to-one relationship with
    /// <see cref="StockDataModel"/>.  StockSnapshotDataModel is intended to be Immutable after it is
    /// persisted.
    /// </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    [Table]
    public class StockSnapshotDataModel
    {
        private EntityRef<StockDataModel> _stock;

        /// <summary>   Gets or sets the identifier of the stock snapshot. </summary>
        ///
        /// <value> The identifier of the stock snapshot. </value>
        [Column(
          IsPrimaryKey = true,
          IsDbGenerated = true,
          DbType = "int NOT NULL IDENTITY",
          AutoSync = AutoSync.OnInsert)]
        public int StockSnapshotId { get; set; }

        /// <summary>   Gets the index of the stock. </summary>
        ///
        /// <value> The stock index. </value>
        [Column(
           DbType = "NVARCHAR(10) NOT NULL",    
           AutoSync = AutoSync.OnInsert)]
        public string StockIndex { get; private set; }

        /// <summary>   Gets or sets the stock.  Many-to-one relationship. </summary>
        ///
        /// <value> The stock. </value>
        [Association(
            Name = "FK_StockSnapshots_Stock",
            IsForeignKey = true,
            Storage = "_stock",
            ThisKey = "StockIndex",
            OtherKey = "StockIndex")]
        public StockDataModel Stock {
            get
            {
                return _stock.Entity;
            }
            set
            {
                StockIndex = value.StockIndex;
                _stock.Entity = value;
            }
        }

        /// <summary>
        /// Gets or sets the Date/Time of the tombstone.  A StockSnapshot's price is effective from this
        /// tombstone until the next StockSnapshot's tombstone.
        /// </summary>
        ///
        /// <value> The tombstone. </value>
        [Column(
           DbType = "datetime NOT NULL",
           AutoSync = AutoSync.OnInsert)]
        public DateTime Tombstone { get; set; }

        /// <summary>   Gets or sets the price of the stock at the Date/Time of the tombstone. </summary>
        ///
        /// <value> The price. </value>
        [Column(
            DbType = "money NOT NULL",
            AutoSync = AutoSync.OnInsert)]
        public Decimal Price { get; set; }

        /// <summary>   Returns a string that represents the current StockSnapshotDataModel. </summary>
        ///
        /// <returns>   A string that represents the current StockSnapshotDataModel. </returns>
        public override string ToString()
        {
            return String.Format("Id: {0}, StockIndex: {1}, Price: {2}, Tombstone: {3}", StockSnapshotId, StockIndex, Price, Tombstone);
        }
    }
}
