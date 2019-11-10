namespace CarcassonneDiscovery.Logic.Execution
{
    using System;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Result of start move action execution.
    /// </summary>
    public class StartMoveExecutionResult : GameExecutionResult
    {
        /// <summary>
        /// Tile to be placed in the move.
        /// </summary>
        public ITileScheme Tile { get; set; }

        /// <summary>
        /// Color of player on move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Constructor for correct game action.
        /// </summary>
        /// <param name="tile">Tile to be placed.</param>
        /// <param name="color">Color of the player on move.</param>
        public StartMoveExecutionResult(ITileScheme tile, PlayerColor color) : base()
        {
            Tile = tile;
            Color = color;
        }

        /// <inheritdoc />
        public StartMoveExecutionResult(RuleViolationType type) : base(type)
        {
            if (type == RuleViolationType.Ok)
            {
                throw new InvalidOperationException($"Cannot create instance of {GetType().Name} with argument {type} using this constructor.");
            }
        }
    }
}
