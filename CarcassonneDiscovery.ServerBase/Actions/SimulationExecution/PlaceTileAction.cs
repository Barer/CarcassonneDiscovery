namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Place tile action.
    /// </summary>
    public class PlaceTileAction : ServerAction
    {
        /// <summary>
        /// Color of player placing the tile.
        /// </summary>
        protected PlayerColor Color { get; set; }

        /// <summary>
        /// Tile to be placed.
        /// </summary>
        protected ITileScheme Tile { get; set; }

        /// <summary>
        /// Coordinates of the tile.
        /// </summary>
        protected Coords Coords { get; set; }

        /// <summary>
        /// Orientation of the tile.
        /// </summary>
        protected TileOrientation Orientation { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="color">Color of player placing the tile.</param>
        /// <param name="tile">Tile to be placed.</param>
        /// <param name="coords">Coordinates of the tile.</param>
        /// <param name="orientation">Orientation of the tile.</param>
        public PlaceTileAction(PlayerColor color, ITileScheme tile, Coords coords, TileOrientation orientation)
        {
            Color = color;
            Tile = tile;
            Coords = coords;
            Orientation = orientation;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.PlaceTile(Color, Tile, Coords, Orientation);

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
                    ServerServiceProvider.ClientMessager.SendToPlayer(Color, result.ExecutionResult.ToServerResponse());
                    break;
            }
        }
    }
}
