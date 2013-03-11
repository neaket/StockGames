using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Net;
using System.Text;
using System.Windows;
using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace StockGames.Tests.CommunciatonModule
{
    [TestClass]
    [Tag("Communication")]
    public class ServerCommunicationTests : WorkItemTest
    {
        private const string SERVERURI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp/";
        private Stream stream;

        [TestMethod]
        [Asynchronous]
        public void TestServerStatus()
        {
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(SERVERURI));
            request.BeginGetResponse(getCallBack, request);
        }

        [TestMethod]
        [Asynchronous]
        public void CreateFrameworkTest()
        {
            stream = Application.GetResourceStream(new Uri("Sawtooth.xml", UriKind.Relative)).Stream;
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(SERVERURI + "TestUnit"));
            request.Method = "PUT";
            request.ContentType = "text/xml";
            request.Credentials = new NetworkCredential("andrew", "andrew");
            request.BeginGetRequestStream(new AsyncCallback(getRequestStreamCallback), request);
        }

        [TestMethod]
        [Asynchronous]
        public void PostModelToServerTest()
        {
            stream = Application.GetResourceStream(new Uri("Sawtooth.zip", UriKind.Relative)).Stream;
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(SERVERURI + "TestUnit?zdir=Sawtooth"));
            request.Method = "POST";
            request.ContentType = "application/zip";
            request.Credentials = new NetworkCredential("andrew", "andrew");

            request.BeginGetRequestStream(new AsyncCallback(postModelBeginGetRequestStreamCallback), request);
        }

        [TestMethod]
        [Asynchronous]
        public void PostNewFilesToServerTest()
        {
            stream = Application.GetResourceStream(new Uri("update.zip", UriKind.Relative)).Stream;
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(SERVERURI + "TestUnit"));
            request.Method = "POST";
            request.ContentType = "application/zip";
            request.Credentials = new NetworkCredential("andrew", "andrew");

            request.BeginGetRequestStream(new AsyncCallback(postModelBeginGetRequestStreamCallback), request);
        }

        [TestMethod]
        [Asynchronous]
        public void StartSimulationTest()
        {
            //TODO try and figure out what the server really wants for input here, for now this works
            stream = Application.GetResourceStream(new Uri("simulation.txt", UriKind.Relative)).Stream;
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(SERVERURI + "TestUnit/simulation"));
            request.Method = "PUT";
            request.ContentType = "text/xml";
            request.Credentials = new NetworkCredential("andrew", "andrew");
            request.BeginGetRequestStream(new AsyncCallback(postModelBeginGetRequestStreamCallback), request);
        }

        [TestMethod]
        [Asynchronous]
        public void ZCheckSimulationStatus()
        {
            HttpWebRequest request = WebRequest.CreateHttp(SERVERURI + "?sim=status");
            request.BeginGetResponse(new AsyncCallback(getRequestStreamCallback), request);
        }

        private void pollSimStatus(IAsyncResult result)
        {
            EnqueueCallback(() =>
                {
                    IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
                    HttpWebRequest request = (HttpWebRequest)result.AsyncState;

                    HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;
                    using (IsolatedStorageFileStream fileStream = storage.CreateFile("SimResult.xml"))
                    {
                        if (fileStream != null)
                        {
                            response.GetResponseStream().CopyTo(fileStream);
                        }
                    }
                });
        }

        [TestMethod]
        [Asynchronous]
        public void GetSimulationResultsTest()
        {
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(SERVERURI + "StockGames3/results"));
            request.BeginGetResponse(new AsyncCallback(getCallBack), request);
        }

        private void postModelBeginGetRequestStreamCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            Stream putStream = request.EndGetRequestStream(result);

            stream.CopyTo(putStream);
            putStream.Close();
            stream.Close();

            request.BeginGetResponse(new AsyncCallback(getCallBack), request);
        }

        private void getCallBack(IAsyncResult result)
        {
            EnqueueCallback(() =>
            {
                HttpWebRequest request = (HttpWebRequest)result.AsyncState;

                HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;
                System.Diagnostics.Debug.WriteLine(response.StatusDescription);
                Assert.Equals(HttpStatusCode.OK, response.StatusCode);
            });

            EnqueueTestComplete();
        }

        private void getRequestStreamCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            Stream putStream = request.EndGetRequestStream(result);

            using (StreamReader sr = new StreamReader(stream))
            {
                using (StreamWriter writer = new StreamWriter(putStream, Encoding.UTF8))
                {
                    writer.Write(sr.ReadToEnd());
                }
            }
            putStream.Close();
            stream.Close();

            request.BeginGetResponse(new AsyncCallback(getCallBack), request);
        }
    }
}
