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
using System.IO;

namespace StockGames.CommunicationModule
{
    /// <summary>
    /// Command that is used to implement the simulation started transition funtionality
    /// Should be called when a simulation started transition occurs
    /// </summary>
    ///
    /// <remarks>   Andrew Jeffery, 3/1/2013. </remarks>
    public class StartSimCommand : ICommand
    {
        private ServerEntity myServer;
        private Mutex myStateMutex;

        /// <summary>
        /// Command that is used to implement the simulation started transition funtionality
        /// Should be called when a simulation started transition occurs
        /// </summary>
        public StartSimCommand(Mutex stateMutex)
        {
            myStateMutex = stateMutex;
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
                var textStream = Application.GetResourceStream(new Uri("simulation.txt", UriKind.Relative)).Stream;
                HttpWebRequest request =
                        (HttpWebRequest)WebRequest.CreateHttp(ServerEntity.serverURI + myServer.currentModel.domainName + "/simulation");
                request.Method = "PUT";
                request.Credentials = myServer.serverCredentials;
                request.ContentType = "text/xml";

                //Sets up wait reset, waits until the stream has be retrieved before continuing
                var wait_handle = new ManualResetEvent(false);
                var result = request.BeginGetRequestStream((ar) => wait_handle.Set(), null);
                wait_handle.WaitOne();

                var webStream = request.EndGetRequestStream(result);

                textStream.CopyTo(webStream);

                //Close streams after writing data
                webStream.Close();
                textStream.Close();

                var responseWait = new ManualResetEvent(false);
                var responseResult = request.BeginGetResponse((ar) => responseWait.Set(), null);
                responseWait.WaitOne();

                HttpWebResponse response = request.EndGetResponse(responseResult) as HttpWebResponse;
                if (!response.StatusCode.Equals(HttpStatusCode.OK))
                {
                    //throw new WebException("Bad Http Status Code");
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
