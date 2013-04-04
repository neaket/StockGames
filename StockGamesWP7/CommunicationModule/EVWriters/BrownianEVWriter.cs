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
using StockGames.Entities;

namespace StockGames.CommunicationModule.EVWriters
{
    public class BrownianEVWriter : IEVWriter
    {
        public void writeSingleStockEVFile(string outpath, string stockIndex)
        {
            var snapshot = StockService.Instance.GetLatestStockSnapshot(stockIndex);
            var stock = StockService.Instance.GetStock(stockIndex);

            using (var myStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var myFile = myStorage.CreateFile(System.IO.Path.Combine(outpath, "trial.ev")))
                {
                    using (var myStream = new StreamWriter(myFile))
                    {
                        myStream.WriteLine(string.Format("00:01:00:00 InStockIndex {0}", stock.StockIntegerIndex));
                        //myStream.WriteLine(string.Format("00:01:00:00 InStockPrice {0}", snapshot.Price));
                        //myStream.WriteLine("00:01:00:00 InTime 1");
                    }
                }
            }
        }

        public void writeEVFile(string outpath, string stockIndex)
        {
            //check to see if writing all stocks
            if (stockIndex == null)
            {
                using (var myStorage = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var myFile = myStorage.CreateFile(System.IO.Path.Combine(outpath, "trial.ev")))
                    {
                        using (var myStream = new StreamWriter(myFile))
                        {
                            var stocks = StockService.Instance.GetStocks();
                            foreach (StockEntity stock in stocks)
                            {
                                var snapshot = StockService.Instance.GetLatestStockSnapshot(stock.StockIndex);
                                myStream.WriteLine(string.Format("00:01:00:00 InStockIndex {0}", stock.StockIntegerIndex));
                                //myStream.WriteLine(string.Format("00:01:00:00 InStockPrice {0}", snapshot.Price));
                                //myStream.WriteLine("00:01:00:00 InTime 1");
                            }
                        }
                    }
                }
            }
            else
            {
                writeSingleStockEVFile(outpath, stockIndex);
            }
        }

    }
}
