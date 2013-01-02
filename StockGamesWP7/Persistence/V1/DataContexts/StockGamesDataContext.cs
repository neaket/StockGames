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

        public Table<StockModel> Stocks
        {
            get
            {
                return GetTable<StockModel>();
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

        public Table<PortfolioEntryModel> PortfolioEntries
        {
            get
            {
                return GetTable<PortfolioEntryModel>();
            }
        }
    }
}
