using System.Collections.Generic;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Messaging;
using StockGames.Persistence.V1.DataModel;

namespace StockGames.Missions
{
    /// <summary>
    /// Class represent a mission that follows the user's progress as they buy stocks in the market
    /// </summary>
    ///
    /// <remarks>   Jon Panke, 3/1/2013. </remarks>
    public class MissionBuyStocks : Mission
    {
        private readonly List<string> _newTradeStockIndexes = new List<string>();

        /// <summary>
        /// mission specific id to differiate between other missions
        /// </summary>
        public override long MissionId
        {
            get { return 0x0001; }
        }

        /// <summary>
        /// Attribute for the mission title
        /// </summary>
        public override string MissionTitle
        {
            get { return "Buy two stocks"; }
        }

        /// <summary>
        /// Attribute for the mission text description
        /// </summary>
        public override string MissionDescription
        {
            get { return "Buy two stocks on your portfolio"; }
        }

        /// <summary>
        /// Mission specific behavior for strating up a new mission
        /// </summary>
        public override void StartMission()
        {
            base.StartMission();

            Messenger.Default.Register<PortfolioTradeAddedMessageType>(this, PortfolioTradeAdded);
        }

        /// <summary>
        /// Mission specific behavior for when a mission need to be flag as completed
        /// </summary>
        protected override void MissionCompleted()
        {
            base.MissionCompleted();
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
