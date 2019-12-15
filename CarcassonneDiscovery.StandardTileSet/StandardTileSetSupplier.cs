namespace CarcassonneDiscovery.Resources.StandardTileSet
{
    using System;
    using System.Collections.Generic;
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Tools;

    /// <summary>
    /// Supplier of standard tile set.
    /// </summary>
    public class StandardTileSetSupplier : ITileSupplier
    {
        /// <summary>
        /// List of remaining tiles.
        /// </summary>
        protected List<ITileScheme> _RemainingTiles;

        /// <summary>
        /// First tile to be played.
        /// </summary>
        protected ITileScheme _First;

        /// <summary>
        /// Random number generator.
        /// </summary>
        protected Random _Rnd;

        /// <inheritdoc />
        public int RemainingCount => _RemainingTiles.Count;

        /// <summary>
        /// Default constructor.
        /// </summary>
        public StandardTileSetSupplier()
        {
            _Rnd = new Random();

            _RemainingTiles = new List<ITileScheme>();

            LoadTiles();
        }

        /// <inheritdoc />
        public ITileScheme GetNextTile()
        {
            if (_RemainingTiles.Count == 0)
            {
                return null;
            }

            int index = _Rnd.Next(_RemainingTiles.Count);

            var result = _RemainingTiles[index];
            _RemainingTiles.RemoveAt(index);

            return result;
        }

        /// <inheritdoc />
        public ITileScheme GetFirstTile()
        {
            return _First;
        }

        /// <inheritdoc />
        public void ReturnTile(ITileScheme tile)
        {
            _RemainingTiles.Add(tile);
        }

        /// <summary>
        /// Loads standard tile set to the list.
        /// </summary>
        private void LoadTiles()
        {
            // TODO: Load the tiles with 3 types of regions...

            // 8-tiles
            _RemainingTiles.Add(Create8Tile(RegionType.Mountain));
            _RemainingTiles.Add(Create8Tile(RegionType.Grassland));
            _RemainingTiles.Add(Create8Tile(RegionType.Sea));

            // 4,4-tiles
            _RemainingTiles.Add(Create44Tile(RegionType.Mountain, RegionType.Grassland, false));
            _RemainingTiles.Add(Create44Tile(RegionType.Mountain, RegionType.Grassland, false));
            _RemainingTiles.Add(Create44Tile(RegionType.Mountain, RegionType.Grassland, false));
            _RemainingTiles.Add(Create44Tile(RegionType.Mountain, RegionType.Sea, true));
            _RemainingTiles.Add(Create44Tile(RegionType.Mountain, RegionType.Sea, true));
            _RemainingTiles.Add(Create44Tile(RegionType.Mountain, RegionType.Sea, false));
            _RemainingTiles.Add(Create44Tile(RegionType.Grassland, RegionType.Sea, true));
            _RemainingTiles.Add(Create44Tile(RegionType.Grassland, RegionType.Sea, true));
            _RemainingTiles.Add(Create44Tile(RegionType.Grassland, RegionType.Sea, false));

            // 1,7-tiles
            _RemainingTiles.Add(Create17Tile(RegionType.Mountain, RegionType.Grassland, false));
            _RemainingTiles.Add(Create17Tile(RegionType.Mountain, RegionType.Grassland, false));
            _RemainingTiles.Add(Create17Tile(RegionType.Mountain, RegionType.Grassland, false));
            _RemainingTiles.Add(Create17Tile(RegionType.Mountain, RegionType.Sea, true));
            _RemainingTiles.Add(Create17Tile(RegionType.Mountain, RegionType.Sea, true));
            _RemainingTiles.Add(Create17Tile(RegionType.Mountain, RegionType.Sea, false));
            _RemainingTiles.Add(Create17Tile(RegionType.Grassland, RegionType.Sea, true));
            _RemainingTiles.Add(Create17Tile(RegionType.Grassland, RegionType.Sea, true));
            _RemainingTiles.Add(Create17Tile(RegionType.Grassland, RegionType.Sea, false));

            // 1,1,1,1,4-tiles
            _RemainingTiles.Add(Create11114Tile(RegionType.Grassland, RegionType.Grassland, RegionType.Mountain, RegionType.Mountain, RegionType.Sea, false, false, false, false));

            // 1,1,1,5-tiles (2 types)
            _RemainingTiles.Add(Create1115Tile(RegionType.Mountain, RegionType.Mountain, RegionType.Mountain, RegionType.Grassland, false, false, false));
            _RemainingTiles.Add(Create1115Tile(RegionType.Mountain, RegionType.Mountain, RegionType.Mountain, RegionType.Grassland, false, false, false));
            _RemainingTiles.Add(Create1115Tile(RegionType.Mountain, RegionType.Mountain, RegionType.Mountain, RegionType.Sea, false, true, false));
            _RemainingTiles.Add(Create1115Tile(RegionType.Mountain, RegionType.Mountain, RegionType.Mountain, RegionType.Sea, false, false, false));
            _RemainingTiles.Add(Create1115Tile(RegionType.Grassland, RegionType.Grassland, RegionType.Grassland, RegionType.Mountain, false, false, false));
            _RemainingTiles.Add(Create1115Tile(RegionType.Grassland, RegionType.Grassland, RegionType.Grassland, RegionType.Mountain, false, false, false));
            _RemainingTiles.Add(Create1115Tile(RegionType.Grassland, RegionType.Grassland, RegionType.Grassland, RegionType.Sea, false, true, false));
            _RemainingTiles.Add(Create1115Tile(RegionType.Grassland, RegionType.Grassland, RegionType.Grassland, RegionType.Sea, false, false, false));
            _RemainingTiles.Add(Create1115Tile(RegionType.Sea, RegionType.Sea, RegionType.Sea, RegionType.Mountain, false, true, false));
            _RemainingTiles.Add(Create1115Tile(RegionType.Sea, RegionType.Sea, RegionType.Sea, RegionType.Mountain, false, false, false));
            _RemainingTiles.Add(Create1115Tile(RegionType.Sea, RegionType.Sea, RegionType.Sea, RegionType.Grassland, false, true, false));
            _RemainingTiles.Add(Create1115Tile(RegionType.Sea, RegionType.Sea, RegionType.Sea, RegionType.Grassland, false, false, false));

            // 1,1,6-tiles (2 types)
            _RemainingTiles.Add(Create116Tile(RegionType.Mountain, RegionType.Mountain, RegionType.Grassland, false, false));
            _RemainingTiles.Add(Create116Tile(RegionType.Mountain, RegionType.Mountain, RegionType.Grassland, false, false));
            _RemainingTiles.Add(Create116Tile(RegionType.Mountain, RegionType.Mountain, RegionType.Sea, true, false));
            _RemainingTiles.Add(Create116Tile(RegionType.Mountain, RegionType.Mountain, RegionType.Sea, false, false));
            _RemainingTiles.Add(Create116Tile(RegionType.Grassland, RegionType.Grassland, RegionType.Mountain, false, false));
            _RemainingTiles.Add(Create116Tile(RegionType.Grassland, RegionType.Grassland, RegionType.Mountain, false, false));
            _RemainingTiles.Add(Create116Tile(RegionType.Grassland, RegionType.Grassland, RegionType.Sea, true, false));
            _RemainingTiles.Add(Create116Tile(RegionType.Grassland, RegionType.Grassland, RegionType.Sea, false, false));
            _RemainingTiles.Add(Create116Tile(RegionType.Sea, RegionType.Sea, RegionType.Mountain, true, false));
            _RemainingTiles.Add(Create116Tile(RegionType.Sea, RegionType.Sea, RegionType.Mountain, false, false));
            _RemainingTiles.Add(Create116Tile(RegionType.Sea, RegionType.Sea, RegionType.Grassland, true, false));
            _RemainingTiles.Add(Create116Tile(RegionType.Sea, RegionType.Sea, RegionType.Grassland, false, false));

            // 1,6,1-tiles (2 types)
            _RemainingTiles.Add(Create161Tile(RegionType.Mountain, RegionType.Grassland, RegionType.Mountain, false, false));
            _RemainingTiles.Add(Create161Tile(RegionType.Mountain, RegionType.Grassland, RegionType.Mountain, false, false));
            _RemainingTiles.Add(Create161Tile(RegionType.Mountain, RegionType.Sea, RegionType.Mountain, true, false));
            _RemainingTiles.Add(Create161Tile(RegionType.Mountain, RegionType.Sea, RegionType.Mountain, false, false));
            _RemainingTiles.Add(Create161Tile(RegionType.Grassland, RegionType.Mountain, RegionType.Grassland, false, false));
            _RemainingTiles.Add(Create161Tile(RegionType.Grassland, RegionType.Mountain, RegionType.Grassland, false, false));
            _RemainingTiles.Add(Create161Tile(RegionType.Grassland, RegionType.Sea, RegionType.Grassland, true, true));
            _RemainingTiles.Add(Create161Tile(RegionType.Grassland, RegionType.Sea, RegionType.Grassland, true, false));
            _RemainingTiles.Add(Create161Tile(RegionType.Sea, RegionType.Mountain, RegionType.Sea, true, false));
            _RemainingTiles.Add(Create161Tile(RegionType.Sea, RegionType.Mountain, RegionType.Sea, false, false));
            _RemainingTiles.Add(Create161Tile(RegionType.Sea, RegionType.Grassland, RegionType.Sea, true, false));
            _RemainingTiles.Add(Create161Tile(RegionType.Sea, RegionType.Grassland, RegionType.Sea, false, false));

            // Missing 422 + 2231 + 2213 - but will be replaced by new resource, OK.

            // TEMP
            _First = Create161Tile(RegionType.Sea, RegionType.Mountain, RegionType.Grassland, true, false);
        }

        #region Tile patterns
        /// <summary>
        /// Creates "8-tile".
        /// </summary>
        /// <param name="type">Type of region on the tile.</param>
        /// <returns>Created tile.</returns>
        private TileScheme Create8Tile(RegionType type)
        {
            return new TileScheme
            {
                Id = "L8",
                CityAmount = 0,
                Regions = new RegionInfo[1]
                {
                    new RegionInfo
                    {
                        Type = type,
                        Borders = TileOrientation.NESW,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>()
                    }
                },
                RegionsOnBorders = new int[4]
                {
                    0,
                    0,
                    0,
                    0
                }
            };
        }

        /// <summary>
        /// Creates "4,4-tile".
        /// </summary>
        /// <param name="type1">First type of region on the tile.</param>
        /// <param name="type2">Second type of region on the tile.</param>
        /// <param name="city">Is there a city between the regions?</param>
        /// <returns>Created tile.</returns>
        private TileScheme Create44Tile(RegionType type1, RegionType type2, bool city)
        {
            var result = new TileScheme
            {
                Id = "L44",
                CityAmount = 0,
                Regions = new RegionInfo[2]
                {
                    new RegionInfo
                    {
                        Type = type1,
                        Borders = TileOrientation.NE,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>()
                        {
                            1
                        }
                    },
                    new RegionInfo
                    {
                        Type = type2,
                        Borders = TileOrientation.SW,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            0
                        }
                    }
                },
                RegionsOnBorders = new int[4]
                {
                    0,
                    0,
                    1,
                    1
                }
            };

            if (city)
            {
                result.CityAmount = 1;
                result.Regions[0].NeighboringCities.Add(0);
                result.Regions[1].NeighboringCities.Add(0);
            }

            return result;
        }

        /// <summary>
        /// Creates "1,1,6-tile".
        /// </summary>
        /// <param name="type1">Type of region on index 0.</param>
        /// <param name="type2">Type of region on index 1.</param>
        /// <param name="type3">Type of region on index 2.</param>
        /// <param name="city13">Is there a city between the regions 1 and 3?</param>
        /// <param name="city23">Is there a city between the regions 2 and 3?</param>
        /// <returns>Created tile.</returns>
        private TileScheme Create116Tile(RegionType type1, RegionType type2, RegionType type3, bool city13, bool city23)
        {
            var result = new TileScheme
            {
                Id = "L116",
                CityAmount = 0,
                Regions = new RegionInfo[3]
                {
                    new RegionInfo
                    {
                        Type = type1,
                        Borders = TileOrientation.N,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            2
                        }
                    },
                    new RegionInfo
                    {
                        Type = type2,
                        Borders = TileOrientation.E,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            2
                        }
                    },
                    new RegionInfo
                    {
                        Type = type3,
                        Borders = TileOrientation.SW,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            0,
                            1
                        }
                    }
                },
                RegionsOnBorders = new int[4]
                {
                    0,
                    1,
                    2,
                    2
                }
            };

            if (city13)
            {
                result.Regions[0].NeighboringCities.Add(result.CityAmount);
                result.Regions[2].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city23)
            {
                result.Regions[1].NeighboringCities.Add(result.CityAmount);
                result.Regions[2].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            return result;
        }

        /// <summary>
        /// Creates "1,6,1-tile".
        /// </summary>
        /// <param name="type1">Type of region on index 0.</param>
        /// <param name="type2">Type of region on index 1.</param>
        /// <param name="type3">Type of region on index 2.</param>
        /// <param name="city12">Is there a city between the regions 1 and 2?</param>
        /// <param name="city23">Is there a city between the regions 2 and 3?</param>
        /// <returns>Created tile.</returns>
        private TileScheme Create161Tile(RegionType type1, RegionType type2, RegionType type3, bool city12, bool city23)
        {
            var result = new TileScheme
            {
                Id = "L161",
                CityAmount = 0,
                Regions = new RegionInfo[3]
                {
                    new RegionInfo
                    {
                        Type = type1,
                        Borders = TileOrientation.N,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            1
                        }
                    },
                    new RegionInfo
                    {
                        Type = type2,
                        Borders = TileOrientation.EW,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            0,
                            2
                        }
                    },
                    new RegionInfo
                    {
                        Type = type3,
                        Borders = TileOrientation.S,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            1
                        }
                    }
                },
                RegionsOnBorders = new int[4]
                {
                    0,
                    1,
                    2,
                    1
                }
            };

            if (city12)
            {
                result.Regions[0].NeighboringCities.Add(result.CityAmount);
                result.Regions[1].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city23)
            {
                result.Regions[1].NeighboringCities.Add(result.CityAmount);
                result.Regions[2].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            return result;
        }

        /// <summary>
        /// Creates "1,7-tile".
        /// </summary>
        /// <param name="type1">First type of region on the tile.</param>
        /// <param name="type2">Second type of region on the tile.</param>
        /// <param name="city">Is there a city between the regions?</param>
        /// <returns>Created tile.</returns>
        private TileScheme Create17Tile(RegionType type1, RegionType type2, bool city)
        {
            var result = new TileScheme
            {
                Id = "L17",
                CityAmount = 0,
                Regions = new RegionInfo[2]
                {
                    new RegionInfo
                    {
                        Type = type1,
                        Borders = TileOrientation.N,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            1
                        }
                    },
                    new RegionInfo
                    {
                        Type = type2,
                        Borders = TileOrientation.ESW,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            0
                        }
                    },
                },
                RegionsOnBorders = new int[4]
                {
                    0,
                    1,
                    1,
                    1
                }
            };

            if (city)
            {
                result.CityAmount = 1;
                result.Regions[0].NeighboringCities.Add(0);
                result.Regions[1].NeighboringCities.Add(0);
            }

            return result;
        }

        /// <summary>
        /// Creates "4,2,2-tile".
        /// </summary>
        /// <param name="type1">Type of region on index 0.</param>
        /// <param name="type2">Type of region on index 1.</param>
        /// <param name="type3">Type of region on index 2.</param>
        /// <param name="city12">Is there a city between the regions 1 and 2?</param>
        /// <param name="city13">Is there a city between the regions 1 and 3?</param>
        /// <param name="city23">Is there a city between the regions 2 and 3?</param>
        /// <returns>Created tile.</returns>
        private TileScheme Create422Tile(RegionType type1, RegionType type2, RegionType type3, bool city12, bool city13, bool city23)
        {
            var result = new TileScheme
            {
                Id = "L422",
                CityAmount = 0,
                Regions = new RegionInfo[3]
                {
                    new RegionInfo
                    {
                        Type = type1,
                        Borders = TileOrientation.NE,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            1,
                            2
                        }
                    },
                    new RegionInfo
                    {
                        Type = type2,
                        Borders = TileOrientation.S,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            0,
                            2
                        }
                    },
                    new RegionInfo
                    {
                        Type = type3,
                        Borders = TileOrientation.W,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            0,
                            1
                        }
                    }
                },
                RegionsOnBorders = new int[4]
                {
                    0,
                    0,
                    1,
                    2
                }
            };

            if (city12)
            {
                result.Regions[0].NeighboringCities.Add(result.CityAmount);
                result.Regions[1].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city13)
            {
                result.Regions[0].NeighboringCities.Add(result.CityAmount);
                result.Regions[2].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city23)
            {
                result.Regions[1].NeighboringCities.Add(result.CityAmount);
                result.Regions[2].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            return result;
        }

        /// <summary>
        /// Creates "1,1,1,5-tile".
        /// </summary>
        /// <param name="type1">Type of region on index 0.</param>
        /// <param name="type2">Type of region on index 1.</param>
        /// <param name="type3">Type of region on index 2.</param>
        /// <param name="type4">Type of region on index 3.</param>
        /// <param name="city14">Is there a city between the regions 1 and 4?</param>
        /// <param name="city24">Is there a city between the regions 2 and 4?</param>
        /// <param name="city34">Is there a city between the regions 3 and 4?</param>
        /// <returns>Created tile.</returns>
        private TileScheme Create1115Tile(RegionType type1, RegionType type2, RegionType type3, RegionType type4, bool city14, bool city24, bool city34)
        {
            var result = new TileScheme
            {
                Id = "L1115",
                CityAmount = 0,
                Regions = new RegionInfo[4]
                {
                    new RegionInfo
                    {
                        Type = type1,
                        Borders = TileOrientation.N,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            3
                        }
                    },
                    new RegionInfo
                    {
                        Type = type2,
                        Borders = TileOrientation.E,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            3
                        }
                    },
                    new RegionInfo
                    {
                        Type = type3,
                        Borders = TileOrientation.S,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            3
                        }
                    },
                    new RegionInfo
                    {
                        Type = type4,
                        Borders = TileOrientation.W,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            0,
                            1,
                            2
                        }
                    }
                },
                RegionsOnBorders = new int[4]
                {
                    0,
                    1,
                    2,
                    3
                }
            };

            if (city14)
            {
                result.Regions[0].NeighboringCities.Add(result.CityAmount);
                result.Regions[3].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city24)
            {
                result.Regions[1].NeighboringCities.Add(result.CityAmount);
                result.Regions[3].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city34)
            {
                result.Regions[2].NeighboringCities.Add(result.CityAmount);
                result.Regions[3].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            return result;
        }

        /// <summary>
        /// Creates "1,1,1,1,4-tile".
        /// </summary>
        /// <param name="type1">Type of region on index 0.</param>
        /// <param name="type2">Type of region on index 1.</param>
        /// <param name="type3">Type of region on index 2.</param>
        /// <param name="type4">Type of region on index 3.</param>
        /// <param name="type5">Type of region on index 4.</param>
        /// <param name="city15">Is there a city between the regions 1 and 5?</param>
        /// <param name="city25">Is there a city between the regions 2 and 5?</param>
        /// <param name="city35">Is there a city between the regions 3 and 5?</param>
        /// <param name="city45">Is there a city between the regions 4 and 5?</param>
        /// <returns>Created tile.</returns>
        private TileScheme Create11114Tile(RegionType type1, RegionType type2, RegionType type3, RegionType type4, RegionType type5, bool city15, bool city25, bool city35, bool city45)
        {
            var result = new TileScheme
            {
                Id = "L11114",
                CityAmount = 0,
                Regions = new RegionInfo[5]
                {
                    new RegionInfo
                    {
                        Type = type1,
                        Borders = TileOrientation.N,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            4
                        }
                    },
                    new RegionInfo
                    {
                        Type = type2,
                        Borders = TileOrientation.E,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            4
                        }
                    },
                    new RegionInfo
                    {
                        Type = type3,
                        Borders = TileOrientation.S,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            4
                        }
                    },
                    new RegionInfo
                    {
                        Type = type4,
                        Borders = TileOrientation.W,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            4
                        }
                    },
                    new RegionInfo()
                    {
                        Type = type5,
                        Borders = TileOrientation.None,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            0,
                            1,
                            2,
                            3
                        }
                    }
                },
                RegionsOnBorders = new int[4]
                {
                    0,
                    1,
                    2,
                    3
                }
            };

            if (city15)
            {
                result.Regions[0].NeighboringCities.Add(result.CityAmount);
                result.Regions[4].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city25)
            {
                result.Regions[1].NeighboringCities.Add(result.CityAmount);
                result.Regions[4].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city35)
            {
                result.Regions[2].NeighboringCities.Add(result.CityAmount);
                result.Regions[4].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city45)
            {
                result.Regions[3].NeighboringCities.Add(result.CityAmount);
                result.Regions[4].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            return result;
        }

        /// <summary>
        /// Creates "2,2,3,1-tile".
        /// </summary>
        /// <param name="type1">Type of region on index 0.</param>
        /// <param name="type2">Type of region on index 1.</param>
        /// <param name="type3">Type of region on index 2.</param>
        /// <param name="type4">Type of region on index 3.</param>
        /// <param name="city12">Is there a city between the regions 1 and 2?</param>
        /// <param name="city13">Is there a city between the regions 1 and 3?</param>
        /// <param name="city23">Is there a city between the regions 2 and 3?</param>
        /// <param name="city34">Is there a city between the regions 3 and 4?</param>
        /// <returns>Created tile.</returns>
        private TileScheme Create2231Tile(RegionType type1, RegionType type2, RegionType type3, RegionType type4, bool city12, bool city13, bool city23, bool city34)
        {
            var result = new TileScheme
            {
                Id = "L2231",
                CityAmount = 0,
                Regions = new RegionInfo[4]
                {
                    new RegionInfo
                    {
                        Type = type1,
                        Borders = TileOrientation.N,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            1,
                            2
                        }
                    },
                    new RegionInfo()
                    {
                        Type = type2,
                        Borders = TileOrientation.E,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            0,
                            2
                        }
                    },
                    new RegionInfo
                    {
                        Type = type3,
                        Borders = TileOrientation.S,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            0,
                            1,
                            3
                        }
                    },
                    new RegionInfo
                    {
                        Type = type4,
                        Borders = TileOrientation.W,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            2
                        }
                    }
                },
                RegionsOnBorders = new int[4]
                {
                    0,
                    1,
                    2,
                    3
                }
            };

            if (city12)
            {
                result.Regions[0].NeighboringCities.Add(result.CityAmount);
                result.Regions[1].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city13)
            {
                result.Regions[0].NeighboringCities.Add(result.CityAmount);
                result.Regions[2].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city23)
            {
                result.Regions[1].NeighboringCities.Add(result.CityAmount);
                result.Regions[2].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city34)
            {
                result.Regions[2].NeighboringCities.Add(result.CityAmount);
                result.Regions[3].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            return result;
        }

        /// <summary>
        /// Creates "2,2,1,3-tile".
        /// </summary>
        /// <param name="type1">Type of region on index 0.</param>
        /// <param name="type2">Type of region on index 1.</param>
        /// <param name="type3">Type of region on index 2.</param>
        /// <param name="type4">Type of region on index 3.</param>
        /// <param name="city12">Is there a city between the regions 1 and 2?</param>
        /// <param name="city14">Is there a city between the regions 1 and 4?</param>
        /// <param name="city24">Is there a city between the regions 2 and 4?</param>
        /// <param name="city34">Is there a city between the regions 3 and 4?</param>
        /// <returns>Created tile.</returns>
        private TileScheme Create2213Tile(RegionType type1, RegionType type2, RegionType type3, RegionType type4, bool city12, bool city14, bool city24, bool city34)
        {
            var result = new TileScheme
            {
                Id = "L2213",
                CityAmount = 0,
                Regions = new RegionInfo[4]
                {
                    new RegionInfo
                    {
                        Type = type1,
                        Borders = TileOrientation.N,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            1,
                            3
                        }
                    },
                    new RegionInfo
                    {
                        Type = type2,
                        Borders = TileOrientation.E,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            0,
                            3
                        }
                    },
                    new RegionInfo
                    {
                        Type = type3,
                        Borders = TileOrientation.S,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            3
                        }
                    },
                    new RegionInfo
                    {
                        Type = type4,
                        Borders = TileOrientation.W,
                        NeighboringCities = new List<int>(),
                        NeighboringRegions = new List<int>
                        {
                            0,
                            1,
                            2
                        }
                    }
                },
                RegionsOnBorders = new int[4]
                {
                    0,
                    1,
                    2,
                    3
                }
            };

            if (city12)
            {
                result.Regions[0].NeighboringCities.Add(result.CityAmount);
                result.Regions[1].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city14)
            {
                result.Regions[0].NeighboringCities.Add(result.CityAmount);
                result.Regions[3].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city24)
            {
                result.Regions[1].NeighboringCities.Add(result.CityAmount);
                result.Regions[3].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            if (city34)
            {
                result.Regions[2].NeighboringCities.Add(result.CityAmount);
                result.Regions[3].NeighboringCities.Add(result.CityAmount);
                result.CityAmount++;
            }

            return result;
        }
        #endregion
    }
}
