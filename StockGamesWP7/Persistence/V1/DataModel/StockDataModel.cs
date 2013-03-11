using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StockGames.Persistence.V1.DataModel
{
    [Table]
    public class StockDataModel
    {
        private EntitySet<StockSnapshotDataModel> _snapshots = new EntitySet<StockSnapshotDataModel>();

        [Column(
            IsPrimaryKey = true,
            DbType = "NVARCHAR(10) NOT NULL",
            AutoSync = AutoSync.OnInsert)]
        public string StockIndex { get; set; }

        [Column(
            DbType = "NVARCHAR(100) NOT NULL",
            AutoSync = AutoSync.OnInsert)]
        public string CompanyName { get; set; }

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
