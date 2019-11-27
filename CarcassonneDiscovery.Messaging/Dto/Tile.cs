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
        /// Scheme of 
        /// </summary>
        public TileScheme Scheme { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="scheme">Scheme of the tile.</param>
        public Tile(ITileScheme scheme)
        {
            Scheme = scheme.Copy();
        }
    }
}
