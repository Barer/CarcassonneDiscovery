namespace CarcassonneDiscovery.Tools
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Tuple consisting of <see cref="Coords"/> and region identifier.
    /// </summary>
    public struct CoordsRegionIdTuple
    {
        /// <summary>
        /// Coordinates.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Region identifier.
        /// </summary>
        public int RegionId { get; set; }
    }
}
