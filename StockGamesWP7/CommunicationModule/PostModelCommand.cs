﻿using System;
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
                    ZipModule zipEngine = new ZipModule();
                    if (!myStorage.DirectoryExists(@"StockGamesModel\ServerModels\" + myServer.getModelName()))
                    {
                        throw new IsolatedStorageException("Model Directory does not exist!");
                    }
                    zipEngine.CreateZip(System.IO.Path.Combine("StockGamesModel/ServerModels", myServer.getModelName() + ".zip"), null, @"StockGamesModel\ServerModels\" + myServer.getModelName());

                    HttpWebRequest request =
                        (HttpWebRequest)WebRequest.CreateHttp(ServerEntity.serverURI + ServerEntity.domainName);
                    request.Method = "POST";
                    request.Credentials = myServer.serverCredentials;
                    request.ContentType = "application/zip";

                    //Sets up wait reset, waits until the stream has be retrieved before continuing
                    var wait_handle = new ManualResetEvent(false); 
                    var result = request.BeginGetRequestStream((ar) => wait_handle.Set(), null); 
                    wait_handle.WaitOne(); 
                   
                    Stream webStream = request.EndGetRequestStream(result);
                    String targetpath = System.IO.Path.Combine("StockGamesModel/ServerModels", myServer.getModelName() + ".zip");

                    if (!myStorage.FileExists(targetpath))
                        throw new IsolatedStorageException("Model Zip doesnot exist!");

                    Stream myStream = new IsolatedStorageFileStream(targetpath, FileMode.Open, myStorage);
                    myStream.CopyTo(webStream);

                    //Close streams after writing data
                    webStream.Close();
                    myStream.Close();
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