using System.Data.Linq.Mapping;

namespace StockGames.Persistance.V1.DataModel
{
    [Table]
    public class PortfolioModel
    {
        [Column(
            IsPrimaryKey = true,
            IsDbGenerated = true,
            DbType = "INT NOT NULL IDENTITY",
            CanBeNull = false,
            AutoSync = AutoSync.OnInsert)]
        public int PortfolioId { get; set; }

        [Column(
            DbType = "NVARCHAR(100) NOT NULL",
            CanBeNull = false,
            AutoSync = AutoSync.OnInsert)]
        public string Name { get; set; }
    }
}
