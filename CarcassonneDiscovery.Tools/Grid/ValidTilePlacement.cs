namespace CarcassonneDiscovery.Tools
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Information about possible tile placement.
    /// </summary>
    public struct ValidTilePlacement
    {
        /// <summary>
        /// Coordinates of the tile.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Orientation of the tile.
        /// </summary>
        public TileOrientation Orientation { get; set; }
    }
}
