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
            StartGameEngine();
        }

        private void StartGameEngine()
        {
            Mission mission = null;
            if(mission == null){
                mission = new MissionBuyStocks();
                _missions.Add(mission.MissionId, mission);
                mission.StartMission();
            }         
        }

        public void UpdateGameEngine(long id)
        {
            Mission mission;
            if (id == 0x0001)
            {
                mission = new MakeMoneyMission();
                _missions.Add(mission.MissionId, mission);
                mission.StartMission();
            }
            else if (id == 0x0002)
            {
                mission = new SellStockMission();
                _missions.Add(mission.MissionId, mission);
                mission.StartMission();                
            }
            else if (id == 0x0003)
            {
                mission = new PortfolioProfitMission();
                _missions.Add(mission.MissionId, mission);
                mission.StartMission();
            }
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
