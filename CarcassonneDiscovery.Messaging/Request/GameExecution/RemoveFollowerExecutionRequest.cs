namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Request for remove follower action execution.
    /// </summary>
    public class RemoveFollowerExecutionRequest : IRequest
    {
        /// <summary>
        /// Color of player making the move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Coordinates of the tile where the follower is removed from.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Constructor from <see cref="ClientRequest"/>.
        /// </summary>
        /// <param name="request">Request DTO.</param>
        public RemoveFollowerExecutionRequest(ClientRequest request)
        {
            ValidationHelper.CheckType(request.Type, ClientRequestType.RemoveFollower);

            ValidationHelper.CheckHasValue(request.Color, "Player on move must be specified.");
            ValidationHelper.CheckHasValue(request.Coords, "Tile coordinates must be specified.");

            Color = request.Color.Value;
            Coords = request.Coords.Value;
        }

        /// <inheritdoc />
        public ClientRequest ToServerResponse()
        {
            return new ClientRequest
            {
                Type = ClientRequestType.RemoveFollower,

                Color = Color,
                Coords = Coords
            };
        }
    }
}