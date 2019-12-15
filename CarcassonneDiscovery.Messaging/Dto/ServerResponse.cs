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

        /// <summary>
        /// Exit code of the response.
        /// </summary>
        public ExitCode ExitCode { get; set; }

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
        /// Names of the players.
        /// </summary>
        public string[] PlayerNames { get; set; } = null;

        /// <summary>
        /// Name of player or tile set.
        /// </summary>
        public string Name { get; set; } = null;

        #region Serialization settings
        /// <summary>
        /// Should the <see cref="Color"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeColor()
        {
            return Color.HasValue;
        }

        /// <summary>
        /// Should the <see cref="Tile"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeTile()
        {
            return Tile != null;
        }

        /// <summary>
        /// Should the <see cref="Coords"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeCoords()
        {
            return Coords.HasValue;
        }

        /// <summary>
        /// Should the <see cref="Orientation"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeOrientation()
        {
            return Orientation.HasValue;
        }

        /// <summary>
        /// Should the <see cref="RegionId"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeRegionId()
        {
            return RegionId.HasValue;
        }

        /// <summary>
        /// Should the <see cref="Score"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeScore()
        {
            return Score.HasValue;
        }

        /// <summary>
        /// Should the <see cref="PlayerAmount"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializePlayerAmount()
        {
            return PlayerAmount.HasValue;
        }

        /// <summary>
        /// Should the <see cref="FollowerAmount"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeFollowerAmount()
        {
            return FollowerAmount.HasValue;
        }

        /// <summary>
        /// Should the <see cref="PlayerOrder"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializePlayerOrder()
        {
            return PlayerOrder != null;
        }

        /// <summary>
        /// Should the <see cref="PlayerNames"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializePlayerNames()
        {
            return PlayerNames != null;
        }

        /// <summary>
        /// Should the <see cref="Name"/> property be serialized.
        /// </summary>
        /// <returns>True if the property should be serialized.</returns>
        public bool ShouldSerializeName()
        {
            return Name != null;
        }
        #endregion
    }
}
