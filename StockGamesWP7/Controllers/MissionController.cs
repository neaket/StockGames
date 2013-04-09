using System.Collections.Generic;
using StockGames.Missions;

namespace StockGames.Controllers
{
    /// <summary>
    /// Prvoides the core services for missions, and allows the mission to be added to the game.
    /// </summary>
    ///
    /// <remarks>   Jon Panke, 3/1/2013. </remarks>
    public class MissionController
    {
        #region instance

        private static readonly MissionController instance = new MissionController();

        /// <summary>
        /// Locally stored instance of this class used to implement singleton design
        /// </summary>
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

        /// <summary>
        /// Changes/unlocks missions based on the missions completed currently
        /// </summary>
        /// <param name="id"></param>
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

        /// <summary>
        /// Use to get a list of the current missions that are in the game
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Mission> GetMissions()
        {
            return _missions.Values;
        }

        /// <summary>
        /// Get a mission with the type id specified
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Mission GetMission(long id)
        {
            return _missions[id];
        }
    }
}
