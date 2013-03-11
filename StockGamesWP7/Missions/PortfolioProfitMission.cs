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
            get { return "Net gain 100$ in portfolio"; }
        }

        public override string MissionDescription
        {
            get { return "Net gain 100$ in portfolio by buying and selling stocks"; }
        }

        public override void StartMission()
        {
            base.StartMission();

            Messenger.Default.Register<GameTimeUpdatedMessageType>(this, CheckPortfolioValue);
        }

        private void CheckPortfolioValue(GameTimeUpdatedMessageType message)
        {
        }
    }   
}
