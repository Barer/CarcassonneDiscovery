namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Tools;

    /// <summary>
    /// Tile representation in messages.
    /// </summary>
    public class Tile
    {
        /// <summary>
        /// Scheme of the tile.
        /// </summary>
        public TileScheme Value { get; set; }

        /// <summary>
        /// Empty constructor.
        /// </summary>
        public Tile()
        {
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="scheme">Scheme of the tile.</param>
        public Tile(ITileScheme scheme)
        {
            Value = scheme?.Copy();
        }
    }
}
