﻿using System;
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
    /// <summary>
    /// Top level class for all comunication functionality, works as a controller between communication and game play client
    /// </summary>
    /// 
    /// <remarks> Andrew Jeffery, 2/27/2013</remarks>
    public class CommunicationManager
    {
        private const string SERVER_URI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp/";
        private const string MODEL_NAME = "TestUnit";
        
        private static readonly CommunicationManager instance =
            new CommunicationManager(new ServerEntity(SERVER_URI, new NetworkCredential("andrew", "andrew")));

        private ServerEntity hostServer;

        /// <summary>
        /// string representation of the current model
        /// </summary>
        public string currentModel { get; private set; }

        private Dictionary<string, ModelManger> models = new Dictionary<string,ModelManger>();

        /// <summary>
        /// Constructor for all comunication functionality, works as a controller between communication and game play client
        /// </summary>
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
            ModelManger tempModel = new ModelManger("BrownianMotion", "CD++Models/BrownianMotion", "CD++Models/BrownianMotion/BrownianMotion.xml", "BrownianNew", 168, new BrownianEVWriter(), new BrownianParser());
            //models.Add("Sawtooth", tempModel);
            //models.Add("Random", new ModelManger("Random", @"CD++Models/Random", @"CD++Models/Random/Random.xml", "TestUnit", 24, new RandomEVWriter(), new RandomParser()));
            models.Add("Brownian", tempModel);
            
        }
        /// <summary>
        /// Used to get the only active instance of this class, used to enforce singleton pattern
        /// </summary>
        public static CommunicationManager GetInstance
        {
            get { return instance; }
        }

        /// <summary>
        /// starts the communication protocol to obtain news simulation data from the server
        /// </summary>
        /// <param name="stockIndex"></param>
        public void requestStockUpdate(string stockIndex)
        {
            hostServer.createCommThread(stockIndex, models[currentModel]);
        }

        /// <summary>
        /// gets the current model based on its string index
        /// </summary>
        /// <param name="modelName"></param>
        /// <returns></returns>
        public ModelManger getModel(string modelName)
        {
            return models[modelName];
        }
    }
}
