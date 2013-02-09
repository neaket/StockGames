using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;

namespace StockGames.Persistence.V1
{
    /// <summary>
    /// This class is used to store common state information about the current game.
    /// </summary>
    [DataContract]
    public class GameState
    {
        private const string DirectoryPath = "V1";
        private const string FilePath = DirectoryPath + @"\GameState";
        private static readonly DataContractSerializer Serializer = new DataContractSerializer(typeof(GameState));

        #region instance

        private static GameState _instance;

        public static GameState Instance
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

        private GameState()
        {
            //TODO
            GameTime = DateTime.Now.AddHours(1);
        }

        #endregion
        
        #region persistence

        public void Save()
        {
            using (var isoFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isoFile.DirectoryExists(DirectoryPath))
                    isoFile.CreateDirectory(DirectoryPath);

                using (var stream = isoFile.OpenFile(FilePath, FileMode.OpenOrCreate))
                {
                    Serializer.WriteObject(stream, this);
                }
            }
        }

        private static GameState Load()
        {
            GameState gameState;

            using (var isoFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoFile.DirectoryExists(DirectoryPath) && isoFile.FileExists(FilePath))
                {
                    using (var stream = isoFile.OpenFile(FilePath, FileMode.OpenOrCreate))
                    {

                        gameState = (GameState) Serializer.ReadObject(stream);
                    }
                }
                else
                {
                    gameState = new GameState();
                }
            }

            return gameState;
        }

        #endregion
        
        /// <summary>
        /// True if a Game has already been created; otherwise false.
        /// </summary>
        [DataMember]
        public bool ExistingGame { get; set; }

        /// <summary>
        /// The id of the main portfolio.
        /// </summary>
        [DataMember]
        public int MainPortfolioId { get; set; }

        /// <summary> Gets the current game time. </summary>
        [DataMember]
        public DateTime GameTime { get; private set; }
    }
}
