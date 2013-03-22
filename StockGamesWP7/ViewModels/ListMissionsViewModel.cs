using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Controllers;
using StockGames.Missions;
using StockGames.Views;

namespace StockGames.ViewModels
{
    /// <summary>   The ListMissionsViewModel is used by the <see cref="ListMissionsView" />. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/20/2013. </remarks>
    public class ListMissionsViewModel : ViewModelBase
    {
        /// <summary>   A list of currently viewable missions. </summary>
        ///
        /// <value> The missions. </value>
        public IEnumerable<Mission> Missions { get; private set; }
        private Mission _selectedMission;

        /// <summary>   Gets or sets the selected mission. When changed the MissionView is displayed on the GUI. </summary>
        ///
        /// <value> The selected mission. </value>
        public Mission SelectedMission
        {
            get { return _selectedMission; }
            set
            {
                _selectedMission = value;
                if (_selectedMission != null) ViewMission();
            } 
        }

        /// <summary>   Initializes a new instance of the ListMissionsViewModel class. </summary>
        public ListMissionsViewModel()
        {
            Missions = MissionController.Instance.GetMissions();
        }

        private void ViewMission()
        {
            var uri = new Uri("/Views/MissionView.xaml?MissionId=" + SelectedMission.MissionId, UriKind.Relative);
            Messenger.Default.Send(uri, "Navigate");
            SelectedMission = null;
            RaisePropertyChanged("SelectedMission");
        }
    }
}
