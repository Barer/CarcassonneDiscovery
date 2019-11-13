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

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            var fp = obj as FollowerPlacement;

            if (fp == null)
            {
                return false;
            }

            return fp.Coords == Coords && fp.Color == Color && fp.RegionId == RegionId;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return Coords.GetHashCode() ^ ((int)Color << 24) ^ (RegionId << 8);
        }
    }
}
