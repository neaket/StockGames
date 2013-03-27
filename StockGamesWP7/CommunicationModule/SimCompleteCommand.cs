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
    public class SimCompleteCommand :ICommand
    {
        private Mutex myStateMutex;
        private ServerEntity myServer;

        public SimCompleteCommand(Mutex stateMutex)
        {
            myStateMutex = stateMutex;
        }
        
        public bool CanExecute(object parameter)
        {
            throw new NotImplementedException();
        }

        public event EventHandler CanExecuteChanged;

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
               myStateMutex.ReleaseMutex();
               throw;
            }

            myStateMutex.ReleaseMutex();
        }
    }
}
