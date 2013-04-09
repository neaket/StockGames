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
    /// <summary>
    /// Command that is used to implement the transition funtionality for the 
    /// post model to server transition
    /// Should be called when a simulation post model to server transition occurs
    /// </summary>
    ///
    /// <remarks>   Andrew Jeffery, 3/1/2013. </remarks>
    public class PostModelCommand : ICommand
    {
        private ServerEntity myServer;
        private Mutex myStateMutex;

        public PostModelCommand(Mutex stateMutex)
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
                using (IsolatedStorageFile myStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    //delete extra ev files
                    var evfiles = myStorage.GetFileNames(System.IO.Path.Combine(myServer.getModelName(), "*.ev"));
                    foreach (string filename in evfiles)
                    {
                        myStorage.DeleteFile(filename);
                    }
                    //write new zipfile
                    myServer.currentModel.writeEV(myServer.getModelName(), null);
                    //delete extra zip files
                    var zipfiles = myStorage.GetFileNames(System.IO.Path.Combine(myServer.getModelName(), "*.zip"));
                    foreach (string filename in zipfiles)
                    {
                        myStorage.DeleteFile(filename);
                    }
                    //write new zipfile
                    ZipModule zipEngine = new ZipModule();
                    if (!myStorage.DirectoryExists(myServer.getModelName()))
                    {
                        throw new IsolatedStorageException("Model Directory does not exist!");
                    }
                    zipEngine.CreateZip( myServer.getModelName() + ".zip", null, myServer.getModelName());

                    HttpWebRequest request =
                        (HttpWebRequest)WebRequest.CreateHttp(new Uri(ServerEntity.serverURI + myServer.currentModel.domainName + "?zdir=" + myServer.getModelName()));
                    request.Method = "POST";
                    request.Credentials = myServer.serverCredentials;
                    request.ContentType = "application/zip";

                    //Sets up wait reset, waits until the stream has be retrieved before continuing
                    var requestWait = new ManualResetEvent(false); 
                    var requestResult = request.BeginGetRequestStream((ar) => requestWait.Set(), null); 
                    requestWait.WaitOne(); 
                   
                    Stream webStream = request.EndGetRequestStream(requestResult);
                    String targetpath = myServer.getModelName() + ".zip";

                    if (!myStorage.FileExists(targetpath))
                        throw new IsolatedStorageException("Model Zip doesnot exist!");

                    Stream myStream = new IsolatedStorageFileStream(targetpath, FileMode.Open, myStorage);
                    myStream.CopyTo(webStream);

                    //Close streams after writing data
                    webStream.Close();
                    myStream.Close();

                    var responseWait = new ManualResetEvent(false);
                    var responseResult = request.BeginGetResponse((ar) => responseWait.Set(), null);
                    responseWait.WaitOne();

                    HttpWebResponse response = request.EndGetResponse(responseResult) as HttpWebResponse;
                    if (!response.StatusCode.Equals(HttpStatusCode.OK))
                    {
                        //throw new WebException("Bad Http Status Code");
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
