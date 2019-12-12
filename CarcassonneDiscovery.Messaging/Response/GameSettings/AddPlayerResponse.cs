namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Response after add player action execution.
    /// </summary>
    public class AddPlayerResponse : IResponse
    {
        /// <inheritdoc />
        public ExitCode ExitCode { get; set; }

        /// <summary>
        /// Color of the added player.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Name of the added player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="exitCode">Response exit code.</param>
        /// <param name="color">Color of the removed player.</param>
        /// <param name="name">Name of the added player.</param>
        public AddPlayerResponse(ExitCode exitCode, PlayerColor color = PlayerColor.None, string name = null)
        {
            ExitCode = exitCode;
            Color = color;
            Name = name;
        }

        /// <summary>
        /// Constructor from <see cref="ServerResponse"/>.
        /// </summary>
        /// <param name="response">Request DTO.</param>
        public AddPlayerResponse(ServerResponse response)
        {
            ValidationHelper.CheckType(response.Type, ServerResponseType.AddPlayer);

            ExitCode = response.ExitCode;

            if (ExitCode == ExitCode.Ok)
            {
                ValidationHelper.CheckHasValue(response.Color, "Requested color must be specified.");
                ValidationHelper.CheckHasValue(response.Name, "Player name must be specified.");

                Color = response.Color.Value;
                Name = response.Name;
            }

        }

        /// <inheritdoc />
        public ServerResponse ToServerResponse()
        {
            return new ServerResponse
            {
                Type = ServerResponseType.AddPlayer,
                ExitCode = ExitCode,

                Color = Color,
                Name = Name
            };
        }
    }
}
