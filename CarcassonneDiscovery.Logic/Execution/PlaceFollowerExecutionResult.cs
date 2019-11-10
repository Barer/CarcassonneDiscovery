namespace CarcassonneDiscovery.Logic.Execution
{
    using System;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Result of place follower action execution.
    /// </summary>
    public class PlaceFollowerExecutionResult : GameExecutionResult
    {
        /// <summary>
        /// Color of player making the move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Coordinates of the tile where the placed follower.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Identifier of region where the placed follower.
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// Constructor for correct game action.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="coords">Coordinates of the tile where the placed follower.</param>
        /// <param name="regionId">Identifier of region where the placed follower.</param>
        public PlaceFollowerExecutionResult(PlayerColor color, Coords coords, int regionId) : base()
        {
            Color = color;
            Coords = coords;
            RegionId = regionId;
        }

        /// <inheritdoc />
        public PlaceFollowerExecutionResult(RuleViolationType type) : base(type)
        {
            if (type == RuleViolationType.Ok)
            {
                throw new InvalidOperationException($"Cannot create instance of {GetType().Name} with argument {type} using this constructor.");
            }
        }
    }
}
