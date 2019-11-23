namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Base class for game execution request result.
    /// </summary>
    /// <typeparam name="TGameExecutionResult">Execution result type.</typeparam>
    public class GameExecutionRequestResultBase<TGameExecutionResult> : RequestResultBase<GameExecutionRequestExitCode>
        where TGameExecutionResult : GameExecutionResult
    {
        /// <summary>
        /// Result of execution.
        /// </summary>
        public TGameExecutionResult ExecutionResult { get; set; }
    }
}
