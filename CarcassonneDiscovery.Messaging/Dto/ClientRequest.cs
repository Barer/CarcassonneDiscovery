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

        #region Serialization settings
        /// <summary>
        /// Should the <see cref="Color"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeColor()
        {
            return Color.HasValue;
        }

        /// <summary>
        /// Should the <see cref="Tile"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeTile()
        {
            return Tile != null;
        }

        /// <summary>
        /// Should the <see cref="Coords"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeCoords()
        {
            return Coords.HasValue;
        }

        /// <summary>
        /// Should the <see cref="Orientation"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeOrientation()
        {
            return Orientation.HasValue;
        }

        /// <summary>
        /// Should the <see cref="RegionId"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeRegionId()
        {
            return RegionId.HasValue;
        }

        /// <summary>
        /// Should the <see cref="Name"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeName()
        {
            return Name != null;
        }
        #endregion
    }
}
