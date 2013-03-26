using System.Net;
using System.Threading;
using StockGames.Persistence.V1.DataModel;

namespace StockGames.CommunicationModule
{
    public class ServerEntity
    {
        public const string serverURI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp/";
        public const string serverOutFile = "results/simOut_134_117_53_66_8080.out";
        public NetworkCredential serverCredentials { get; private set; }
        public string currentModel{ get; private set; }
        public const string domainName = "TestUnit";
        private SimStates simStatus;
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
        private static Mutex stateMachineMutex = new Mutex(false, "StateMachine");
        private static Mutex serverQueMutex = new Mutex(true, "ServerQue");
        private static StockSnapshotDataModel StockSnapshot;

        public ServerEntity(string serverAddress, NetworkCredential hostCredentials)
        {
            myServer = new ServerStateMachine(this);
            currentModel = "Sawtooth";
            ModelWriter modelconstructor = new ModelWriter();
            modelconstructor.writeModeltoStorage("Sawtooth", "CD++Models/Sawtooth", @"StockGamesModel\ServerModels\Sawtooth");
        }

        public void createCommThread()
        {
            //Lock or Wait on server Que mutex
            serverQueMutex.WaitOne();
            Thread thread = new Thread(new ThreadStart(StartSimulation));
            thread.Start();
        }

        public void StartSimulation()
        {
            myServer.MoveNext(Command.PostModel, new PostModelCommand());
            myServer.MoveNext(Command.StartSim, new StartSimCommand());
            //Initial Status Check
            myServer.MoveNext(Command.CheckStatus, new CheckStatusCommand());
            while(simStatus.Equals(SimStates.RUNNING))
            {
                Thread.Sleep(500);
                //Continous Status Check
                myServer.MoveNext(Command.CheckStatus, new CheckStatusCommand());
            }
            myServer.MoveNext(Command.SimComplete, new SimCompleteCommand());
            myServer.MoveNext(Command.GetResults, new GetResultsCommand());

            //Release mutex
            serverQueMutex.ReleaseMutex();
        }

        
        private HttpWebRequest createPostRequest()
        {
            HttpWebRequest request =
                (HttpWebRequest) WebRequest.CreateHttp(serverURI + domainName);
            request.Method = POST;
            request.Credentials = serverCredentials;
            request.ContentType = "text/xml";
            return request;
        }

        private HttpWebRequest createPutRequest()
        {
            HttpWebRequest request =
                (HttpWebRequest) WebRequest.CreateHttp(serverURI + domainName);
            request.Method = PUT;
            request.Credentials = serverCredentials;
            request.ContentType = "text/xml";
            return request;
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
}
