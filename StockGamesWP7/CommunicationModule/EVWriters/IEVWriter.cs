using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StockGames.CommunicationModule.EVWriters
{
    public interface IEVWriter
    {
        void writeEVFile(string outpath, string stockIndex);
    }
}
