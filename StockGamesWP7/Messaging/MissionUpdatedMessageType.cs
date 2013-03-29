using StockGames.Missions;

namespace StockGames.Messaging
{
    /// <summary>   The MissionUpdatedMessageType is intended to be sent whenever a Mission is updated. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class MissionUpdatedMessageType
    {
        /// <summary>   Gets the identifier of the mission. </summary>
        ///
        /// <value> The identifier of the mission. </value>
        public long MissionId { get; private set; }

        /// <summary>   Gets the mission status. </summary>
        ///
        /// <value> The mission status. </value>
        public MissionStatus MissionStatus { get; private set; }

        /// <summary>   Initializes a new instance of the MissionUpdatedMessageType class.  </summary>
        ///
        /// <param name="missionId">        The identifier of the mission. </param>
        /// <param name="missionStatus">    The mission status. </param>
        public MissionUpdatedMessageType(long missionId, MissionStatus missionStatus)
        {
            MissionId = missionId;
            MissionStatus = missionStatus;
        }
    }
}
