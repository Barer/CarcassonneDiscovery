namespace CarcassonneDiscovery.Logic
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Request for remove follower action execution.
    /// </summary>
    public class RemoveFollowerExecutionRequest : GameExecutionRequest
    {
        /// <summary>
        /// Color of player making the move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Coordinates of the tile where the follower is removed from.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="coords">Coordinates of the tile where the follower is removed from.</param>
        public RemoveFollowerExecutionRequest(PlayerColor color, Coords coords) : base()
        {
            Color = color;
            Coords = coords;
        }
    }
}