using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using System.Collections.Generic;
using SharpGIS;
using System.IO;

namespace StockGames.CommunicationProtocol
{
    public sealed class MessageHandler
    {
        private static MessageHandler instance;

        private static bool ISRUNNING = false;
        private static int EVENTBUFFERSIZE = 250;
        private static string serverURI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp";
        private static string serverURITest = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp/life";

        private MessageEvent[] messageEvents;
        private int eHead;
        private int eTail;

        private  Queue<Message> messageQueue;
        private MessageCoder messageCoder;

        private MessageHandler()
        {
            messageEvents = new MessageEvent[EVENTBUFFERSIZE];
            eHead = eTail = 0;
            messageCoder = MessageCoder.Instance;
            messageQueue = new Queue<Message>();
        }

        public static MessageHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageHandler();
                }
                return instance;
            }
        }

        public void RunHandler()
        {
            ISRUNNING = true;
            while (ISRUNNING)
            {
                if (messageQueue.Count != 0)
                {
                    Message m = messageQueue.Dequeue();

                    if (m is ClientMessage)
                    {

                        messageCoder.EncodeMessage(m as ClientMessage);
                    }
                    else if (m is ServerMessage)
                    {
                        messageCoder.DecodeMessage(m as ServerMessage);
                    }
                }
                else ISRUNNING = false;
            }
        }

        public bool IsRunning()
        {
            return ISRUNNING;
        }

        public MessageEvent GetMessageEvent(int n)
        {
            return messageEvents[n];
        }

        public bool RequestServer()
        {
            Uri temp = new Uri(serverURITest + "/" + "results/");
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                temp);

            request.BeginGetResponse(GetSimulationCallback, request);

            return true;
        }

        public bool RequestServer2()
        {
            using (IsolatedStorageFile xmlFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (xmlFile.FileExists("message.xml"))
                {
                    string ServerMessage = "test";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                            new Uri(serverURI + "/" + xmlFile.GetFileNames("message.xml")));
                    
                    request.BeginGetResponse(GetSimulationCallback, request);
            
                    ServerMessage sm = new ServerMessage(ServerMessage, 3);
                    messageQueue.Enqueue(sm);
                    return true;
                }
                return false;
            }
        }

        private void GetSimulationCallback(IAsyncResult result)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();

            HttpWebRequest request = result.AsyncState as HttpWebRequest;
            if (request != null)
            {
                WebResponse response = request.EndGetResponse(result);
                
                using (IsolatedStorageFileStream fileStream = storage.CreateFile("resultstemp.zip"))
                {
                    if (fileStream != null)
                    {
                        response.GetResponseStream().CopyTo(fileStream);

                        // TODO put this in a separate method atleast
                        UnZipper un = new UnZipper(fileStream);
                        foreach (String filename in un.GetFileNamesInZip())
                        {
                            Stream stream = un.GetFileStream(filename);
                            StreamReader reader = new StreamReader(stream);
                            System.Diagnostics.Debug.WriteLine(reader.ReadLine());

                        }
                    }
                }                
            }
        }


        public void AddEvent(MessageEvent evnt)
        {
            if (eTail == EVENTBUFFERSIZE - 1)
            {
                //TODO
            }
            else if (eTail < EVENTBUFFERSIZE - 1)
            {
                evnt.SetEventNumber(eTail);
                messageEvents[eTail] = evnt;
                eTail += 1;

                ClientMessage m = new ClientMessage(evnt.GetStockReference(),
                    evnt.GetStockValue(), evnt.GetEventNumber());
                messageQueue.Enqueue(m);
            }
        }
    }
}
