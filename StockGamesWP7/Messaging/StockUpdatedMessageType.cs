namespace StockGames.Messaging
{
    /// <summary>   The PortfolioUpdatedMessageType is intended to be sent whenever a Stock is Updated. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class StockUpdatedMessageType
    {
        /// <summary>   The stock index is used to identify a particular stock in the stock market. </summary>
        ///
        /// <value> The stock index. </value>
        public string StockIndex { get; private set; }

        /// <summary>   Initializes a new instance of the StockUpdatedMessageType class. </summary>
        ///
        /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
        ///
        /// <param name="stockIndex">   The stock index. </param>
        public StockUpdatedMessageType(string stockIndex)
        {
            StockIndex = stockIndex;
        }
    }
}
