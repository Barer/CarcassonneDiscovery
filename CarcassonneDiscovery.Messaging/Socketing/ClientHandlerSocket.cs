namespace CarcassonneDiscovery.Messaging
{
    using System.Net.Sockets;

    /// <summary>
    /// Socket on the server side handling communication with single client.
    /// </summary>
    public class ClientHandlerSocket : MessageSocket<ServerResponse, ClientRequest>
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="socket">Socket to the client.</param>
        public ClientHandlerSocket(Socket socket)
        {
            Socket = socket;
        }

        /// <inheritdoc />
        protected override void StartSocket()
        {
            // Nothing to be done, socket has been already started
        }

        /// <inheritdoc />
        protected override void StopSocket()
        {
            try
            {
                // TODO: Send disconnect message
                // TODO: Maybe move to common base class
            }
            catch
            {
            }

            try
            {
                Socket.Disconnect(false);
                Socket.Close();
            }
            catch
            {
            }

            Socket = null;
        }
    }
}
