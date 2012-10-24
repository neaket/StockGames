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
    public class MessageQueue
    {
        private Message[] mQueue;
        private int mQueueSize;
        private MessageCoder mCoder;

        private int mHead;
        private int mTail;

        public MessageQueue(int size, MessageCoder coder)
        {
            mCoder = coder;

            mQueueSize = size;
            mQueue = new Message[mQueueSize];

            mHead = mTail = 0;
        }

        public void pop()
        {

        }

        public void push(Message m)
        {
            if (mTail + 1 < mQueueSize - 1)
            {
                mQueue[mTail + 1] = m;
                mTail += 1;
            }
        }
    }
}
