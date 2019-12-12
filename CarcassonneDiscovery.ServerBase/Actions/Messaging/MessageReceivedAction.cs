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
            ServerAction action;

            switch (Message.Type)
            {
                case ClientRequestType.JoinAsPlayer:
                    action = new AddPlayerAction(ClientId, Message.Color.Value, Message.Name); // TODO - by response
                    ServerServiceProvider.ServerController.EnqueueActionAsFirst(action);
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

            ServerAction action;

            switch (Message.Type)
            {
                case ClientRequestType.PlaceTile:
                    action = new PlaceTileAction(new PlaceTileExecutionRequest(Message));
                    ServerServiceProvider.ServerController.EnqueueActionAsFirst(action);
                    break;

                case ClientRequestType.PlaceFollower:
                    action = new PlaceFollowerAction(new PlaceFollowerExecutionRequest(Message));
                    ServerServiceProvider.ServerController.EnqueueActionAsFirst(action);
                    break;

                case ClientRequestType.RemoveFollower:
                    action = new RemoveFollowerAction(new RemoveFollowerExecutionRequest(Message));
                    ServerServiceProvider.ServerController.EnqueueActionAsFirst(action);
                    break;

                case ClientRequestType.PassMove:
                    action = new PassMoveAction(new PassMoveExecutionRequest(Message));
                    ServerServiceProvider.ServerController.EnqueueActionAsFirst(action);
                    break;
            }
        }
    }
}
