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
    public class SellStockMission : Mission
    {
        private readonly List<string> _newTradeStockIndexes = new List<string>();

        public override long MissionId
        {
            get { return 0x0003; }
        }

        public override string MissionTitle
        {
            get { return "Sell a stock"; }
        }

        public override string MissionDescription
        {
            get { return "Sell a Stock in yourportfolio"; }
        }

        public override void StartMission()
        {
            base.StartMission();
            Messenger.Default.Register<PortfolioTradeAddedMessageType>(this, PortfolioTradeMade);
        }

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
