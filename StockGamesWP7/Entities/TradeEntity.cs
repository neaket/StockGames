using System.Diagnostics;

namespace StockGames.Entities
{
    public class TradeEntity
    {
        public string StockIndex { get; set; }
        public int Quantity { get; set; }
        public decimal PurchasedPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal Gain
        {
            get
            {
                return CurrentPrice - PurchasedPrice;
            }
        }
        public decimal GainPercentage {
            get
            {
                Debug.Assert(PurchasedPrice > 0);
                
                return Gain / PurchasedPrice;
            }
        }
    }
}
