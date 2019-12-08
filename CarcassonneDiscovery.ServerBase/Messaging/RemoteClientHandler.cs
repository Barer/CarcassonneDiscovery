namespace CarcassonneDiscovery.Server
{
    using System;
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Client handler for remote clients.
    /// </summary>
    public class RemoteClientHandler : IClientHandler
    {
        /// <iheritdoc />
        public event Action<ClientRequest> MessageReceived = (cr) => { };

        /// <iheritdoc />
        public string Id { get; protected set; }

        /// <summary>
        /// Socket to client.
        /// </summary>
        protected ClientHandlerSocket Socket { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="socket">Socket to client.</param>
        public RemoteClientHandler(ClientHandlerSocket socket)
        {
            Socket = socket;
            Socket.MessageReceived += OnMessageReceived;
            OnMessageReceived();
        }

        /// <iheritdoc />
        public void Disconnect()
        {
            Socket.Stop();
            ServerServiceProvider.Logger.Log("Client was disconnected from the server.", LogLevel.Normal, LogType.Messaging);
        }

        /// <iheritdoc />
        public void SendMessage(ServerResponse msg)
        {
            Socket.SendMessage(msg);
            ServerServiceProvider.Logger.Log("Message sent to client.", LogLevel.Normal, LogType.Messaging);
        }

        /// <summary>
        /// When a message is recevied, enqueues it in the action queue.
        /// </summary>
        public void OnMessageReceived()
        {
            while (true)
            {
                var msg = Socket.GetNextMessage();

                if (msg == null)
                    break;

                ServerServiceProvider.ServerController.EnqueueAction(new MessageReceivedAction(Id, msg));
            }
        }
    }
}
