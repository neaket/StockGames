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
using System.Xml;
using System.IO;
using System.IO.IsolatedStorage;
using SharpGIS;
using System.Text;

namespace StockGames.CommunicationModule
{
    public static class ServerCommunication
    {
        private const string serverURI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp/";
        private const string modelName = "StockGamesTest3";
        private const string userString = ""; //TODO create a way to create a testing framework unique us

        private static NetworkCredential serverCredentials = new NetworkCredential("andrew", "andrew");

        public static void CreateStockGamesFramework()
        {
            HttpWebRequest request = WebRequest.CreateHttp(serverURI + modelName);

            request.Method = "PUT";
            request.ContentType = "text/xml";
            request.Credentials = serverCredentials;

            request.BeginGetRequestStream(new AsyncCallback(frameworkRequestCallback), request);
        }

        public static void UpdateModel()
        {
            HttpWebRequest request = WebRequest.CreateHttp(serverURI + modelName);

            request.Method = "Post";
            request.ContentType = "text/xml";
            request.Credentials = serverCredentials;

            request.BeginGetRequestStream(new AsyncCallback(beginGetUpdateModelCallback), request);
        }

        public static void StartSimulation()
        {
            HttpWebRequest request = WebRequest.CreateHttp(serverURI + modelName + "/simulation");

            request.Method = "PUT";
            request.ContentType = "text/xml";
            request.Credentials = serverCredentials;

            request.BeginGetRequestStream(beginGetSimulationCallback, request);
        }

        public static void RequestServerResults()
        {
            HttpWebRequest request = WebRequest.CreateHttp(serverURI + modelName + "/results");
            request.BeginGetResponse(beginGetZipResponseStreamCallback, request);
        }


        private static void frameworkRequestCallback(IAsyncResult result)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            Stream putStream = request.EndGetRequestStream(result);

            if(storage.FileExists("StockGamesModel/StockGames.xml"))
            {
                using (IsolatedStorageFileStream ISStream = new IsolatedStorageFileStream(
                    "StockGamesModel/StockGames.xml", FileMode.Open, storage))
                {
                    using (StreamReader reader = new StreamReader(ISStream))
                    {
                        using (StreamWriter writer = new StreamWriter(putStream, Encoding.UTF8))
                        {
                            writer.Write(reader.ReadToEnd());
                        }
                    }
                }
            }
            putStream.Close();

            request.BeginGetResponse(new AsyncCallback(beginGetStatusCallBack), request);
        }

        private static void beginGetUpdateModelCallback(IAsyncResult result)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            Stream putStream = request.EndGetRequestStream(result);

            if (storage.FileExists("StockGamesModel/trial.ev"))
            {
                using (IsolatedStorageFileStream ISStream = new IsolatedStorageFileStream(
                    "StockGamesModel/trial.ev", FileMode.Open, storage))
                {
                    using (StreamReader reader = new StreamReader(ISStream))
                    {
                        using (StreamWriter writer = new StreamWriter(putStream, Encoding.UTF8))
                        {
                            writer.Write(reader.ReadToEnd());
                        }
                    }
                }
            }
            putStream.Close();

            request.BeginGetResponse(new AsyncCallback(beginGetStatusCallBack), request);
        }

        private static void beginGetSimulationCallback(IAsyncResult result)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            Stream putStream = request.EndGetRequestStream(result);

            if(storage.FileExists("StockGamesModel/simulation.txt"))
            {
                using (IsolatedStorageFileStream ISStream = new IsolatedStorageFileStream(
                        "StockGamesModel/simulation.txt", FileMode.Open, storage))
                {
                    using (StreamReader reader = new StreamReader(ISStream))
                    {
                        using (StreamWriter writer = new StreamWriter(putStream, Encoding.UTF8))
                        {
                            writer.Write(reader.ReadToEnd());
                        }
                    }
                }
            }
            putStream.Close();
            request.BeginGetResponse(beginGetStatusCallBack, request);
        }

        private static void beginGetZipResponseStreamCallback(IAsyncResult result)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                WebResponse response = request.EndGetResponse(result);

                using (IsolatedStorageFileStream fileStream = storage.CreateFile("StockGamesModel/SimulationResults.zip"))
                {
                    if (fileStream != null)
                    {
                        response.GetResponseStream().CopyTo(fileStream);
                    }
                }
            }
        }

        private static void beginGetStatusCallBack(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;
            }
        }
    }
}
