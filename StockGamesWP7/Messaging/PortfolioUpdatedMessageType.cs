using StockGames.Persistence.V1.DataModel;

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

    public class PortfolioTradeAddedMessageType : PortfolioUpdatedMessageType
    {
        public string StockIndex { get; private set; }
        public TradeType TradeType { get; private set; }

        public PortfolioTradeAddedMessageType(int portfolioId, string stockIndex, TradeType tradeType)
            : base(portfolioId)
        {
            StockIndex = stockIndex;
            TradeType = tradeType;
        }
    }
}
