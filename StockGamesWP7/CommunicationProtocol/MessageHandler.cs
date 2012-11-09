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
using System.IO;

namespace StockGames.CommunicationProtocol
{
    public sealed class MessageHandler
    {
        private static MessageHandler instance;

        private static bool ISRUNNING = false;
        private static int EVENTBUFFERSIZE = 250;

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
