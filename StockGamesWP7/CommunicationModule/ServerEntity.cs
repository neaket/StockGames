using System.Net;
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

        /// <summary>
        /// the login credentials for the RISE server framework
        /// </summary>
        public NetworkCredential serverCredentials { get; private set; }

        /// <summary>
        /// Collection of attributes and functions for the current model
        /// </summary>
        public ModelManger currentModel{ get; private set; }

        /// <summary>
        /// state of the current simulation on the RISE server
        /// </summary>
        public SimStates simStatus { get; private set; }

        /// <summary>
        /// flags represting the states of the RISE server simulation
        /// </summary>
        public enum SimStates
        {
            /// <summary> the state of the server when no simulation is ongoing /// </summary>
            IDLE,
            /// <summary> the state of the server when a simulation is setuping and compiling /// </summary>
            INIT,
            /// <summary> the state of the server when a simulation is currently running /// </summary>
            RUNNING,
            /// <summary> the state of the server when a simulation has been stopped /// </summary>
            STOPPING,
            /// <summary> the state of the server when a simulation has completed/// </summary>
            DONE,
            /// <summary> the state of the server when a simulation has been aborted /// </summary>
            ABORTED,
            /// <summary> the state of the server when setup or simulation error has occurred /// </summary>
            ERROR
        }

        private const string POST = "POST";
        private const string PUT = "PUT";

        private ServerStateMachine myServer;
        
        private static Mutex serverQueMutex = new Mutex(false, "ServerQue");
        
        /// <summary>
        /// Constructor used to link a serverEntity to its state machine, and stores the networ credentials
        /// </summary>
        /// <param name="serverAddress"></param>
        /// <param name="hostCredentials"></param>
        public ServerEntity(string serverAddress, NetworkCredential hostCredentials)
        {
            myServer = new ServerStateMachine(this);
            serverCredentials = hostCredentials;
        }

        /// <summary>
        /// Creates a Work thread to handle the client server communication
        /// </summary>
        /// <param name="stockIndex"></param>
        /// <param name="model"></param>
        public void createCommThread(string stockIndex, ModelManger model)
        {
            currentModel = model;
            Work w = new Work(stockIndex, myServer, this, serverQueMutex);
            Thread thread = new Thread(new ThreadStart(w.StartSimulation));
            thread.Start();
        }

        /// <summary>
        /// getter for the string representation of the current active simulation model
        /// </summary>
        /// <returns></returns>
        public string getModelName()
        {
            return this.currentModel.modelName;
        }
        
        /// <summary>
        /// updates the server simulation state to the requested value
        /// </summary>
        /// <param name="state"></param>
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
