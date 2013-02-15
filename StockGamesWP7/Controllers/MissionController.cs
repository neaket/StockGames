using System.Collections.Generic;
using StockGames.Missions;

namespace StockGames.Controllers
{
    public class MissionController
    {
        #region instance

        private static readonly MissionController instance = new MissionController();
        public static MissionController Instance
        {
            get { return instance; }
        }

        #endregion

        private readonly Dictionary<long, Mission> _missions = new Dictionary<long, Mission>();

        private MissionController()
        {
            var mission = new MissionBuyStocks();
            _missions.Add(mission.MissionId, mission);
        }

        public IEnumerable<Mission> GetMissions()
        {
            return _missions.Values;
        }

        public Mission GetMission(long id)
        {
            return _missions[id];
        }
    }
}
