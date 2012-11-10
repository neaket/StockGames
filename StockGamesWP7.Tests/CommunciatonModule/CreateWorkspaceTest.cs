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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Threading;
using Microsoft.Silverlight.Testing;
using System.Text;

namespace StockGames.Tests.CommunciatonModule
{
    [TestClass]
    public class CreateWorkspaceTest : SilverlightTest
    {
        private const string SERVERURI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp/";
        private Stream stream;

        [TestMethod]
        [Asynchronous]
        public void TestServerStatus()
        {
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(SERVERURI));
            
            request.BeginGetResponse(GetCallBack, request);
        }

        [TestMethod]
        [Asynchronous]
        public void CreateFrameworkTest()
        {
            stream = Application.GetResourceStream(new Uri("fire.xml", UriKind.Relative)).Stream;
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(SERVERURI + "fire2"));
            request.Method = "PUT";
            request.ContentType = "text/xml";
            request.Credentials = new NetworkCredential("andrew", "andrew");
            request.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), request);
        }

        [TestMethod]
        [Asynchronous]
        public void PostModelToServerTest()
        {
            stream = Application.GetResourceStream(new Uri("fire.zip", UriKind.Relative)).Stream;
            HttpWebRequest request = WebRequest.CreateHttp(new Uri(SERVERURI + "fire2?zdir=fire"));
            request.Method = "POST";
            request.ContentType = "application/zip";
            request.Credentials = new NetworkCredential("andrew", "andrew");

            request.BeginGetRequestStream(new AsyncCallback(postModelBeginGetRequestStreamCallback), request);
        }

        private void postModelBeginGetRequestStreamCallback(IAsyncResult result)
        {
            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            Stream putStream = request.EndGetRequestStream(result);

            stream.CopyTo(putStream);
            putStream.Close();
            stream.Close();

            request.BeginGetResponse(new AsyncCallback(GetCallBack), request);
        }

        public void GetCallBack(IAsyncResult result)
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

        public void GetRequestStreamCallback(IAsyncResult result)
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

            request.BeginGetResponse(new AsyncCallback(GetCallBack), request);
        }
    }
}