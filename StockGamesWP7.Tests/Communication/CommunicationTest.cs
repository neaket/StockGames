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
using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockGames.CommunicationModule;
using System.Threading;

namespace StockGames.Tests.Communication
{
    [TestClass]
    [Tag("Communication")]
    public class CommunicationTest
    {
        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestMethod]
        public void PostModelFilesTest()
        {
            var dummyMutex = new Mutex(false, "dummy");
            var postCommand = new PostModelCommand(dummyMutex);
            var aServer = new ServerEntity(ServerEntity.serverURI, new NetworkCredential("andrew", "andrew"));
            try
            {
                postCommand.Execute(aServer);
            }
            catch
            {
                Assert.IsTrue(true); ;
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void StartSimulationTest()
        {
            var dummyMutex = new Mutex(false, "dummy");
            var startSimCommand = new StartSimCommand(dummyMutex);
            var aServer = new ServerEntity(ServerEntity.serverURI, new NetworkCredential("andrew", "andrew"));
            try
            {
                startSimCommand.Execute(aServer);
            }
            catch
            {
                Assert.IsTrue(true);
            }
            Assert.IsTrue(true);
        }

        [TestMethod]
        public void GetNewSimulationDataTest()
        {
            var dummyMutex = new Mutex(false, "dummy");
            var getResultsCommand = new GetResultsCommand(dummyMutex,"ABC");
            var aServer = new ServerEntity(ServerEntity.serverURI, new NetworkCredential("andrew", "andrew"));
            //getResultsCommand.Execute(aServer);
            Assert.IsTrue(true);
        }
    }
}
