using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StockGames.Persistence.V1.DataModel
{
    /// <summary>
    /// PortfolioEntryDataModel is an abstract class that is used to persist portfolio entries.
    /// 
    ///  <see cref="PortfolioTransactionDataModel"/> and <see cref="PortfolioTradeDataModel"/> are
    ///  implementations of this class.
    /// </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    [Table]
    [InheritanceMapping(Code = EntryCode.Transaction, Type = typeof(PortfolioTransactionDataModel), IsDefault = true)]
    [InheritanceMapping(Code = EntryCode.Trade, Type = typeof(PortfolioTradeDataModel))]
    public abstract class PortfolioEntryDataModel
    {
        /// <summary>
        /// The code that represents an Entry Type. 
        /// </summary>
        ///
        /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
        public enum EntryCode
        {
            /// <summary>   A PortfolioTransaction. </summary>
            Transaction = 1,
            /// <summary>   A PortfolioTrade. </summary>
            Trade = 2
        }

        private EntityRef<PortfolioDataModel> _portfolio;

        /// <summary>   Gets or sets the identifier of the entry. </summary>
        ///
        /// <value> The identifier of the entry. </value>
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

        /// <summary>   Gets or sets the portfolio. </summary>
        ///
        /// <value> The portfolio. </value>
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

        /// <summary>   Gets or sets the EntryCode.  Used by InheritanceMapping to determine the Entries
        /// class during serialization.</summary>
        ///
        /// <value> The entry code. </value>
        [Column(IsDiscriminator=true)]
        public EntryCode Code { get; private set; }

        /// <summary>   Gets or sets the Date/Time of the tombstone for the entry. </summary>
        ///
        /// <value> The tombstone. </value>
        [Column(
           DbType = "datetime NOT NULL",
           AutoSync = AutoSync.OnInsert)]
        public DateTime Tombstone { get; set; }

        /// <summary>
        /// Gets or sets the amount in the Entry.  The value can be positive or negative depending on the Entry.
        /// 
        /// Note:  The amount is applied to adjust the corresponding Portfolio balance, when an Entry is added.
        /// </summary>
        ///
        /// <value> The amount. </value>
        [Column(
            DbType = "money NOT NULL",
            AutoSync = AutoSync.OnInsert)]
        public decimal Amount { get; set; }
    }
}
