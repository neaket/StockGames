using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Messaging;
using StockGames.Persistence.V1.DataModel;
using StockGames.Controllers;

namespace StockGames.Missions
{
    public class MissionBuyStocks : Mission
    {
        private readonly List<string> _newTradeStockIndexes = new List<string>();

        public override long MissionId
        {
            get { return 0x0001; }
        }

        public override string MissionTitle
        {
            get { return "Buy two stocks"; }
        }

        public override string MissionDescription
        {
            get { return "Buy two stocks on your portfolio"; }
        }

        public override void StartMission()
        {
            base.StartMission();

            Messenger.Default.Register<PortfolioTradeAddedMessageType>(this, PortfolioTradeAdded);
        }

        protected override void MissionCompleted()
        {
            base.MissionCompleted();
            MissionController.Instance.UpdateGameEngine(this.MissionId);
            ShowMissionToast("100% Completed");
            Messenger.Default.Unregister<PortfolioTradeAddedMessageType>(this);
        }

        private void PortfolioTradeAdded(PortfolioTradeAddedMessageType message)
        {
            if (message.TradeType != TradeType.Buy)
                return;

            if (!_newTradeStockIndexes.Contains(message.StockIndex))
            {
                _newTradeStockIndexes.Add(message.StockIndex);
            }

            if (_newTradeStockIndexes.Count == 1)
            {
                ShowMissionToast("50% Completed");
                Messenger.Default.Send(new MissionUpdatedMessageType(MissionId, MissionStatus));
            }
            if (_newTradeStockIndexes.Count == 2)
            {
                MissionCompleted();
            }
        }
    }
}
