namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Pass move action.
    /// </summary>
    public class PassMoveAction : ServerAction
    {
        /// <summary>
        /// Color of player passing the move.
        /// </summary>
        protected PlayerColor Color { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="color">Color of player passing the move.</param>
        public PassMoveAction(PlayerColor color)
        {
            Color = color;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.PassMove(Color);

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
                    ServerServiceProvider.ClientMessager.SendToPlayer(Color, result.ExecutionResult.ToServerResponse());
                    break;
            }
        }
    }
}
