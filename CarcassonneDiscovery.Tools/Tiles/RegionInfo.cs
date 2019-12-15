namespace CarcassonneDiscovery.Tools
{
    using System.Collections.Generic;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Class for storing information about region.
    /// </summary>
    public class RegionInfo
    {
        /// <summary>
        /// Identifier of the region.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Landscape type of the region.
        /// </summary>
        public RegionType Type { get; set; }

        /// <summary>
        /// Borders of tile where the region is located.
        /// </summary>
        public TileOrientation Borders { get; set; }

        /// <summary>
        /// List of cities located on the region.
        /// </summary>
        public List<int> NeighboringCities { get; set; } = new List<int>();

        /// <summary>
        /// List of regions neighboring the region.
        /// </summary>
        public List<int> NeighboringRegions { get; set; } = new List<int>();
    }
}
