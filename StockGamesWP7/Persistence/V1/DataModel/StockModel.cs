using System;
using System.Data.Linq.Mapping;

namespace StockGames.Persistence.V1.DataModel
{
    [Table]
    public class StockModel
    {
        [Column(
            IsPrimaryKey = true,
            DbType = "NVARCHAR(10) NOT NULL",
            AutoSync = AutoSync.OnInsert)]
        public string StockIndex { get; set; }

        [Column(
            DbType = "NVARCHAR(100) NOT NULL",
            AutoSync = AutoSync.OnInsert)]
        public string CompanyName { get; set; }

        [Column(
            DbType = "money NOT NULL",
            AutoSync = AutoSync.OnUpdate)]
        public Decimal CurrentPrice { get; set; }
        
        [Column(
            DbType = "money NOT NULL",
            AutoSync = AutoSync.OnUpdate)]
        public Decimal PreviousPrice { get; set; }
    }
}
