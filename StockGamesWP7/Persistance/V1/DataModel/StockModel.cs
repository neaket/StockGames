using System;
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

namespace StockGames.Persistance.V1.DataModel
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

    }
}
