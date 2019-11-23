namespace CarcassonneDiscovery.Server
{
    /// <summary>
    /// Exit codes for remove player request.
    /// </summary>
    public enum RemovePlayerRequestExitCode
    {
        /// <summary>
        /// Player has been successfully removed.
        /// </summary>
        Ok,

        /// <summary>
        /// It is not before the game.
        /// </summary>
        WrongSimulationState,

        /// <summary>
        /// Color cannot be removed if it has not been added.
        /// </summary>
        ColorNotAdded
    }
}
