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
using System.Data.Linq;
using StockGames.Models;
using StockGames.Persistance.V1.DataModel;

namespace StockGames.Persistance.V1.DataContexts
{
    public class StockGamesDataContext : DataContext
    {
        private const string DbConnectionString = @"DataSource = 'isostore:StockGamesDB.sdf';";
        private const string ReadOnlyDbConnectionString = DbConnectionString + " File Mode='Read Only';";

        private StockGamesDataContext(string dbConnectionString)
            : base(dbConnectionString)
        { }

        public static StockGamesDataContext GetReadOnly() {
            return new StockGamesDataContext(ReadOnlyDbConnectionString);
        }

        public static StockGamesDataContext GetReadWrite()
        {
            return new StockGamesDataContext(DbConnectionString);
        }

        public Table<StockModel> Stocks
        {
            get
            {
                return this.GetTable<StockModel>();
            }
        }

        public Table<MarketModel> Markets
        {
            get
            {
                return this.GetTable<MarketModel>();
            }
        }

        public Table<StockSnapshotModel> StockSnapshots
        {
            get
            {
                return this.GetTable<StockSnapshotModel>();
            }
        }


    }
}
