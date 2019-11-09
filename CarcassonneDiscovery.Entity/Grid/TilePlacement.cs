namespace CarcassonneDiscovery.Entity
{
    /// <summary>
    /// Placement of a tile in the grid. Also contains information about follower placed on the tile.
    /// </summary>
    public class TilePlacement
    {
        /// <summary>
        /// Coordinates of the tile.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Scheme of the tile.
        /// </summary>
        public ITileScheme TileScheme { get; set; }

        /// <summary>
        /// Orientation of the tile.
        /// </summary>
        public TileOrientation Orientation { get; set; }

        /// <summary>
        /// Placement of a follower.
        /// </summary>
        public FollowerPlacement FollowerPlacement { get; set; }
    }
}
