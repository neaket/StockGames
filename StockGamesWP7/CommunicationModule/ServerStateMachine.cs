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
    /// <summary>
    /// Enum that is representative of state the server is currently in
    /// </summary>
    public enum ProcessState
    {
        /// <summary> Ready state for the server /// </summary>
        Ready,      
        /// <summary> Setup state for the server/// </summary>
        Setup,      
        /// <summary> Running state for the server/// </summary>
        Running,    
        /// <summary> Done state for the server/// </summary>
        Done        
    }

    /// <summary>
    /// Enum that is representative of transitions bewteen the states of the server 
    /// </summary>
    public enum Command
    {
        /// <summary> transition representing posting a model to the server/// </summary>
        PostModel,      
        /// <summary> transition representing a request to start the simulation on the server/// </summary>
        StartSim,       
        /// <summary> transition representing checking the RISE server's current status/// </summary>
        CheckStatus,    
        /// <summary> transition representing getting the result of a completed simulation/// </summary>
        GetResults,     
        /// <summary> transition representing the simulation completing/// </summary>
        SimComplete,    
        /// <summary> transition representing aborting the simulation/// </summary>
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
                { new StateTransition(ProcessState.Ready, Command.PostModel), ProcessState.Setup },
                { new StateTransition(ProcessState.Ready, Command.Abort), ProcessState.Ready },
                { new StateTransition(ProcessState.Setup, Command.CheckStatus), ProcessState.Setup },
                { new StateTransition(ProcessState.Setup, Command.StartSim), ProcessState.Running },
                { new StateTransition(ProcessState.Setup, Command.Abort), ProcessState.Ready },
                { new StateTransition(ProcessState.Running, Command.CheckStatus), ProcessState.Running },
                { new StateTransition(ProcessState.Running, Command.Abort), ProcessState.Ready },
                { new StateTransition(ProcessState.Done, Command.GetResults), ProcessState.Ready },
                { new StateTransition(ProcessState.Done, Command.Abort), ProcessState.Ready },
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