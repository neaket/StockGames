using System.Collections.Generic;
using System.Reflection;
using Microsoft.Phone.Controls;
using Microsoft.Silverlight.Testing;


namespace StockGames.Tests
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();
            const bool runUnitTests = true;
            if (runUnitTests)
            {
                Content = UnitTestSystem.CreateTestPage();
                IMobileTestPage testPage = Content as IMobileTestPage;
                if (testPage != null)
                {
                    BackKeyPress += (x, xe) =>
                        xe.Cancel = testPage.NavigateBack();
                }
            }
        }
    }
}