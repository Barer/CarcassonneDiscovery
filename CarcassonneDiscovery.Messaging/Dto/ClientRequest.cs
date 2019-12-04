namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Message from client to server.
    /// </summary>
    public class ClientRequest
    {
        /// <summary>
        /// Type of message.
        /// </summary>
        public ClientRequestType Type { get; set; }

        /// <summary>
        /// Color of the player.
        /// </summary>
        public PlayerColor? Color { get; set; } = null;

        /// <summary>
        /// Placed tile.
        /// </summary>
        public Tile Tile { get; set; } = null;

        /// <summary>
        /// Coordinates of tile.
        /// </summary>
        public Coords? Coords { get; set; } = null;

        /// <summary>
        /// Orientation of tile.
        /// </summary>
        public TileOrientation? Orientation { get; set; } = null;

        /// <summary>
        /// Id of region.
        /// </summary>
        public int? RegionId { get; set; } = null;

        /// <summary>
        /// Name of the player.
        /// </summary>
        public string Name { get; set; }
    }
}
