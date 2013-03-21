using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;

namespace StockGames.Persistence.V1.DataModel
{
    /// <summary>   The type of Trade. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public enum TradeType
    {
        /// <summary>   Buy indicates when a trade represents buying stocks. </summary>
        Buy = 0,
        /// <summary>   Sell indicates when a trade represents selling stocks. </summary>
        Sell = 1,
        //Short = 2,
        //Cover = 3,
    }

    /// <summary>   The PortfolioTradeDataModel is used to persist portfolio trades. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class PortfolioTradeDataModel : PortfolioEntryDataModel
    {
        private EntityRef<StockSnapshotDataModel> _stockSnapshot;
        
        [Column(
          DbType = "int",
          CanBeNull = false,
          AutoSync = AutoSync.OnInsert)]
        private int StockSnapshotId { get; set; }

        /// <summary>
        /// Gets or sets the stock snapshot that this trade applies to. This class assumes that a stock
        /// snapshot is Immutable.
        /// </summary>
        ///
        /// <value> The stock snapshot. </value>
        [Association(
            IsForeignKey = true,
            Storage = "_stockSnapshot",
            ThisKey = "StockSnapshotId",
            OtherKey = "StockSnapshotId")]
        public StockSnapshotDataModel StockSnapshot
        {
            get
            {
                return _stockSnapshot.Entity;
            }
            set
            {
                StockSnapshotId = value.StockSnapshotId;

                _stockSnapshot.Entity = value;
            }
        }

        [Column(
           DbType = "int",
           CanBeNull = false,
           AutoSync = AutoSync.OnInsert)]
        private int? _tradeType;

        /// <summary>   Gets or sets the type of the trade. </summary>
        ///
        /// <value> The type of the trade. </value>
        public TradeType TradeType
        {
            get
            {
                Debug.Assert(_tradeType != null, "_tradeType cannot be null");
                return (TradeType) _tradeType;
            }
            set { _tradeType = (int) value; }
        }

        /// <summary>   Gets or sets the quantity. </summary>
        ///
        /// <value> The quantity. </value>
        [Column(
           DbType = "int",
           CanBeNull=false,
           AutoSync = AutoSync.OnInsert)]
        public int Quantity { get; set; }
    }
}
