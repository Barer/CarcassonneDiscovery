namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Place follower action.
    /// </summary>
    public class PlaceFollowerAction : ServerAction
    {
        /// <summary>
        /// Color of player placing the follower.
        /// </summary>
        protected PlayerColor Color { get; set; }

        /// <summary>
        /// Coordinates of the follower.
        /// </summary>
        protected Coords Coords { get; set; }

        /// <summary>
        /// Identifier of the region.
        /// </summary>
        protected int RegionId { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="color">Color of player removing the follower.</param>
        /// <param name="coords">Coordinates of the follower.</param>
        /// <param name="regionId">Identifier of the region.</param>
        public PlaceFollowerAction(PlayerColor color, Coords coords, int regionId)
        {
            Color = color;
            Coords = coords;
            RegionId = regionId;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.PlaceFollower(Color, Coords, RegionId);

            switch (result.ExitCode)
            {
                case GameExecutionRequestExitCode.Ok:
                    ServerServiceProvider.Logger.Log("Folower placed.", LogLevel.Normal, LogType.SimulationExecution);
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
