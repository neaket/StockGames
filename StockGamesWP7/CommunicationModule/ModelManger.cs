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
using StockGames.CommunicationModule.EVWriters;
using StockGames.CommunicationModule.Parsers;

namespace StockGames.CommunicationModule
{
    public class ModelManger
    {

        public string modelName { get; private set; }
        public string modelXml { get; private set; }
        public string sourcePath { get; private set; }
        public string domainName { get; private set; }
        private IEVWriter evWriter;
        private IParser outParser;
        public int modelHourAdvance { get; private set; }

        public ModelManger(string name, string path, string xmlPath, string frameworkName, int hours, IEVWriter writer, IParser parser)
        {
            modelName = name;
            sourcePath = path;
            modelXml = xmlPath;
            domainName = frameworkName;
            evWriter = writer;
            outParser = parser;
            modelHourAdvance = hours;

            ModelWriter modelWriter = new ModelWriter();
            modelWriter.writeModeltoStorage(modelName, sourcePath, modelName);
        }

       public void writeEV(string path, string stockIndex)
        {
            using (var myStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //delete extra ev files
                var evfiles = myStorage.GetFileNames(System.IO.Path.Combine(path, "*.ev"));
                foreach (string filename in evfiles)
                {
                    myStorage.DeleteFile(filename);
                }
            }

            evWriter.writeEVFile(path, stockIndex);
            
        }

        public void parseZipFile(string filePath, string stockIndex)
        {
            outParser.parseZipFile(filePath, stockIndex);
        }

    }
}
