namespace CarcassonneDiscovery.Logic
{
    using System;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Result of place tile action execution.
    /// </summary>
    public class PlaceTileExecutionResult : GameExecutionResult
    {
        /// <summary>
        /// Color of player making the move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// The placed tile.
        /// </summary>
        public ITileScheme Tile { get; set; }

        /// <summary>
        /// Coordinates of the tile.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Orientation of the tile.
        /// </summary>
        public TileOrientation Orientation { get; set; }

        /// <summary>
        /// Constructor for correct game action.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="tile">The placed tile.</param>
        /// <param name="coords">Coordinates of the tile.</param>
        /// <param name="orientation">Orientation of the tile.</param>
        public PlaceTileExecutionResult(PlayerColor color, ITileScheme tile, Coords coords, TileOrientation orientation) : base()
        {
            Color = color;
            Tile = tile;
            Coords = coords;
            Orientation = orientation;
        }

        /// <inheritdoc />
        public PlaceTileExecutionResult(RuleViolationType type) : base(type)
        {
            if (type == RuleViolationType.Ok)
            {
                throw new InvalidOperationException($"Cannot create instance of {GetType().Name} with argument {type} using this constructor.");
            }
        }
    }
}
