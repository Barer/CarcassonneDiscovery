namespace CarcassonneDiscovery.Logic
{
    using System;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Result of start game action execution.
    /// </summary>
    public class StartGameExecutionResult : GameExecutionResult
    {
        /// <summary>
        /// Parameters of the game.
        /// </summary>
        public GameParams GameParams { get; set; }

        /// <summary>
        /// The first placed tile.
        /// </summary>
        public ITileScheme FirstTile { get; set; }

        /// <summary>
        /// Coordinates of the first tile.
        /// </summary>
        public Coords FirstTileCoords { get; set; }

        /// <summary>
        /// Orientation of the first tile.
        /// </summary>
        public TileOrientation FirstTileOrientation { get; set; }

        /// <summary>
        /// Constructor for correct game action.
        /// </summary>
        /// <param name="gameParams">Parameters of the game.</param>
        /// <param name="tile">The first placed tile.</param>
        /// <param name="coords">Coordinates of the first tile.</param>
        /// <param name="orientation">Orientation of the first tile.</param>
        public StartGameExecutionResult(GameParams gameParams, ITileScheme tile, Coords coords, TileOrientation orientation) : base()
        {
            GameParams = gameParams;
            FirstTile = tile;
            FirstTileCoords = coords;
            FirstTileOrientation = orientation;
        }

        /// <inheritdoc />
        public StartGameExecutionResult(RuleViolationType type) : base(type)
        {
            if (type == RuleViolationType.Ok)
            {
                throw new InvalidOperationException($"Cannot create instance of {GetType().Name} with argument {type} using this constructor.");
            }
        }
    }
}
