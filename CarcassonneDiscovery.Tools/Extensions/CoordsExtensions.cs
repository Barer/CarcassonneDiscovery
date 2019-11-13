namespace CarcassonneDiscovery.Tools
{
    using System;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Extensions of <see cref="Coords"/> class.
    /// </summary>
    public static class CoordsExtensions
    {
        /// <summary>
        /// Computes coordinates of a neighboring tile in a grid for given direction.
        /// </summary>
        /// <param name="coords">Coordinates of a tile.</param>
        /// <param name="direction">Direction where the neighboring tile is located.</param>
        /// <returns>Coordinates of a neighboring tile.</returns>
        public static Coords GetNeighboringCoords(Coords coords, TileOrientation direction)
        {
            switch (direction)
            {
                case TileOrientation.N:
                    return new Coords(coords.X, coords.Y - 1);
                case TileOrientation.E:
                    return new Coords(coords.X + 1, coords.Y);
                case TileOrientation.S:
                    return new Coords(coords.X, coords.Y + 1);
                case TileOrientation.W:
                    return new Coords(coords.X - 1, coords.Y);
            }

            throw new ArgumentException("Invalid direction.");
        }
    }
}
