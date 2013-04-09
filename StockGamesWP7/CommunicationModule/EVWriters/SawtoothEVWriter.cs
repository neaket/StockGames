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
using StockGames.Persistence.V1.Services;
using System.IO.IsolatedStorage;
using System.IO;

namespace StockGames.CommunicationModule.EVWriters
{
    /// <summary>
    /// Specific ev writer for the Sawtooth simulation model, writes ev to
    /// isolated storage
    /// </summary>
    ///
    /// <remarks>   Andrew Jeffery, 3/1/2013. </remarks>
    public class SawtoothEVWriter : IEVWriter
    {
        public void writeEVFile(string outpath, string stockIndex)
        {
            var snapshot = StockService.Instance.GetLatestStockSnapshot(stockIndex);

            using (var myStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var myFile = myStorage.CreateFile(System.IO.Path.Combine(outpath, "trial.ev")))
                {
                    using (var myStream = new StreamWriter(myFile))
                    {
                        myStream.WriteLine(string.Format("00:01:00:00 InStockPrice {0}", snapshot.Price));
                        myStream.WriteLine("00:01:00:00 InTime 1");
                    }
                }
            }
        }
    }
}
