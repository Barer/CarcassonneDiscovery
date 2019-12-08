namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Message received action.
    /// </summary>
    public class MessageReceivedAction : ServerAction
    {
        /// <summary>
        /// Identifier of a client sending the message.
        /// </summary>
        protected string ClientId { get; set; }

        /// <summary>
        /// Message received from the client.
        /// </summary>
        protected ClientRequest Message { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="clientId">Identifier of a client sending the message.</param>
        /// <param name="msg">Message received from the client.</param>
        public MessageReceivedAction(string clientId, ClientRequest msg)
        {
            ClientId = clientId;
            Message = msg;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            switch (Message.Type)
            {
                case ClientRequestType.JoinAsPlayer:
                    ServerServiceProvider.ServerController.EnqueueActionAsFirst(new AddPlayerAction(Message.Color.Value, Message.Name));
                    break;

                case ClientRequestType.PlaceTile:
                case ClientRequestType.PlaceFollower:
                case ClientRequestType.RemoveFollower:
                case ClientRequestType.PassMove:
                    AuthenticatePlayerAndExecute();
                    break;

                default:
                    ServerServiceProvider.Logger.Log("Unknown client request type.", LogLevel.Warning, LogType.Messaging);
                    break;
            }
        }

        /// <summary>
        /// Authenticates the player and executes the action.
        /// </summary>
        protected void AuthenticatePlayerAndExecute()
        {
            var expectedClientId = ServerServiceProvider.ClientMessager.Players[Message.Color.HasValue ? Message.Color.Value : Entity.PlayerColor.None];

            if (ClientId != expectedClientId)
            {
                ServerServiceProvider.Logger.Log($"Client {ClientId} wanted to make game action for player: {Message.Color.Value}", LogLevel.Warning, LogType.Messaging);
                return;
            }

            switch (Message.Type)
            {
                case ClientRequestType.PlaceTile:
                    ServerServiceProvider.ServerController.EnqueueActionAsFirst(new PlaceTileAction(Message.ToPlaceTileExecutionRequest()));
                    break;
                case ClientRequestType.PlaceFollower:
                    ServerServiceProvider.ServerController.EnqueueActionAsFirst(new PlaceFollowerAction(Message.ToPlaceFollowerExecutionRequest()));
                    break;
                case ClientRequestType.RemoveFollower:
                    ServerServiceProvider.ServerController.EnqueueActionAsFirst(new RemoveFollowerAction(Message.ToRemoveFollowerExecutionRequest()));
                    break;
                case ClientRequestType.PassMove:
                    ServerServiceProvider.ServerController.EnqueueActionAsFirst(new PassMoveAction(Message.ToPassMoveExecutionRequest()));
                    break;
            }
        }
    }
}
