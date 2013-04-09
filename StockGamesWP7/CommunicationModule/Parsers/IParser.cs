using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockGames.CommunicationModule.Parsers
{
    /// <summary>
    /// Interface used to determine the functionality that all ev file parser must implement
    /// </summary>
    ///
    /// <remarks>   Andrew Jeffery, 3/1/2013. </remarks>
    public interface IParser
    {
        /// <summary>
        /// All out file parses must be able to parse the zip file and retreive data they need specific to the 
        /// currrent model
        /// </summary>
 
        void parseZipFile(string zipFile, string stockIndex);
    }
}
