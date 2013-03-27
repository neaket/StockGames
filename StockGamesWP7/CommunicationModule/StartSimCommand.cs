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
using System.Threading;

namespace StockGames.CommunicationModule
{
    public class StartSimCommand : ICommand
    {
        private ServerEntity myServer;
        private Mutex myStateMutex;

        public StartSimCommand(Mutex stateMutex)
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
                HttpWebRequest request =
                        (HttpWebRequest)WebRequest.CreateHttp(ServerEntity.serverURI + ServerEntity.domainName + "/simulation");
                request.Method = "PUT";
                request.Credentials = myServer.serverCredentials;
                request.ContentType = "text/xml";

                //Sets up wait reset, waits until the stream has be retrieved before continuing
                var wait_handle = new ManualResetEvent(false);
                var result = request.BeginGetRequestStream((ar) => wait_handle.Set(), null);
                wait_handle.WaitOne();

                WebResponse response = request.EndGetResponse(result);

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
