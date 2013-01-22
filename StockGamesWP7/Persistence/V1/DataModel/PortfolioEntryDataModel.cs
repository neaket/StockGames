using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StockGames.Persistence.V1.DataModel
{
    [Table]
    [InheritanceMapping(Code = 1, Type = typeof(PortfolioTradeDataModel), IsDefault = true)]
    public class PortfolioEntryDataModel
    {
        private EntityRef<PortfolioDataModel> _portfolio;

        [Column(
            IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL IDENTITY",
            AutoSync = AutoSync.OnInsert)]
        public int EntryId { get; set; }

        [Column(
            DbType = "INT NOT NULL",
            AutoSync = AutoSync.OnInsert)]
        private int PortfolioId { get; set; }

        [Association(
            Storage = "_portfolio",
            ThisKey = "PortfolioId",
            OtherKey = "PortfolioId")]
        public PortfolioDataModel Portfolio
        {
            get
            {
                return _portfolio.Entity;
            }
            set 
            { 
                PortfolioId = value.PortfolioId;
                _portfolio.Entity = value;
            }
        }

        [Column(IsDiscriminator=true)]
        private int Code { get; set; }

        [Column(
           DbType = "datetime NOT NULL",
           AutoSync = AutoSync.OnInsert)]
        public DateTime Tombstone { get; set; }

        [Column(
            DbType = "money NOT NULL",
            AutoSync = AutoSync.OnInsert)]
        public decimal Amount { get; set; }
    }
}
