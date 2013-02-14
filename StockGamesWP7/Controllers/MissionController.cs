using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Messaging;
using StockGames.Missions;

namespace StockGames.Controllers
{
    public class MissionController
    {
        private const string FilePath = @"Missions";
        private static readonly DataContractSerializer Serializer = new DataContractSerializer(
            typeof(Dictionary<long, Mission>), 
            new List<Type>
                {
                    typeof(MissionBuyStocks)
                });

        #region instance

        private static readonly MissionController instance = new MissionController();
        public static MissionController Instance
        {
            get { return instance; }
        }

        #endregion

        private readonly Dictionary<long, Mission> _missions;

        #region Serialization
        
        private MissionController()
        {
            using (var isoFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoFile.FileExists(FilePath))
                {
                    using (var stream = isoFile.OpenFile(FilePath, FileMode.OpenOrCreate))
                    {

                        _missions = (Dictionary<long, Mission>)Serializer.ReadObject(stream);
                    }

                    foreach (var mission in _missions.Values)
                    {
                        mission.ResumeFromLoad();
                    }
                }
                else
                {
                    _missions = new Dictionary<long, Mission>();
                    ConstructMissions();
                }
            }

            Messenger.Default.Register<MissionUpdatedMessageType>(this, MissionUpdate);
        }

        

        private void SaveMissions()
        {
            using (var isoFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = isoFile.OpenFile(FilePath, FileMode.OpenOrCreate))
                {
                    Serializer.WriteObject(stream, _missions);
                }
            }
        }

        #endregion



        private void ConstructMissions()
        {
            var mission = new MissionBuyStocks();
            _missions.Add(mission.MissionId, mission);
        }

        private void MissionUpdate(MissionUpdatedMessageType obj)
        {
            SaveMissions();
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
