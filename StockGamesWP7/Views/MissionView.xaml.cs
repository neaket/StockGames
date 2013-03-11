using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using StockGames.ViewModels;

namespace StockGames.Views
{
    public partial class MissionView : PhoneApplicationPage
    {
        public MissionView()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            var parameters = NavigationContext.QueryString;
            var missionIdStr = parameters["MissionId"];
            var missionId = Convert.ToInt64(missionIdStr);

            var vm = DataContext as MissionViewModel;
            Debug.Assert(vm != null, "View Model Must Be Set");

            vm.LoadMissionCommand.Execute(missionId);
        }
    }
}