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
using GalaSoft.MvvmLight.Messaging;
using StockGames.Messaging;
using StockGames.Controllers;
using StockGames.Persistence.V1.Services;
using StockGames.Persistence.V1;
using StockGames.Persistence.V1.DataModel;


namespace StockGames.Missions
{
    public class PortfolioProfitMission : Mission
    {
        public override long MissionId
        {
            get { return 0x0004; }
        }

        public override string MissionTitle
        {
            get { return "Net gain in portfolio"; }
        }

        public override string MissionDescription
        {
            get { return "Make a net gain in your portfolio by buying and selling stocks"; }
        }

        public override void StartMission()
        {
            base.StartMission();
            Messenger.Default.Register<PortfolioTradeAddedMessageType>(this, CheckPortfolioValue);
        }

        protected override void MissionCompleted()
        {
            base.MissionCompleted();
            Messenger.Default.Unregister<PortfolioTradeAddedMessageType>(this, CheckPortfolioValue);
        }

        private void CheckPortfolioValue(PortfolioTradeAddedMessageType message)
        {
            var balance = PortfolioService.Instance.GetPortfolio(GameState.Instance.MainPortfolioId);
            if (balance.Balance > 10000)
                MissionCompleted();
            return;
        }
    }   
}
