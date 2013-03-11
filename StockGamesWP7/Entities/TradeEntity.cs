using System.Diagnostics;

namespace StockGames.Entities
{
    public class TradeEntity
    {
        public string StockIndex { get; set; }
        public int Quantity { get; set; }
        public decimal AveragePurchasedPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Gain
        {
            get
            {
                return CurrentPrice - AveragePurchasedPrice;
            }
        }
        public decimal GainPercentage {
            get
            {
                Debug.Assert(AveragePurchasedPrice > 0);
                
                return Gain / AveragePurchasedPrice;
            }
        }
    }
}
