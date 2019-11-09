namespace CarcassonneDiscovery.Entity
{
    /// <summary>
    /// Placement of a follower on a tile.
    /// </summary>
    public class FollowerPlacement
    {
        /// <summary>
        /// Coordinates of the tile where the follower is placed.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Color of the follower.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Coordinates of the region where the follower is placed.
        /// </summary>
        public int RegionId { get; set; }
    }
}
