namespace CarcassonneDiscovery.Server
{
    /// <summary>
    /// Exit codes for game execution requests.
    /// </summary>
    public enum GameExecutionRequestExitCode
    {
        /// <summary>
        /// Game action has been successfully executed.
        /// </summary>
        Ok,

        /// <summary>
        /// It is invalid simulation state to do the action.
        /// </summary>
        WrongSimulationState,

        /// <summary>
        /// There was an error while executing the action.
        /// </summary>
        Error
    }
}
