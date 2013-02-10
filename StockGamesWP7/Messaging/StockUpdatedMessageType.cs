namespace StockGames.Messaging
{
    public class StockUpdatedMessageType
    {
        public string StockIndex { get; private set; }
        public StockUpdatedMessageType(string stockIndex)
        {
            StockIndex = stockIndex;
        }
    }
}
