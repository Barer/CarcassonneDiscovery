namespace CarcassonneDiscovery.Server
{
    using System;
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Handler of client connection.
    /// </summary>
    public interface IClientHandler
    {
        /// <summary>
        /// Client id.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Invokes when request was received from client.
        /// </summary>
        event Action<ClientRequest> MessageReceived;

        /// <summary>
        /// Disconnects client from the server.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Sends message to client.
        /// </summary>
        /// <param name="msg">Message to be sent.</param>
        void SendMessage(ServerResponse msg);
    }
}
