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

namespace StockGames.CommunicationProtocol
{
    public sealed class CommunicationProtocol
    {
        private static CommunicationProtocol instance;

        private MessageQueue mQueue;
        private MessageHandler mHandler;
        private MessageCoder mCoder;

        private Object clientReference;

        private CommunicationProtocol()
        {
            mQueue = MessageQueue.Instance;
            mHandler = MessageHandler.Instance;
            mCoder = MessageCoder.Instance;

            mQueue.AddMessageCoder(mCoder);
            mHandler.AddMessageQueue(mQueue);
            mCoder.AddMessageHandler(mHandler);
        }

        public static CommunicationProtocol Init
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

        public void AddEvent(MessageEvent e)
        {
            mHandler.AddEvent(e);

            if (!mHandler.IsRunning())
            {
                mHandler.RunHandler();
            }
        }

        public void AddClientReference(Object reference)
        {
            clientReference = reference;
        }

        public MessageHandler GetMessageHandler()
        {
            return mHandler;
        }

        public MessageQueue GetMessageQueue()
        {
            return mQueue;
        }
    }
}
