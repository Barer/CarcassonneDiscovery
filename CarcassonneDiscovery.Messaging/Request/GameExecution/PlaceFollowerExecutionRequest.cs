namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Request for place follower action execution.
    /// </summary>
    public class PlaceFollowerExecutionRequest : IRequest
    {
        /// <summary>
        /// Color of player making the move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Coordinates of the tile where the placed follower.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Identifier of region where the placed follower.
        /// </summary>
        public int RegionId { get; set; }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public PlaceFollowerExecutionRequest()
        {
            // TODO: Needs standard constructor (for both clients.
            // Empty
        }

        /// <summary>
        /// Constructor from <see cref="ClientRequest"/>.
        /// </summary>
        /// <param name="request">Request DTO.</param>
        public PlaceFollowerExecutionRequest(ClientRequest request)
        {
            ValidationHelper.CheckType(request.Type, ClientRequestType.PlaceFollower);

            ValidationHelper.CheckHasValue(request.Color, "Player on move must be specified.");
            ValidationHelper.CheckHasValue(request.Coords, "Tile coordinates must be specified.");
            ValidationHelper.CheckHasValue(request.RegionId, "Region Id must be specified.");

            Color = request.Color.Value;
            Coords = request.Coords.Value;
            RegionId = request.RegionId.Value;
        }

        /// <inheritdoc />
        public ClientRequest ToServerResponse()
        {
            return new ClientRequest
            {
                Type = ClientRequestType.PlaceFollower,

                Color = Color,
                Coords = Coords,
                RegionId = RegionId
            };
        }
    }
}
