namespace CarcassonneDiscovery.Server
{
    using System;
    using System.Collections.Generic;
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Messaging;

    using ClientId = System.String;

    /// <summary>
    /// Sends messages to and receives messages from clients.
    /// </summary>
    public class ClientMessager
    {
        /// <summary>
        /// Connected clients.
        /// </summary>
        protected Dictionary<ClientId, IClientHandler> Clients { get; set; } = new Dictionary<ClientId, IClientHandler>();

        /// <summary>
        /// Ids of the players.
        /// </summary>
        public Dictionary<PlayerColor, ClientId> Players { get; set; } = new Dictionary<PlayerColor, ClientId>();

        /// <summary>
        /// Server socket.
        /// </summary>
        protected ServerSocket Socket { get; set; }

        /// <summary>
        /// Starts the socket.
        /// </summary>
        /// <param name="port">Port number.</param>
        public void StartSocket(int port)
        {
            Socket = new ServerSocket { Port = port };
            Socket.ClientConnected += (chs) =>
            {
                lock (Clients)
                {
                    var clientId = Guid.NewGuid().ToString();
                    var rch = new RemoteClientHandler(chs);
                    Clients.Add(clientId, rch);

                    ServerServiceProvider.Logger.Log($"New client connected: {clientId}", LogLevel.Normal, LogType.Messaging);
                }
            };
            Socket.Start();
        }

        /// <summary>
        /// Stops the socket.
        /// </summary>
        public void StopSocket()
        {
            Socket?.Stop();
        }

        /// <summary>
        /// Disconnects all clients from the server.
        /// </summary>
        public void DisconnectAll()
        {
            foreach (var client in Clients.Values)
            {
                client.Disconnect();
            }
        }

        /// <summary>
        /// Adds player client record.
        /// </summary>
        /// <param name="color">Color of player.</param>
        /// <param name="clientId">Client controlling the player.</param>
        public void AddPlayer(PlayerColor color, ClientId clientId)
        {
            Players.Add(color, clientId);
        }

        /// <summary>
        /// Sends execution result to a client.
        /// </summary>
        /// <param name="clientId">Id of the client.</param>
        /// <param name="msg">Message to be sent.</param>
        public virtual void SendToClient(ClientId clientId, ServerResponse msg)
        {
            if (!Clients.TryGetValue(clientId, out var clientHandler))
            {
                ServerServiceProvider.Logger.Log($"Unknown client: {clientId}", LogLevel.Warning, LogType.Messaging);
                return;
            }

            clientHandler.SendMessage(msg);

            ServerServiceProvider.Logger.Log($"Message sent to client {clientId}.", LogLevel.Normal, LogType.Messaging);
        }

        /// <summary>
        /// Sends execution result to a player.
        /// </summary>
        /// <param name="color">Color of the player.</param>
        /// <param name="msg">Message to be sent.</param>
        public virtual void SendToPlayer(PlayerColor color, ServerResponse msg)
        {
            if (Players.TryGetValue(color, out var clientId))
            {
                SendToClient(clientId, msg);
            }
            else
            {
                ServerServiceProvider.Logger.Log("Cannot find player with given color.", LogLevel.ProgramError, LogType.Messaging);
            }
        }

        /// <summary>
        /// Sends execution result to all clients.
        /// </summary>
        /// <param name="msg">Game execution result to be sent.</param>
        public virtual void SendToAll(ServerResponse msg)
        {
            foreach (var clientId in Clients.Keys)
            {
                SendToClient(clientId, msg);
            }

            ServerServiceProvider.Logger.Log("Message sent to all clients.", LogLevel.Normal, LogType.Messaging);
        }
    }
}
