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
using StockGames.Models;
using StockGames.Controllers;

namespace StockGames.CommunicationModule
{
    public sealed class ServerCommunication
    {
        private StockEntity _Stock;
        private readonly static ServerCommunication instance = new ServerCommunication();

        private const string serverURI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp/";
        private const string modelName = "TestUnit";
        private const string serverOutFile = "results/simOut_134_117_53_66_8080.out";
        private const string userString = ""; //TODO create a way to create a testing framework unique us


        private const char SimulationNotStarted = 'n';
        private const char SimulationStarted = 's';
        private const char SimulationEnded = 'e';
        private const char SimulationResultsStored = 'r';

        private static char CommunicationState = SimulationNotStarted;

        private readonly NetworkCredential serverCredentials = new NetworkCredential("andrew", "andrew");

        private Stream stream;

        private ServerCommunication()
        {
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
            ModelWriter modelconstructor = new ModelWriter();
            modelconstructor.writeModeltoStorage("Sawtooth", @"StockGamesModel\ServerModels");
        }

        public static ServerCommunication GetInstance
        {
            get { return instance; }
        }

        public void StartSimulation(StockEntity stock)
        {
            //Create Model Zip File
//            ZipModule zipEngine = new ZipModule();
//            zipEngine.CreateZip("Sawtooth2.zip", null, "StockGamesModel/CD++Models/Sawtooth");

            HttpWebRequest request = WebRequest.CreateHttp(serverURI + modelName);

            stream = Application.GetResourceStream(new Uri("Sawtooth.zip", UriKind.Relative)).Stream;
           
            request.Method = "POST";
            request.ContentType = "application/zip";
            request.Credentials = new NetworkCredential("andrew", "andrew");
            //Trigger POST command for the model
            request.BeginGetRequestStream(new AsyncCallback(postModelBeginGetRequestStreamCallback), request);
          
           _Stock = stock;
        }

        private void postModelBeginGetRequestStreamCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            Stream putStream = request.EndGetRequestStream(result);

            stream.CopyTo(putStream);
            putStream.Close();
            stream.Close();

            //Trigger PUT command for starting simulation
            HttpWebRequest request2 = WebRequest.CreateHttp(serverURI + modelName + "/simulation");
            request2.Method = "PUT";
            request.ContentType = "text/xml";
            request2.Credentials = serverCredentials;
            request2.BeginGetResponse(new AsyncCallback(getStatusCodeCallback), request2);
        }

        public void SimulationStatusRequest()
        {
            HttpWebRequest request = WebRequest.CreateHttp(serverURI + modelName + "?sim=status");
            request.BeginGetResponse(new AsyncCallback(getStatusCodeCallback), request);
        }

        private void getStatusCodeCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;
                if (response.StatusCode == HttpStatusCode.OK && CommunicationState == SimulationNotStarted)
                {
                    HttpWebRequest request2 = WebRequest.CreateHttp(serverURI + modelName + "/simulation");
                    request2.Method = "PUT";
                    request2.ContentType = "text/xml";
                    request2.Credentials = serverCredentials;

                    request2.BeginGetRequestStream(new AsyncCallback(beginGetSimulationCallback), request2);
                }
                else if (response.StatusCode == HttpStatusCode.OK && CommunicationState == SimulationStarted)
                {
                    HttpWebRequest request3 = WebRequest.CreateHttp(serverURI + modelName + "?sim=status") as HttpWebRequest;
                    request3.BeginGetResponse(new AsyncCallback(beginGetSimulationStatusCallback), request3);
                }
                else if (response.StatusCode == HttpStatusCode.OK && CommunicationState == SimulationEnded)
                {
                    HttpWebRequest request4 = WebRequest.CreateHttp(serverURI + modelName + "/results");
                    request4.BeginGetResponse(new AsyncCallback(beginGetZipResponseStreamCallback), request4);
                }
            }
        }

        private void beginGetSimulationCallback(IAsyncResult result)
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                HttpWebRequest request = result.AsyncState as HttpWebRequest;
                using (Stream putStream = request.EndGetRequestStream(result))
                {

                    if (storage.FileExists(@"StockGamesModel\simulation.txt"))
                    {
                        IsolatedStorageFileStream ISStream = null;
                        using (ISStream = new IsolatedStorageFileStream(
                                @"C\simulation.txt", FileMode.OpenOrCreate, storage))
                        {
                            ISStream.CopyTo(putStream);
                        }
                    }
                }
                CommunicationState = SimulationStarted;

                request.BeginGetResponse(new AsyncCallback((blah) => { }), request);
                SimulationStatusRequest();
            }
        }

        private void beginGetSimulationStatusCallback(IAsyncResult result)
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                HttpWebRequest request = result.AsyncState as HttpWebRequest;
                if (request != null)
                {
                    WebResponse response = request.EndGetResponse(result);
                    using (var fileStream = new IsolatedStorageFileStream("StockGamesModel/SimStatus.xml",
                        FileMode.OpenOrCreate, storage))
                    {
                        response.GetResponseStream().CopyTo(fileStream);
                    }
                }
                string s = ParseXMLFile();
                switch (s)
                {
                    case "DONE":
                        CommunicationState = SimulationEnded;
                        HttpWebRequest request2 = WebRequest.CreateHttp(serverURI + modelName);
                        request2.BeginGetResponse(new AsyncCallback(getStatusCodeCallback), request2);
                        break;
                    default:
                        break;
                }
            }
        }

        private void beginGetZipResponseStreamCallback(IAsyncResult result)
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {

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
                CommunicationState = SimulationResultsStored;
                ReadSimulationOutput();
            }
        }

        public string ParseXMLFile()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {

                if (storage.FileExists("StockGamesModel/SimStatus.xml"))
                {
                    using (IsolatedStorageFileStream ISStream = new IsolatedStorageFileStream(
                            "StockGamesModel/SimStatus.xml", FileMode.Open, storage))
                    {
                        using (StreamReader reader = new StreamReader(ISStream))
                        {
                            string result;
                            for (string test = reader.ReadLine(); test != null; test = reader.ReadLine())
                            {
                                if (test.Contains("DONE")) { result = "DONE"; return result; }
                                else if (test.Contains("IDLE")) { result = "IDLE"; return result; }
                                else if (test.Contains("INIT")) { result = "INIT"; return result; }
                                else if (test.Contains("RUNNING")) { result = "RUNN"; return result; }
                                else if (test.Contains("STOPPING")) { result = "STOP"; return result; }
                                else if (test.Contains("ABORTED")) { result = "ABOR"; return result; }
                                else if (test.Contains("ERROR")) { result = "ERR"; return result; }
                            }
                        }
                    }
                }
                return "ERR";
            }
        }

        public ServerResponse ReadSimulationOutput()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                ServerResponse response = new ServerResponse(0, 0);

                if (storage.FileExists("StockGamesModel/SimulationResults.zip"))
                {
                    using (IsolatedStorageFileStream ISStream = new IsolatedStorageFileStream(
                            "StockGamesModel/SimulationResults.zip", FileMode.Open, storage))
                    {
                        UnZipper un = new UnZipper(ISStream);
                        foreach (String filename in un.GetFileNamesInZip())
                        {
                            Stream stream = un.GetFileStream(serverOutFile);
                            StreamReader reader = new StreamReader(stream);
                            string[] lines = reader.ReadToEnd().Split('\n');
                            foreach (string line in lines)
                            {
                                string[] words = line.Split(' ');
                                int arrayIndex = 0;
                                foreach (string word in words)
                                {
                                    if (word.Equals("outtime"))
                                    {
                                        response.Time = Int32.Parse(words[arrayIndex + 1]);
                                    }
                                    if (word.Equals("outstockprice"))
                                    {
                                        response.StockPrice = Int32.Parse(words[arrayIndex + 1]);
                                        //TODO: information for the update is here
                                    }
                                    arrayIndex += 1;
                                }
                                arrayIndex = 0;
                            }
                        }
                    }                    

                    _Stock.PreviousPrice = _Stock.CurrentPrice;
                    _Stock.CurrentPrice = response.StockPrice;
                    CommandInvoker CmdInvoker = CommandInvoker.Instance;
                    CmdInvoker.FetchCommand(CommandInvoker.CHANGE_STOCK_DATA, _Stock);
                    CommunicationState = SimulationNotStarted;
                }
                return response;
            }
        }
    }
}

