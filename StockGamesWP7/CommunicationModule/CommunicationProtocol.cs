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
using System.Xml;
using System.Text;

namespace StockGames.CommunicationModule
{
    public sealed class CommunicationProtocol
    {
        private static CommunicationProtocol instance = null;
        private static bool FrameWorkCreated = false;

        public static CommunicationProtocol Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommunicationProtocol();
                }
                return instance;
            }
        }


        private CommunicationProtocol()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //Check the storage for a Properties file
                //If none no framework on the server
                if (!storage.DirectoryExists("StockGamesModel"))
                {
                    storage.CreateDirectory("StockGamesModel");
                    FrameWorkCreated = true;
                }
                if (!storage.FileExists(@"StockGamesModel\simulation.txt"))
                {
                    storage.CreateFile(@"StockGamesModel\simulation.txt");
                }
            }
            if (FrameWorkCreated)
            {
                CreateSimulationTextFile();
            }
        }

        private void CreateSimulationTextFile()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            if (!storage.FileExists(@"StockGamesModel\simulation.txt"))
            {
                IsolatedStorageFileStream stream = null;
                using (stream = storage.CreateFile(@"StockGamesModel\simulation.txt"))
                {
                    using (StreamWriter writer = new StreamWriter(stream))
                    {
                        writer.WriteLine("Run the Simulation");
                    }
                }
            }
        }
    }
}
