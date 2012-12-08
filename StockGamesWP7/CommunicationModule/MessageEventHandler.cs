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
                    ServerCommunication.StartSimulation();
                    System.Diagnostics.Debug.WriteLine("Got Here");
                    messageEv.SimulationStarted = true;
                }

                else if (messageEv.SimulationStarted)
                {
                    //ServerCommunication.RequestSimulationStatus();
                    string simStatus = ServerCommunication.ParseXMLFile();
                    System.Diagnostics.Debug.WriteLine(simStatus);
                    switch (simStatus)
                    {
                        case "DONE":
                            messageEv.IsSimulated = true;
                            break;
                        default:
                            System.Diagnostics.Debug.WriteLine("Got Here2");
                            
                            break;
                    }
                }
                else if (messageEv.IsSimulated)
                {
                    System.Diagnostics.Debug.WriteLine("SERVERSIMDONE");
                    messageEv.IsFinished = true;
                }
            }
        }
    }
}
