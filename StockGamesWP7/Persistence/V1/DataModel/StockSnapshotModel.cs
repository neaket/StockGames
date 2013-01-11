using System;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace StockGames.Persistence.V1.DataModel
{
    [Table]
    public class StockSnapshotModel
    {
        private EntityRef<StockModel> _stock;
        
        [Column(
           IsPrimaryKey = true,
           IsDbGenerated = false,
           DbType = "NVARCHAR(10) NOT NULL",
           CanBeNull = false,           
           AutoSync = AutoSync.OnInsert)]
        private string StockIndex { get; set; }

        [Association(
            Name = "FK_StockSnapshots_Stock",
            IsForeignKey = true,
            Storage = "_stock",
            ThisKey = "StockIndex",
            OtherKey = "StockIndex")]
        public StockModel Stock {
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

        [Column(
           IsPrimaryKey = true,
           IsDbGenerated = false,
           DbType = "datetime NOT NULL",
           CanBeNull = false,
           AutoSync = AutoSync.OnInsert)]
        public DateTime Tombstone { get; set; }

        [Column(
            DbType = "money NOT NULL",
            CanBeNull = false,
            AutoSync = AutoSync.OnInsert)]
        public Decimal Price { get; set; }
    }
}
