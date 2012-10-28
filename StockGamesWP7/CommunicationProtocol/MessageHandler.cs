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
    public sealed class MessageHandler
    {
        private static int EVENTBUFFERSIZE = 250;
        private static MessageHandler instance;

        private MessageEvent[] mEvents;
        private int eHead;
        private int eTail;

        private MessageQueue mQueue;

        private MessageHandler()
        {
            mEvents = new MessageEvent[EVENTBUFFERSIZE];
            eHead = eTail = 0;
        }

        public static MessageHandler Instance
        {
            get
            {
                if (Instance == null)
                {
                    instance = new MessageHandler();
                }
                return instance;
            }
        }

        public void RunHandler()
        {
            while (true)
            {
                if (mQueue.QueueFilled() != 0) mQueue.Pop();    
            }
        }

        public void AddMessageQueue(MessageQueue q)
        {
            mQueue = q;
        }

        public void FetchServerResponse()
        {

        }

        public bool RequestServer()
        {
            return true;
        }

        public void AddEvent(MessageEvent e)
        {
            if (eTail == EVENTBUFFERSIZE - 1)
            {
                //TODO
            }
            else if (eTail < EVENTBUFFERSIZE - 1)
            {
                e.SetEventNumber(eTail);
                mEvents[eTail] = e;
                eTail += 1;

                ClientMessage m = new ClientMessage(e.GetStockReference(),
                    e.GetStockValue(), e.GetEventNumber());
                mQueue.Push(m);
            }
        }

        /*private void QueueHelper()
        {
            MessageEvent[] tempQueue = new MessageEvent[EVENTBUFFERSIZE];
            for (int i = 0; i < EVENTBUFFERSIZE; i++)
            {
                if (i == EVENTBUFFERSIZE - 1) mEvents = tempQueue;
                else tempQueue[i] = mEvents[i + 1];
            }
        }**/
    }
}
