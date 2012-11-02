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
using StockGames.CommunicationProtocol;
using System.Windows.Navigation;

namespace StockGames
{
    public partial class MainPage : PhoneApplicationPage
    {
        CommunicationProtocol.CommunicationProtocol cp;

        // Constructor
        public MainPage()
        {
            InitializeComponent();
            System.Diagnostics.Debug.WriteLine("test");
            cp = CommunicationProtocol.CommunicationProtocol.Instance;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            cp.AddEvent(new MessageEvent(4, 200));
          
        }
    }
}