﻿using System;
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
using StockGames.Commands;

namespace StockGames.Controllers
{
    public class CommandInvoker
    {
        private List<IStockCommand> _commands;

        //Command String representations
        public static string REQUEST_UPDATE_STOCK = "REQUEST_UPDATE";
        public static string CHANGE_STOCK_DATA = "CHANGE_STOCK_DATA";

        private static CommandInvoker _Instance = null;
        public static CommandInvoker Instance
        {
            get
            {
                if (_Instance == null)
                {
                    // TODO add MessageHandler
                    _Instance = new CommandInvoker();
                }
                return _Instance;
            }
        }

        private CommandInvoker()
        {
            _commands = new List<IStockCommand>();

            //Populate List with Commands
            _commands.Add( new RequestStockUpdateCmd() );
            _commands.Add( new PushStockChangesCmd());
        }

        /// <summary>
        /// Fetchs and executes a requested command, if a command name matchs the string provided.
        /// </summary>
        /// <param name="cmdName">
        /// String name of the command to execute
        /// </param>
        /// <param name="o">
        /// arguement for the execute method of the command, may be null if unused
        /// </param>
        public void FetchCommand(string cmdName, object o)
        {
            foreach (IStockCommand cmd in _commands)
            {
                if ( cmd.CommandName.Equals(cmdName) )
                {
                    cmd.Execute(o);
                    return;
                }
            }
        }

    }
}
