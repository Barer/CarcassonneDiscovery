namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Logic;
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Pass move action.
    /// </summary>
    public class PassMoveAction : ServerAction
    {
        /// <summary>
        /// Game action request.
        /// </summary>
        protected PassMoveExecutionRequest Request { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="request">Game action request.</param>
        public PassMoveAction(PassMoveExecutionRequest request)
        {
            Request = request;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.PassMove(Request.Color);

            switch (result.ExitCode)
            {
                case GameExecutionRequestExitCode.Ok:
                    ServerServiceProvider.Logger.Log("Move passed.", LogLevel.Normal, LogType.SimulationExecution);
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
