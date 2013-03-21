using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StockGames.Persistence.V1.DataModel
{
    /// <summary>  The StockDataModel is used to persist Stocks. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    [Table]
    public class StockDataModel
    {
        private readonly EntitySet<StockSnapshotDataModel> _snapshots = new EntitySet<StockSnapshotDataModel>();

        /// <summary>   The stock index is used to identify a particular stock in the stock market. </summary>
        ///
        /// <value> The stock index. </value>
        [Column(
            IsPrimaryKey = true,
            DbType = "NVARCHAR(10) NOT NULL",
            AutoSync = AutoSync.OnInsert)]
        public string StockIndex { get; set; }

        /// <summary>   The name of the company that a stock represents. </summary>
        ///
        /// <value> The name of the company. </value>
        [Column(
            DbType = "NVARCHAR(100) NOT NULL",
            AutoSync = AutoSync.OnInsert)]
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets the StockSnapshots on the Stock.  A one-to-many relationship with
        /// <see cref="StockSnapshotDataModel"/>.
        /// </summary>
        ///
        /// <value> The stock snapshots. </value>
        [Association(
            Storage = "_snapshots",
            ThisKey = "StockIndex",
            OtherKey = "StockIndex")]
        public IEnumerable<StockSnapshotDataModel> Snapshots
        {
            get
            {
                return _snapshots;
            }
        }
    }
}
