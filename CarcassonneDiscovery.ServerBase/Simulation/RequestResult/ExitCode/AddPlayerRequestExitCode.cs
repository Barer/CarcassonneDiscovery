namespace CarcassonneDiscovery.Server
{
    /// <summary>
    /// Exit codes for add player requests.
    /// </summary>
    public enum AddPlayerRequestExitCode
    {
        /// <summary>
        /// Player has been successfully added.
        /// </summary>
        Ok,

        /// <summary>
        /// It is not before the game.
        /// </summary>
        WrongSimulationState,

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
        TooManyPlayers
    }
}
