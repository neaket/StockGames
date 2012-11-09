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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StockGames;
using StockGames.Views;

namespace StockGames.Tests.Views
{
    [TestClass]
    public class ListStocksViewTest
    {
        [TestMethod]
        public void pass()
        {
            ListStocksView view = new ListStocksView(); 
        }
    }
}
