using StockGames.Persistence.V1.DataModel;

namespace StockGames.Messaging
{
    /// <summary>   The PortfolioTradeAddedMessageType is intended to be sent whenever a Trade is added to a Portfolio. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class PortfolioTradeAddedMessageType : PortfolioUpdatedMessageType
    {
        /// <summary>   The stock index is used to identify a particular stock in the stock market. </summary>
        ///
        /// <value> The stock index. </value>
        public string StockIndex { get; private set; }

        /// <summary>   Gets the type of the trade. </summary>
        ///
        /// <value> The type of the trade. </value>
        public TradeType TradeType { get; private set; }

        /// <summary>   Initializes a new instance of the PortfolioUpdatedMessageType class. </summary>
        ///
        /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
        ///
        /// <param name="portfolioId">  Identifier for the portfolio. </param>
        /// <param name="stockIndex">   The stock index. </param>
        /// <param name="tradeType">    The type of the trade. </param>
        public PortfolioTradeAddedMessageType(int portfolioId, string stockIndex, TradeType tradeType)
            : base(portfolioId)
        {
            StockIndex = stockIndex;
            TradeType = tradeType;
        }
    }
}