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
            // Game requests × Connecting requests
            switch (Message)
            {
                // TODO
                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}
