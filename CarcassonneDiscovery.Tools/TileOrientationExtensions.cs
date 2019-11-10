namespace CarcassonneDiscovery.Tools
{
    using System;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Extension methods for <see cref="TileOrientation"/>
    /// </summary>
    public static class TileOrientationExtensions
    {
        /// <summary>
        /// Rotates tile orientation by given angle.
        /// </summary>
        /// <param name="orientation">Orientation to be rotated.</param>
        /// <param name="angle">Angle of rotation.</param>
        /// <returns>Rotated orientation.</returns>
        public static TileOrientation Rotate(this TileOrientation orientation, TileOrientation angle)
        {
            int shift;

            switch (angle)
            {
                case TileOrientation.N:
                    shift = 0;
                    break;
                case TileOrientation.E:
                    shift = 1;
                    break;
                case TileOrientation.S:
                    shift = 2;
                    break;
                case TileOrientation.W:
                    shift = 3;
                    break;
                default:
                    throw new ArgumentException("Invalid angle of rotation.");
            }

            var rotated = (int)orientation << shift;
            var result = (((rotated & 0b1111000) >> 4) | rotated) & 0b1111;

            return (TileOrientation)result;
        }

        /// <summary>
        /// Gets tile orientation that was initial to given rotated angle.
        /// </summary>
        /// <param name="orientation">Orientation after rotation.</param>
        /// <param name="angle">Angle of rotation.</param>
        /// <returns>Orientation before rotation.</returns>
        public static TileOrientation Derotate(this TileOrientation orientation, TileOrientation angle)
        {
            int shift;

            switch (angle)
            {
                case TileOrientation.N:
                    shift = 0;
                    break;
                case TileOrientation.E:
                    shift = 3;
                    break;
                case TileOrientation.S:
                    shift = 2;
                    break;
                case TileOrientation.W:
                    shift = 1;
                    break;
                default:
                    throw new ArgumentException("Invalid angle of rotation.");
            }

            var rotated = (int)orientation << shift;
            var result = (((rotated & 0b1111000) >> 4) | rotated) & 0b1111;

            return (TileOrientation)result;
        }
    }
}
