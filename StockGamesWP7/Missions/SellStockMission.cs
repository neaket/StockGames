using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.Generic;
using StockGames.Messaging;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Persistence.V1.DataModel;

namespace StockGames.Missions
{
    /// <summary>
    /// Class represent a mission that follows the user's progress as they sell stocks back to the market
    /// </summary>
    ///
    /// <remarks>   Jon Panke, 3/1/2013. </remarks>
    public class SellStockMission : Mission
    {
        private readonly List<string> _newTradeStockIndexes = new List<string>();

        /// <summary>
        /// mission specific id to differiate between other missions
        /// </summary>
        public override long MissionId
        {
            get { return 0x0003; }
        }

        /// <summary>
        /// Attribute for the mission title
        /// </summary>
        public override string MissionTitle
        {
            get { return "Sell a stock"; }
        }

        /// <summary>
        /// Attribute for the mission text description
        /// </summary>
        public override string MissionDescription
        {
            get { return "Sell a Stock in yourportfolio"; }
        }

        /// <summary>
        /// Mission specific behavior for strating up a new mission
        /// </summary>
        public override void StartMission()
        {
            base.StartMission();
            Messenger.Default.Register<PortfolioTradeAddedMessageType>(this, PortfolioTradeMade);
        }

        /// <summary>
        /// Mission specific behavior for when a mission need to be flag as completed
        /// </summary>
        protected override void MissionCompleted()
        {
            base.MissionCompleted();
            Messenger.Default.Unregister<PortfolioTradeAddedMessageType>(this, PortfolioTradeMade);
        }

        private void PortfolioTradeMade(PortfolioTradeAddedMessageType message)
        {
            if (message.TradeType == TradeType.Sell)
                MissionCompleted();
            return;
        }
    }
}
