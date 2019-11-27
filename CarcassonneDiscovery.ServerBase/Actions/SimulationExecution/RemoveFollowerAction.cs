namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Logic;
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Remove follower action.
    /// </summary>
    public class RemoveFollowerAction : ServerAction
    {
        /// <summary>
        /// Game action request.
        /// </summary>
        protected RemoveFollowerExecutionRequest Request { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="request">Game action request.</param>
        public RemoveFollowerAction(RemoveFollowerExecutionRequest request)
        {
            Request = request;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.RemoveFollower(Request.Color, Request.Coords);

            switch (result.ExitCode)
            {
                case GameExecutionRequestExitCode.Ok:
                    ServerServiceProvider.Logger.Log("Tile placed.", LogLevel.Normal, LogType.SimulationExecution);
                    ServerServiceProvider.ClientMessager.SendToAll(result.ExecutionResult.ToServerResponse());
                    new StartMoveAction().Execute();
                    break;

                case GameExecutionRequestExitCode.WrongSimulationState:
                    ServerServiceProvider.Logger.Log("WrongSimulationState.", LogLevel.ProgramError, LogType.SimulationExecutionError);
                    break;

                case GameExecutionRequestExitCode.Error:
                    ServerServiceProvider.Logger.Log($"Error: {result.ExecutionResult.RuleViolationType}", LogLevel.Warning, LogType.SimulationExecutionError);
                    ServerServiceProvider.ClientMessager.SendToPlayer(Request.Color, result.ExecutionResult.ToServerResponse());
                    break;
            }
        }
    }
}
