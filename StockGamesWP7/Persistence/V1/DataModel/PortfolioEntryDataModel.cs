﻿using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StockGames.Persistence.V1.DataModel
{
    [Table]
    [InheritanceMapping(Code = EntryCode.Transaction, Type = typeof(PortfolioTransactionDataModel), IsDefault = true)]
    [InheritanceMapping(Code = EntryCode.Trade, Type = typeof(PortfolioTradeDataModel))]
    public class PortfolioEntryDataModel
    {
        public enum EntryCode
        {
            Transaction = 1,
            Trade = 2
        }

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
        public EntryCode Code { get; private set; }

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