using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StockGames.Persistence.V1.DataModel
{
    [Table]
    public class PortfolioModel
    {
        private EntitySet<PortfolioEntryModel> _entries = new EntitySet<PortfolioEntryModel>();
        private decimal _balance;

        [Column(
            IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "int NOT NULL IDENTITY",
            AutoSync = AutoSync.OnInsert)]
        public int PortfolioId { get; set; }

        [Column(
            DbType = "NVARCHAR(100) NOT NULL",
            AutoSync = AutoSync.OnInsert)]
        public string Name { get; set; }

        [Association(
            Storage = "_entries",
            ThisKey = "PortfolioId",
            OtherKey = "PortfolioId")]
        public IEnumerable<PortfolioEntryModel> Entries
        {
            get
            {
                return _entries;
            }
        }

        [Column(
            DbType = "money NOT NULL",
            CanBeNull = false,
            AutoSync = AutoSync.Always)]
        public decimal Balance
        {
            get
            {
                return _balance;
            }
            private set
            {
                if (value < 0)
                {
                    throw new ArgumentException("A Portfolio balance cannot be negative.");
                }
                _balance = value;
            }
        }

        public void AddEntry(PortfolioEntryModel entry)
        {
            Balance += entry.Amount;
            entry.Portfolio = this;
            _entries.Add(entry);
        }
    }
}
