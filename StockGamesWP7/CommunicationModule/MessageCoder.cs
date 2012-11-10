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

namespace StockGames.CommunicationModule
{
    public sealed class MessageCoder
    {
        private static MessageCoder instance;
        public static MessageCoder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageCoder();
                }
                return instance;
            }
        }

        private MessageHandler messageHandler; 

        private MessageCoder() { }

        public void AddMessageHandler(MessageHandler handler)
        {
            messageHandler = handler;
        }
       
        public void DecodeMessage(ServerMessage message)
        {
            //TODO
            //message.MessageString;
        }

        public void EncodeMessage(ClientMessage message)
        {
            if (ServerCommunication.RequestServerResults())
            {
                int eventNum = message.EventReference;

                messageHandler.GetMessageEvent(eventNum).IsSent = true;

                if (!messageHandler.IsRunning)
                {
                    messageHandler.RunHandler();
                }
            }
        }
    }
}
