using System;
using System.Data.Linq.Mapping;

namespace StockGames.Persistence.V1.DataModel
{
    [Table]
    public class StockModel
    {
        [Column(
            IsPrimaryKey = true,
            IsDbGenerated = false,
            DbType = "NVARCHAR(10) NOT NULL",
            CanBeNull = false,
            AutoSync = AutoSync.OnInsert)]
        public string StockIndex { get; set; }

        [Column(
            DbType = "NVARCHAR(100) NOT NULL",
            CanBeNull = false,
            AutoSync = AutoSync.OnInsert)]
        public string CompanyName { get; set; }

        [Column(
            DbType = "money NOT NULL",
            CanBeNull = false,
            AutoSync = AutoSync.OnUpdate)]
        public Decimal CurrentPrice { get; set; }
        
        [Column(
            DbType = "money NOT NULL",
            CanBeNull = false,
            AutoSync = AutoSync.OnUpdate)]
        public Decimal PreviousPrice { get; set; }

    }
}
