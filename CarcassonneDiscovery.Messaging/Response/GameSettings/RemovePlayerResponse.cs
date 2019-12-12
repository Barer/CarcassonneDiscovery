namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Result of remove player request.
    /// </summary>
    public class RemovePlayerResponse : IResponse
    {
        /// <inheritdoc />
        public ExitCode ExitCode { get; set; }

        /// <summary>
        /// Color of the removed player.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="exitCode">Response exit code.</param>
        /// <param name="color">Color of the removed player.</param>
        public RemovePlayerResponse(ExitCode exitCode, PlayerColor color = PlayerColor.None)
        {
            ExitCode = exitCode;
            Color = color;
        }

        /// <summary>
        /// Constructor from <see cref="ServerResponse"/>.
        /// </summary>
        /// <param name="response">Request DTO.</param>
        public RemovePlayerResponse(ServerResponse response)
        {
            ValidationHelper.CheckType(response.Type, ServerResponseType.RemovePlayer);

            ExitCode = response.ExitCode;

            if (ExitCode == ExitCode.Ok)
            {
                ValidationHelper.CheckHasValue(response.Color, "Removed color must be specified.");

                Color = response.Color.Value;
            }
        }

        /// <inheritdoc />
        public ServerResponse ToServerResponse()
        {
            return new ServerResponse
            {
                Type = ServerResponseType.RemovePlayer,
                ExitCode = ExitCode,

                Color = Color
            };
        }
    }
}
