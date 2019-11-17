namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Result of <see cref="GameSimulator.RemoveFollower(PlayerColor, Coords)"/> request.
    /// </summary>
    public class RemoveFollowerRequestResult : GameExecutionRequestResultBase<RemoveFollowerExecutionResult>
    {
    }
}
