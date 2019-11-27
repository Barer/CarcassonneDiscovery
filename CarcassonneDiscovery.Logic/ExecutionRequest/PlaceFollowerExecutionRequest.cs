namespace CarcassonneDiscovery.Logic
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Request for place follower action execution.
    /// </summary>
    public class PlaceFollowerExecutionRequest : GameExecutionRequest
    {
        /// <summary>
        /// Color of player making the move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Coordinates of the tile where the placed follower.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Identifier of region where the placed follower.
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="coords">Coordinates of the tile where the placed follower.</param>
        /// <param name="regionId">Identifier of region where the placed follower.</param>
        public PlaceFollowerExecutionRequest(PlayerColor color, Coords coords, int regionId)
        {
            Color = color;
            Coords = coords;
            RegionId = regionId;
        }
    }
}
