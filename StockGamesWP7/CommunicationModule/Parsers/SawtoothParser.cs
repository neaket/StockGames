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
using System.IO.IsolatedStorage;
using SharpGIS;
using System.IO;
using StockGames.Persistence.V1.DataModel;
using StockGames.Persistence.V1.Services;

namespace StockGames.CommunicationModule.Parsers
{
    /// <summary>
    /// Specific parser for the Sawtooth simulation model, writes stock price to
    /// the database in isolated storage as new stock snap shot
    /// </summary>
    ///
    /// <remarks>   Andrew Jeffery, 3/1/2013. </remarks>
    public class SawtoothParser : IParser
    {
        /// <summary>
        /// Specific parser for the Sawtooth simulation model, writes stock price to
        /// the database in isolated storage as new stock snap shot
        /// </summary>
        public void parseZipFile(string zipFile, string stockIndex)
        {
            using (IsolatedStorageFile myStorage = IsolatedStorageFile.GetUserStoreForApplication() )
            {
                using (IsolatedStorageFileStream ISStream = new IsolatedStorageFileStream(zipFile, FileMode.Open, myStorage))
                {
                    UnZipper un = new UnZipper(ISStream);
                    foreach (String filename in un.GetFileNamesInZip())
                    {
                        if (filename.Contains(".out"))
                        {
                            Stream stream = un.GetFileStream(filename);
                            StreamReader reader = new StreamReader(stream);
                            string[] lines = reader.ReadToEnd().Split('\n');
                            foreach (string line in lines)
                            {
                                string[] words = line.Split(' ');
                                int arrayIndex = 0;
                                foreach (string word in words)
                                {
                                    if (word.Equals("outstockprice"))
                                    {
                                        StockSnapshotDataModel previousSnapShot = StockService.Instance.GetLatestStockSnapshot(stockIndex);
                                        DateTime tombstone = previousSnapShot.Tombstone.AddHours(1);
                                        StockService.Instance.AddStockSnapshot(stockIndex, Convert.ToDecimal(words[arrayIndex + 1]), tombstone);
                                    }
                                    arrayIndex += 1;
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
