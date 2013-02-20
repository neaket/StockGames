using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Windows;
using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SharpCompress.Writer;
using SharpCompress.Common;

namespace StockGames.Tests
{
    [TestClass]
    [Tag("Zip")]
    public class ZipTesting
    {
        [TestMethod]
        public void TestCreateZip()
        {
            // This example zips Resource Streams (Files added to the project using visual studio and set to "Content")
            // It could be adapted to use a *.ev file that is added to the IsolatedApplicationStorage
            var directory = @"SawToothFolder\";
            string[] filePaths = new string[] {"Sawtooth.ma", "SawtoothType.cpp", "SawtoothType.h", "trial.ev"}; //located in the SawToothFolder

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // Create the zip file in Isolated Storage
                using (var zip = store.OpenFile("SawToothNewZip.zip", FileMode.OpenOrCreate))
                {
                    using (var zipWriter = WriterFactory.Open(zip, ArchiveType.Zip, CompressionType.None)) // Note contains a lot of compression methods
                    {
                        foreach (var filePath in filePaths)
                        {
                            // open the Resource Files (could be adapted to read from isolated storage file or even a MemoryStream?)
                            var file = Application.GetResourceStream(new Uri(directory + filePath, UriKind.Relative));
                            // Add the file to the zip folder
                            zipWriter.Write(filePath, file.Stream);
                        }
                    }
                }
            }

        }
    }
}
