using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace StockGames.Persistance.V1.DataModel
{
    [Table]
    public class MarketModel
    {

        [Column(
           IsPrimaryKey = true,
           IsDbGenerated = false,
           DbType = "NVARCHAR(10) NOT NULL",
           CanBeNull = false,
           AutoSync = AutoSync.OnInsert)]
        public string MarketId { get; set; }

        [Column(
            DbType = "NVARCHAR(100) NOT NULL",
            CanBeNull = false,
            AutoSync = AutoSync.OnInsert)]
        public string MarketName { get; set; }
        
        public override bool Equals(object obj)
        {
            var other = obj as MarketModel;
            if (other == null) return false;

            return MarketId == other.MarketId;
        }

        public override int GetHashCode()
        {
            return MarketId.GetHashCode();
        }
    }
}
