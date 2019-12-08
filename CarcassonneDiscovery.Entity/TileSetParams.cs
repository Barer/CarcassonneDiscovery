namespace CarcassonneDiscovery.Entity
{
    using System;

    /// <summary>
    /// Parameters of a tile set.
    /// </summary>
    public class TileSetParams
    {
        /// <summary>
        /// Name of the tile set.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Tile supplier builder function.
        /// </summary>
        public Func<ITileSupplier> TileSupplierBuilder { get; set; } = new Func<ITileSupplier>(() => null);
    }
}
