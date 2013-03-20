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

namespace StockGames.CommunicationModule
{
    public class PostModelCommand :ICommand
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
            using (IsolatedStorageFile myStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                ZipModule zipEngine = new ZipModule();
                if(!myStorage.DirectoryExists(@"StockGamesModel\SeverModels\" + myServer.getModelName()) )
                {
                    throw new IsolatedStorageException("Model Directory does not exist!");
                }
                zipEngine.CreateZip(myServer.getModelName() + ".zip", null, @"StockGamesModel\SeverModels\" + myServer.getModelName());
                
                HttpWebRequest request =
                    (HttpWebRequest)WebRequest.CreateHttp(ServerEntity.serverURI + ServerEntity.domainName);
                request.Method = "POST";
                request.Credentials = myServer.serverCredentials;
                request.ContentType = "application/zip";

                request.BeginGetRequestStream(new AsyncCallback(requestStreamCallback), request);
            }
        }

        private void requestStreamCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            Stream webStream = request.EndGetRequestStream(result);

            string targetpath = System.IO.Path.Combine("StockGamesModel\SeverModels" , myServer.getModelName());
            targetpath = System.IO.Path.Combine(targetpath, myServer.getModelName()+".zip");

            using (IsolatedStorageFile myStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!myStorage.FileExists(targetpath))
                    throw new IsolatedStorageException("Model Zip doesnot exist!");

                Stream myStream = new IsolatedStorageFileStream( targetpath, FileMode.Open, myStorage);
                myStream.CopyTo(webStream);

                //Close streams after writing data
                webStream.Close();
                myStream.Close();
            }
         }

            
    }
}
