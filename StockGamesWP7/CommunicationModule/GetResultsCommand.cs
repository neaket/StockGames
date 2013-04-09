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
    /// <summary>
    /// Command that is used to implement the transition funtionality for the 
    /// get simulation results transition
    /// Should be called when a get simulation results transition occurs
    /// </summary>
    ///
    /// <remarks>   Andrew Jeffery, 3/1/2013. </remarks>
    public class GetResultsCommand : ICommand
    {
        private ServerEntity myServer;
        private Mutex myStateMutex;
        private string currentStock;

        /// <summary>
        /// Command that is used to implement the transition funtionality for the 
        /// get simulation results transition
        /// Should be called when a get simulation results transition occurs
        /// </summary>
        public GetResultsCommand(Mutex stateMutex, string stockIndex)
        {
            myStateMutex = stateMutex;
            currentStock = stockIndex;
        }

        /// <summary>
        /// required by interface, not implemented due to time constraints
        /// </summary>
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// eventhandler for signaling if the executability has changed
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// when call performs a request to the RISE and fetchs the data zip file
        /// </summary>
        /// <param name="parameter"></param>
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
