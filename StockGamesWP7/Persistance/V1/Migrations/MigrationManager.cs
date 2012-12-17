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
using StockGames.Persistance.V1.DataContexts;

namespace StockGames.Persistance.V1.Migrations
{
    public static class MigrationManager
    {
        public static void InitializeDatabase()
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                if (!context.DatabaseExists())
                {
                    context.CreateDatabase();
                }                
            }

            // TODO optimize this code with versions.
            InitialCreate.Update();
        }
    }
}
