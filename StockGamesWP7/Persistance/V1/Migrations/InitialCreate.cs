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
using Microsoft.Phone.Data.Linq;
using StockGames.Models;
using StockGames.Persistance.V1.DataContexts;
using StockGames.Persistance.V1.DataModel;

namespace StockGames.Persistance.V1.Migrations
{
    public class InitialCreate
    {
        public const int Version = 1;

        public static void Update()
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
               
                var updater = context.CreateDatabaseSchemaUpdater();
                if (updater.DatabaseSchemaVersion > Version) {
                    return;
                }
                updater.AddTable<StockModel>();
                updater.AddColumn<StockModel>("StockIndex");
                updater.AddColumn<StockModel>("CompanyName");

                updater.AddTable<MarketModel>();
                updater.AddColumn<MarketModel>("MarketID");
                updater.AddColumn<MarketModel>("MarketName");

                updater.AddTable<StockSnapshotModel>();
                updater.AddColumn<StockSnapshotModel>("StockIndex");
                updater.AddColumn<StockSnapshotModel>("MarketID");
                updater.AddColumn<StockSnapshotModel>("Tombstone");
                updater.AddColumn<StockSnapshotModel>("Price");

                updater.DatabaseSchemaVersion = Version;
                updater.Execute();
            }
        }
    }
}
