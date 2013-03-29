using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockGames.CommunicationModule.EVWriters
{
    interface IEVWriter
    {
        public void writeEVFile(string outpath, string stockIndex);
    }
}
