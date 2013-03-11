using StockGames.Missions;

namespace StockGames.Messaging
{
    public class MissionUpdatedMessageType
    {
        public long MissionId { get; private set; }
        public MissionStatus MissionStatus { get; private set; }

        public MissionUpdatedMessageType(long missionId, MissionStatus missionStatus)
        {
            MissionId = missionId;
            MissionStatus = missionStatus;
        }
    }
}
