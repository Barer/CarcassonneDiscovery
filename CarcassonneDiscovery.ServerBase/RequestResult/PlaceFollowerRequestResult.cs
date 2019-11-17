namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Result of <see cref="GameSimulator.PlaceFollower(PlayerColor, Coords, int)"/> request.
    /// </summary>
    public class PlaceFollowerRequestResult : GameExecutionRequestResultBase<PlaceFollowerExecutionResult>
    {
    }
}
