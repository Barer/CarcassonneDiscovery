namespace CarcassonneDiscovery.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// Socket on the server side handling communication with all clients.
    /// </summary>
    public class ServerSocket : MessageSocket<ServerResponse, Tuple<ClientHandlerSocket, ClientRequest>>
    {
        /// <summary>
        /// Event when socket has started.
        /// </summary>
        public event Action<int> SocketStarted = (port) => { };

        /// <summary>
        /// Event when socket has stopped.
        /// </summary>
        public event Action SocketStopped = () => { };

        /// <summary>
        /// Event when new client has connected to the server.
        /// </summary>
        public event Action<ClientHandlerSocket> ClientConnected = (chs) => { };

        /// <summary>
        /// Event when client has disconnected from the server.
        /// </summary>
        public event Action<ClientHandlerSocket> ClientDisconnected = (chs) => { };

        /// <summary>
        /// List of client handlers.
        /// </summary>
        protected List<ClientHandlerSocket> ConnectedClientSockets { get; set; }

        /// <summary>
        /// Port of the socket.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public ServerSocket() : base()
        {
            ConnectedClientSockets = new List<ClientHandlerSocket>();
        }

        /// <inheritdoc />
        protected override void ListeningLoop()
        {
            try
            {
                while (true)
                {
                    var newClient = Socket.Accept();

                    var newChs = new ClientHandlerSocket(this, newClient);

                    ConnectedClientSockets.Add(newChs);

                    ClientConnected.Invoke(newChs);

                    newChs.Start();
                }
            }
            catch (ThreadAbortException)
            {
            }
        }

        /// <summary>
        /// Gets messages from client.
        /// </summary>
        /// <param name="chs">Client handler socket.</param>
        internal void GetMessagesFromClientHandler(ClientHandlerSocket chs)
        {
            bool received = false;

            // Get all received messages
            while (true)
            {
                ClientRequest rqst = chs.GetNextMessage();

                if (rqst == null)
                {
                    break;
                }

                received = true;

                EnqueueMessage(new Tuple<ClientHandlerSocket, ClientRequest>(chs, rqst));
            }

            if (received)
            {
                MessageReceivedInvoke();
            }
        }

        /// <summary>
        /// Client has disconnected.
        /// </summary>
        /// <param name="chs">Client handler socket.</param>
        public void DisconnectedClient(ClientHandlerSocket chs)
        {
            ConnectedClientSockets.Remove(chs);
            ClientDisconnected.Invoke(chs);
        }

        /// <inheritdoc />
        protected override void StartSocket()
        {
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port));
            Socket.Listen(1024);

            SocketStarted.Invoke(Port);
        }

        /// <inheritdoc />
        protected override void StopSocket()
        {
            foreach (var chs in ConnectedClientSockets)
            {
                // TODO: Send disconnect message
                chs.Stop();
            }

            if (Socket != null)
            {
                Socket.Close();
                Socket = null;
            }

            ConnectedClientSockets.Clear();

            SocketStopped.Invoke();
        }

        /// <summary>
        /// Sends server response to all players.
        /// </summary>
        /// <param name="response">Response message.</param>
        public void SendToAll(ServerResponse response)
        {
            foreach (var clientSocket in ConnectedClientSockets)
            {
                clientSocket.SendMessage(response);
            }
        }

        // TODO: needs to be rewritten - updated (at least synchronized with actual Server/MessageController)
    }
}
