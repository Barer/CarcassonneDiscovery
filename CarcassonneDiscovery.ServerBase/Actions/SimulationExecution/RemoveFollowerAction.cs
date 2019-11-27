namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Remove follower action.
    /// </summary>
    public class RemoveFollowerAction : ServerAction
    {
        /// <summary>
        /// Color of player removing the follower.
        /// </summary>
        protected PlayerColor Color { get; set; }

        /// <summary>
        /// Coordinates of the follower.
        /// </summary>
        protected Coords Coords { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="color">Color of player removing the follower.</param>
        /// <param name="coords">Coordinates of the follower.</param>
        public RemoveFollowerAction(PlayerColor color, Coords coords)
        {
            Color = color;
            Coords = coords;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.RemoveFollower(Color, Coords);

            switch (result.ExitCode)
            {
                case GameExecutionRequestExitCode.Ok:
                    ServerServiceProvider.Logger.Log("Tile placed.", LogLevel.Normal, LogType.SimulationExecution);
                    ServerServiceProvider.ClientMessager.SendToAll(result.ExecutionResult);
                    new StartMoveAction().Execute();
                    break;

                case GameExecutionRequestExitCode.WrongSimulationState:
                    ServerServiceProvider.Logger.Log("WrongSimulationState.", LogLevel.ProgramError, LogType.SimulationExecutionError);
                    break;

                case GameExecutionRequestExitCode.Error:
                    ServerServiceProvider.Logger.Log($"Error: {result.ExecutionResult.RuleViolationType}", LogLevel.Warning, LogType.SimulationExecutionError);
                    ServerServiceProvider.ClientMessager.SendToPlayer(Color, result.ExecutionResult);
                    break;
            }
        }
    }
}
