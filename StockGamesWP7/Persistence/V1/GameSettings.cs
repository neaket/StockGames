using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace StockGames.Persistence.V1
{
    [DataContract]
    public class GameSettings
    {
        private const string FileName = "GameSettings";
        private static readonly DataContractSerializer Serializer = new DataContractSerializer(typeof(GameSettings));

        #region instance

        private static GameSettings _instance;

        public static GameSettings Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = Load();
                }
                return _instance;
            }
        }

        private GameSettings()
        {
        }

        #endregion
        
        #region persistence

        public void Save()
        {
            using (var isoFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var stream = isoFile.OpenFile(FileName, FileMode.OpenOrCreate))
                {
                    Serializer.WriteObject(stream, this);
                }
            }
        }

        private static GameSettings Load()
        {
            GameSettings gameSettings;

            using (var isoFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoFile.FileExists(FileName))
                {
                    using (var stream = isoFile.OpenFile(FileName, FileMode.OpenOrCreate))
                    {

                        gameSettings = (GameSettings) Serializer.ReadObject(stream);
                    }
                }
                else
                {
                    gameSettings = new GameSettings();
                }
            }

            return gameSettings;
        }

        #endregion
        
        [DataMember]
        public bool ExistingGame { get; set; }
    }
}
