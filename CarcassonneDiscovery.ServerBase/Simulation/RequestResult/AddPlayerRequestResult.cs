namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Result of <see cref="GameSimulator.AddPlayer(PlayerColor, string)"/> request.
    /// </summary>
    public class AddPlayerRequestResult : RequestResultBase<AddPlayerRequestExitCode>
    {
        /// <summary>
        /// Color of the added player.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Name of the added player.
        /// </summary>
        public string Name { get; set; }
    }
}
