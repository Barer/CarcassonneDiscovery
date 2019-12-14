namespace CarcassonneDiscovery.AiClient
{
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Object that acts like a player.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// Color of the player.
        /// </summary>
        PlayerColor Color { get; set; }

        /// <summary>
        /// Tells player that game is started.
        /// </summary>
        /// <param name="msg">Server response.</param>
        void StartGame(StartGameResponse msg);

        /// <summary>
        /// Tells player that move is started.
        /// </summary>
        /// <param name="msg">Server response.</param>
        void StartMove(StartMoveResponse msg);

        /// <summary>
        /// Tells player that tile is placed.
        /// </summary>
        /// <param name="msg">Server response.</param>
        void PlaceTile(PlaceTileResponse msg);

        /// <summary>
        /// Tells player that follower is placed.
        /// </summary>
        /// <param name="msg">Server response.</param>
        void PlaceFollower(PlaceFollowerResponse msg);

        /// <summary>
        /// Tells player that follower is removed.
        /// </summary>
        /// <param name="msg">Server response.</param>
        void RemoveFollower(RemoveFollowerResponse msg);

        /// <summary>
        /// Tells player that player makes no action with followers.
        /// </summary>
        /// <param name="msg">Server response.</param>
        void PassMove(PassMoveResponse msg);

        /// <summary>
        /// Asks player for move.
        /// </summary>
        /// <returns>Request for the move.</returns>
        ClientRequest GetMove();
    }
}
