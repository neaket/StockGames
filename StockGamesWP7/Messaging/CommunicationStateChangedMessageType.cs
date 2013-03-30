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
using StockGames.CommunicationModule;

namespace StockGames.Messaging
{
    public class CommunicationStateChangedMessageType
    {
        public ProcessState CurrentState { get; private set; }

        public CommunicationStateChangedMessageType(ProcessState currentState) 
        {
            CurrentState = currentState;
        }
    }
}
