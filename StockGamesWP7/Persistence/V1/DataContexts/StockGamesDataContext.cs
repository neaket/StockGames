using System.Data.Linq;
using StockGames.Persistence.V1.DataModel;

namespace StockGames.Persistence.V1.DataContexts
{
    public class StockGamesDataContext : DataContext
    {
        private const string DbConnectionString = @"DataSource = 'isostore:StockGamesDB.sdf';";
        private const string ReadOnlyDbConnectionString = DbConnectionString + " File Mode='Read Only';";

        private StockGamesDataContext(string dbConnectionString)
            : base(dbConnectionString)
        { }

        /// <summary>
        /// Intended for ReadOnly DataContext operations.
        /// </summary> 
        /// <returns>A new Readonly DataContext.</returns>
        public static StockGamesDataContext GetReadOnly() {
            return new StockGamesDataContext(ReadOnlyDbConnectionString);
        }

        /// <summary>
        /// Intended for Write DataContext operations.  If you do not need to Write, use <see cref="GetReadOnly()"/>
        /// </summary> 
        /// <returns>A new ReadWrite DataContext.</returns>
        public static StockGamesDataContext GetReadWrite()
        {
            return new StockGamesDataContext(DbConnectionString);
        }

        public Table<StockDataModel> Stocks
        {
            get
            {
                return GetTable<StockDataModel>();
            }
        }

        public Table<StockSnapshotDataModel> StockSnapshots
        {
            get
            {
                return GetTable<StockSnapshotDataModel>();
            }
        }

        public Table<PortfolioDataModel> Portfolios
        {
            get
            {
                return GetTable<PortfolioDataModel>();
            }
        }

        public Table<PortfolioEntryDataModel> PortfolioEntries
        {
            get
            {
                return GetTable<PortfolioEntryDataModel>();
            }
        }
    }
}
