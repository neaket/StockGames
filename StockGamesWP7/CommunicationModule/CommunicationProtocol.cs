using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace StockGames.CommunicationModule
{
    public sealed class CommunicationProtocol
    {
        private static CommunicationProtocol instance = null;

        private MessageHandler messageHandler;
        private MessageCoder messageCoder;

        private Object clientReference;

        private CommunicationProtocol()
        {
            messageHandler = MessageHandler.Instance;
            messageCoder = MessageCoder.Instance;

            messageCoder.AddMessageHandler(messageHandler);
        }

        public static CommunicationProtocol Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommunicationProtocol();
                }
                return instance;
            }
        }

        public void AddEvent(MessageEvent evnt)
        {
            messageHandler.AddEvent(evnt);

            if (!messageHandler.IsRunning())
            {
                messageHandler.RunHandler();
            }
        }

        public void AddClientReference(Object reference)
        {
            clientReference = reference;
        }

        public MessageHandler GetMessageHandler()
        {
            return messageHandler;
        }
    }
}
