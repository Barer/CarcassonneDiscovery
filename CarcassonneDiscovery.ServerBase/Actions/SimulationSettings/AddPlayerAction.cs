namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Add player action.
    /// </summary>
    public class AddPlayerAction : ServerAction
    {
        /// <summary>
        /// Id of client.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Color of the player.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Name of the player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="color">Color of the player.</param>
        /// <param name="name">Name of the player.</param>
        public AddPlayerAction(PlayerColor color, string name)
        {
            Color = color;
            Name = name;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.AddPlayer(Color, Name);

            switch (result.ExitCode)
            {
                case AddPlayerRequestExitCode.Ok: // TODO - send response
                    ServerServiceProvider.ClientMessager.AddPlayer(Color, ClientId);
                    ServerServiceProvider.Logger.Log($"Added player: {Name}: {Color}", LogLevel.Normal, LogType.Messaging);                    
                    break;

                case AddPlayerRequestExitCode.WrongSimulationState: // TODO - send response
                    ServerServiceProvider.Logger.Log("Add player failed: WrongSimulationState", LogLevel.Normal, LogType.Messaging);
                    break;

                case AddPlayerRequestExitCode.ColorAlreadyAdded:
                case AddPlayerRequestExitCode.InvalidColor:
                case AddPlayerRequestExitCode.TooManyPlayers: // TODO - send response
                    ServerServiceProvider.Logger.Log($"Add player failed: {result.ExitCode}", LogLevel.Normal, LogType.Messaging);
                    break;
            }
        }
    }
}
