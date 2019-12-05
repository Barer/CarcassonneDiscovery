namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Place tile action.
    /// </summary>
    public class PlaceTileAction : ServerAction
    {
        /// <summary>
        /// Game action request.
        /// </summary>
        protected PlaceTileExecutionRequest Request { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="request">Game action request.</param>
        public PlaceTileAction(PlaceTileExecutionRequest request)
        {
            Request = request;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.PlaceTile(Request.Color, Request.Tile, Request.Coords, Request.Orientation);

            switch (result.ExitCode)
            {
                case GameExecutionRequestExitCode.Ok:
                    ServerServiceProvider.Logger.Log("Tile placed.", LogLevel.Normal, LogType.SimulationExecution);
                    ServerServiceProvider.ClientMessager.SendToAll(result.ExecutionResult.ToServerResponse());
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
