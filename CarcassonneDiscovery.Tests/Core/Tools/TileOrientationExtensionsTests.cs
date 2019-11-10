namespace CarcassonneDiscovery.Tests.Core.Tools
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Tools;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Tests <see cref="TileOrientationExtensions"/>
    /// </summary>
    [TestClass]
    public class TileOrientationExtensionsTests
    {
        /// <summary>
        /// All correct angles of rotation.
        /// </summary>
        private TileOrientation[] allAngles = new TileOrientation[] { TileOrientation.N, TileOrientation.E, TileOrientation.S, TileOrientation.W };

        /// <summary>
        /// Rotates all orientation by north.
        /// </summary>
        [TestMethod]
        public void RotateByNorth_ReturnsSameAngle()
        {
            foreach (var orientation in AllOrientations())
            {
                var rotated = orientation.Rotate(TileOrientation.N);

                Assert.AreEqual(rotated, orientation);
            }
        }

        /// <summary>
        /// Rotates all orientation by east.
        /// </summary>
        [TestMethod]
        public void RotateByEast_ReturnsCorrectAngle()
        {
            var rotatedPart = new Dictionary<TileOrientation, TileOrientation>
            {
                { TileOrientation.None, TileOrientation.None },
                { TileOrientation.N, TileOrientation.E },
                { TileOrientation.E, TileOrientation.S },
                { TileOrientation.S, TileOrientation.W },
                { TileOrientation.W, TileOrientation.N }
            };

            foreach (var orientation in AllOrientations())
            {
                var rotated = orientation.Rotate(TileOrientation.E);

                var expectedN = rotatedPart[orientation & TileOrientation.N];
                var expectedE = rotatedPart[orientation & TileOrientation.E];
                var expectedS = rotatedPart[orientation & TileOrientation.S];
                var expectedW = rotatedPart[orientation & TileOrientation.W];

                var expected = expectedN | expectedE | expectedS | expectedW;

                Assert.AreEqual(expected, rotated);
            }
        }

        /// <summary>
        /// Rotates all orientation by south.
        /// </summary>
        [TestMethod]
        public void RotateBySouth_ReturnsCorrectAngle()
        {
            var rotatedPart = new Dictionary<TileOrientation, TileOrientation>
            {
                { TileOrientation.None, TileOrientation.None },
                { TileOrientation.N, TileOrientation.S },
                { TileOrientation.E, TileOrientation.W },
                { TileOrientation.S, TileOrientation.N },
                { TileOrientation.W, TileOrientation.E }
            };

            foreach (var orientation in AllOrientations())
            {
                var rotated = orientation.Rotate(TileOrientation.S);

                var expectedN = rotatedPart[orientation & TileOrientation.N];
                var expectedE = rotatedPart[orientation & TileOrientation.E];
                var expectedS = rotatedPart[orientation & TileOrientation.S];
                var expectedW = rotatedPart[orientation & TileOrientation.W];

                var expected = expectedN | expectedE | expectedS | expectedW;

                Assert.AreEqual(expected, rotated);
            }
        }

        /// <summary>
        /// Rotates all orientation by west.
        /// </summary>
        [TestMethod]
        public void RotateByWest_ReturnsCorrectAngle()
        {
            var rotatedPart = new Dictionary<TileOrientation, TileOrientation>
            {
                { TileOrientation.None, TileOrientation.None },
                { TileOrientation.N, TileOrientation.W },
                { TileOrientation.E, TileOrientation.N },
                { TileOrientation.S, TileOrientation.E },
                { TileOrientation.W, TileOrientation.S }
            };

            foreach (var orientation in AllOrientations())
            {
                var rotated = orientation.Rotate(TileOrientation.W);

                var expectedN = rotatedPart[orientation & TileOrientation.N];
                var expectedE = rotatedPart[orientation & TileOrientation.E];
                var expectedS = rotatedPart[orientation & TileOrientation.S];
                var expectedW = rotatedPart[orientation & TileOrientation.W];

                var expected = expectedN | expectedE | expectedS | expectedW;

                Assert.AreEqual(expected, rotated);
            }
        }

        /// <summary>
        /// Derotates all orientations.
        /// </summary>
        [TestMethod]
        public void Derotate_ReturnsInverseOfRotate()
        {
            foreach (var orientation in AllOrientations())
            {
                foreach (var angle in allAngles)
                {
                    var derotated = orientation.Derotate(angle);
                    var rotatedBack = derotated.Rotate(angle);

                    Assert.AreEqual(orientation, rotatedBack);
                }
            }
        }

        /// <summary>
        /// Rotates all orientations by angles. Checks whether angle is invalid.
        /// </summary>
        [TestMethod]
        public void Rotate_InvalidAngleThrowsArgumentException_ElseOk()
        {
            foreach (var orientation in AllOrientations())
            {
                foreach (var angle in AllOrientations())
                {
                    if (allAngles.Contains(angle))
                    {
                        orientation.Rotate(angle);
                    }
                    else
                    {
                        Assert.ThrowsException<ArgumentException>(() => orientation.Rotate(angle));
                    }
                }
            }
        }

        /// <summary>
        /// Derotates all orientations by angles. Checks whether angle is invalid.
        /// </summary>
        [TestMethod]
        public void Derotate_InvalidAngleThrowsArgumentException_ElseOk()
        {
            foreach (var orientation in AllOrientations())
            {
                foreach (var angle in AllOrientations())
                {
                    if (allAngles.Contains(angle))
                    {
                        orientation.Derotate(angle);
                    }
                    else
                    {
                        Assert.ThrowsException<ArgumentException>(() => orientation.Derotate(angle));
                    }
                }
            }
        }

        /// <summary>
        /// Returns all correct tile orientations.
        /// </summary>
        /// <returns>Enumerator on all correct tile orientation.</returns>
        protected IEnumerable<TileOrientation> AllOrientations()
        {
            foreach (var angleN in new TileOrientation[] { TileOrientation.N, TileOrientation.None })
            {
                foreach (var angleE in new TileOrientation[] { TileOrientation.E, TileOrientation.None })
                {
                    var angleNE = angleE | angleN;

                    foreach (var angleS in new TileOrientation[] { TileOrientation.S, TileOrientation.None })
                    {
                        var angleNES = angleNE | angleS;

                        foreach (var angleW in new TileOrientation[] { TileOrientation.W, TileOrientation.None })
                        {
                            var angleNESW = angleNES | angleW;

                            yield return angleNESW;
                        }
                    }
                }
            }
        }
    }
}
