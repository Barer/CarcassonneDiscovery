namespace CarcassonneDiscovery.Logic.Execution
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

        #endregion

        #region Game start
        /// <summary>
        /// The game parameters are inconsistent.
        /// </summary>
        InconsistentGameParameters,
        #endregion

        #region Move start
        /// <summary>
        /// There is no tile remaining in the set.
        /// </summary>
        NoTileRemaining,
        #endregion

        #region Tile placement
        #endregion

        #region Follower placement
        #endregion

        #region Follower removement
        #endregion

        #region Move passing
        #endregion
    }
}
