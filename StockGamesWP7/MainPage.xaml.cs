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
using System.Xml;
using System.Text;
using System.IO;
using StockGames.CommunicationModule;

namespace StockGames
{
    public partial class MainPage : PhoneApplicationPage
    {

        CommunicationProtocol communication;
        MessageCoder mc;
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            communication = CommunicationProtocol.Instance;
            mc = MessageCoder.Instance;
        }

        private void ViewStocks_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Views/ListStocksView.xaml", UriKind.Relative));

            MessageEvent myEvent = new MessageEvent();
            MessageEventHandler myEventHandler = new MessageEventHandler(myEvent, mc);

            myEvent.ActivateMessageEvent(3, 6);
        }
    }
}