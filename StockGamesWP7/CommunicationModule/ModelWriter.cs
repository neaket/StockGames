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

        public void writeFiletoStorage(string filename, string sourcePath, string targetPath)
        {
            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!isolatedStorage.FileExists(System.IO.Path.Combine(targetPath, filename)))
                {
                    var resourceStream = Application.GetResourceStream(new Uri(System.IO.Path.Combine(sourcePath,filename), UriKind.Relative));
                    if (resourceStream != null)
                    {
                        //Check for Null path or Directory exists already, if not create it
                        if (!string.IsNullOrEmpty(targetPath) && !isolatedStorage.DirectoryExists(targetPath))
                            isolatedStorage.CreateDirectory(targetPath);

                        Stream stream = resourceStream.Stream;
                        using (IsolatedStorageFileStream filestream = isolatedStorage.CreateFile(System.IO.Path.Combine(targetPath, filename)))
                        {
                            stream.CopyTo(filestream);
                        }
                    }
                }
            }
        }

        public void writeModeltoStorage(string modelName, string sourcePath, string targetPath)
        {
            writeFiletoStorage(modelName + ".ev", sourcePath, targetPath);
            writeFiletoStorage(modelName + ".ma", sourcePath, targetPath);
            //writeFiletoStorage(modelName + "Type.cpp", sourcePath, targetPath);
            //writeFiletoStorage(modelName + "Type.h", sourcePath, targetPath);
        }

    }
}
