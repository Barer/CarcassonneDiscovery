namespace CarcassonneDiscovery.Entity
{
    /// <summary>
    /// Supplier of the tiles.
    /// </summary>
    public interface ITileSupplier
    {
        /// <summary>
        /// Returns amount of remaining tiles in the set.
        /// </summary>
        int RemainingCount { get; }

        /// <summary>
        /// Returns next tile in the set.
        /// </summary>
        /// <returns>Next tile.</returns>
        ITileScheme GetNextTile();

        /// <summary>
        /// Returns first tile in the set.
        /// </summary>
        /// <returns>First tile.</returns>
        ITileScheme GetFirstTile();

        /// <summary>
        /// Gives tile back to the set.
        /// </summary>
        /// <param name="tile">Tile to be given back to the set.</param>
        void ReturnTile(ITileScheme tile);
    }
}