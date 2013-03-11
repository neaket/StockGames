using System.Data.Common;
using Microsoft.Phone.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockGames.Persistence.V1.DataContexts;
using StockGames.Persistence.V1.DataModel;

namespace StockGames.Tests.Persistence.V1.DataContexts
{
    [TestClass]
    [Tag("Persistence")]
    public class StockGamesDataContextTests
    {
        [TestInitialize]
        public void Initialize()
        {
            using (var context = StockGamesDataContext.GetReadWrite())
            {
                if (!context.DatabaseExists())
                {
                    context.CreateDatabase();
                }
            }
        }

        [TestMethod]
        public void ReadOnlyCannotWrite()
        {
            try
            {
                using (var context = StockGamesDataContext.GetReadOnly())
                {
                    context.Stocks.InsertOnSubmit(new StockDataModel());
                    context.SubmitChanges();
                }

                Assert.Fail("Exception should of been thrown");
            }
            catch (DbException)
            {
            }
        }
    }
}
