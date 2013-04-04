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
using System.IO.IsolatedStorage;
using System.IO;
using StockGames.Persistence.V1.DataModel;
using StockGames.Persistence.V1.Services;
using System.Collections.Generic;
using StockGames.CommunicationModule.EVWriters;
using StockGames.CommunicationModule.Parsers;

namespace StockGames.CommunicationModule
{
    public class CommunicationManager
    {
        private const string SERVER_URI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp/";
        private const string MODEL_NAME = "TestUnit";
        
        private static readonly CommunicationManager instance =
            new CommunicationManager(new ServerEntity(SERVER_URI, new NetworkCredential("andrew", "andrew")));

        private ServerEntity hostServer;
        public string currentModel { get; private set; }

        private Dictionary<string, ModelManger> models = new Dictionary<string,ModelManger>(); 

        public CommunicationManager(ServerEntity myServer)
        {
            //Setup Server instance
            hostServer = myServer;

            currentModel = "Brownian";   //default value for active model

            //Create File and Directory Structure for in/out files for Server
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storage.DirectoryExists("StockGamesModel"))
                {
                    storage.CreateDirectory("StockGamesModel");

                    using (var file = storage.CreateFile(@"StockGamesModel\simulation.txt"))
                    {
                        using (var writer = new StreamWriter(file))
                        {
                            writer.Write("some text;");
                        }
                    }
                }
            }

            //Setup Models
            ModelManger tempModel = new ModelManger("BrownianMotion", "CD++Models/BrownianMotion", "CD++Models/BrownianMotion/BrownianMotion.xml", "BrownianNew", 24, new BrownianEVWriter(), new BrownianParser());
            //models.Add("Sawtooth", tempModel);
            //models.Add("Random", new ModelManger("Random", @"CD++Models/Random", @"CD++Models/Random/Random.xml", "TestUnit", 24, new RandomEVWriter(), new RandomParser()));
            models.Add("Brownian", tempModel);
            
        }

        public static CommunicationManager GetInstance
        {
            get { return instance; }
        }

        public void requestStockUpdate(string stockIndex)
        {
            hostServer.createCommThread(stockIndex, models[currentModel]);
        }

        public ModelManger getModel(string modelName)
        {
            return models[modelName];
        }
    }
}
