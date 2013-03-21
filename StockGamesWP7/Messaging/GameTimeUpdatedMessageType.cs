using System;

namespace StockGames.Messaging
{
    /// <summary>   The GameTimeUpdatedMessageType is intended to be sent through the messaging system, when the GameTime changes. </summary>
    ///
    /// <remarks>   Nick Eaket, 3/21/2013. </remarks>
    public class GameTimeUpdatedMessageType
    {
        /// <summary>   Gets the GameTime. </summary>
        ///
        /// <value> The GameTime. </value>
        public DateTime GameTime { get; private set; }

        /// <summary>   Initializes a new instance of the GameTimeUpdatedMessageType class. </summary>
        /// 
        /// <param name="gameTime"> The GameTime. </param>
        public GameTimeUpdatedMessageType(DateTime gameTime)
        {
            GameTime = gameTime;
        }
    }
}
