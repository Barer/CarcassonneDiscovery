namespace CarcassonneDiscovery.UserClient
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Localization of rule violation messages.
    /// </summary>
    public class RuleViolationTypeLocalization
    {
        /// <summary>
        /// Gets localization for rule violation.
        /// </summary>
        /// <param name="ruleViolation">Rule violation.</param>
        /// <returns>Localized message.</returns>
        public string this[RuleViolationType ruleViolation]
        {
            get
            {
                switch (ruleViolation)
                {
                    case RuleViolationType.RegionOccupied:
                        return "The region is already occupied by a follower.";
                    case RuleViolationType.InvalidCoordinates:
                        return "Invalid coordinates.";
                    case RuleViolationType.NotOnMove:
                        return "You are not on move.";
                    case RuleViolationType.NoFollowerAvailable:
                        return "You have no follower available";
                    case RuleViolationType.NoNeighboringTile:
                        return "Tile must be placed next to other tile.";
                    case RuleViolationType.IncompatibleSurroundings:
                        return "The surrounding tiles has different region types.";
                    case RuleViolationType.InvalidTile:
                        return "You are placing a wrong tile.";
                    case RuleViolationType.InvalidMovePhase:
                        return "You are not supposed to do that now.";
                    case RuleViolationType.Ok:
                        return "OK.";
                    default:
                        return "Unexpected result.";
                }
            }
        }
    }
}
