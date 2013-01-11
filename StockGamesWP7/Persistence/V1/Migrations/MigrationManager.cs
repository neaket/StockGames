using StockGames.Persistence.V1.DataContexts;

namespace StockGames.Persistence.V1.Migrations
{
    public static class MigrationManager
    {
        private static readonly object Lock = new object();
        
        /// <summary>
        /// This method should be called to create the DataContext or to upgrade an existing DataContext to the latest Migration.
        /// </summary>
        public static void InitializeDataContext()
        {
            lock (Lock)
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

        /// <summary>
        /// This method deletes an existing DataContext if it exists.
        /// </summary>
        public static void IfExistsRemoveDataContext()
        {
            lock (Lock)
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
