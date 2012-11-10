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

        public MessageHandler MessageHandler
        {
            get;
            private set;
        }

        private CommunicationProtocol()
        {
            MessageHandler = MessageHandler.Instance;
        }

        public void AddEvent(MessageEvent messageEvent)
        {
            MessageHandler.AddEvent(messageEvent);

            if (!MessageHandler.IsRunning)
            {
                MessageHandler.RunHandler();
            }
        }
    }
}
