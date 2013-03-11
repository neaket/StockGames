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
using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Core;
using System.Text;

namespace StockGames.CommunicationModule
{
    public class ZipModule
    {

        private static int COMPRESSIONLEVEL = 0;       //0-9, 9 being the highest level of compression
        private static int BYTE_BUFFER_SIZE = 4096;

        // Recurses down the folder structure
        //
        private void CompressFolder(string path, ZipOutputStream zipStream, int folderOffset, IsolatedStorageFile isolatedStorage)
        {

            string[] files = isolatedStorage.GetFileNames(System.IO.Path.Combine(path, "*.*"));

            foreach (string filename in files)
            {
                string filenameWithPath = System.IO.Path.Combine(path, filename);
                // Makes the name in zip based on the folder
                string entryName = filenameWithPath.Substring(folderOffset);
                // Removes drive from name and fixes slash direction
                entryName = ZipEntry.CleanName(entryName);
                ZipEntry newEntry = new ZipEntry(entryName);
                // Note the zip format stores 2 second granularity
                newEntry.DateTime = isolatedStorage.GetLastWriteTime(filenameWithPath).DateTime;

                // To permit the zip to be unpacked by built-in extractor in WinXP and Server2003, WinZip 8, Java, and other older code,
                // you need to do one of the following: Specify UseZip64.Off, or set the Size.
                // If the file may be bigger than 4GB, or you do not need WinXP built-in compatibility, you do not need either,
                // but the zip will be in Zip64 format which not all utilities can understand.
                zipStream.UseZip64 = UseZip64.Off;
                using (IsolatedStorageFileStream stream = new IsolatedStorageFileStream(filenameWithPath, System.IO.FileMode.Open, isolatedStorage))
                {
                    newEntry.Size = stream.Length;
                }

                zipStream.PutNextEntry(newEntry);

                // Zip the file in buffered chunks
                // the "using" will close the stream even if an exception occurs
                byte[] buffer = new byte[BYTE_BUFFER_SIZE];
                using (IsolatedStorageFileStream streamReader = isolatedStorage.OpenFile(filenameWithPath, System.IO.FileMode.Open))
                {
                    StreamUtils.Copy(streamReader, zipStream, buffer);
                }
                zipStream.CloseEntry();
            }
            string[] folders = isolatedStorage.GetDirectoryNames(System.IO.Path.Combine(path, "*.*"));
            foreach (string folder in folders)
            {
                CompressFolder(System.IO.Path.Combine(path, folder), zipStream, folderOffset, isolatedStorage);
            }
        }

        // Compresses the files in the nominated folder, and creates a zip file on disk named as outPathname.
        // Make take a Password, Null disables the option 
        public void CreateZip(string outPathname, string password, string folderName)
        {

            using (IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (var fileStreamOut = new IsolatedStorageFileStream(outPathname, System.IO.FileMode.OpenOrCreate, isolatedStorage))
                {
                    ZipOutputStream zipStream = new ZipOutputStream(fileStreamOut);

                    zipStream.SetLevel(COMPRESSIONLEVEL);

                    zipStream.Password = password;  // optional. Null is the same as not setting.

                    // This setting will strip the leading part of the folder path in the entries, to
                    // make the entries relative to the starting folder.
                    // To include the full path for each entry up to the drive root, assign folderOffset = 0.

                    // int folderOffset = folderName.Length + (folderName.EndsWith("\\") ? 0 : 1); // currently not used for WP7
                    int folderOffset = 0;

                    CompressFolder(folderName, zipStream, folderOffset, isolatedStorage);

                    zipStream.Close();
                }
            }
        }


    }
}