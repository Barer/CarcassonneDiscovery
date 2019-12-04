namespace CarcassonneDiscovery.Messaging
{
    using System;
    using System.IO;
    using System.Net;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

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
            if (Socket != null)
            {
                Socket.Disconnect(false);
                Socket.Close();
                Socket = null;
            }
        }

        /// <inheritdoc />
        protected override void ListeningLoop()
        {
            const int BUFFER_SIZE = 4096;
            char[] DELIM = new char[] { '$' };

            StringBuilder backBuffer = new StringBuilder();
            byte[] buffer = new byte[BUFFER_SIZE];

            int received;

            try
            {
                while (true)
                {
                    try
                    {
                        received = Socket.Receive(buffer);
                    }
                    catch (SocketException)
                    {
                        ServerDisconnected.Invoke();
                        Stop();
                        break;
                    }

                    backBuffer.Append(Encoding.UTF8.GetString(buffer, 0, received));

                    if (received == BUFFER_SIZE)
                    {
                        continue;
                    }

                    string[] msgStr = backBuffer.ToString().Split(DELIM, StringSplitOptions.RemoveEmptyEntries);
                    backBuffer = new StringBuilder();

                    foreach (string s in msgStr)
                    {
                        StringReader msgReader = new StringReader(s);

                        ServerResponse msg = null;

                        try
                        {
                            msg = (ServerResponse)ReceiveMsgSerializer.Deserialize(msgReader);
                        }
                        catch (Exception)
                        {                            
                            continue;
                        }

                        EnqueueMessage(msg);

                        MessageReceivedInvoke();
                    }
                }
            }
            catch (ThreadAbortException)
            {
            }
        }
    }
}
