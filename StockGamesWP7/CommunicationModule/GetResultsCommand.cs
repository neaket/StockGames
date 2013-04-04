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

                HttpWebRequest request = WebRequest.CreateHttp(ServerEntity.serverURI + myServer.currentModel.domainName + "/results");

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
                    myServer.currentModel.parseZipFile("StockGamesModel/SimulationResults.zip", currentStock);
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
