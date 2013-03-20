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
using SharpGIS;
using System.IO;

namespace StockGames.CommunicationModule
{
    public class GetResultsCommand : ICommand
    {
        private ServerEntity myServer;

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (parameter.GetType() == typeof(ServerEntity))
            {
                myServer = (ServerEntity)parameter;
            }
            else
            {
                throw new ArgumentException();
            }

            HttpWebRequest request = WebRequest.CreateHttp(ServerEntity.serverURI + ServerEntity.domainName + "/results");
            request.BeginGetResponse(new AsyncCallback(getResponseCallback), request);
        }

        private void getResponseCallback(IAsyncResult result)
        {
            using (IsolatedStorageFile myStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                HttpWebRequest request = result.AsyncState as HttpWebRequest;
                if (request != null)
                {
                    WebResponse response = request.EndGetResponse(result);
                    using (IsolatedStorageFileStream myStream = myStorage.CreateFile("StockGamesModel/SimulationResults.zip"))
                    {
                        if (myStream != null)
                        {
                            response.GetResponseStream().CopyTo(myStream);
                        }
                    }
                }
                using (IsolatedStorageFileStream ISStream = new IsolatedStorageFileStream(
                            "StockGamesModel/SimStatus.xml", FileMode.Open, myStorage))
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
            }
        }
    }
}
