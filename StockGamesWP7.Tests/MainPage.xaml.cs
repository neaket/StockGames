using System.Collections.Generic;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Testing;
using Microsoft.Phone.Testing.Harness;


namespace StockGames.Tests
{
    public partial class MainPage : PhoneApplicationPage
    {
        public MainPage()
        {
            InitializeComponent();

            // Add or comment out tags below as necessary.
            var tags = new TagList
                {
                    "ALL",
                    "Communication",
                    "Persistence",
                    "ViewModels",
                    "Views",
                };

            UnitTestSettings settings = UnitTestSystem.CreateDefaultSettings();
            settings.SampleTags = tags;
            settings.TagExpression = tags.ToString();

            this.Content = UnitTestSystem.CreateTestPage(settings);
        }

        class TagList : List<string>
        {
            public override string ToString()
            {
                if (this.Count < 1)
                    return "";
                if (this.Count == 1)
                    return this[0];


                string tags = "";
                for (int i = 0; i < this.Count - 1; i++)
                {
                    tags += this[i] + "+";
                }
                tags += this[this.Count - 1];

                return tags;
            }
        }
    }
}