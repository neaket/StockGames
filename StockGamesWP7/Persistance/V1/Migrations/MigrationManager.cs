using StockGames.Persistance.V1.DataContexts;

namespace StockGames.Persistance.V1.Migrations
{
    public static class MigrationManager
    {
        private static object _lock = new object();
        public static void InitializeDatabase()
        {
            lock (_lock)
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

        public static void IfExistsRemoveDatabase()
        {
            lock (_lock)
            {
             
                using (var context = StockGamesDataContext.GetReadWrite())
                {
                    if (context.DatabaseExists())
                    {
                        context.DeleteDatabase();
                    }
                }

            }
        }
    }
}
