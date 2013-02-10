namespace StockGames.Messaging
{
    public class PortfolioUpdatedMessageType
    {
        public int PortfolioId { get; private set;}

        public PortfolioUpdatedMessageType(int portfolioId)
        {
            PortfolioId = portfolioId;
        }
    }
}
