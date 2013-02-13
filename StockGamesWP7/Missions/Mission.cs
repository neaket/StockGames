using System.Diagnostics;
using GalaSoft.MvvmLight.Messaging;
using StockGames.Messaging;

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
        }
    }

    public enum MissionStatus
    {
        NotStarted,
        InProgress,
        Completed
    }
}
