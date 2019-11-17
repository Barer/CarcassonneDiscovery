namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Result of <see cref="GameSimulator.RemovePlayer(PlayerColor)"/> request.
    /// </summary>
    public class RemovePlayerRequestResult : RequestResultBase<RemovePlayerRequestExitCode>
    {
        /// <summary>
        /// Color of the removed player.
        /// </summary>
        public PlayerColor Color { get; set; }
    }
}
