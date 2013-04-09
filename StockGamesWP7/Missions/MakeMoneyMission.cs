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
using StockGames.Persistence.V1.Services;
using StockGames.Persistence.V1;
using System.Linq;
using System.Collections.Generic;
using StockGames.Persistence.V1.DataModel;
using StockGames.Entities;

namespace StockGames.Missions
{
    /// <summary>
    /// Class represent a mission that follows the user's progress as they attempt to
    /// have a profitable stock in their portfolio
    /// </summary>
    ///
    /// <remarks>   Jon Panke, 3/1/2013. </remarks>
    public class MakeMoneyMission : Mission
    {
        private List<decimal> StockValueList;

        /// <summary>
        /// mission specific id to differiate between other missions
        /// </summary>
        public override long MissionId
        {
            get { return 0x0002; }
        }

        /// <summary>
        /// Attribute for the mission title
        /// </summary>
        public override string MissionTitle
        {
            get { return "Gain profit on a stock"; } 
        }

        /// <summary>
        /// Attribute for the mission text description
        /// </summary>
        public override string MissionDescription
        {
            get { return "Make profit on at least one stock in your portfilio";  }
        }

        /// <summary>
        /// Mission specific behavior for strating up a new mission
        /// </summary>
        public override void StartMission()
        {
            base.StartMission();

            StockValueList = new List<decimal>();

            var trades = PortfolioService.Instance.GetGroupedTrades(GameState.Instance.MainPortfolioId);
            foreach (var e in trades)
            {
                var trade = e as TradeEntity;
                string temp = trade.StockIndex;
                StockValueList.Add(StockService.Instance.GetStock(temp).CurrentPrice);
            }
            Messenger.Default.Register<GameTimeUpdatedMessageType>(this, CheckStockValues);
        }

        /// <summary>
        /// Mission specific behavior for when a mission need to be flag as completed
        /// </summary>
        protected override void MissionCompleted()
        {
            base.MissionCompleted();
            Messenger.Default.Unregister<GameTimeUpdatedMessageType>(this, CheckStockValues);
        }

        private void CheckStockValues(GameTimeUpdatedMessageType message)
        {
            List<decimal> NewStockValues = new List<decimal>();

            var trades = PortfolioService.Instance.GetGroupedTrades(GameState.Instance.MainPortfolioId);
            foreach (var e in trades)
            {
                var trade = e as TradeEntity;
                string temp = trade.StockIndex;
                NewStockValues.Add(StockService.Instance.GetStock(temp).CurrentPrice);
            }

            for (int i = 0; i < NewStockValues.Count; i++)
            {
                if (NewStockValues.ElementAt(i) > StockValueList.ElementAt(i))
                {
                    MissionCompleted();
                    return;
                }
                else
                {
                    StockValueList = NewStockValues;
                }
            }
        }
    }
}
