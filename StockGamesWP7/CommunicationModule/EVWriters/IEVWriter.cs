using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockGames.CommunicationModule.EVWriters
{
    /// <summary>
    /// Interface used to allow IEWriter child to be interchangable so that a model can receive data it needs
    /// to run a simultation
    /// </summary>
    ///
    /// <remarks>   Andrew Jeffery, 3/1/2013. </remarks>
    public interface IEVWriter
    {
        /// <summary>
        /// All ev writers must have this core functionality, writes the ev file to the declared outpath
        /// </summary>
        void writeEVFile(string outpath, string stockIndex);
    }
}
