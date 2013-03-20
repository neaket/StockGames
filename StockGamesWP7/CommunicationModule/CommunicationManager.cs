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

namespace StockGames.CommunicationModule
{
    public class CommunicationManager
    {
        private const string SERVER_URI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp/";
        private const string MODEL_NAME = "TestUnit";
        private const string SERVER_OUT_FILE = "results/simOut_134_117_53_66_8080.out";

        private readonly static CommunicationManager instance = new CommunicationManager( new ServerEntity(SERVER_URI, new NetworkCredential("andrew", "andrew") ) );

        private ServerEntity hostServer;
        
        public CommunicationManager(ServerEntity myServer)
        {
            //Setup Server instance
            hostServer = myServer;

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
            //Write Models
            ModelWriter modelconstructor = new ModelWriter();
            modelconstructor.writeModeltoStorage("Sawtooth", "CD++Models/Sawtooth", @"StockGamesModel\ServerModels\Sawtooth");
        }

        public static CommunicationManager GetInstance
        {
            get { return instance; }
        }

        public void requestStockUpdate()
        {
            hostServer.createCommThread();
        }

    }
}
