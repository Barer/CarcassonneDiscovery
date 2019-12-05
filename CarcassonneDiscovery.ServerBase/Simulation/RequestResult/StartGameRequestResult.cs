namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Result of <see cref="GameSimulator.StartGame()"/> request.
    /// </summary>
    public class StartGameRequestResult : GameExecutionRequestResultBase<StartGameExecutionResult>
    {
        /// <summary>
        /// Names of players.
        /// </summary>
        public string[] PlayerNames { get; set; }
    }
}
