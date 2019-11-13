namespace CarcassonneDiscovery.Logic
{
    /// <summary>
    /// Type of rule violation.
    /// </summary>
    public enum RuleViolationType
    {
        #region General
        /// <summary>
        /// No rule has been violated.
        /// </summary>
        Ok,

        /// <summary>
        /// Cannot perform the action in current move phase.
        /// </summary>
        InvalidMovePhase,

        /// <summary>
        /// Player is not on move.
        /// </summary>
        NotOnMove,

        /// <summary>
        /// Invalid coordinates were given for the action.
        /// </summary>
        InvalidCoordinates,

        /// <summary>
        /// The game state is inconsistent.
        /// </summary>
        InconsistentGameState,
        #endregion

        #region Move start
        /// <summary>
        /// There is no tile remaining in the set.
        /// </summary>
        NoTileRemaining,
        #endregion

        #region Tile placement
        /// <summary>
        /// Specified tile is not selected to be placed.
        /// </summary>
        InvalidTile,

        /// <summary>
        /// Tile placement is invalid because of incompatible surroundings.
        /// </summary>
        IncompatibleSurroundings,

        /// <summary>
        /// Tile placement is invalid because the tile has no neighboring tile.
        /// </summary>
        NoNeighboringTile,
        #endregion

        #region Follower placement
        /// <summary>
        /// Player has no follower available.
        /// </summary>
        NoFollowerAvailable,

        /// <summary>
        /// Region is already occupied.
        /// </summary>
        RegionOccupied,
        #endregion

        #region Follower removement
        #endregion

        #region Move passing
        #endregion

        #region Game end
        /// <summary>
        /// There are still tiles remaining.
        /// </summary>
        TilesRemaining,
        #endregion
    }
}
