using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
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