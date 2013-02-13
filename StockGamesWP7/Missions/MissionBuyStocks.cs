using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Controllers;
using StockGames.Messaging;

namespace StockGames.Missions
{
    public class MissionBuyStocks : Mission
    {
        private List<string> _newTradeStockIndexes = new List<string>();

        public override long MissionId
        {
            get { return 0x7f6ae6b1; }
        }

        public override string MissionTitle
        {
            get { return "Buy 2 stocks"; }
        }

        public override string MissionDescription
        {
            get { return "Add 2 unique stocks to your portfolio."; }
        }

        public MissionBuyStocks()
        {
        }

        public override void StartMission()
        {
            base.StartMission();

            Messenger.Default.Register<PortfolioTradeAddedMessageType>(this, PortfolioTradeAdded);
        }

        protected override void MissionCompleted()
        {
            base.MissionCompleted();

            Messenger.Default.Unregister<PortfolioTradeAddedMessageType>(this);
        }

        private void PortfolioTradeAdded(PortfolioTradeAddedMessageType message)
        {
            if (!_newTradeStockIndexes.Contains(message.StockIndex))
            {
                _newTradeStockIndexes.Add(message.StockIndex);
            }

            if (_newTradeStockIndexes.Count == 2)
            {
                MissionCompleted();
            }
        }
    }
}
