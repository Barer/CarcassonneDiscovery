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

        /// <inheritdoc />
        public override void Execute()
        {
            switch (Message.Type)
            {
                case ClientRequestType.JoinAsPlayer:
                    new AddPlayerAction(Message.Color.Value, Message.Name).Execute();
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
                    new PlaceTileAction(Message.ToPlaceTileExecutionRequest()).Execute();
                    break;
                case ClientRequestType.PlaceFollower:
                    new PlaceFollowerAction(Message.ToPlaceFollowerExecutionRequest()).Execute();
                    break;
                case ClientRequestType.RemoveFollower:
                    new RemoveFollowerAction(Message.ToRemoveFollowerExecutionRequest()).Execute();
                    break;
                case ClientRequestType.PassMove:
                    new PassMoveAction(Message.ToPassMoveExecutionRequest()).Execute();
                    break;
            }
        }
    }
}
