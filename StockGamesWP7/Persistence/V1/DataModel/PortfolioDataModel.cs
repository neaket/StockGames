using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StockGames.Persistence.V1.DataModel
{
    /// <summary>   The portfolio data model is used to persist portfolios. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    [Table]
    public class PortfolioDataModel
    {
        private EntitySet<PortfolioEntryDataModel> _entries = new EntitySet<PortfolioEntryDataModel>();
        private decimal _balance;

        /// <summary>   A unique identifier of a portfolio. </summary>
        ///
        /// <value>  A unique identifier of a portfolio. </value>
        [Column(
            IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "int NOT NULL IDENTITY",
            AutoSync = AutoSync.OnInsert)]
        public int PortfolioId { get; set; }

        /// <summary>   Gets or sets the name of a Portfolio. </summary>
        ///
        /// <value> The name. </value>
        [Column(
            DbType = "NVARCHAR(100) NOT NULL",
            AutoSync = AutoSync.OnInsert)]
        public string Name { get; set; }

        /// <summary>   Gets all of the portfolio entries on the portfolio. </summary>
        ///
        /// <value> The portfolio entries. </value>
        [Association(
            Storage = "_entries",
            ThisKey = "PortfolioId",
            OtherKey = "PortfolioId")]
        public EntitySet<PortfolioEntryDataModel> Entries
        {
            get
            {
                return _entries;
            }
        }

        /// <summary>   Gets or sets the balance on a portfolio. </summary>
        ///
        /// <value> The balance. </value>
        ///
        /// ### <exception cref="System.ArgumentException"> Thrown if the balance is negative. </exception>
        [Column(
            DbType = "money NOT NULL",
            CanBeNull = false,
            AutoSync = AutoSync.Always)]
        public decimal Balance
        {
            get
            {
                return _balance;
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentException("A Portfolio balance cannot be negative.");
                }
                _balance = value;
            }
        }

        
    }
}
