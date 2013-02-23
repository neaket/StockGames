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
using System.IO;

namespace StockGames.CommunicationModule
{
    public class ModelWriter
    {

        public void writeFiletoStorage(string filename, string writePath)
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isolatedStorage.FileExists(System.IO.Path.Combine(writePath, filename)))
                {
                    Stream stream = Application.GetResourceStream(new Uri(filename, UriKind.Relative)).Stream;
                    using (IsolatedStorageFileStream filestream = isolatedStorage.CreateFile(System.IO.Path.Combine(writePath, filename)))
                    {
                        stream.CopyTo(filestream);
                    }
                }
            }
        }

        public void writeModeltoStorage(string modelName, string writePath)
        {
            writeFiletoStorage(modelName + ".ev", writePath);
            writeFiletoStorage(modelName + ".ma", writePath);
            writeFiletoStorage(modelName + "Type.cpp", writePath);
            writeFiletoStorage(modelName + "Type.h", writePath);
        }

    }
}
