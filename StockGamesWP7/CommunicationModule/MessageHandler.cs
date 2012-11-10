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

namespace StockGames.CommunicationModule
{
    public sealed class MessageHandler
    {
        private static MessageHandler instance;
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

        private const int eventBufferSize = 250;

        private MessageEvent[] messageEvents;
        private int eHead;
        private int eTail;

        private  Queue<IMessage> messageQueue;
        private MessageCoder messageCoder;

        public bool IsRunning
        {
            get;
            private set;
        }

        private MessageHandler()
        {
            messageEvents = new MessageEvent[eventBufferSize];
            eHead = eTail = 0;
            messageCoder = MessageCoder.Instance;
            messageQueue = new Queue<IMessage>();
        }



        public void RunHandler()
        {
            IsRunning = true;
            while (IsRunning)
            {
                if (messageQueue.Count != 0)
                {
                    IMessage m = messageQueue.Dequeue();

                    if (m is ClientMessage)
                    {

                        messageCoder.EncodeMessage(m as ClientMessage);
                    }
                    else if (m is ServerMessage)
                    {
                        messageCoder.DecodeMessage(m as ServerMessage);
                    }
                }
                else IsRunning = false;
            }
        }

        

        public MessageEvent GetMessageEvent(int n)
        {
            return messageEvents[n];
        }

        public void AddEvent(MessageEvent messageEvent)
        {
            if (eTail == eventBufferSize - 1)
            {
                //TODO
            }
            else if (eTail < eventBufferSize - 1)
            {
                messageEvent.EventNumber = eTail;
                messageEvents[eTail] = messageEvent;
                eTail += 1;

                ClientMessage m = new ClientMessage(messageEvent.StockReference,
                    messageEvent.StockValue, messageEvent.EventNumber);
                messageQueue.Enqueue(m);
            }
        }
    }
}
