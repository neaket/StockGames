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

namespace StockGames.CommunicationProtocol
{
    public static class ServerCommunication
    {
        private static string serverURI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/DCDpp";
        private static string serverURITest = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp/life";

        private static string workSpaceXML = "<DCDpp><Servers>" + 
            "<Server IP=\"134.117.53.66\" PORT=\"8080\">" +
            "<MODEL>StockGames</MODEL></Server>" +
            "</Servers></DCDpp>";

        public static void CreateServerModelWorkspace()
        {
            Uri temp = new Uri(serverURI + "/" + "StockGames");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(temp);

            request.Method = "PUT";
            request.ContentType = "text/xml";
            request.BeginGetRequestStream(GetRequestStreamCallback, request);
        }

        //Only to be called when we need to update the model on the
        //Server
        private static void PostModelToServer()
        {
            Uri temp = new Uri(serverURITest + "/" + "StockGames?" + "");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(temp);

            request.Method = "POST";

            request.BeginGetResponse(GetSimulationCallback, request);
        }

        public static void StartSimulation()
        {
            Uri temp = new Uri(serverURITest + "/" + "simulation");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(temp);

            request.Method = "PUT";

            request.BeginGetResponse(GetSimulationCallback, request);
        }

        public static bool RequestServerResults()
        {
            Uri temp = new Uri(serverURITest + "/" + "results/");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(temp);

            request.BeginGetResponse(GetSimulationCallbackZip, request);

            return true;
        }

        private static void GetSimulationCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                WebResponse response = request.EndGetResponse(result);
                System.Diagnostics.Debug.WriteLine(response.ToString());
            }
        }

        private static void GetRequestStreamCallback(IAsyncResult result) 
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            Stream putStream = request.EndGetRequestStream(result);

            byte[] byteArray = System.Text.Encoding.UTF8.GetBytes(workSpaceXML);

            putStream.Write(byteArray, 0, byteArray.Length);
            putStream.Flush();
            putStream.Close();

            request.BeginGetResponse(GetSimulationCallback, request);
        }

        private static void GetResponseStreamCallback(IAsyncResult result)
        {
            HttpWebRequest request = (HttpWebRequest)result.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(result);
            using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream()))
            {
                string answer = httpWebStreamReader.ReadToEnd();
                //For debug: show results
                System.Diagnostics.Debug.WriteLine(answer);
            }
        }

        private static void GetSimulationCallbackZip(IAsyncResult result)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                WebResponse response = request.EndGetResponse(result);

                using (IsolatedStorageFileStream fileStream = storage.CreateFile("resultstemp.zip"))
                {
                    if (fileStream != null)
                    {
                        response.GetResponseStream().CopyTo(fileStream);

                        // TODO put this in a separate method atleast
                        UnZipper un = new UnZipper(fileStream);
                        foreach (String filename in un.GetFileNamesInZip())
                        {
                            Stream stream = un.GetFileStream(filename);
                            StreamReader reader = new StreamReader(stream);
                            System.Diagnostics.Debug.WriteLine(reader.ReadLine());

                        }
                    }
                }
            }
        }
    }
}
