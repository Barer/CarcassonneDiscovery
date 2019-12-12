namespace CarcassonneDiscovery.Messaging
{
    /// <summary>
    /// Message from client to server.
    /// </summary>
    public interface IRequest
    {
        /// <summary>
        /// Creates request DTO.
        /// </summary>
        /// <returns>DTO representing the request.</returns>
        ClientRequest ToServerResponse();
    }
}
