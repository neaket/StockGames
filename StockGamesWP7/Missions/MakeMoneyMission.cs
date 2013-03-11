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
using StockGames.Controllers;
using System.Collections.Generic;
using StockGames.Persistence.V1.DataModel;
using StockGames.Entities;

namespace StockGames.Missions
{
    public class MakeMoneyMission : Mission
    {
        private List<decimal> StockValueList;

        public override long MissionId
        {
            get { return 0x0002; }
        }

        public override string MissionTitle
        {
            get { return "Gain profit on a stock"; } 
        } 

        public override string MissionDescription
        {
            get { return "Make profit on at least one stock in your portfilio";  }
        }

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
        
        protected override void MissionCompleted()
        {
            base.MissionCompleted();
            MissionController.Instance.UpdateGameEngine(this.MissionId);
            ShowMissionToast("100% Completed");
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
                }
                else
                {
                    StockValueList = NewStockValues;
                }
            }
        }
    }
}
