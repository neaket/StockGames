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

        private Message[] messageQueue;

        private MessageCoder messageCoder;

        private int queueHead;
        private int queueTail;

        private MessageQueue()
        {
            messageQueue = new Message[QUEUESIZE];
            queueHead = queueTail = 0;
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

        public void AddMessageCoder(MessageCoder coder)
        {
            messageCoder = coder;
        }

        public int QueueFilled()
        {
            return queueTail;
        }

        public void Pop()
        {
            if (messageQueue[queueHead] is ClientMessage)
            {
                ClientMessage message = (ClientMessage)messageQueue[queueHead];

                QueueHelper();
                queueTail -= 1;

                messageCoder.EncodeMessage(message);
            }
            else if (messageQueue[queueHead] is ServerMessage)
            {
                ServerMessage message = (ServerMessage)messageQueue[queueHead];

                QueueHelper();
                queueTail -= 1;

                messageCoder.DecodeMessage(message);
            }
        }

        public void Push(Message message)
        {
            if (queueTail + 1 < QUEUESIZE - 1)
            {
                messageQueue[queueTail + 1] = message;
                queueTail += 1;
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
                if (i == QUEUESIZE - 1) messageQueue = tempQueue;  //full queue
                else tempQueue[i] = messageQueue[i + 1];
            }
        }
    }
}
