using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Threading;
using Coding4Fun.Toolkit.Controls;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Messaging;
using StockGames.Controllers;

namespace StockGames.Missions
{
    public abstract class Mission
    {
        public MissionStatus MissionStatus { get; protected set; }

        public abstract long MissionId { get; }
        public abstract string MissionTitle { get; }
        public abstract string MissionDescription { get; }

        protected Mission()
        {
            MissionStatus = MissionStatus.NotStarted;
        }

        public virtual void StartMission()
        {
            Debug.Assert(MissionStatus == MissionStatus.NotStarted, "This method should only be called once");

            MissionStatus = MissionStatus.InProgress;
            Messenger.Default.Send(new MissionUpdatedMessageType(MissionId, MissionStatus));
        }

        protected virtual void MissionCompleted()
        {
            Debug.Assert(MissionStatus != MissionStatus.Completed, "This method should only be called once");

            MissionStatus = MissionStatus.Completed;
            Messenger.Default.Send(new MissionUpdatedMessageType(MissionId, MissionStatus));

            MissionController.Instance.UpdateGameEngine(this.MissionId);
            ShowMissionToast("100% Complete");
        }


        protected void ShowMissionToast(string message)
        {
            // Note: unfortunately a toast can only be added to the current view.
            // If the user navigates to a new view the Toast Notification will no longer be displayed.
            var toast = new ToastPrompt
                {
                    Title = "Mission - " + MissionTitle,
                    Message = message,
                    TextOrientation = Orientation.Vertical
                };
            // A timer is used to hopefully show the toast at the end of the current user action 
            // Specifically for when the current view is changed right after the toast is displayed)
            var delayTimer = new DispatcherTimer();
            delayTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
            delayTimer.Tick += (sender, args) =>
                {
                    toast.Show();
                    delayTimer.Stop();
                };
            delayTimer.Start();
        }
    }

    public enum MissionStatus
    {
        NotStarted,
        InProgress,
        Completed
    }
}
