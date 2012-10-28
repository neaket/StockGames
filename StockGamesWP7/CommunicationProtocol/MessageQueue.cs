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
    /*
     * 
    * */
    public sealed class MessageQueue
    {
        private static int QUEUESIZE = 256;
        private static MessageQueue instance;

        private Message[] mQueue;

        private MessageCoder mCoder;

        private int mHead;
        private int mTail;

        private MessageQueue()
        {
            mQueue = new Message[QUEUESIZE];
            mHead = mTail = 0;
        }

        public static MessageQueue Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageQueue();
                }
                return instance;
            }
        }

        public void AddMessageCoder(MessageCoder c)
        {
            mCoder = c;
        }

        public int QueueFilled()
        {
            return mTail;
        }

        public void Pop()
        {
            Message m = mQueue[mHead];

            QueueHelper();
            mTail -= 1;

            if (m is ClientMessage)
            {
                mCoder.EncodeMessage(m);
            }
            else if (m is ServerMessage)
            {
                mCoder.DecodeMessage(m);
            }
        }

        public void Push(Message m)
        {
            if (mTail + 1 < QUEUESIZE - 1)
            {
                mQueue[mTail + 1] = m;
                mTail += 1;
            }
            else
            {
                //have a message sent to the screen stating server is busy
            }
        }

        private void QueueHelper()
        {
            Message[] tempQueue = new Message[QUEUESIZE];
            for (int i = 0; i < QUEUESIZE; i++)
            {
                if (i == QUEUESIZE - 1) mQueue = tempQueue;  //full queue
                else tempQueue[i] = mQueue[i + 1];
            }
        }
    }
}
