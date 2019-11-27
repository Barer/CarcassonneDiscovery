namespace CarcassonneDiscovery.Server
{
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
        protected Dictionary<ClientId, IClientHandler> Clients { get; set; }

        /// <summary>
        /// Ids of the players.
        /// </summary>
        public Dictionary<PlayerColor, ClientId> Players { get; set; }

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
            };

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
