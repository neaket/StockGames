﻿using System.Net;
using System.Threading;
using StockGames.Persistence.V1.DataModel;
using System;
using StockGames.Messaging;
using System.Windows;

namespace StockGames.CommunicationModule
{
    /// <summary>
    /// Class representation of the RISE server that the mobile client connects too
    /// </summary>
    ///
    /// <remarks>   Andrew Jeffery, 3/1/2013. </remarks>
    public class ServerEntity
    {
        /// <summary>
        /// URI of the RISE server simulation space
        /// </summary>
        public const string serverURI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp/";
        public NetworkCredential serverCredentials { get; private set; }
        public ModelManger currentModel{ get; private set; }
        public SimStates simStatus { get; private set; }
        public enum SimStates
        {
            IDLE,
            INIT,
            RUNNING,
            STOPPING,
            DONE,
            ABORTED,
            ERROR
        }

        private const string POST = "POST";
        private const string PUT = "PUT";

        private ServerStateMachine myServer;
        
        private static Mutex serverQueMutex = new Mutex(false, "ServerQue");
        
        public ServerEntity(string serverAddress, NetworkCredential hostCredentials)
        {
            myServer = new ServerStateMachine(this);
            serverCredentials = hostCredentials;
        }

        public void createCommThread(string stockIndex, ModelManger model)
        {
            currentModel = model;
            Work w = new Work(stockIndex, myServer, this, serverQueMutex);
            Thread thread = new Thread(new ThreadStart(w.StartSimulation));
            thread.Start();
        }

        public string getModelName()
        {
            return this.currentModel.modelName;
        }

        public void updateSimState(SimStates state)
        {
            simStatus = state;
        }
    }

    class Work
    {
        private string stockIndex;
        private ServerStateMachine myServer;
        private ServerEntity hostServer;
        private static Mutex serverQueMutex;
        private static Mutex stateMachineMutex = new Mutex(false, "StateMachine");
        
        public Work(string sIndex, ServerStateMachine stateMachine, ServerEntity server, Mutex queMutex)
        {
            stockIndex = sIndex;
            myServer = stateMachine;
            hostServer = server;
            serverQueMutex = queMutex;
        }

        public void StartSimulation()
        {
            serverQueMutex.WaitOne();
            try
            {
                myServer.MoveNext(Command.PostModel, new PostModelCommand(stateMachineMutex));
                myServer.MoveNext(Command.StartSim, new StartSimCommand(stateMachineMutex));
                //Initial Status Check
                myServer.MoveNext(Command.CheckStatus, new CheckStatusCommand(stateMachineMutex));
                while (!(hostServer.simStatus).Equals(ServerEntity.SimStates.DONE))
                {
                    Thread.Sleep(500);
                    //Continous Status Check
                    myServer.MoveNext(Command.CheckStatus, new CheckStatusCommand(stateMachineMutex));
                }
                myServer.MoveNext(Command.SimComplete, new SimCompleteCommand(stateMachineMutex));
                myServer.MoveNext(Command.GetResults, new GetResultsCommand(stateMachineMutex, (string)stockIndex));
                Messaging.MessengerWrapper.Send(new CommunicationCompletedType());
            }
            catch(Exception e) 
            {
                if (e is WebException)
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() => MessageBox.Show("Connection to Server Failed! \nTry again later."));
                    myServer.MoveNext(Command.Abort, new SimCompleteCommand(stateMachineMutex));
                }
                else
                    throw;
            }
            finally
            {
                //Release mutex
                serverQueMutex.ReleaseMutex();
            }
        }
    }

}
