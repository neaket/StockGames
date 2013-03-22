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
using StockGames.Persistence.V1;
using GalaSoft.MvvmLight.Messaging;

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
            GameState.Instance.GameTime = GameState.Instance.GameTime.AddHours(1);
        }
    }
}
