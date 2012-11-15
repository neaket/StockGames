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

namespace StockGames.Persistance.DataContexts
{
    public class StockGamesDataContext : DataContext
    {
        private const string dbConnectionString = @"isostore:StockGamesDB.sdf";
        private const string readOnlyDbConnectionString = dbConnectionString + ";File Mode=read only";

        private StockGamesDataContext(string dbConnectionString)
            : base(dbConnectionString)
        { }

        public static StockGamesDataContext GetReadOnly() {
            return new StockGamesDataContext(readOnlyDbConnectionString);
        }

        public static StockGamesDataContext GetReadWrite()
        {
            return new StockGamesDataContext(dbConnectionString);
        }

        public Table<StockEntity> Stocks
        {
            get
            {
                return this.GetTable<StockEntity>();
            }
        }


    }
}
