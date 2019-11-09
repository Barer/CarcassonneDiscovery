namespace CarcassonneDiscovery.Entity
{
    /// <summary>
    /// Phase of the move.
    /// </summary>
    public enum MoveWorkflow
    {
        /// <summary>
        /// Game has just started.
        /// </summary>
        GameStarted,

        /// <summary>
        /// The first tile has just been placed.
        /// </summary>
        FirstTilePlaced,

        /// <summary>
        /// Move has just started, the tile was selected.
        /// </summary>
        NextTileSelected,

        /// <summary>
        /// The tile was just placed.
        /// </summary>
        TilePlaced,

        /// <summary>
        /// The follower has just been placed or removed or the player passed the move.
        /// </summary>
        MoveEnded,

        /// <summary>
        /// The game has ended and scores for the remaining followers will be counted.
        /// </summary>
        EndFollowerCounting,

        /// <summary>
        /// The game has ended and the results are final.
        /// </summary>
        GameEnded
    }
}
