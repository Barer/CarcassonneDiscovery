namespace CarcassonneDiscovery.Logic
{
    using System;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Result of remove follower action execution.
    /// </summary>
    public class RemoveFollowerExecutionResult : GameExecutionResult
    {
        /// <summary>
        /// Color of player making the move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Coordinates of the tile where the follower is removed from.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Points scored for the follower.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// Constructor for correct game action.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="coords">Coordinates of the tile where the follower is removed from.</param>
        /// <param name="score">Points scored for the follower.</param>
        public RemoveFollowerExecutionResult(PlayerColor color, Coords coords, int score) : base()
        {
            Color = color;
            Coords = coords;
            Score = score;
        }

        /// <inheritdoc />
        public RemoveFollowerExecutionResult(RuleViolationType type) : base(type)
        {
            if (type == RuleViolationType.Ok)
            {
                throw new InvalidOperationException($"Cannot create instance of {GetType().Name} with argument {type} using this constructor.");
            }
        }
    }
}