namespace CarcassonneDiscovery.Logic
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Request for place tile action execution.
    /// </summary>
    public class PlaceTileExecutionRequest : GameExecutionRequest
    {
        /// <summary>
        /// Color of player making the move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// The placed tile.
        /// </summary>
        public ITileScheme Tile { get; set; }

        /// <summary>
        /// Coordinates of the tile.
        /// </summary>
        public Coords Coords { get; set; }

        /// <summary>
        /// Orientation of the tile.
        /// </summary>
        public TileOrientation Orientation { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="tile">The placed tile.</param>
        /// <param name="coords">Coordinates of the tile.</param>
        /// <param name="orientation">Orientation of the tile.</param>
        public PlaceTileExecutionRequest(PlayerColor color, ITileScheme tile, Coords coords, TileOrientation orientation)
        {
            Color = color;
            Tile = tile;
            Coords = coords;
            Orientation = orientation;
        }
    }
}
