using System.Net;
using System.Threading;
using StockGames.Persistence.V1.DataModel;
using System;

namespace StockGames.CommunicationModule
{
    public class ServerEntity
    {
        public const string serverURI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp/";
        public const string serverOutFile = "results/simOut_134_117_53_66_8080.out";
        public NetworkCredential serverCredentials { get; private set; }
        public string currentModel{ get; private set; }
        public const string domainName = "TestUnit";
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
            currentModel = "Sawtooth";
            serverCredentials = hostCredentials;
            ModelWriter modelconstructor = new ModelWriter();
            modelconstructor.writeModeltoStorage("Sawtooth", "CD++Models/Sawtooth", @"Sawtooth");
        }

        public void createCommThread(string stockIndex)
        {
            //Lock or Wait on server Que mutex
            Work w = new Work(stockIndex, myServer, this, serverQueMutex);
            Thread thread = new Thread(new ThreadStart(w.StartSimulation));
            thread.Start();
        }

        public string getModelName()
        {
            return this.currentModel;
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
            }
            catch 
            {
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
