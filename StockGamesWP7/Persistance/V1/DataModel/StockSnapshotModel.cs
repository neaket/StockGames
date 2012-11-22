﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace StockGames.Persistance.V1.DataModel
{
    [Table]
    public class StockSnapshotModel
    {
        private EntityRef<StockModel> stock = new EntityRef<StockModel>();
        private EntityRef<MarketModel> market = new EntityRef<MarketModel>();
        
        
        [Column(
           IsPrimaryKey = true,
           IsDbGenerated = false,
           DbType = "NVARCHAR(10) NOT NULL",
           CanBeNull = false,           
           AutoSync = AutoSync.OnInsert)]
        public string StockIndex { get; set; }


        [Association(
            Name = "FK_StockSnapshots_Stock",
            IsForeignKey = true,
            Storage = "stock",
            ThisKey = "StockIndex",
            OtherKey = "StockIndex")]
        public StockModel Stock {
            get
            {
                return stock.Entity;
            }
            set
            {
                stock.Entity = value;
            }
        }


        [Column(
           IsPrimaryKey = true,
           IsDbGenerated = false,
           DbType = "NVARCHAR(10) NOT NULL",
           CanBeNull = false,
           AutoSync = AutoSync.OnInsert)]
        public string MarketID { get; set; }
        
        [Association(
            Name = "FK_StockSnapshots_Market",
            IsForeignKey = true,
            Storage = "market",
            ThisKey = "MarketID",
            OtherKey = "MarketID")]
        public MarketModel Market {
            get
            {
                return market.Entity;
            }
            set
            {
                market.Entity = value;
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
