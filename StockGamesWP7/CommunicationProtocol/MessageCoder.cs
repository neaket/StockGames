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

        }

        private void toXML(Message m)
        {
        }
    }
}
