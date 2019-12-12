namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Messaging;

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
        /// <param name="clientId">Id of client.</param>
        /// <param name="color">Color of the player.</param>
        /// <param name="name">Name of the player.</param>
        public AddPlayerAction(string clientId, PlayerColor color, string name)
        {
            ClientId = clientId;
            Color = color;
            Name = name;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.AddPlayer(Color, Name);

            var response = new AddPlayerResponse(result.ExitCode, Color, Name).ToServerResponse();

            if (result.ExitCode == ExitCode.Ok)
            {
                ServerServiceProvider.ClientMessager.AddPlayer(Color, ClientId);

                ServerServiceProvider.Logger.Log($"Added player: {Name}: {Color}", LogLevel.Normal, LogType.Messaging);
                ServerServiceProvider.ClientMessager.SendToClient(ClientId, response);
            }
            else
            {
                ServerServiceProvider.Logger.LogExitCode(result.ExitCode, "AddPlayer", LogType.Messaging);
                ServerServiceProvider.ClientMessager.SendToClient(ClientId, response);
            }
        }
    }
}
