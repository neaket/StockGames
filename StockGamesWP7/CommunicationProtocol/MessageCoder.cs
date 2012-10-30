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
        private static string URIAddress;

        private MessageHandler handler;

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

        public void AddMessageHandler(MessageHandler h)
        {
            handler = h;
        }
       
        public void DecodeMessage(Message m)
        {
        }

        public void EncodeMessage(Message m)
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
            if (handler.RequestServer())
            {
                ClientMessage n = (ClientMessage)m;
                int eventNum = n.GetEventReference();

                handler.GetMessageEvent(eventNum).WasSent();

                if (! handler.IsRunning())
                {
                    handler.RunHandler();
                }
            }
        }
    }
}
