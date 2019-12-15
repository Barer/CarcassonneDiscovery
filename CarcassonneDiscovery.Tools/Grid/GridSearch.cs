namespace CarcassonneDiscovery.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarcassonneDiscovery.Entity;

    using Grid = System.Collections.Generic.Dictionary<Entity.Coords, Entity.TilePlacement>;

    /// <summary>
    /// Search methods on a grid.
    /// </summary>
    public class GridSearch
    {
        /// <summary>
        /// Collection of cardinal directions.
        /// </summary>
        private static readonly TileOrientation[] CardinalDirections = new TileOrientation[] { TileOrientation.N, TileOrientation.E, TileOrientation.S, TileOrientation.W };

        /// <summary>
        /// Checks whether tile could be placed in the grid.
        /// </summary>
        /// <param name="grid">Grid with placed tiles.</param>
        /// <param name="tile">Tile to be placed.</param>
        /// <returns>True if tile can be placed into the grid; otherwise false.</returns>
        public bool IsTilePlaceable(Grid grid, ITileScheme tile)
        {
            return GetAllTilePlacements(grid, tile).Any();
        }

        /// <summary>
        /// Returns all possible tile placements.
        /// </summary>
        /// <param name="grid">Grid with placed tiles.</param>
        /// <param name="tile">Tile to be placed.</param>
        /// <returns>Enumerable with all possible correct tile placement.</returns>
        public IEnumerable<ValidTilePlacement> GetAllTilePlacements(Grid grid, ITileScheme tile)
        {
            // Get all empty places next to placed
            var emptyCoordsList = new HashSet<Coords>();

            foreach (var coordsWithTile in grid.Keys)
            {
                foreach (var neighCoordsDirection in CardinalDirections)
                {
                    var neighCoords = coordsWithTile.GetNeighboringCoords(neighCoordsDirection);

                    if (!grid.ContainsKey(neighCoords))
                    {
                        emptyCoordsList.Add(neighCoords);
                    }
                }
            }

            // Check all actual places and orientations
            foreach (var coords in emptyCoordsList)
            {
                foreach (var orientation in CardinalDirections)
                {
                    if (CheckTilePlacement(grid, tile, coords, orientation) == GridRuleViolation.Ok)
                    {
                        yield return new ValidTilePlacement
                        {
                            Coords = coords,
                            Orientation = orientation
                        };
                    }
                }
            }
        }

        /// <summary>
        /// Checks whether given tile placement is correct.
        /// </summary>
        /// <param name="grid">Grid with placed tiles.</param>
        /// <param name="tile">Tile to be placed.</param>
        /// <param name="coords">Coordinates of the tile.</param>
        /// <param name="orientation">Orientation of the tile.</param>
        /// <returns><see cref="GridRuleViolation.Ok"/> if tile placement is correct; otherwise other <see cref="GridRuleViolation"/>.</returns>
        public GridRuleViolation CheckTilePlacement(Grid grid, ITileScheme tile, Coords coords, TileOrientation orientation)
        {
            if (grid.ContainsKey(coords))
            {
                return GridRuleViolation.NotEmptyCoords;
            }

            bool hasNeighbor = false;

            foreach (var border in CardinalDirections)
            {
                var neighCoords = coords.GetNeighboringCoords(border);

                if (grid.TryGetValue(neighCoords, out var neighTilePlacement))
                {
                    hasNeighbor = true;

                    var neighTile = neighTilePlacement.TileScheme;

                    // Check region types
                    var borderOnThisScheme = border.Derotate(orientation);
                    var borderOnNeighScheme = border.Rotate(TileOrientation.S).Derotate(neighTilePlacement.Orientation);

                    var thisRegionId = tile.GetRegionOnBorder(borderOnThisScheme);
                    var neighRegionId = neighTile.GetRegionOnBorder(borderOnNeighScheme);

                    var thisType = tile.GetRegionType(thisRegionId);
                    var neighType = neighTile.GetRegionType(neighRegionId);

                    if (thisType != neighType)
                    {
                        return GridRuleViolation.IncompatibleSurroundings;
                    }
                }
            }

            if (!hasNeighbor)
            {
                return GridRuleViolation.NoNeighboringTile;
            }

            return GridRuleViolation.Ok;
        }

        /// <summary>
        /// Checks whether given region is occupied by any follower.
        /// </summary>
        /// <param name="grid">Grid with placed tiles.</param>
        /// <param name="coords">Coordinates of the tile where the region is located on.</param>
        /// <param name="regionId">Identifier of the region.</param>
        /// <returns>True if the region is occupied by any follower; otherwise false.</returns>
        public bool IsRegionOccupied(Grid grid, Coords coords, int regionId)
        {
            var searchResult = Search(grid, coords, regionId, stopWhenOccupied: true);

            return searchResult.IsOccupied;
        }

        /// <summary>
        /// Checks whether given region is closed.
        /// </summary>
        /// <param name="grid">Grid with placed tiles.</param>
        /// <param name="coords">Coordinates of the tile where the region is located on.</param>
        /// <param name="regionId">Identifier of the region.</param>
        /// <returns>True if the region is closed; otherwise false.</returns>
        public bool IsRegionClosed(Grid grid, Coords coords, int regionId)
        {
            var searchResult = Search(grid, coords, regionId, stopWhenOpen: true);

            return searchResult.IsClosed;
        }

        /// <summary>
        /// Returns score for follower placement.
        /// </summary>
        /// <param name="grid">Grid with placed tiles.</param>
        /// <param name="coords">Coordinates of the tile where the follower is located on.</param>
        /// <param name="regionId">Identifier of the region where the follower is located on.</param>
        /// <param name="forceOpen">Should the score be counter as the region is not closed?</param>
        /// <returns>Points scored for the follower.</returns>
        public int GetScoreForFollower(Grid grid, Coords coords, int regionId, bool forceOpen = false)
        {
            var search = Search(grid, coords, regionId);

            var tileCount = search.RegionInformation.Count;
            var isOpen = !search.IsClosed || forceOpen || tileCount <= 2;

            switch (grid[coords].TileScheme.GetRegionType(regionId))
            {
                case RegionType.Grassland:
                    return isOpen ? tileCount : tileCount * 2;

                case RegionType.Sea:
                    var citiesOnSea = search.RegionInformation.Sum(x => x.Value.CitiesIds.Count);
                    return isOpen ? citiesOnSea : citiesOnSea + tileCount;

                case RegionType.Mountain:
                    var citiesOnMountain = new Dictionary<Coords, SortedSet<int>>();

                    foreach (var tileInfo in search.RegionInformation)
                    {
                        // Add cities on region
                        var cityIds = new SortedSet<int>(tileInfo.Value.CitiesIds);
                        citiesOnMountain.Add(tileInfo.Key, cityIds);

                        // Add cities from neighbouring glasslands
                        foreach (var nId in tileInfo.Value.NeighboringRegionIds)
                        {
                            if (grid[tileInfo.Key].TileScheme.GetRegionType(nId) == RegionType.Grassland)
                            {
                                var nSearch = Search(grid, tileInfo.Key, nId);

                                foreach (var nTileInfo in nSearch.RegionInformation)
                                {
                                    if (citiesOnMountain.TryGetValue(nTileInfo.Key, out var citiesOnTile))
                                    {
                                        foreach (var cId in grid[tileInfo.Key].TileScheme.GetRegionCities(nId))
                                        {
                                            citiesOnTile.Add(cId);
                                        }
                                    }
                                    else
                                    {
                                        citiesOnMountain.Add(nTileInfo.Key, new SortedSet<int>(grid[tileInfo.Key].TileScheme.GetRegionCities(nId)));
                                    }
                                }
                            }
                        }
                    }

                    var citiesOnMountainCount = citiesOnMountain.Sum(x => x.Value.Count);
                    return isOpen ? citiesOnMountainCount : citiesOnMountainCount * 2;
            }

            throw new ArgumentException("Invalid region type");
        }

        /// <summary>
        /// Performs DFS in a grid and gets information about landscape region.
        /// </summary>
        /// <param name="grid">Grid with placed tiles.</param>
        /// <param name="initialCoords">Coordinates of the initial region.</param>
        /// <param name="initialRegionId">Identifier of the initial region.</param>
        /// <param name="stopWhenOccupied">Should the search end when the region is surely occupied by some follower?</param>
        /// <param name="stopWhenOpen">Should the search end when the region is surely open?</param>
        /// <returns>Result of the search.</returns>
        /// <remarks>
        /// If search stops using some "stopWhen*" flag, it is only guaranteed, that given value is set in <see cref="GridSearchResult"/>.
        /// </remarks>
        protected GridSearchResult Search(Grid grid, Coords initialCoords, int initialRegionId, bool stopWhenOccupied = false, bool stopWhenOpen = false)
        {
            var result = new GridSearchResult
            {
                RegionInformation = new Dictionary<Coords, RegionSearchInfo>(),
                IsClosed = true,
                IsOccupied = false,
            };

            var regionInfo = result.RegionInformation;

            var stack = new Stack<CoordsRegionIdTuple>();

            // Init the search
            stack.Push(new CoordsRegionIdTuple
            {
                Coords = initialCoords,
                RegionId = initialRegionId
            });

            // Propagate
            while (stack.Count > 0)
            {
                var currentRegion = stack.Pop();
                var currentCoords = currentRegion.Coords;
                var currentRegionId = currentRegion.RegionId;

                if (!grid.TryGetValue(currentCoords, out var currentTilePlacement))
                {
                    throw new ArgumentException("Invalid coordinates of region.");
                }

                var currentOrientation = currentTilePlacement.Orientation;
                var currentScheme = currentTilePlacement.TileScheme;

                // Get already found info
                if (regionInfo.TryGetValue(currentCoords, out var currentRegionInformation))
                {
                    // If we were in given region, continue with another
                    if (currentRegionInformation.RegionIds.Contains(currentRegionId))
                    {
                        continue;
                    }
                }
                else
                {
                    currentRegionInformation = new RegionSearchInfo
                    {
                        RegionIds = new SortedSet<int>(),
                        NeighboringRegionIds = new SortedSet<int>(),
                        CitiesIds = new SortedSet<int>()
                    };

                    regionInfo.Add(currentCoords, currentRegionInformation);
                }

                // Check if the region is occupied
                if (currentTilePlacement.FollowerPlacement != null && currentTilePlacement.FollowerPlacement.RegionId == currentRegionId)
                {
                    result.IsOccupied = true;

                    if (stopWhenOccupied)
                    {
                        return result;
                    }
                }

                // Add new info
                currentRegionInformation.RegionIds.Add(currentRegionId);

                foreach (var cId in currentScheme.GetRegionCities(currentRegionId))
                {
                    currentRegionInformation.CitiesIds.Add(cId);
                }

                foreach (var rId in currentScheme.GetRegionNeighbors(currentRegionId))
                {
                    currentRegionInformation.NeighboringRegionIds.Add(rId);
                }

                // Get all neighboring tiles and regions
                var borders = currentScheme.GetRegionBorders(currentRegionId);

                foreach (var border in CardinalDirections)
                {
                    // If tile is not on given border, continue
                    if ((border & borders) != border)
                    {
                        continue;
                    }

                    // Rotate correctly
                    var neighDir = border.Rotate(currentOrientation);
                    var neighCoords = currentCoords.GetNeighboringCoords(neighDir);

                    // If neighbouring place is not empty
                    if (grid.TryGetValue(neighCoords, out var neighTilePlacement))
                    {
                        var neighTileBorder = neighDir.Rotate(TileOrientation.S).Derotate(neighTilePlacement.Orientation);

                        var neighRegionId = neighTilePlacement.TileScheme.GetRegionOnBorder(neighTileBorder);

                        stack.Push(new CoordsRegionIdTuple
                        {
                            Coords = neighCoords,
                            RegionId = neighRegionId
                        });
                    }
                    else
                    {
                        result.IsClosed = false;

                        if (stopWhenOpen)
                        {
                            return result;
                        }
                    }
                }
            }

            return result;
        }
    }
}
