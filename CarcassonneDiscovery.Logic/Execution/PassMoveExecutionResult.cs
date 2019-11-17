namespace CarcassonneDiscovery.Logic
{
    using System;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Result of pass move action execution.
    /// </summary>
    public class PassMoveExecutionResult : GameExecutionResult
    {
        /// <summary>
        /// Color of player making the move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Constructor for correct game action.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        public PassMoveExecutionResult(PlayerColor color) : base()
        {
            Color = color;
        }

        /// <inheritdoc />
        public PassMoveExecutionResult(RuleViolationType type) : base(type)
        {
            if (type == RuleViolationType.Ok)
            {
                throw new InvalidOperationException($"Cannot create instance of {GetType().Name} with argument {type} using this constructor.");
            }
        }
    }
}
