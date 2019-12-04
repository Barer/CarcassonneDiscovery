namespace CarcassonneDiscovery.Messaging
{
    /// <summary>
    /// Type of message from client to server.
    /// </summary>
    public enum ClientRequestType
    {
        /// <summary>
        /// Join as player request.
        /// </summary>
        JoinAsPlayer,

        #region Game execution request
        /// <summary>
        /// Place tile execution request.
        /// </summary>
        PlaceTile,

        /// <summary>
        /// Place follower execution request.
        /// </summary>
        PlaceFollower,

        /// <summary>
        /// Remove follower execution request.
        /// </summary>
        RemoveFollower,

        /// <summary>
        /// Pass move execution request.
        /// </summary>
        PassMove,
        #endregion
    }
}
