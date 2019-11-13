namespace CarcassonneDiscovery.Tools
{
    using System.Collections.Generic;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Result of search by <see cref="GridSearch"/>.
    /// </summary>
    public struct GridSearchResult
    {
        /// <summary>
        /// Information about the region.
        /// </summary>
        public Dictionary<Coords, RegionSearchInfo> RegionInformation { get; set; }

        /// <summary>
        /// Is the region closed?
        /// </summary>
        public bool IsClosed { get; set; }

        /// <summary>
        /// Is the region occupied by any follower?
        /// </summary>
        public bool IsOccupied { get; set; }
    }
}
