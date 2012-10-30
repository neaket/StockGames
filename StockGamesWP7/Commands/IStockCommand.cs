using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace StockGames.Commands
{
    interface IStockCommand : ICommand
    {
        /// <summary>
        /// Gets the string representation of a StockCommand's name
        /// </summary>
        /// <returns>
        /// the string command name
        /// </returns>
        string GetCmdName();
    }
}
