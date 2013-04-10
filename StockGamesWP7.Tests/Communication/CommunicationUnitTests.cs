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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Phone.Testing;
using StockGames.CommunicationModule;
using System.IO.IsolatedStorage;
using StockGames.CommunicationModule.EVWriters;
using StockGames.CommunicationModule.Parsers;


namespace StockGames.Tests.Communication
{
    [TestClass]
    [Tag("Communication")]
    public class CommunicationUnitTests
    {
        private string dir = "TestDirectory";
        private string zipDir = "ZipDirectory";

        [TestInitialize]
        public void Initialize()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storage.DirectoryExists(zipDir)) { storage.CreateDirectory(zipDir); }
            }
        }

        [TestMethod]
        public void ModelWriterTest()
        {
            ModelWriter mw = new ModelWriter();
            mw.writeModeltoStorage("BrownianMotion", "CD++Models\\BrownianMotion" ,dir);

            using(IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                Assert.IsTrue(storage.FileExists(dir + "\\BrownianMotion.ma"));
            }
        }

        [TestMethod]
        public void ZipModuleTest()
        {
            ZipModule zm = new ZipModule();
            zm.CreateZip(zipDir + ".zip", null, dir);

            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                Assert.IsTrue(storage.FileExists(zipDir + ".zip"));
            }
        }

        [TestMethod]
        public void ServerStateMachineTest() 
        {
            ServerStateMachine sm = new ServerStateMachine(new ServerEntity("www.google.com", new NetworkCredential("dumb", "dumber")));
            try
            {
                sm.GetNext(Command.GetResults);
            }catch
            {
                Assert.IsTrue(true);
            }
            try
            {
                sm.GetNext(Command.CheckStatus);
            }
            catch
            {
                Assert.IsTrue(true);
            } 
            Assert.IsTrue(sm.GetNext(Command.PostModel).Equals(ProcessState.Setup));
            try
            {
                sm.GetNext(Command.StartSim);
            }
            catch
            {
                Assert.IsTrue(true);
            }
        }

        [TestMethod]
        public void ModelManagerTest()
        {
            RandomEVWriter rev = new RandomEVWriter();
            RandomParser rp = new RandomParser();
            ModelManger mm = new ModelManger("BrownianMotion", dir, "test", "REST", 9000, rev, rp);

            Assert.Equals(mm.modelName, "BrownianMotion");
            Assert.Equals(mm.sourcePath, dir);
            Assert.Equals(mm.modelXml, "test");
            Assert.Equals(mm.domainName, "REST");
            Assert.Equals(mm.modelHourAdvance, 9000);
        }

        [TestMethod]
        public void createModelEV()
        {
            RandomEVWriter rev = new RandomEVWriter();
            RandomParser rp = new RandomParser();
            ModelManger mm = new ModelManger("BrownianMotion", "CD++Models/BrownianMotion", "CD++Models/BrownianMotion/BrownianMotion.xml", "BrownianNew", 168, new TestEVWriter(), new BrownianParser());

            using (var storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                storage.CreateDirectory("test");
                mm.writeEV("test", "ABC");
                Assert.IsTrue(storage.FileExists("test\\trial.ev"));
            }
        }

    }

    class TestEVWriter : IEVWriter
    {
        public void writeEVFile(string outpath, string stockIndex)
        {
            ModelWriter mw = new ModelWriter();
            mw.writeModeltoStorage("test", "CD++Models\\BrownianMotion", "test");    
        }
    }
};
