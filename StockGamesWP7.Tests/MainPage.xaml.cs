using System.Collections.Generic;
using System.Reflection;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Testing;


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
                UnitTestSettings settings = UnitTestSystem.CreateDefaultSettings();
                settings.TagExpression = "All";
                settings.TestHarness.Settings = settings;

                settings.SampleTags = new System.Collections.Generic.List<string>
                {
                    "Persistance",
                    "Test2"
                };
                this.Content = UnitTestSystem.CreateTestPage(settings);
            }
        }
    }
}