namespace CarcassonneDiscovery.Server
{
    using System;
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Client handler for remote clients.
    /// </summary>
    public class RemoteClientHandler : IClientHandler
    {
        /// <inheritdoc />
        public event Action<ClientRequest> MessageReceived = (cr) => { };

        /// <inheritdoc />
        public string Id { get; protected set; }

        /// <summary>
        /// Socket to client.
        /// </summary>
        protected ClientHandlerSocket Socket { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="socket">Socket to client.</param>
        /// <param name="id">Id of the client.</param>
        public RemoteClientHandler(ClientHandlerSocket socket, string id)
        {
            Id = id;
            Socket = socket;
            Socket.MessageReceived += OnMessageReceived;
            OnMessageReceived();
        }

        /// <inheritdoc />
        public void Disconnect()
        {
            Socket.Stop();
            ServerServiceProvider.Logger.Log("Client was disconnected from the server.", LogLevel.Normal, LogType.Messaging);
        }

        /// <inheritdoc />
        public void SendMessage(ServerResponse msg)
        {
            Socket.SendMessage(msg);
            ServerServiceProvider.Logger.Log("Message sent to client.", LogLevel.Normal, LogType.Messaging);
        }

        /// <summary>
        /// When a message is received, enqueues it in the action queue.
        /// </summary>
        public void OnMessageReceived()
        {
            while (true)
            {
                var msg = Socket.GetNextMessage();

                if (msg == null)
                {
                    break;
                }

                ServerServiceProvider.ServerController.EnqueueAction(new MessageReceivedAction(Id, msg));
            }
        }
    }
}
