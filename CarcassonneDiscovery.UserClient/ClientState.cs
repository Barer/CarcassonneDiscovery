namespace CarcassonneDiscovery.UserClient
{
    /// <summary>
    /// State of the client.
    /// </summary>
    public enum ClientState
    {
        /// <summary>
        /// Application has not started yet.
        /// </summary>
        BEFORE_APPLICATION_START,

        /// <summary>
        /// User has not connected to the server yet.
        /// </summary>
        BEFORE_CONNECTING,

        /// <summary>
        /// User has connected to the server bust has not signed as a player.
        /// </summary>
        BEFORE_SIGNING,

        /// <summary>
        /// User has signed as a player and is waiting for the game to start.
        /// </summary>
        WAITING_FOR_START,

        /// <summary>
        /// Application is currently transiting to the game board.
        /// </summary>
        MOVING_TO_GAME,

        /// <summary>
        /// Game is currently in player.
        /// </summary>
        GAME_IN_PLAY,

        /// <summary>
        /// Game has ended.
        /// </summary>
        GAME_ENDED
    }
}
