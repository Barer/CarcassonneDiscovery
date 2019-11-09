namespace CarcassonneDiscovery.Entity
{
    /// <summary>
    /// Orientation of a tile.
    /// </summary>
    /// <remarks>
    /// It is also used as location on tile border or direction to neighboring tiles.
    /// </remarks>
    public enum TileOrientation : int
    {
        #region Cardinal directions
        /// <summary>
        /// North.
        /// </summary>
        N = 0b0001,

        /// <summary>
        /// East.
        /// </summary>
        E = 0b0010,

        /// <summary>
        /// South.
        /// </summary>
        S = 0b0100,

        /// <summary>
        /// West.
        /// </summary>
        W = 0b1000,
        #endregion

        #region Total 0 orientation
        /// <summary>
        /// No orientation
        /// </summary>
        None = 0b0000,
        #endregion

        #region Total 2 orientations
        /// <summary>
        /// North and east.
        /// </summary>
        NE = N | E,

        /// <summary>
        /// North and south.
        /// </summary>
        NS = N | S,

        /// <summary>
        /// North and west.
        /// </summary>
        NW = N | W,

        /// <summary>
        /// East and south.
        /// </summary>
        ES = E | S,

        /// <summary>
        /// East and west.
        /// </summary>
        EW = E | W,

        /// <summary>
        /// South and west.
        /// </summary>
        SW = S | W,
        #endregion

        #region Total 3 orientations
        /// <summary>
        /// North, east and south.
        /// </summary>
        NES = N | E | S,

        /// <summary>
        /// North, east and west.
        /// </summary>
        NEW = N | E | W,

        /// <summary>
        /// North, south and west.
        /// </summary>
        NSW = N | S | W,

        /// <summary>
        /// East, south and west.
        /// </summary>
        ESW = E | S | W,
        #endregion

        #region Total 4 orientations
        /// <summary>
        /// North, east, south and west.
        /// </summary>
        NESW = N | E | S | W
        #endregion
    }
}
