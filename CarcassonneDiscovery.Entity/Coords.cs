namespace CarcassonneDiscovery.Entity
{
    /// <summary>
    /// Coordinates of a tile in a grid.
    /// </summary>
    public struct Coords
    {
        /// <summary>
        /// X coordinate.
        /// </summary>
        public int X { get; set; }

        /// <summary>
        /// Y coordinate.
        /// </summary>
        public int Y { get; set; }

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="x">X coordinate.</param>
        /// <param name="y">Y coordinate.</param>
        public Coords(int x, int y)
        {
            X = x;
            Y = y;
        }

        /// <summary>
        /// Compares two coordinates on equality.
        /// </summary>
        /// <param name="a">Coordinates on the left side.</param>
        /// <param name="b">Coordinates on the right side.</param>
        /// <returns>True if the two coordinates are equal; otherwise false.</returns>
        public static bool operator ==(Coords a, Coords b)
        {
            return a.X == b.X && a.Y == b.Y;
        }

        /// <summary>
        /// Compares two coordinates on inequality.
        /// </summary>
        /// <param name="a">Coordinates on the left side.</param>
        /// <param name="b">Coordinates on the right side.</param>
        /// <returns>True if the two coordinates are not equal; otherwise false.</returns>
        public static bool operator !=(Coords a, Coords b)
        {
            return !(a == b);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            if (obj is Coords)
            {
                return this == (Coords)obj;
            }

            return false;
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            return (X << 16) ^ Y;
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"[{X},{Y}]";
        }
    }
}
