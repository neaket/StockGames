using System;
using System.Diagnostics;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using StockGames.ViewModels;

namespace StockGames.Views
{
    /// <summary>   The mission view is used to display a Mission on the GUI. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    public partial class MissionView : PhoneApplicationPage
    {
        /// <summary>   Initializes a new instance of the MissionsView class. </summary>
        public MissionView()
        {
            InitializeComponent();
        }

        /// <summary>   Called when a page becomes the active page in a frame. </summary>
        ///
        /// <param name="e">    An object that contains the event data. </param>
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