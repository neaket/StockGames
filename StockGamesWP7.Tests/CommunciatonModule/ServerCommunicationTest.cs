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
using System.IO;
using System.Threading;
using Microsoft.Silverlight.Testing;
using System.Text;
using SharpGIS;
using System.IO.IsolatedStorage;

namespace StockGames.Tests.CommunciatonModule
{
    [TestClass]
    public class ServerCommunicationTests : SilverlightTest
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

        //[TestMethod]
        //[Asynchronous]
        //public void PostModelToServerTest()
        //{
        //    stream = Application.GetResourceStream(new Uri("Sawtooth.zip", UriKind.Relative)).Stream;
        //    HttpWebRequest request = WebRequest.CreateHttp(new Uri(SERVERURI + "TestUnit?zdir=Sawtooth"));
        //    request.Method = "POST";
        //    request.ContentType = "application/zip";
        //    request.Credentials = new NetworkCredential("andrew", "andrew");

        //    request.BeginGetRequestStream(new AsyncCallback(postModelBeginGetRequestStreamCallback), request);
        //}

        [TestMethod]
        [Asynchronous]
        public void PostNewFilesToServerTest()
        {
            stream = Application.GetResourceStream(new Uri("update.zip", UriKind.Relative)).Stream;
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(SERVERURI + "TestUnit"));
            request.Method = "POST";
            request.ContentType = "text/plain";
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
            request.BeginGetResponse(new AsyncCallback(pollSimStatus), request);
        }

        private void pollSimStatus(IAsyncResult result)
        {
            EnqueueCallback(() =>
                {
                    HttpWebRequest request = (HttpWebRequest)result.AsyncState;

                    HttpWebResponse response = request.EndGetResponse(result) as HttpWebResponse;
                    System.Diagnostics.Debug.WriteLine(response.StatusDescription);
                    //if (response.GetResponseStream() != null)
                    //{
                      //  StreamReader writer = new StreamReader(response.GetResponseStream());
                        //System.Diagnostics.Debug.WriteLine(writer.ReadToEnd());
                    //}
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
