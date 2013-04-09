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
using System.Threading;

namespace StockGames.CommunicationModule
{
    /// <summary>
    /// Command that is used to implement the simulation complete transition funtionality
    /// Should be called when a simulation complete transtion occurs
    /// </summary>
    ///
    /// <remarks>   Andrew Jeffery, 3/1/2013. </remarks>
    public class SimCompleteCommand :ICommand
    {
        private Mutex myStateMutex;
        private ServerEntity myServer;

        /// <summary>
        /// Command that is used to implement the transition funtionality for the 
        /// simulation complete transition
        /// Should be called when a simulation complete transition occurs
        /// </summary>
        public SimCompleteCommand(Mutex stateMutex)
        {
            myStateMutex = stateMutex;
        }

        /// <summary>
        /// required by interface, not implemented due to time constraints
        /// </summary>
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// eventhandler for signaling if the executability has changed
        /// </summary>
        public event EventHandler CanExecuteChanged;

        /// <summary>
        /// Changes the state to the next one in the state transition
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter)
        {
            myStateMutex.WaitOne();
            try
            {
                if (parameter.GetType() == typeof(ServerEntity))
                {
                    myServer = (ServerEntity)parameter;
                }
                else
                {
                    throw new ArgumentException();
                }
            }
            catch
            {
               throw;
            }
            finally
            {
                myStateMutex.ReleaseMutex();
            }
        }
    }
}
