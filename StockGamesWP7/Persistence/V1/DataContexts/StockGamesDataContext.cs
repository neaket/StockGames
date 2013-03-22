using System.Data.Linq;
using StockGames.Persistence.V1.DataModel;

namespace StockGames.Persistence.V1.DataContexts
{
    /// <summary>
    /// The stock games data context persists most data used by this application. A data context can
    /// be thought of as a database for a more typical Database Management System.
    /// </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
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

        /// <summary>   The table storing the stocks. </summary>
        ///
        /// <value> The stocks. </value>
        public Table<StockDataModel> Stocks
        {
            get
            {
                return GetTable<StockDataModel>();
            }
        }

        /// <summary>   The table storing the stock snapshots. </summary>
        ///
        /// <value> The stock snapshots. </value>
        public Table<StockSnapshotDataModel> StockSnapshots
        {
            get
            {
                return GetTable<StockSnapshotDataModel>();
            }
        }

        /// <summary>   The table storing the portfolios. </summary>
        ///
        /// <value> The portfolios. </value>
        public Table<PortfolioDataModel> Portfolios
        {
            get
            {
                return GetTable<PortfolioDataModel>();
            }
        }

        /// <summary>   The table storing the portfolio entries. </summary>
        ///
        /// <value> The portfolio entries. </value>
        public Table<PortfolioEntryDataModel> PortfolioEntries
        {
            get
            {
                return GetTable<PortfolioEntryDataModel>();
            }
        }
    }
}
