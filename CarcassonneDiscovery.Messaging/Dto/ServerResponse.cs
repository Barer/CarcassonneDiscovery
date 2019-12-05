namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Message from server to client.
    /// </summary>
    public class ServerResponse
    {
        /// <summary>
        /// Type of message.
        /// </summary>
        public ServerResponseType Type { get; set; }

        #region Game execution
        /// <summary>
        /// Type of rule violation.
        /// </summary>
        public RuleViolationType? RuleViolation { get; set; } = null;

        /// <summary>
        /// Color of the player.
        /// </summary>
        public PlayerColor? Color { get; set; } = null;

        /// <summary>
        /// Placed tile.
        /// </summary>
        public Tile Tile { get; set; } = null;

        /// <summary>
        /// Coordinates of tile.
        /// </summary>
        public Coords? Coords { get; set; } = null;

        /// <summary>
        /// Orientation of tile.
        /// </summary>
        public TileOrientation? Orientation { get; set; } = null;

        /// <summary>
        /// Id of region.
        /// </summary>
        public int? RegionId { get; set; } = null;

        /// <summary>
        /// Points scored.
        /// </summary>
        public int? Score { get; set; } = null;

        /// <summary>
        /// Removements of followers in the end of the game.
        /// </summary>
        public ServerResponse[] FollowerRemovements { get; set; } = null;
        #endregion

        #region Game parameters (in game execution)
        /// <summary>
        /// Number of players in the game.
        /// </summary>
        public int? PlayerAmount { get; set; } = null;

        /// <summary>
        /// Number of followers for each player.
        /// </summary>
        public int? FollowerAmount { get; set; } = null;

        /// <summary>
        /// Order of players in the game.
        /// </summary>
        public PlayerColor[] PlayerOrder { get; set; } = null;

        /// <summary>
        /// Name of the tile set.
        /// </summary>
        public string TileSetName { get; set; } = null;

        /// <summary>
        /// Names of the players.
        /// </summary>
        public string[] PlayerNames { get; set; } = null;
        #endregion
    }
}
