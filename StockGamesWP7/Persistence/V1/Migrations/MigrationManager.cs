using StockGames.Messaging;
using StockGames.Persistence.V1.DataContexts;
using GalaSoft.MvvmLight.Messaging;

namespace StockGames.Persistence.V1.Migrations
{
    /// <summary>   The MigrationManager is used to initialize the DataContext. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
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
                MessengerWrapper.MessengerEnabled = false;
                using (var context = StockGamesDataContext.GetReadWrite())
                {
                    if (!context.DatabaseExists())
                    {
                        context.CreateDatabase();
                    }
                }

                // TODO optimize this code with versions.
                InitialCreate.Update();

                MessengerWrapper.MessengerEnabled = true;
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
