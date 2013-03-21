using System.Diagnostics;

namespace StockGames.Entities
{
    /// <summary>
    /// A <see cref="StockGames.Entities.TradeEntity"/> is used to compliment the GUI ViewModels to
    /// display a Trade.  Some <see cref="T:StockGames.Persistence.V1.Services.PortfolioService" />
    /// methods relating to portfolio trades return a <see cref="StockGames.Entities.TradeEntity"/>.
    /// </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    public class TradeEntity
    {
        /// <summary>   The stock index is used to identify a which stock this trade is for. </summary>
        ///
        /// <value> The stock index. </value>
        public string StockIndex { get; set; }

        /// <summary>   The quantity of stocks in this particular trade. </summary>
        ///
        /// <value> The quantity. </value>
        public int Quantity { get; set; }

        /// <summary>   The average price of all trades that contain the same StockIndex. </summary>
        ///
        /// <value> The average purchased price. </value>
        public decimal AveragePurchasedPrice { get; set; }

        /// <summary>   The current price of a stock on the stock market. </summary>
        ///
        /// <value> The current price. </value>
        public decimal CurrentPrice { get; set; }

        /// <summary>
        /// The difference in value between the CurrentPrice and the AveragePurchasedPrice.  Referred to
        /// Gain in a StockPortfolio.
        /// </summary>
        ///
        /// <value> The gain. </value>
        public decimal Gain
        {
            get
            {
                return CurrentPrice - AveragePurchasedPrice;
            }
        }

        /// <summary>   A percentage value calculated by Gain / AveragePurchasedPrice. </summary>
        ///
        /// <value> The gain percentage. </value>
        public decimal GainPercentage {
            get
            {
                Debug.Assert(AveragePurchasedPrice > 0);
                
                return Gain / AveragePurchasedPrice;
            }
        }
    }
}
