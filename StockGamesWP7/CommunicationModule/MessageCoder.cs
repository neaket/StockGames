using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.IO.IsolatedStorage;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml;
using System.IO;
using System.Text;

namespace StockGames.CommunicationModule
{
    public sealed class MessageCoder
    {
        private static MessageCoder instance;
        public static MessageCoder Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new MessageCoder();
                }
                return instance;
            }
        }

        private MessageCoder() { }

        public void DecodeMessage()
        {

        }
       
        public void EncodeMessage(MessageEventArgs messageEvent)
        {
            IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication();
            IsolatedStorageFileStream fileStream = null;
            using (fileStream = storage.CreateFile("StockGamesModel/trial.ev"))
            {
                using (StreamWriter Writer = new StreamWriter(fileStream))
                {
                    string runTime = "00:00:00:05";
                    Writer.WriteLine(runTime + " InStockPrice " + messageEvent.StockInputValue.ToString());
                    Writer.WriteLine(runTime + " InTime " + messageEvent.EventTime.ToString());
                }
            }
            //TODO catch the error
            //ServerCommunication.UpdateModel();
            messageEvent.EventSent = true;
        }
    }
}
