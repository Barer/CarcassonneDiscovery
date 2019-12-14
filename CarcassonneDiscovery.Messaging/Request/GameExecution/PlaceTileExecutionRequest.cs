namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Request for place tile action execution.
    /// </summary>
    public class PlaceTileExecutionRequest : IRequest
    {
        /// <summary>
        /// Color of player making the move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// The placed tile.
        /// </summary>
        public ITileScheme Tile { get; set; }

        /// <summary>
        /// Coordinates of the tile.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Orientation of the tile.
        /// </summary>
        public TileOrientation Orientation { get; set; }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public PlaceTileExecutionRequest()
        {
            // TODO: Needs standard constructor (for both clients.
            // Empty
        }

        /// <summary>
        /// Constructor from <see cref="ClientRequest"/>.
        /// </summary>
        /// <param name="request">Request DTO.</param>
        public PlaceTileExecutionRequest(ClientRequest request)
        {
            ValidationHelper.CheckType(request.Type, ClientRequestType.PlaceTile);

            ValidationHelper.CheckHasValue(request.Color, "Player on move must be specified.");
            ValidationHelper.CheckHasValue(request.Tile, "Tile must be specified.");
            ValidationHelper.CheckHasValue(request.Coords, "Tile coordinates must be specified.");
            ValidationHelper.CheckHasValue(request.Orientation, "Orientation must be specified.");

            Color = request.Color.Value;
            Tile = request.Tile.Value;
            Coords = request.Coords.Value;
            Orientation = request.Orientation.Value;
        }

        /// <inheritdoc />
        public ClientRequest ToServerResponse()
        {
            return new ClientRequest
            {
                Type = ClientRequestType.PlaceTile,

                Color = Color,
                Tile = new Tile(Tile),
                Coords = Coords,
                Orientation = Orientation
            };
        }
    }
}
