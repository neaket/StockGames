
namespace StockGames.Models
{
    public class TradeEntity
    {
        public string StockIndex { get; set; }
        public int Quantity { get; set; }
        public decimal Amount { get; set; }
        public decimal Gain { get; set; }
        public decimal GainPercentage { get; set; }
    }
}
