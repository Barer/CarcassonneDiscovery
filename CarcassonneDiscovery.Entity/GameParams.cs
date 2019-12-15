﻿namespace CarcassonneDiscovery.Entity
{
    /// <summary>
    /// Game rules parameters.
    /// </summary>
    public class GameParams
    {
        /// <summary>
        /// Number of players in the game.
        /// </summary>
        public int PlayerAmount { get; set; }

        /// <summary>
        /// Number of followers for each player.
        /// </summary>
        public int FollowerAmount { get; set; }

        /// <summary>
        /// Order of players in the game.
        /// </summary>
        public PlayerColor[] PlayerOrder { get; set; } = null;

        /// <summary>
        /// Name (and parameters) of the tile set.
        /// </summary>
        public string TileSet { get; set; } = null;
    }
}
