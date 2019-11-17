namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Logic;

    public class GameExecutionRequestResultBase<TGameExecutionResult> : RequestResultBase<GameExecutionRequestExitCode>
        where TGameExecutionResult : GameExecutionResult
    {
        public TGameExecutionResult ExecutionResult { get; set; }
    }
}
