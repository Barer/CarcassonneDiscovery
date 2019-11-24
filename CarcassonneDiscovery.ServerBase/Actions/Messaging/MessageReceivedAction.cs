namespace CarcassonneDiscovery.Server
{
    using ClientRequest = System.Object;
    using ClientId = System.String;

    /// <summary>
    /// Message received action.
    /// </summary>
    public class MessageReceivedAction : ServerAction
    {
        /// <summary>
        /// Identifier of a client sending the message.
        /// </summary>
        protected ClientId ClientId { get; set; }

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
            }
        }
    }
}
