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

        public MessageCoder()
        {

        }

        public static MessageCoder Instance
        private MessageHandler messageHandler;

        public MessageCoder()
        {

        }

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
            handler = handler;
        }
       
        public void DecodeMessage(Message message)
        {
        }

        public void EncodeMessage(Message message)
        {
#if WINDOWS_PHONE
                IsolatedStorageFile xmlFile = IsolatedStorageFile.GetUserStoreForApplication();
#else
                IsolatedStorageFile xmlFile = IsolatedStorageFile.GetUserStoreForDomain();
#endif

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
