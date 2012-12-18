﻿using System;
using System.Diagnostics;
using System.Windows.Input;

namespace StockGames.MVVM
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        public RelayCommand(Action<object> execute, Predicate<object> canExecute )
        {
            if (execute == null) 
                throw new ArgumentNullException("execute");
            _execute = execute;
            _canExecute = canExecute;
        }

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        [DebuggerStepThrough]
        public void Execute(object parameter)
        {
            _execute(parameter);
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { throw new NotImplementedException(); }
            remove { throw new NotImplementedException(); }
        }
    }
}
