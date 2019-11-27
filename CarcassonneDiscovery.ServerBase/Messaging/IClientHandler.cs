namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Handler of client connection.
    /// </summary>
    public interface IClientHandler
    {
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
