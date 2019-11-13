namespace CarcassonneDiscovery.Tools
{
    /// <summary>
    /// Rule violation on grid.
    /// </summary>
    public enum GridRuleViolation
    {
        /// <summary>
        /// No rule was violated.
        /// </summary>
        Ok,

        /// <summary>
        /// Place with given coordinates is not empty.
        /// </summary>
        NotEmptyCoords,

        /// <summary>
        /// Current tile and some placed tile have not compatible region types.
        /// </summary>
        IncompatibleSurroundings,

        /// <summary>
        /// Current tile is not next to any placed tile.
        /// </summary>
        NoNeighboringTile,
    }
}
