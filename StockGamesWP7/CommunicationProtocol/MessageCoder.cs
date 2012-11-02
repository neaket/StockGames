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
    public sealed class MessageCoder
    {
        private static MessageCoder instance;

        private MessageHandler messageHandler;

        public MessageCoder() {}

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

        public void AddMessageHandler(MessageHandler handler)
        {
            messageHandler = handler;
        }
       
        public void DecodeMessage(Message message)
        {
            ServerMessage m = (ServerMessage)message;
            m.GetMessageString();
        }

        public void EncodeMessage(Message message)
        {
            if (messageHandler.RequestServer())
            {
                ClientMessage n = (ClientMessage)message;
                int eventNum = n.GetEventReference();

                messageHandler.GetMessageEvent(eventNum).WasSent();

                if (!messageHandler.IsRunning())
                {
                    messageHandler.RunHandler();
                }
            }
        }

        public void EncodeMessage2(Message message)
        {
            IsolatedStorageFile xmlFile = IsolatedStorageFile.GetUserStoreForApplication();

            IsolatedStorageFileStream fs = null;
            using (fs = xmlFile.CreateFile("message.xml"))
            {
                if (fs != null)
                {
                    //TODO change data to the xml file
                }
            }
            if (messageHandler.RequestServer())
            {
                ClientMessage n = (ClientMessage)message;
                int eventNum = n.GetEventReference();

                messageHandler.GetMessageEvent(eventNum).WasSent();

                if (! messageHandler.IsRunning())
                {
                    messageHandler.RunHandler();
                }
            }
        }
    }
}
