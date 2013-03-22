using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Messaging;

namespace StockGames.Persistence.V1
{
    // TODO this class should not be singleton, and ViewModels should not be able to access it

    /// <summary>
    /// This class is used to persist common state information for the application.
    /// </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    [DataContract]
    public class GameState
    {
        private const string DirectoryPath = "V1";
        private const string FilePath = DirectoryPath + @"\GameState";
        private static readonly DataContractSerializer Serializer = new DataContractSerializer(typeof(GameState));

        #region instance

        private static GameState _instance;

        /// <summary>   Gets the singleton GameState instance. </summary>
        ///
        /// <value> The instance. </value>
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
        }

        #endregion
        
        #region persistence

        /// <summary>   Persists the GameState into the ApplicationStorage of the phone. </summary>
        ///
        /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
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


        private DateTime _gameTime;
        /// <summary> Gets the current game time. </summary>
        [DataMember]
        public DateTime GameTime 
        { 
            get
            {
                return _gameTime;
            } 
            set { 
                _gameTime = value;
                Messenger.Default.Send(new GameTimeUpdatedMessageType(_gameTime));
            }
        }
    }
}
