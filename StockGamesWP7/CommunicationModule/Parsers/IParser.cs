using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockGames.CommunicationModule.Parsers
{
    public interface IParser
    {
        void parseZipFile(string zipFile, string stockIndex);
    }
}
