using System;
using System.Diagnostics;
using StockGames.Persistence.V1;
using StockGames.Persistence.V1.Services;
using StockGames.CommunicationModule;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Messaging;

namespace StockGames.Controllers
{
    /// <summary>
    /// The TimeController handles changing the current games time.  TimeController is responsible
    /// for ensuring that the database contains data for the current DateTime.
    /// </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    public class TimeController
    {
        private static readonly TimeController instance = new TimeController();

        /// <summary>   Gets the singleton instance. </summary>
        ///
        /// <value> The singleton instance. </value>
        public static TimeController Instance
        {
            get { return instance; }
        }

        /// <summary>   Advance the game time by one hour. </summary>
        ///
        /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
        public void AdvanceTimeByHour()
        {
            SetGameTime(GameState.Instance.GameTime.AddHours(1));
        }

        private void SetGameTime(DateTime gameTime)
        {
            if (gameTime > GameState.Instance.GameDataExpiryTime)
            {
                UpdateGameData(gameTime.AddDays(1));
            }

            GameState.Instance.GameTime = gameTime;
        }

        private void UpdateGameData(DateTime until)
        {
            // make a communication module call
            CommunicationManager comMgr = CommunicationManager.GetInstance;
            int timeAdvance = comMgr.getModel(comMgr.currentModel).modelHourAdvance;
            comMgr.requestStockUpdate(null);
            Messenger.Default.Register<CommunicationCompletedType>(this, (message) =>
            {
                GameState.Instance.GameDataExpiryTime = GameState.Instance.GameDataExpiryTime.AddHours(timeAdvance);
                return; // exit this message handler
           });
        }

        // TODO remove me
        [Obsolete]
        private void AddRandomStockSnapshots(DateTime until)
        {

            var previousExpiry = GameState.Instance.GameDataExpiryTime;
            var deltaHours = (int)(until - previousExpiry).TotalHours;
            Debug.Assert(previousExpiry.AddHours(deltaHours) == until);

            var rand = new Random();

            foreach (var stock in StockService.Instance.GetStocks())
            {
                var tombstones = new DateTime[deltaHours];
                var prices = new decimal[deltaHours];
                for (int i = 0; i < deltaHours; i++)
                {
                    tombstones[i] = previousExpiry.AddHours(i + 1);
                    prices[i] = ((decimal) rand.Next(1, 50000))/100;
                }

                StockService.Instance.AddStockSnapshots(stock.StockIndex, prices, tombstones);
            }
        }
    }
}
