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

        private Queue<IMessage> messageQueue;
        private List<MessageEventArgs> eventQueue;
        private MessageCoder messageCoder;

        public bool IsRunning
        {
            get;
            private set;
        }

        private MessageHandler()
        {
            eventQueue = new List<MessageEventArgs>(eventBufferSize);
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

        public void RemoveMessageEvent(MessageEventArgs messageEvent)
        {
            eventQueue.RemoveAt(messageEvent.EventIdentifier);
        }

        public void AddEvent(MessageEventArgs messageEvent)
        {
            //TODO Write code that willdo something is the list is full
            if (eventQueue.Count < eventBufferSize)
            {
                int index = 0;
                foreach (MessageEventArgs evnt in eventQueue)
                {
                    if (evnt == null)
                    {
                        eventQueue.Insert(index, messageEvent);
                        messageEvent.EventIdentifier = index;
                    }
                    index++;
                }
                ClientMessage m = new ClientMessage(messageEvent.EventTime, messageEvent.StockInputValue);
                m.EventReference = index;
                messageQueue.Enqueue(m);
            }
        }
    }
}
