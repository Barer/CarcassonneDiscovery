namespace CarcassonneDiscovery.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Extension methods for <see cref="ITileScheme"/>.
    /// </summary>
    public static class ITileSchemeExtensions
    {
        /// <summary>
        /// Cardinal directions
        /// </summary>
        private static readonly TileOrientation[] CardinalDirections = new TileOrientation[] { TileOrientation.N, TileOrientation.E, TileOrientation.S, TileOrientation.W };

        /// <summary>
        /// Compares two tile schemes on equality.
        /// </summary>
        /// <param name="scheme">Tile scheme.</param>
        /// <param name="other">Another tile scheme.</param>
        /// <returns>True if the two schemes are equal; otherwise false.</returns>
        public static bool TileEquals(this ITileScheme scheme, ITileScheme other)
        {
            if (scheme.CityAmount != other.CityAmount)
            {
                return false;
            }

            var regions = scheme.RegionAmount;

            if (regions != other.RegionAmount)
            {
                return false;
            }

            try
            {
                for (int rId = 0; rId < regions; rId++)
                {
                    if (!RangeCheck(scheme.GetRegionCities(rId), other.GetRegionCities(rId)) ||
                        !RangeCheck(scheme.GetRegionNeighbors(rId), other.GetRegionNeighbors(rId)) ||
                        (scheme.GetRegionBorders(rId) != other.GetRegionBorders(rId)) ||
                        (scheme.GetRegionType(rId) != other.GetRegionType(rId)))
                    {
                        return false;
                    }
                }

                foreach (var border in CardinalDirections)
                {
                    if (scheme.GetRegionOnBorder(border) != other.GetRegionOnBorder(border))
                    {
                        return false;
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw new ArgumentException("Tile scheme is not consistent.");
            }

            return true;
        }

        /// <summary>
        /// Creates copy of tile in standard <see cref="TileScheme" /> representation.
        /// </summary>
        /// <param name="scheme">Tile to be copied.</param>
        /// <returns>Copy of tile in standard representation.</returns>
        public static TileScheme Copy(this ITileScheme scheme)
        {
            var result = new TileScheme
            {
                Id = scheme.Id,
                CityAmount = scheme.CityAmount,
                Regions = new RegionInfo[scheme.RegionAmount],
                RegionsOnBorders = new int[4]
                {
                    scheme.GetRegionOnBorder(TileOrientation.N),
                    scheme.GetRegionOnBorder(TileOrientation.E),
                    scheme.GetRegionOnBorder(TileOrientation.S),
                    scheme.GetRegionOnBorder(TileOrientation.W)
                }
            };

            for (int rId = 0; rId < result.RegionAmount; rId++)
            {
                result.Regions[rId] = new RegionInfo
                {
                    Id = rId,
                    Type = scheme.GetRegionType(rId),
                    Borders = scheme.GetRegionBorders(rId),
                    NeighboringCities = scheme.GetRegionCities(rId).ToList(),
                    NeighboringRegions = scheme.GetRegionNeighbors(rId).ToList()
                };
            }

            return result;
        }

        /// <summary>
        /// Checks whether given tile scheme is consistent.
        /// </summary>
        /// <param name="scheme">Tile scheme.</param>
        /// <returns>True if the scheme is consistent; otherwise false.</returns>
        public static bool IsConsistent(this ITileScheme scheme)
        {
            int cities = scheme.CityAmount;
            int regions = scheme.RegionAmount;

            // Check consistency of regions and tile borders
            var checkedBorders = new List<TileOrientation>();

            // Check consistency of region neighbourhood
            var neighbouring = new bool[cities, cities];

            // Check if region range could be accessed
            try
            {
                for (int rId = 0; rId < regions; rId++)
                {
                    // Get neighbouring cities
                    foreach (var nId in scheme.GetRegionNeighbors(rId))
                    {
                        if (nId < 0 || nId >= regions)
                        {
                            return false;
                        }

                        neighbouring[rId, nId] = true;
                    }

                    // Check city id range
                    foreach (var cId in scheme.GetRegionCities(rId))
                    {
                        if (cId < 0 || cId >= cities)
                        {
                            return false;
                        }
                    }

                    // Check borders
                    var regionBorders = scheme.GetRegionBorders(rId);

                    foreach (var border in CardinalDirections)
                    {
                        // If the region is on border
                        if ((regionBorders & border) == border)
                        {
                            // If there is already a region on the same border
                            if (checkedBorders.Contains(border))
                            {
                                return false;
                            }

                            // Else check consistency with tile
                            checkedBorders.Add(border);

                            if (scheme.GetRegionOnBorder(border) != rId)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                return false;
            }

            // Check consistency of neighboring regions
            for (int i = 0; i < regions - 1; i++)
            {
                // Region cannot be its own neighbor
                if (neighbouring[i, i])
                {
                    return false;
                }

                for (int j = 1; j < regions - 1; j++)
                {
                    // Check symmetry
                    if (neighbouring[i, j] != neighbouring[j, i])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Compares two enumerable collections on set equality.
        /// </summary>
        /// <typeparam name="T">Comparable item type.</typeparam>
        /// <param name="a">Enumerable.</param>
        /// <param name="b">Other enumerable.</param>
        /// <returns>True if the two collections contain same items; otherwise false.</returns>
        private static bool RangeCheck<T>(IEnumerable<T> a, IEnumerable<T> b)
            where T : IComparable
        {
            var aAr = a.OrderBy(x => x).ToArray();
            var bAr = b.OrderBy(x => x).ToArray();

            var length = aAr.Length;

            if (bAr.Length != length)
            {
                return false;
            }

            for (int i = 0; i < length; i++)
            {
                if (!aAr[i].Equals(bAr[i]))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
