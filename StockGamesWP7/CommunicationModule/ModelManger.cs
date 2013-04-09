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
    /// <summary>
    /// Tracks all model specifc funtions and attributes for one given model
    /// </summary>
    ///
    /// <remarks>   Andrew Jeffery, 3/1/2013. </remarks>
    public class ModelManger
    {
        /// <summary>
        /// Attribute used to store the model name
        /// </summary>
        public string modelName { get; private set; }

        /// <summary>
        /// path to the xml file used for framework setup on the rise server in the application space
        /// </summary>
        public string modelXml { get; private set; }

        /// <summary>
        /// path to the model folder in the application space
        /// </summary>
        public string sourcePath { get; private set; }

        /// <summary>
        /// name of the framework space on the RISE server
        /// </summary>
        public string domainName { get; private set; }
        private IEVWriter evWriter;
        private IParser outParser;

        /// <summary>
        /// Amount of hours that the simulation provides
        /// </summary>
        public int modelHourAdvance { get; private set; }

        /// <summary>
        /// ModelManger acts as a collection point for any model specific values of functions
        /// </summary>
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
