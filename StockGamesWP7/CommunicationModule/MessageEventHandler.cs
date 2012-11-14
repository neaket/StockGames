using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockGames.CommunicationModule
{
    class MessageEventHandler
    {

        private MessageCoder messageCoder;

        public MessageEventHandler(MessageEvent messageEvent, MessageCoder coder)
        {
            messageCoder = coder;
            messageEvent.MessageEv += new MessageEvent.MessageEventHandler(HandleMessage);
        }

        void HandleMessage(object sender, MessageEventArgs messageEv)
        {
            while (!messageEv.IsFinished)
            {
                //if the model wasn't updated with the corresponding ev file
                if (!messageEv.EventSent)
                {
                    messageCoder.EncodeMessage(messageEv);
                }
                if (!messageEv.SimulationStarted)
                {
                    //ServerCommunication.StartSimulation();
                    messageEv.IsFinished = true;
                }
            }
        }
    }
}
