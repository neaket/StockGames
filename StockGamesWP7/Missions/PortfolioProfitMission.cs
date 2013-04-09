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
    /// <summary>
    /// Class represent a mission that follows the user's progress as they attempt to make
    /// a net profit in their portfolio
    /// </summary>
    ///
    /// <remarks>   Jon Panke, 3/1/2013. </remarks>
    public class PortfolioProfitMission : Mission
    {

        /// <summary>
        /// mission specific id to differiate between other missions
        /// </summary>
        public override long MissionId
        {
            get { return 0x0004; }
        }

        /// <summary>
        /// Attribute for the mission title
        /// </summary>
        public override string MissionTitle
        {
            get { return "Net gain in portfolio"; }
        }

        /// <summary>
        /// Attribute for the mission text description
        /// </summary>
        public override string MissionDescription
        {
            get { return "Make a net gain in your portfolio by buying and selling stocks"; }
        }

        /// <summary>
        /// Mission specific behavior for strating up a new mission
        /// </summary>
        public override void StartMission()
        {
            base.StartMission();
            Messenger.Default.Register<PortfolioTradeAddedMessageType>(this, CheckPortfolioValue);
        }

        /// <summary>
        /// Mission specific behavior for when a mission need to be flag as completed
        /// </summary>
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
