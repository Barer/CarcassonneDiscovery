namespace CarcassonneDiscovery.Messaging
{
    using System;
    using System.IO;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading;

    /// <summary>
    /// Socket on the server side handling communication with single client.
    /// </summary>
    public class ClientHandlerSocket : MessageSocket<ServerResponse, ClientRequest>
    {
        /// <summary>
        /// Event when client has disconnected.
        /// </summary>
        public event Action Disconnected = () => { };

        /// <summary>
        /// Parent server socket.
        /// </summary>
        protected ServerSocket Parent;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="parent">Parent server socket.</param>
        /// <param name="socket">Socket to the client.</param>
        public ClientHandlerSocket(ServerSocket parent, Socket socket)
        {
            Socket = socket;
            Parent = parent;

            MessageReceived += () => parent.GetMessagesFromClientHandler(this);
            Disconnected += () => parent.DisconnectedClient(this);
        }


        /// <inheritdoc />
        protected override void ListeningLoop()
        {
            const int BUFFER_SIZE = 4096;
            char[] DELIM = new char[] { '$' };

            StringBuilder backBuffer = new StringBuilder();
            byte[] buffer = new byte[BUFFER_SIZE];

            int received = -1;

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
                        Disconnected.Invoke();
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
                        ClientRequest msg = null;

                        try
                        {
                            msg = (ClientRequest)ReceiveMsgSerializer.Deserialize(msgReader);
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
