using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using StockGames.Commands;

namespace StockGames.CommunicationModule
{
    class MessageEvent
    {
        public delegate void MessageEventHandler(object sender, MessageEventArgs messageEvent);

        public event MessageEventHandler MessageEv;

        public void ActivateMessageEvent(int StockValue, int CurrentTime)
        {
            MessageEventArgs messageArgs = new MessageEventArgs(CurrentTime, StockValue);
            MessageEv(this, messageArgs);
        }
    }
}
