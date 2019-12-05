namespace CarcassonneDiscovery.Tools
{
    using System;
    using System.Collections.Generic;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Standard implementation of <see cref="ITileScheme"/>.
    /// </summary>
    public class TileScheme : ITileScheme
    {
        /// <inheritdoc />
        public string Id { get; set; }

        /// <inheritdoc />
        public int CityAmount { get; set; }

        /// <inheritdoc />
        public int RegionAmount => Regions?.Length ?? 0;

        /// <summary>
        /// Information about regions.
        /// </summary>
        internal RegionInfo[] Regions { get; set; }

        /// <summary>
        /// Regions on tile borders.
        /// </summary>
        internal Dictionary<TileOrientation, int> RegionsOnBorders { get; set; }

        /// <inheritdoc />
        public TileOrientation GetRegionBorders(int id)
        {
            RegionIdRangeCheck(id);

            return Regions[id].Borders;
        }

        /// <inheritdoc />
        public IEnumerable<int> GetRegionCities(int id)
        {
            RegionIdRangeCheck(id);

            return Regions[id].NeighboringCities;
        }

        /// <inheritdoc />
        public IEnumerable<int> GetRegionNeighbors(int id)
        {
            RegionIdRangeCheck(id);

            return Regions[id].NeighboringRegions;
        }

        /// <inheritdoc />
        public int GetRegionOnBorder(TileOrientation border)
        {
            if (RegionsOnBorders.TryGetValue(border, out int regionId))
            {
                return regionId;
            }

            throw new ArgumentOutOfRangeException("Invalid border specified. Expected cardinal direction.");
        }

        /// <inheritdoc />
        public RegionType GetRegionType(int id)
        {
            RegionIdRangeCheck(id);

            return Regions[id].Type;
        }

        /// <summary>
        /// Checks whether region id is in correct range.
        /// </summary>
        /// <param name="id">Id of the region.</param>
        protected void RegionIdRangeCheck(int id)
        {
            if (id < 0 || id >= CityAmount)
            {
                throw new ArgumentOutOfRangeException("Invalid identifier of region.");
            }
        }
    }
}
