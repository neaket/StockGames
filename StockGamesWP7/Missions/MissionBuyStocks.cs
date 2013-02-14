using System.Collections.Generic;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Messaging;
using StockGames.Persistence.V1.DataModel;

namespace StockGames.Missions
{
    [DataContract]
    public class MissionBuyStocks : Mission
    {
        // public to support serialization
        [DataMember]  
        public List<string> NewTradeStockIndexes { get; set; } 

        public override long MissionId
        {
            get { return 0x7f6ae6b1; }
        }

        public override string MissionTitle
        {
            get { return "Buy two stocks"; }
        }

        public override string MissionDescription
        {
            get { return "Buy two stocks on your portfolio"; }
        }

        public MissionBuyStocks()
        {
            NewTradeStockIndexes = new List<string>();
        }

        public override void ResumeFromLoad()
        {
            if (MissionStatus == MissionStatus.InProgress)
            {
                Messenger.Default.Register<PortfolioTradeAddedMessageType>(this, PortfolioTradeAdded);
            }
        }

        public override void StartMission()
        {
            base.StartMission();

            Messenger.Default.Register<PortfolioTradeAddedMessageType>(this, PortfolioTradeAdded);
        }

        protected override void MissionCompleted()
        {
            base.MissionCompleted();

            ShowMissionToast("100% Completed");

            Messenger.Default.Unregister<PortfolioTradeAddedMessageType>(this);
        }

        private void PortfolioTradeAdded(PortfolioTradeAddedMessageType message)
        {
            if (message.TradeType != TradeType.Buy)
                return;

            if (!NewTradeStockIndexes.Contains(message.StockIndex))
            {
                NewTradeStockIndexes.Add(message.StockIndex);
            }

            if (NewTradeStockIndexes.Count == 1)
            {
                ShowMissionToast("50% Completed");
                Messenger.Default.Send(new MissionUpdatedMessageType(MissionId, MissionStatus));
            }
            if (NewTradeStockIndexes.Count == 2)
            {
                MissionCompleted();
            }
        }
    }
}
