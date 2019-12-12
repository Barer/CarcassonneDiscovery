namespace CarcassonneDiscovery.Messaging
{
    using System;
    using System.Net;
    using System.Net.Sockets;

    /// <summary>
    /// Socket on the client side.
    /// </summary>
    public class ClientSocket : MessageSocket<ClientRequest, ServerResponse>
    {
        /// <summary>
        /// Event when connection to server is lost.
        /// </summary>
        public event Action ServerDisconnected = () => { };

        /// <summary>
        /// Port of the server.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// IP address of the server.
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ClientSocket() : base()
        {
        }

        /// <inheritdoc />
        protected override void StartSocket()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Connect(IPAddress.Parse(IP), Port);
        }

        /// <inheritdoc />
        protected override void StopSocket()
        {
            /*
            try
            {
                // TODO: Send disconnect message
                // TODO: Maybe move to common base class
            }
            catch
            {
            }
            */

            if (Socket != null)
            {
                Socket.Disconnect(false);
                Socket.Close();
                Socket = null;
            }
        }
    }
}
