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

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.AddPlayer(Color, Name);

            switch (result.ExitCode)
            {
                case AddPlayerRequestExitCode.Ok:
                    throw new System.NotImplementedException(); // TODO

                case AddPlayerRequestExitCode.WrongSimulationState:
                    throw new System.NotImplementedException(); // TODO

                case AddPlayerRequestExitCode.ColorAlreadyAdded:
                case AddPlayerRequestExitCode.InvalidColor:
                case AddPlayerRequestExitCode.TooManyPlayers:
                    throw new System.NotImplementedException(); // TODO
            }
        }
    }
}
