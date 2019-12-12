namespace CarcassonneDiscovery.Messaging
{
    /// <summary>
    /// Message from server to client.
    /// </summary>
    public interface IResponse
    {
        /// <summary>
        /// Response exit code.
        /// </summary>
        ExitCode ExitCode { get; set; }

        /// <summary>
        /// Creates response DTO.
        /// </summary>
        /// <returns>DTO representing the response.</returns>
        ServerResponse ToServerResponse();
    }
}
