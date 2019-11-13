namespace CarcassonneDiscovery.Tools
{
    using System.Collections.Generic;

    /// <summary>
    /// Information about region gained by search.
    /// </summary>
    public class RegionSearchInfo
    {
        /// <summary>
        /// Identifiers of regions on tile which are a part of the given landscape region.
        /// </summary>
        public SortedSet<int> RegionIds { get; set; }

        /// <summary>
        /// Identifiers of regions on tile neighboring the given landscape region.
        /// </summary>
        public SortedSet<int> NeighboringRegionIds { get; set; }

        /// <summary>
        /// Identifiers of cities on tile located on the given landscape region.
        /// </summary>
        public SortedSet<int> CitiesIds { get; set; }
    }
}
