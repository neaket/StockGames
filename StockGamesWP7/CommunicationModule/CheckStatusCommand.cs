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
using System.Threading;

namespace StockGames.CommunicationModule
{
    public class CheckStatusCommand : ICommand
    {
        private ServerEntity myServer;
        private Mutex myStateMutex;

        public CheckStatusCommand(Mutex stateMutex)
        {
            myStateMutex = stateMutex;
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

                HttpWebRequest request = WebRequest.CreateHttp(ServerEntity.serverURI + ServerEntity.domainName + "?sim=status");
                //Sets up wait reset, waits until the stream has be retrieved before continuing
                var wait_handle = new ManualResetEvent(false);
                var result = request.BeginGetResponse((ar) => wait_handle.Set(), null);
                wait_handle.WaitOne();

                using (IsolatedStorageFile myStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (request != null)
                    {
                        WebResponse response = request.EndGetResponse(result);
                        using (var fileStream = new IsolatedStorageFileStream("StockGamesModel/SimStatus.xml", FileMode.OpenOrCreate, myStorage))
                        {
                            response.GetResponseStream().CopyTo(fileStream);
                        }
                    }
                    ServerEntity.SimStates simState = ParseXMLFile();
                    myServer.updateSimState(simState);
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

        public ServerEntity.SimStates ParseXMLFile()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (storage.FileExists("StockGamesModel/SimStatus.xml"))
                {
                    using (IsolatedStorageFileStream ISStream = new IsolatedStorageFileStream("StockGamesModel/SimStatus.xml", FileMode.Open, storage))
                    {
                        using (StreamReader reader = new StreamReader(ISStream))
                        {
                            for (string test = reader.ReadLine(); test != null; test = reader.ReadLine())
                            {
                                if (test.Contains("DONE"))
                                    return ServerEntity.SimStates.DONE;
                                else if (test.Contains("IDLE"))
                                    return ServerEntity.SimStates.IDLE;
                                else if (test.Contains("INIT"))
                                    return ServerEntity.SimStates.INIT;
                                else if (test.Contains("RUNNING"))
                                    return ServerEntity.SimStates.RUNNING;
                                else if (test.Contains("STOPPING"))
                                    return ServerEntity.SimStates.STOPPING;
                                else if (test.Contains("ABORTED"))
                                    return ServerEntity.SimStates.ABORTED;
                                else if (test.Contains("ERROR"))
                                    return ServerEntity.SimStates.ERROR;
                            }
                        }
                    }
                }
                return ServerEntity.SimStates.ERROR;            }
        }
    }
}
