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
using System.Collections.Generic;

namespace StockGames.CommunicationModule
{
    
   public enum ProcessState
    {
        Ready,
        Setup,
        Running,
        Done
    }

    public enum Command
    {
        PostModel,
        StartSim,
        CheckStatus,
        GetResults,
        SimComplete,
        Abort
    }

   public class ServerStateMachine
    {

        private ServerEntity myServer;

        class StateTransition
        {
            readonly ProcessState CurrentState;
            readonly Command Command;

            public StateTransition(ProcessState currentState, Command command)
            {
                CurrentState = currentState;
                Command = command;
            }

            public override int GetHashCode()
            {
                return 17 + 31 * CurrentState.GetHashCode() + 31 * Command.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                StateTransition other = obj as StateTransition;
                return other != null && this.CurrentState == other.CurrentState && this.Command == other.Command;
            }
        }

        Dictionary<StateTransition, ProcessState> transitions;
        public ProcessState CurrentState { get; private set; }

        public ServerStateMachine(ServerEntity hostServer)
        {
            CurrentState = ProcessState.Ready;
            transitions = new Dictionary<StateTransition, ProcessState>
            {
                { new StateTransition(ProcessState.Setup, Command.PostModel), ProcessState.Ready },
                { new StateTransition(ProcessState.Ready, Command.CheckStatus), ProcessState.Ready },
                { new StateTransition(ProcessState.Ready, Command.StartSim), ProcessState.Running },
                { new StateTransition(ProcessState.Running, Command.CheckStatus), ProcessState.Running },
                { new StateTransition(ProcessState.Done, Command.GetResults), ProcessState.Ready },
                { new StateTransition(ProcessState.Running, Command.SimComplete), ProcessState.Done }
            };

            myServer = hostServer;
        }

        public ProcessState GetNext(Command command)
        {
            StateTransition transition = new StateTransition(CurrentState, command);
            ProcessState nextState;
            if (!transitions.TryGetValue(transition, out nextState))
                throw new Exception("Invalid transition: " + CurrentState + " -> " + command);
            return nextState;
        }

        public ProcessState MoveNext(Command command, ICommand operation)
        {
            CurrentState = GetNext(command);
            operation.Execute(myServer);
            return CurrentState;
        }
    }

}