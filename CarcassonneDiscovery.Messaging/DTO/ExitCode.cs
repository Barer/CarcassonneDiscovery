namespace CarcassonneDiscovery.Messaging
{
    /// <summary>
    /// Exit code of the response.
    /// </summary>
    public enum ExitCode
    {
        /// <summary>
        /// Request was successful.
        /// </summary>
        Ok,

        /// <summary>
        /// Request failed, reason is not specified.
        /// </summary>
        Error,

        /// <summary>
        /// Request failed, it was in incorrect state.
        /// </summary>
        InvalidTimeError,

        #region GameExecution errors
        /// <summary>
        /// Request failed, rule was violated.
        /// </summary>
        RuleViolationError,
        #endregion

        #region AddPlayer errors
        /// <summary>
        /// The same color has been already added.
        /// </summary>
        ColorAlreadyAdded,

        /// <summary>
        /// The color is invalid.
        /// </summary>
        InvalidColor,

        /// <summary>
        /// There are already too many players.
        /// </summary>
        TooManyPlayers,
        #endregion

        #region RemovePlayer errors
        /// <summary>
        /// Color cannot be removed if it has not been added.
        /// </summary>
        ColorNotAdded,
        #endregion
    }
}
