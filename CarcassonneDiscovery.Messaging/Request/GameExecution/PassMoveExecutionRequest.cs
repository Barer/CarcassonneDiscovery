namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Request for pass move action execution.
    /// </summary>
    public class PassMoveExecutionRequest : IRequest
    {
        /// <summary>
        /// Color of player making the move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public PassMoveExecutionRequest()
        {
            // TODO: Needs standard constructor (for both clients.
            // Empty
        }

        /// <summary>
        /// Constructor from <see cref="ClientRequest"/>.
        /// </summary>
        /// <param name="request">Request DTO.</param>
        public PassMoveExecutionRequest(ClientRequest request)
        {
            ValidationHelper.CheckType(request.Type, ClientRequestType.PassMove);

            ValidationHelper.CheckHasValue(request.Color, "Player on move must be specified.");

            Color = request.Color.Value;
        }

        /// <inheritdoc />
        public ClientRequest ToServerResponse()
        {
            return new ClientRequest
            {
                Type = ClientRequestType.PassMove,

                Color = Color
            };
        }
    }
}
