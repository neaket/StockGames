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

namespace StockGames.CommunicationProtocol
{
    public sealed class MessageHandler
    {
        private static MessageHandler instance;

        private static bool ISRUNNING = false;
        private static int EVENTBUFFERSIZE = 250;
        private static string serverURI = "http://134.117.53.66:8080/cdpp/sim/workspaces/andrew/dcdpp";

        private MessageEvent[] messageEvents;
        private int eHead;
        private int eTail;

        private MessageQueue messageQueue;

        private MessageHandler()
        {
            messageEvents = new MessageEvent[EVENTBUFFERSIZE];
            eHead = eTail = 0;
        }

        public static MessageHandler Instance
        {
            get
            {
                if (Instance == null)
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
                if (messageQueue.QueueFilled() != 0) messageQueue.Pop();
                else ISRUNNING = false;
            }
        }

        public void AddMessageQueue(MessageQueue queue)
        {
            messageQueue = queue;
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

#if WINDOWS_PHONE
            using (IsolatedStorageFile xmlFile = IsolatedStorageFile.GetUserStoreForApplication())
#else
            using(IsolatedStorageFile xmlFile = IsolatedStorageFile.GetUserStoreForDomain())
#endif
            {
                if (xmlFile.FileExists("message.xml"))
                {
                    string ServerMessage = "";

                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(
                        new Uri(serverURI + "/" + xmlFile.GetFileNames("message.xml")));

                    ServerMessage sm = new ServerMessage(ServerMessage, 3);
                    messageQueue.Push(sm);
                    return true;
                }
                return false;
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
                messageQueue.Push(m);
            }
        }
    }
}
