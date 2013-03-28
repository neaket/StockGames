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
using System.Threading;
using StockGames.Persistence.V1.Services;
using StockGames.Persistence.V1.DataModel;

namespace StockGames.CommunicationModule
{
    public class GetResultsCommand : ICommand
    {
        private ServerEntity myServer;
        private Mutex myStateMutex;
        private string currentStock;

        public GetResultsCommand(Mutex stateMutex, string stockIndex)
        {
            myStateMutex = stateMutex;
            currentStock = stockIndex;
        }

        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            myStateMutex.WaitOne();

            try
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

                //Sets up wait reset, waits until the stream has be retrieved before continuing
                var wait_handle = new ManualResetEvent(false);
                var result = request.BeginGetResponse((ar) => wait_handle.Set(), null);
                wait_handle.WaitOne();

                using (IsolatedStorageFile myStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
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

                    //parser
                    using (IsolatedStorageFileStream ISStream = new IsolatedStorageFileStream("StockGamesModel/SimulationResults.zip", FileMode.Open, myStorage))
                    {
                        UnZipper un = new UnZipper(ISStream);
                        foreach (String filename in un.GetFileNamesInZip())
                        {
                            Stream stream = un.GetFileStream(ServerEntity.serverOutFile);
                            StreamReader reader = new StreamReader(stream);
                            string[] lines = reader.ReadToEnd().Split('\n');
                            foreach (string line in lines)
                            {
                                string[] words = line.Split(' ');
                                int arrayIndex = 0;
                                foreach (string word in words)
                                {
                                    if (word.Equals("outstockprice"))
                                    {
                                        StockSnapshotDataModel previousSnapShot = StockService.Instance.GetLatestStockSnapshot(currentStock);
                                        DateTime tombstone = previousSnapShot.Tombstone.AddHours(1);
                                        StockService.Instance.AddStockSnapshot(currentStock, Convert.ToDecimal(words[arrayIndex + 1]), tombstone);
                                    }
                                    arrayIndex += 1;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
                throw;
            }
            finally
            {
                myStateMutex.ReleaseMutex();
            }
        }
    }
}
