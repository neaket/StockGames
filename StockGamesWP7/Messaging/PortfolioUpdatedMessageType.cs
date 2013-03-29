namespace StockGames.Messaging
{
    /// <summary>   The PortfolioUpdatedMessageType is intended to be sent whenever a Portfolio is Updated. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class PortfolioUpdatedMessageType
    {
        /// <summary>   Gets the identifier of the portfolio. </summary>
        ///
        /// <value> The identifier of the portfolio. </value>
        public int PortfolioId { get; private set;}

        /// <summary>   Initializes a new instance of the PortfolioUpdatedMessageType class. </summary>
        ///
        /// <param name="portfolioId">  The identifier of the portfolio. </param>
        public PortfolioUpdatedMessageType(int portfolioId)
        {
            PortfolioId = portfolioId;
        }
    }
}
