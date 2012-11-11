﻿using System;
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
using System.Xml;
using System.Text;

namespace StockGames.CommunicationModule
{
    public sealed class CommunicationProtocol
    {
        private static CommunicationProtocol instance = null;
        private static bool FrameWorkCreated = false;

        public static CommunicationProtocol Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new CommunicationProtocol();
                }
                return instance;
            }
        }

        public MessageHandler MessageHandler
        {
            get;
            private set;
        }

        private CommunicationProtocol()
        {
            MessageHandler = MessageHandler.Instance;

            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                //Check the storage for a Properties file
                //If none no framework on the server
                if (!storage.DirectoryExists("StockGamesModel"))
                {
                    storage.CreateDirectory("StockGamesModel");
                    FrameWorkCreated = true;
                }
            }
            if (FrameWorkCreated)
            {
                CreateModelXMLConfigFile();
            }
        }

        //TODO have this somewhere else, but for sprint 1 it is here!
        private void CreateModelXMLConfigFile()
        {
            using (IsolatedStorageFile storage = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (!storage.FileExists("StockGamesModel/StockGames.xml"))
                {
                    IsolatedStorageFileStream fileStream = null;
                    using (fileStream = storage.CreateFile("StockGamesModel/StockGames.xml"))
                    {
                        XmlWriterSettings settings = new XmlWriterSettings();
                        settings.Indent = true;
                        settings.Encoding = Encoding.UTF8;

                        using (XmlWriter writer = XmlWriter.Create(fileStream, settings))
                        {
                            writer.WriteStartElement("ConfigFramework");
                            writer.WriteElementString("Doc", "This model simulates a simple sawtooth "
                                + "graph for out initial testing of server comunication. It uses only "
                                + "one machine to do the simulation");

                            writer.WriteStartElement("Files");
                            writer.WriteStartElement("File");
                            writer.WriteAttributeString("ftype", "ma");
                            writer.WriteString("Sawtooth.ma");
                            writer.WriteEndElement();
                            writer.WriteStartElement("File");
                            writer.WriteAttributeString("ftype", "ev");
                            writer.WriteString("trial.ev");
                            writer.WriteEndElement();
                            writer.WriteEndElement();

                            writer.WriteStartElement("DCDpp");
                            writer.WriteStartElement("Servers");
                            writer.WriteStartElement("Server");
                            writer.WriteAttributeString("PORT", "8080");
                            writer.WriteAttributeString("IP", "localhost");
                            writer.WriteStartElement("Zone");
                            writer.WriteString("stocks(0,0)..(19,19)");
                            writer.WriteEndElement();
                            writer.WriteEndElement();
                            writer.WriteEndElement();

                            writer.Flush();
                            writer.Close();
                        }
                    }
                }
            }
            ServerCommunication.CreateStockGamesFramework();
        }

        public void AddEvent(MessageEvent messageEvent)
        {
            MessageHandler.AddEvent(messageEvent);

            if (!MessageHandler.IsRunning)
            {
                MessageHandler.RunHandler();
            }
        }
    }
}
