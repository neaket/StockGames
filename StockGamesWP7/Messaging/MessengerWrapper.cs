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
using GalaSoft.MvvmLight.Messaging;

namespace StockGames.Messaging
{
    /// <summary>
    /// A simple messenger wrapper class that sending functionality can be disabled.  
    /// Intended to ensure no UI updating message is handled on a non-UI thread.
    /// </summary>
    public class MessengerWrapper
    {
        public static bool MessengerEnabled { get; set; }

        static MessengerWrapper()
        {
            MessengerEnabled = true;
        }
        
        public static void Send<TMessage, TTarget>(TMessage message)
        {
            if (!MessengerEnabled) return;

            Messenger.Default.Send<TMessage, TTarget>(message);
        }
        public static void Send<TMessage>(TMessage message)
        {
            if (!MessengerEnabled) return;

            Messenger.Default.Send(message);
        }
        public static void Send<TMessage>(TMessage message, object token)
        {
            if (!MessengerEnabled) return;

            Messenger.Default.Send(message, token);
        }
    }
}
