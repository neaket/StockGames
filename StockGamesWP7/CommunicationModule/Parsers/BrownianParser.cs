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
using StockGames.Persistence.V1.DataModel;
using StockGames.Persistence.V1.Services;
using System.IO;

namespace StockGames.CommunicationModule.Parsers
{
    /// <summary>
    /// Specific parser for the Brownian motion simulation model, writes stock prices to
    /// the database in isolated storage as new stock snap shots for each stock present
    /// </summary>
    ///
    /// <remarks>   Andrew Jeffery, 3/1/2013. </remarks>
    public class BrownianParser :IParser
    {
        public void parseZipFile(string zipFile, string stockIndex)
        {
            using (IsolatedStorageFile myStorage = IsolatedStorageFile.GetUserStoreForApplication())
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
                            string currentStock = null;
                            DateTime lastTombstone = new DateTime();
                            int stockCount=0;
                            var stocks = StockService.Instance.GetStocks();
                            int hours = 0;
                            DateTime[] tomestones = new DateTime[168];
                            Decimal[] prices = new Decimal[168];
                            foreach (string line in lines)
                            {
                                string[] words = line.Split(' ');
                                int arrayIndex = 0;
                                foreach (string word in words)
                                {
                                    if (word.Equals("outstockindex"))
                                    {
                                        stockCount++;
                                        
                                        if(hours > 0)
                                            StockService.Instance.AddStockSnapshots(currentStock, prices, tomestones);
                                        if (stockCount <= stocks.Length)
                                        {
                                            currentStock = stocks[stockCount - 1].StockIndex;
                                            StockSnapshotDataModel previousSnapShot = StockService.Instance.GetLatestStockSnapshot(currentStock);
                                            lastTombstone = new DateTime(previousSnapShot.Tombstone.Ticks);
                                        }
                                        hours = 0;
                                    }
                                    if (word.Equals("outstockprice") && currentStock!=null)
                                    {
                                        hours++;
                                        DateTime tombstone = lastTombstone.AddHours(hours);
                                        tomestones[hours-1] = tombstone;
                                        prices[hours -1] = Convert.ToDecimal(words[arrayIndex + 1]);
                                    }
                                    arrayIndex += 1;
                                }
                            }
                            StockService.Instance.AddStockSnapshots(currentStock, prices, tomestones);
                        }
                    }
                }
            }
        }


        private string convertIntIndextoString(int index)
        {
            int intIndex=index;
            char[] letters = new char[10];
            int count =0;
            while (intIndex > 0)
            {
                int intLetter = intIndex % 100;
                letters[count] = Convert.ToChar(intLetter);
                count++;
                intIndex = intIndex / 100;
            }
            char[] reverseArray = new char[letters.Length];
            for(int i = count; i>0; i--)
            {
                reverseArray[i - 1] = letters[count - i];
            }
            string stockIndex = reverseArray.ToString();
            return stockIndex;
        }
    }
}
