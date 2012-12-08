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

        private static char SimulationNotStarted = 'n';
        private static char SimulationStarted = 's';
        private static char SimulationEnded = 'e';
        private static char SimulationResultsStored = 'r';

        private static char RequestState = SimulationNotStarted;

        private static NetworkCredential serverCredentials = new NetworkCredential("andrew", "andrew");

        public static void StartSimulation()
        {
            HttpWebRequest request = WebRequest.CreateHttp(serverURI + "/simulation");
            request.BeginGetResponse(new AsyncCallback(getStatusCodeCallback), request);
        }

        public static void SimulationStatusRequest()
        {
            HttpWebRequest request = WebRequest.CreateHttp(serverURI + "?sim=status");
            request.BeginGetResponse(new AsyncCallback(getStatusCodeCallback), request);
        }

        private static void getStatusCodeCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK && RequestState == SimulationNotStarted)
                {
                    HttpWebRequest request2 = WebRequest.CreateHttp(serverURI + "/simulation");
                    request2.Method = "PUT";
                    request2.ContentType = "text/xml";
                    request2.Credentials = serverCredentials;

                    System.Diagnostics.Debug.WriteLine("Test1");
                    request2.BeginGetRequestStream(new AsyncCallback(beginGetSimulationCallback), request2);
                }
                else if (response.StatusCode == HttpStatusCode.OK && RequestState == SimulationStarted)
                {
                    System.Diagnostics.Debug.WriteLine("Test2");
                    HttpWebRequest request3 = WebRequest.CreateHttp(serverURI + "?sim=status") as HttpWebRequest;
                    request3.BeginGetRequestStream(new AsyncCallback(beginGetSimulationStatusCallback), request3);
                }
                else if (response.StatusCode == HttpStatusCode.OK && RequestState == SimulationEnded)
                {
                    System.Diagnostics.Debug.WriteLine("Test3");
                    request.BeginGetRequestStream(new AsyncCallback(beginGetZipResponseStreamCallback), request);
                }
                else if (response.StatusCode == HttpStatusCode.OK && RequestState == SimulationResultsStored)
                {
                    System.Diagnostics.Debug.WriteLine("Test4");
                    //TODO: Code to update goes here
                }
            }
        }

        private static void beginGetSimulationCallback(IAsyncResult result)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            Stream putStream = request.EndGetRequestStream(result);

            if(storage.FileExists(@"StockGamesModel\simulation.txt"))
            {
                IsolatedStorageFileStream ISStream = null;
                using (ISStream = new IsolatedStorageFileStream(
                        @"StockGamesModel\simulation.txt", FileMode.Open, storage))
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
            RequestState = SimulationStarted;
            SimulationStatusRequest();
            //request.BeginGetResponse(new AsyncCallback(getStatusCodeCallback), result);
        }

        private static void beginGetSimulationStatusCallback(IAsyncResult result)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                WebResponse response = request.EndGetResponse(result);
                using (IsolatedStorageFileStream fileStream = storage.CreateFile("StockGamesModel/SimStatus.xml"))
                {
                    System.Diagnostics.Debug.WriteLine("Got Here CREATING XML");
                    response.GetResponseStream().CopyTo(fileStream);
                }
            }
            string s = ParseXMLFile();
            switch (s)
            {
                case "DONE":
                    RequestState = SimulationEnded;
                    request.BeginGetResponse(new AsyncCallback(getStatusCodeCallback), request);
                    break;
                default:
                    System.Diagnostics.Debug.WriteLine("Got Here2");
                    request.BeginGetResponse(new AsyncCallback(getStatusCodeCallback), request);
                    break;
            }
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
            RequestState = SimulationResultsStored;
        }

        public static string ParseXMLFile()
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            
            if(storage.FileExists("StockGamesModel/SimStatus.xml"))
            {
                using (IsolatedStorageFileStream ISStream = new IsolatedStorageFileStream(
                        "StockGamesModel/SimStatus.xml", FileMode.Open, storage))
                {
                    using (StreamReader reader = new StreamReader(ISStream))
                    {
                        string result;
                        while (true)
                        {
                            System.Diagnostics.Debug.WriteLine("Got Here PARSING");
                            string test = reader.ReadLine();
                            if (test.Contains("DONE")) { result = "DONE"; return result; }
                            else if (test.Contains("IDLE")) { result = "IDLE"; return result; }
                            else if (test.Contains("INIT")) { result = "INIT"; return result; }
                            else if (test.Contains("RUNNING")) { result = "RUNN"; return result; }
                            else if (test.Contains("STOPPING")) { result = "STOP"; return result; }
                            else if (test.Contains("ABORTED"))  { result = "ABOR"; return result; }
                            else if (test.Contains("ERROR")) { result = "ERR"; return result; }
                        }
                    }
                }
            }
            return "ERR";
        }
    }
}

