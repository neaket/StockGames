using System.Data.Linq;
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
                return GetTable<StockModel>();
            }
        }

        public Table<MarketModel> Markets
        {
            get
            {
                return GetTable<MarketModel>();
            }
        }

        public Table<StockSnapshotModel> StockSnapshots
        {
            get
            {
                return GetTable<StockSnapshotModel>();
            }
        }

        public Table<PortfolioModel> Portfolios
        {
            get
            {
                return GetTable<PortfolioModel>();
            }
        }


    }
}
