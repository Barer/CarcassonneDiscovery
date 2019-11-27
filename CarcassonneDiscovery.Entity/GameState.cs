namespace CarcassonneDiscovery.Entity
{
    using System.Collections.Generic;

    /// <summary>
    /// State of the game.
    /// </summary>
    public class GameState
    {
        /// <summary>
        /// Parameters of the game.
        /// </summary>
        public GameParams Params { get; set; } = new GameParams();

        /// <summary>
        /// Supplier of the tile.
        /// </summary>
        public ITileSupplier TileSupplier { get; set; }

        /// <summary>
        /// Current move phase.
        /// </summary>
        public MoveWorkflow MovePhase { get; set; }

        /// <summary>
        /// Index of player on move.
        /// </summary>
        public int CurrentPlayerIndex { get; set; }

        /// <summary>
        /// Current tile to be placed.
        /// </summary>
        public ITileScheme CurrentTile { get; set; }

        /// <summary>
        /// Coordinates of the last placed tile.
        /// </summary>
        public Coords CurrentTileCoords { get; set; }

        /// <summary>
        /// Board with placed tiles.
        /// </summary>
        public Dictionary<Coords, TilePlacement> Grid { get; set; }

        /// <summary>
        /// Placed followers of the players.
        /// </summary>
        public Dictionary<PlayerColor, List<FollowerPlacement>> PlacedFollowers { get; set; }

        /// <summary>
        /// Scores of the players.
        /// </summary>
        public Dictionary<PlayerColor, int> Scores { get; set; }
    }
}
