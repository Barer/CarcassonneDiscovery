namespace CarcassonneDiscovery.Entity
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Scheme of a tile.
    /// </summary>
    public interface ITileScheme
    {
        /// <summary>
        /// Identifier in a tile set.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Number of cities on the tile.
        /// </summary>
        int CityAmount { get; }

        /// <summary>
        /// Number of regions on the tile.
        /// </summary>
        int RegionAmount { get; }

        /// <summary>
        /// Returns type of given region.
        /// </summary>
        /// <param name="id">Identifier of the region.</param>
        /// <returns>Type of region.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When region id is out of range.</exception>
        RegionType GetRegionType(int id);

        /// <summary>
        /// Returns cities located on given region.
        /// </summary>
        /// <param name="id">Identifier of the region.</param>
        /// <returns>Collection of identifiers of cities.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When region id is out of range.</exception>
        IEnumerable<int> GetRegionCities(int id);

        /// <summary>
        /// Returns regions neighboring given region.
        /// </summary>
        /// <param name="id">Identifier of the region.</param>
        /// <returns>Collection of identifiers of regions.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When region id is out of range.</exception>
        IEnumerable<int> GetRegionNeighbors(int id);

        /// <summary>
        /// Returns borders of tile where given region is located.
        /// </summary>
        /// <param name="id">Identifier of the region.</param>
        /// <returns>Tile borders (uses bit logic).</returns>
        /// <exception cref="ArgumentOutOfRangeException">When region id is out of range.</exception>
        TileOrientation GetRegionBorders(int id);

        /// <summary>
        /// Returns region on given border of tile.
        /// </summary>
        /// <param name="border">Location of the border.</param>
        /// <returns>Identifier of region.</returns>
        /// <exception cref="ArgumentOutOfRangeException">When border is not a cardinal direction.</exception>
        int GetRegionOnBorder(TileOrientation border);
    }
}