using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Controllers;
using StockGames.Messaging;
using StockGames.Missions;

namespace StockGames.ViewModels
{
    public class MissionViewModel : ViewModelBase
    {
        public string MissionTitle { get; private set; }
        public string MissionDesciption { get; private set; }
        public ICommand LoadMissionCommand { get; private set; }
        public ICommand StartMissionCommand { get; private set; }

        private long _missionId;

        private Visibility _missionStartVisible;
        public Visibility MissionStartVisible
        {
            get
            {
                return _missionStartVisible;
            } 
            private set
            {
                _missionStartVisible = value;
                RaisePropertyChanged("MissionStartVisible");
            }
        }

        private Missions.MissionStatus _missionStatus;
        public Missions.MissionStatus MissionStatus
        { 
            get
            {
                return _missionStatus;  // TODO make nicer display strings
            } 
            private set
            {
                _missionStatus = value;
                if (_missionStatus == Missions.MissionStatus.NotStarted)
                {
                    MissionStartVisible = Visibility.Visible;
                }
                else
                {
                    MissionStartVisible = Visibility.Collapsed;

                }
                RaisePropertyChanged("MissionStatus");
            }
        }

        

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
            MissionDesciption = mission.MissionDescription;
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
