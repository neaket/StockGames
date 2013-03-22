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
    /// Wraps the send methods of <see cref="GalaSoft.MvvmLight.Messaging.IMessenger"/>. The
    /// messenger wrapper has the ability to disable sending messages, mainly when the database is
    /// being freshly created.  Intended to ensure no UI updating message is handled on a non-UI
    /// thread.
    /// </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public static class MessengerWrapper
    {
        /// <summary>   Gets or sets a value indicating whether the messenger is enabled. </summary>
        ///
        /// <value> true if the messenger is enabled, false if not. </value>
        public static bool MessengerEnabled { get; set; }

        static MessengerWrapper()
        {
            MessengerEnabled = true;
        }

        /// <summary>
        /// If MessengerEnabled is true.  Sends a message to registered recipients. The message will
        /// reach only recipients that registered for this message type using one of the Register methods,
        /// and that are of the targetType.
        /// </summary>
        ///
        /// <typeparam name="TMessage"> The type of message that will be sent. </typeparam>
        /// <typeparam name="TTarget">  The type of recipients that will receive the message. The message
        ///                             won't be sent to recipients of another type. </typeparam>
        /// <param name="message">  The message to send to registered recipients. </param>
        public static void Send<TMessage, TTarget>(TMessage message)
        {
            if (!MessengerEnabled) return;

            // Only send messages on the UI Thread
            Deployment.Current.Dispatcher.BeginInvoke(() => Messenger.Default.Send<TMessage, TTarget>(message));
        }

        /// <summary>
        /// If MessengerEnabled is true.  Sends a message to registered recipients. The message will
        /// reach only recipients that registered for this message type using one of the Register methods,
        /// and that are of the targetType.
        /// </summary>
        ///
        /// <typeparam name="TMessage"> The type of message that will be sent. </typeparam>
        /// <param name="message">  The message to send to registered recipients. </param>
        public static void Send<TMessage>(TMessage message)
        {
            if (!MessengerEnabled) return;

            // Only send messages on the UI Thread
            Deployment.Current.Dispatcher.BeginInvoke(() => Messenger.Default.Send(message));
        }

        /// <summary>
        /// If MessengerEnabled is true.  Sends a message to registered recipients. The message will
        /// reach only recipients that registered for this message type using one of the Register methods,
        /// and that are of the targetType.
        /// </summary>
        ///
        /// <typeparam name="TMessage"> The type of message that will be sent. </typeparam>
        /// <param name="message">  The message to send to registered recipients. </param>
        /// <param name="token">    A token for a messaging channel. If a recipient registers using a
        ///                         token, and a sender sends a message using the same token, then this
        ///                         message will be delivered to the recipient. Other recipients who did
        ///                         not use a token when registering (or who used a different token) will
        ///                         not get the message. Similarly, messages sent without any token, or
        ///                         with a different token, will not be delivered to that recipient.. </param>
        public static void Send<TMessage>(TMessage message, object token)
        {
            if (!MessengerEnabled) return;

            // Only send messages on the UI Thread
            Deployment.Current.Dispatcher.BeginInvoke(() => Messenger.Default.Send(message, token));
        }
    }
}
