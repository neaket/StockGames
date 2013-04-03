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
                            foreach (string line in lines)
                            {
                                string[] words = line.Split(' ');
                                int arrayIndex = 0;
                                foreach (string word in words)
                                {
                                    if (word.Equals("outIndex"))
                                    {
                                        currentStock = convertIntIndextoString(Convert.ToInt32(word));
                                    }
                                    if (word.Equals("outprice") && currentStock!=null)
                                    {
                                        StockSnapshotDataModel previousSnapShot = StockService.Instance.GetLatestStockSnapshot(currentStock);
                                        DateTime tombstone = previousSnapShot.Tombstone.AddHours(1);
                                        StockService.Instance.AddStockSnapshot(currentStock, Convert.ToDecimal(words[arrayIndex + 1]), tombstone);
                                    }
                                    arrayIndex += 1;
                                }
                            }
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
                letters[count] = Convert.ToChar(intIndex);
                count++;
                intIndex = intIndex / 100;
            }
            char[] reverseArray = new char[10];
            foreach (char letter in letters)
            {
                reverseArray[count - 1] = letter;
                count--;
            }
            string stockIndex = reverseArray.ToString();
            return stockIndex;
        }
    }
}
