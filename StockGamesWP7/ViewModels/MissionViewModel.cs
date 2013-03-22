using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Controllers;
using StockGames.Messaging;
using StockGames.Views;

namespace StockGames.ViewModels
{
    /// <summary>   The MissionViewModel is used by the <see cref="MissionView" />. </summary>
    ///
    /// <remarks>   Jon Panke &amp; Nick Eaket, 3/21/2013. </remarks>
    public class MissionViewModel : ViewModelBase
    {
        /// <summary>   Gets the mission title. </summary>
        ///
        /// <value> The mission title. </value>
        public string MissionTitle { get; private set; }

        /// <summary>   Gets the mission description. </summary>
        ///
        /// <value> The mission description. </value>
        public string MissionDescription { get; private set; }

        /// <summary>
        /// When the LoadMissionCommand is executed with a [int missionId] as a parameter, the mission is
        /// loaded into this ViewModel.
        /// </summary>
        ///
        /// <value> The load mission command. </value>
        public ICommand LoadMissionCommand { get; private set; }

        private long _missionId;

        private Missions.MissionStatus _missionStatus;

        /// <summary>   Gets the current mission status. </summary>
        ///
        /// <value> The mission status. </value>
        public Missions.MissionStatus MissionStatus
        { 
            get
            {
                return _missionStatus;  // TODO make nicer display strings
            } 
            private set
            {
                _missionStatus = value;
                RaisePropertyChanged("MissionStatus");
            }
        }

        /// <summary>   Initializes a new instance of the MissionViewModel class. </summary>
        public MissionViewModel()
        {
            LoadMissionCommand = new RelayCommand<long>(LoadMission);

            Messenger.Default.Register<MissionUpdatedMessageType>(this, MissionUpdated);
        }



        private void LoadMission(long missionId)
        {
            _missionId = missionId;

            var mission = MissionController.Instance.GetMission(missionId);
            MissionTitle = mission.MissionTitle;
            MissionDescription = mission.MissionDescription;
            MissionStatus = mission.MissionStatus;
        }

        private void MissionUpdated(MissionUpdatedMessageType message)
        {
            if (message.MissionId != _missionId)
                return;

            MissionStatus = message.MissionStatus;
        }
    }
}
