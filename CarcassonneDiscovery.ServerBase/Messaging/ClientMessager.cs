namespace CarcassonneDiscovery.Server
{
    using System.Collections.Generic;
    using CarcassonneDiscovery.Entity;

    using ClientId = System.String;
    using ServerResponse = System.Object;

    /// <summary>
    /// Sends messages to and receives messages from clients.
    /// </summary>
    public class ClientMessager
    {
        /// <summary>
        /// Connected clients.
        /// </summary>
        protected Dictionary<ClientId, object> Clients { get; set; }

        /// <summary>
        /// Ids of the players.
        /// </summary>
        public Dictionary<PlayerColor, ClientId> Players { get; set; }

        /// <summary>
        /// Sends message to a client.
        /// </summary>
        /// <param name="clientId">Id of the client.</param>
        /// <param name="msg">Message to be sent.</param>
        public virtual void SendToClient(ClientId clientId, ServerResponse msg)
        {
            // TODO (with error)
            ServerServiceProvider.Logger.Log($"Message sent to client {clientId}.", LogLevel.Normal, LogType.Messaging);
        }

        /// <summary>
        /// Sends message to a player.
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
        /// Sends message to all clients.
        /// </summary>
        /// <param name="msg">Message to be sent.</param>
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
