namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Place follower action.
    /// </summary>
    public class PlaceFollowerAction : ServerAction
    {
        /// <summary>
        /// Game action request.
        /// </summary>
        protected PlaceFollowerExecutionRequest Request { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="request">Game action request.</param>
        public PlaceFollowerAction(PlaceFollowerExecutionRequest request)
        {
            Request = request;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.PlaceFollower(Request.Color, Request.Coords, Request.RegionId);

            switch (result.ExitCode)
            {
                case GameExecutionRequestExitCode.Ok:
                    ServerServiceProvider.Logger.Log("Folower placed.", LogLevel.Normal, LogType.SimulationExecution);
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
