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

namespace StockGames.CommunicationModule
{
    public class StartSimCommand : ICommand
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
            HttpWebRequest request =
                    (HttpWebRequest)WebRequest.CreateHttp(ServerEntity.serverURI + ServerEntity.domainName + "/simulation");
            request.Method = "PUT";
            request.Credentials = myServer.serverCredentials;
            request.ContentType = "text/xml";

            request.BeginGetResponse(new AsyncCallback(getResponseCallback), request);
        }

        private void getResponseCallback(IAsyncResult result)
        {
            //do stuff if need at sim startup on client side
        }
    }
}
