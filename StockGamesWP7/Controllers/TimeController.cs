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

namespace StockGames.Controllers
{
    public class TimeController
    {
        private static readonly TimeController instance = new TimeController();
        public static TimeController Instance
        {
            get { return instance; }
        }

        public void AdvanceTimeByHour()
        {
            GameState.Instance.GameTime.AddHours(1);
        }
    }
}
