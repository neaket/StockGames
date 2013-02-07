using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Diagnostics;

namespace StockGames.Persistence.V1.DataModel
{
    public enum TradeType : int
    {
        Buy = 0,
        Sell = 1,
        Short = 2,
        Cover = 3,
    }

    public class PortfolioTradeDataModel : PortfolioEntryDataModel
    {
        private EntityRef<StockSnapshotDataModel> _stockSnapshot;
        
        [Column(
          DbType = "int",
          CanBeNull = false,
          AutoSync = AutoSync.OnInsert)]
        private int StockSnapshotId { get; set; }
        
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
        public TradeType TradeType
        {
            get
            {
                Debug.Assert(_tradeType != null, "_tradeType cannot be null");
                return (TradeType) _tradeType;
            }
            set { _tradeType = (int) value; }
        }

        [Column(
           DbType = "int",
           CanBeNull=false,
           AutoSync = AutoSync.OnInsert)]
        public int Quantity { get; set; }
    }
}
