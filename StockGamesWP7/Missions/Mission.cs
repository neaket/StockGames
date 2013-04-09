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
    /// <summary>
    /// Abstract class used to ensure that missions can be interchangable by forcing the implementation of
    /// required methods and uniform attributes
    /// </summary>
    ///
    /// <remarks>   Jon Panke, 3/1/2013. </remarks>
    public abstract class Mission
    {
        /// <summary>
        /// Current status of the mission instance
        /// </summary>
        public MissionStatus MissionStatus { get; protected set; }

        /// <summary>
        /// Attribute for the identifing missionId
        /// </summary>
        public abstract long MissionId { get; }

        /// <summary>
        /// Attribute for the mission title
        /// </summary>
        public abstract string MissionTitle { get; }

        /// <summary>
        /// Attribute for the mission text description
        /// </summary>
        public abstract string MissionDescription { get; }

        /// <summary>
        /// Abstract class used to ensure that missions can be interchangable by forcing the implementation of
        /// required methods and uniform attributess
        /// </summary>
        protected Mission()
        {
            MissionStatus = MissionStatus.NotStarted;
        }

        /// <summary>
        /// Default behavior for strating up a new mission
        /// </summary>
        public virtual void StartMission()
        {
            Debug.Assert(MissionStatus == MissionStatus.NotStarted, "This method should only be called once");

            MissionStatus = MissionStatus.InProgress;
            Messenger.Default.Send(new MissionUpdatedMessageType(MissionId, MissionStatus));
        }

        /// <summary>
        /// Default behavior for when a mission need to be flag as completed
        /// </summary>
        protected virtual void MissionCompleted()
        {
            Debug.Assert(MissionStatus != MissionStatus.Completed, "This method should only be called once");

            MissionStatus = MissionStatus.Completed;
            Messenger.Default.Send(new MissionUpdatedMessageType(MissionId, MissionStatus));

            MissionController.Instance.UpdateGameEngine(this.MissionId);
            ShowMissionToast("100% Complete");
        }

        /// <summary>
        /// Use to push a Toast notification when a mission objective is completed
        /// </summary>
        /// <param name="message"></param>
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

    /// <summary>
    /// The states that a mission can be in
    /// </summary>
    public enum MissionStatus
    {
        /// <summary>
        /// mission not started state
        /// </summary>
        NotStarted,

        /// <summary>
        /// mission in progress state
        /// </summary>
        InProgress,

        /// <summary>
        /// mission completed state
        /// </summary>
        Completed
    }
}
