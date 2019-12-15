namespace CarcassonneDiscovery.Socketing
{
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading;

    /// <summary>
    /// Socket on the server side handling communication with all clients.
    /// </summary>
    public class ServerSocket
    {
        /// <summary>
        /// Event when new client has connected to the server.
        /// </summary>
        public event Action<ClientHandlerSocket> ClientConnected = (chs) => { };

        /// <summary>
        /// Event when client has disconnected from the server.
        /// </summary>
        public event Action<ClientHandlerSocket> ClientDisconnected = (chs) => { };

        /// <summary>
        /// Port of the socket.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Thread that listens to server responses.
        /// </summary>
        protected Thread ListeningLoopThread { get; set; }

        /// <summary>
        /// Actual socket.
        /// </summary>
        protected Socket Socket { get; set; }

        /// <summary>
        /// Starts connection and listening to the server.
        /// </summary>
        public void Start()
        {
            // Start the actual socket
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            Socket.Bind(new IPEndPoint(IPAddress.Parse("127.0.0.1"), Port));
            Socket.Listen(1024);

            // Start the thread
            ListeningLoopThread = new Thread(new ThreadStart(ListeningLoop));
            ListeningLoopThread.Start();
        }

        /// <summary>
        /// Stops connection to the server.
        /// </summary>
        public void Stop()
        {
            // Stop the actual socket
            if (Socket != null)
            {
                Socket.Close();
                Socket = null;
            }

            // Stop the thread
            if (ListeningLoopThread != null)
            {
                ListeningLoopThread.Abort();
            }
        }

        /// <summary>
        /// Loop that serves as listener to the server responses.
        /// </summary>
        protected void ListeningLoop()
        {
            try
            {
                while (true)
                {
                    var newClient = Socket.Accept();

                    var newChs = new ClientHandlerSocket(newClient);

                    ClientConnected.Invoke(newChs);

                    newChs.Start();
                }
            }
            catch (ThreadAbortException)
            {
            }
        }
    }
}
