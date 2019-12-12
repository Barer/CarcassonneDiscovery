namespace CarcassonneDiscovery.Messaging
{
    /// <summary>
    /// Type of message from server to client.
    /// </summary>
    public enum ServerResponseType
    {
        #region Game execution result
        /// <summary>
        /// Start game execution result.
        /// </summary>
        StartGame,

        /// <summary>
        /// Start move execution result.
        /// </summary>
        StartMove,

        /// <summary>
        /// Place tile execution result.
        /// </summary>
        PlaceTile,

        /// <summary>
        /// Place follower execution result.
        /// </summary>
        PlaceFollower,

        /// <summary>
        /// Remove follower execution result.
        /// </summary>
        RemoveFollower,

        /// <summary>
        /// Pass move execution result.
        /// </summary>
        PassMove,

        /// <summary>
        /// End game execution result.
        /// </summary>
        EndGame,
        #endregion

        #region Game settings
        /// <summary>
        /// Adding player in game.
        /// </summary>
        AddPlayer,

        /// <summary>
        /// Removing player in game.
        /// </summary>
        RemovePlayer
        #endregion
    }
}
