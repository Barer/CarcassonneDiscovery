namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Result of <see cref="GameSimulator.PlaceTile(PlayerColor, ITileScheme, Coords, TileOrientation)"/> request.
    /// </summary>
    public class PlaceTileRequestResult : GameExecutionRequestResultBase<PlaceTileExecutionResult>
    {
    }
}
