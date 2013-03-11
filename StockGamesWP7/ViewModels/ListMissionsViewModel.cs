using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Controllers;
using StockGames.Missions;

namespace StockGames.ViewModels
{
    public class ListMissionsViewModel : ViewModelBase
    {
        public IEnumerable<Mission> Missions { get; private set; }
        private Mission _selectedMission;
        public Mission SelectedMission
        {
            get { return _selectedMission; }
            set
            {
                _selectedMission = value;
                if (_selectedMission != null) ViewMission();
            } 
        }

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
