namespace CarcassonneDiscovery.UserClient
{
    using CarcassonneDiscovery.SimulationLibrary;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;

    /// <summary>
    /// Tile in the grid.
    /// </summary>
    public partial class TileRectangle : UserControl
    {
        /// <summary>
        /// Event when mouse enters a region.
        /// </summary>
        protected event Action<int> RegionMouseEnter = (i) => { };

        /// <summary>
        /// Event when mouse exits a region.
        /// </summary>
        protected event Action<int> RegionMouseExit = (i) => { };

        /// <summary>
        /// Event when mouse clicks on a region.
        /// </summary>
        public event Action<Coords, int> RegionMouseClick = (coords, i) => { };

        /// <summary>
        /// Event when mouse clicks on a follower.
        /// </summary>
        public event Action<Coords> FollowerMouseClick = (coords) => { };

        /// <summary>
        /// If true, region mouse events are invoked.
        /// </summary>
        protected bool _RegionMouseEventEnabled;

        /// <summary>
        /// If true, follower mouse events are invoked.
        /// </summary>
        protected bool _FollowerMouseEventEnabled;

        /// <summary>
        /// If true, region mouse events are invoked.
        /// </summary>
        public bool RegionMouseEventEnabled
        {
            get => _RegionMouseEventEnabled;
            set
            {
                if (!(_RegionMouseEventEnabled = value))
                {
                    App.Current.Dispatcher.Invoke(() => RegionMouseExit(-1));
                }
            }
        }

        /// <summary>
        /// If true, follower mouse event are invoked.
        /// </summary>
        public bool FollowerMouseEventEnabled
        {
            get => _FollowerMouseEventEnabled;
            set
            {
                if (!(_FollowerMouseEventEnabled = value))
                {
                    Follower_MouseLeave(null, null);
                }
            }
        }

        /// <summary>
        /// Scheme of the tile.
        /// </summary>
        protected TileScheme Scheme;

        /// <summary>
        /// Orientation of the tile.
        /// </summary>
        public TileOrientation Orientation { get; protected set; }

        /// <summary>
        /// Coordinates of the tile.
        /// </summary>
        public Coords Coords;

        /// <summary>
        /// List of shapes connected to the same region (by its id).
        /// </summary>
        protected Dictionary<int, List<Shape>> RegionShapes = new Dictionary<int, List<Shape>>();

        /// <summary>
        /// List of shapes that represent visible cities.
        /// </summary>
        protected List<Shape> CityShapes = new List<Shape>();


        public TileRectangle()
        {
            InitializeComponent();

            RegionMouseEnter += (i) =>
            {
                if (i == -1)
                {
                    foreach (var rs in RegionShapes.Values)
                    {
                        foreach (var r in rs)
                        {
                            r.Opacity = 0.5;
                        }
                    }
                }
                else
                {
                    foreach (var r in RegionShapes[i])
                    {
                        r.Opacity = 0.5;
                    }
                }
            };

            RegionMouseExit += (i) =>
            {
                if (i == -1)
                {
                    foreach (var rs in RegionShapes.Values)
                    {
                        foreach (var r in rs)
                        {
                            r.Opacity = 1;
                        }
                    }
                }
                else
                {
                    foreach (var r in RegionShapes[i])
                    {
                        r.Opacity = 1;
                    }
                }
            };
        }

        #region Layout pattern classes and methods
        /// <summary>
        /// Index of position of city.
        /// </summary>
        protected enum OnTilePosition : int { NE = 0, SE = 1, SW = 2, NW = 3, NC = 4, EC = 5, SC = 6, WC = 7, CC = 8, N = 12, E = 13, S = 14, W = 15 };

        /// <summary>
        /// Tile layout pattern.
        /// </summary>
        protected class TileLayoutData
        {
            /// <summary>
            /// RegionId (in TileScheme) on position (N-E-S-W-NC-EC-SC-WC).
            /// </summary>
            public int[] RegionIds { get; protected set; }

            /// <summary>
            /// Is there a border on position (NE-SE-SW-NW-NC-EC-SC-WC).
            /// </summary>
            public bool[] Borders { get; protected set; }

            /// <summary>
            /// Position of the city between two regions given by Ids[Id0, Id1, Pos].
            /// </summary>
            public Tuple<int, int, OnTilePosition>[] CityRegionData { get; protected set; }

            /// <summary>
            /// Position of follower for given region.
            /// </summary>
            public OnTilePosition[] FollowerPositions { get; protected set; }

            /// <summary>
            /// Default constructor.
            /// </summary>
            /// <param name="regions">Region Id on position.</param>
            /// <param name="borders">Border on position.</param>
            /// <param name="crd">City data.</param>
            /// <param name="fp">Follower positions.</param>
            public TileLayoutData(int[] regions, bool[] borders, Tuple<int, int, OnTilePosition>[] crd, OnTilePosition[] fp)
            {
                RegionIds = regions;
                Borders = borders;
                CityRegionData = crd;
                FollowerPositions = fp;
            }
        }

        /// <summary>
        /// Data about the layout. It is entirely based on TileSchemeLayout (and given tile orientation).
        /// </summary>
        protected readonly Dictionary<TileSchemeLayout, TileLayoutData> LAYOUT_DATA = new Dictionary<TileSchemeLayout, TileLayoutData>()
        {
            {
                TileSchemeLayout.L8, new TileLayoutData(
                    new int[8] { 0, 0, 0, 0, 0, 0, 0, 0 },
                    new bool[8] { false, false, false, false, false, false, false, false },
                    new Tuple<int,int, OnTilePosition>[] { },
                    new OnTilePosition[1]{ OnTilePosition.CC }
                )
            } ,
            {
                TileSchemeLayout.L44, new TileLayoutData(
                    new int[8] { 0, 0, 1, 1, 0, 0, 1, 1  },
                    new bool[8] { false, true, false, true, false, false, false, false },
                    new Tuple<int,int, OnTilePosition>[] {
                        new Tuple<int, int, OnTilePosition>(0, 1, OnTilePosition.CC)
                    },
                    new OnTilePosition[2]{ OnTilePosition.NE, OnTilePosition.SW }
                )
            } ,
            {
                TileSchemeLayout.L116, new TileLayoutData(
                    new int[8] { 0, 1, 2, 2, 2, 2, 2, 2  },
                    new bool[8] { false, false, false, false, true, true, false, false },
                    new Tuple<int,int, OnTilePosition>[] {
                        new Tuple<int, int, OnTilePosition>(0, 2, OnTilePosition.NC),
                        new Tuple<int, int, OnTilePosition>(1, 2, OnTilePosition.EC)
                    },
                    new OnTilePosition[3]{ OnTilePosition.N, OnTilePosition.E, OnTilePosition.CC }
                )
            } ,
            {
                TileSchemeLayout.L161, new TileLayoutData(
                    new int[8] { 0, 1, 2, 1, 1, 1, 1, 1  },
                    new bool[8] { false, false, false, false, true, false, true, false },
                    new Tuple<int,int, OnTilePosition>[] {
                        new Tuple<int, int, OnTilePosition>(0, 1, OnTilePosition.NC),
                        new Tuple<int, int, OnTilePosition>(1, 2, OnTilePosition.SC)
                    },
                    new OnTilePosition[3]{ OnTilePosition.N, OnTilePosition.CC, OnTilePosition.S }
                )
            } ,
            {
                TileSchemeLayout.L17, new TileLayoutData(
                    new int[8] { 0, 1, 1, 1, 1, 1, 1, 1  },
                    new bool[8] { false, false, false, false, true, false, false, false },
                    new Tuple<int,int, OnTilePosition>[] {
                        new Tuple<int, int, OnTilePosition>(0, 1, OnTilePosition.NC)
                    },
                    new OnTilePosition[2]{ OnTilePosition.N, OnTilePosition.CC }
                )
            } ,
            {
                TileSchemeLayout.L11114, new TileLayoutData(
                    new int[8] { 0, 1, 2, 3, 4, 4, 4, 4  },
                    new bool[8] { false, false, false, false, true, true, true, true },
                    new Tuple<int,int, OnTilePosition>[] {
                        new Tuple<int, int, OnTilePosition>(0, 4, OnTilePosition.NC),
                        new Tuple<int, int, OnTilePosition>(1, 4, OnTilePosition.EC),
                        new Tuple<int, int, OnTilePosition>(2, 4, OnTilePosition.SC),
                        new Tuple<int, int, OnTilePosition>(3, 4, OnTilePosition.WC)
                    },
                    new OnTilePosition[5]{ OnTilePosition.N, OnTilePosition.E, OnTilePosition.S, OnTilePosition.W, OnTilePosition.CC }
                )
            } ,
            {
                TileSchemeLayout.L422, new TileLayoutData(
                    new int[8] { 0, 0, 1, 2, 0, 0, 1, 2  },
                    new bool[8] { false, true, true, true, false, false, false, false },
                    new Tuple<int,int, OnTilePosition>[] {
                        new Tuple<int, int, OnTilePosition>(0, 1, OnTilePosition.SE),
                        new Tuple<int, int, OnTilePosition>(0, 2, OnTilePosition.NW),
                        new Tuple<int, int, OnTilePosition>(1, 2, OnTilePosition.SW)
                    },
                    new OnTilePosition[3]{ OnTilePosition.NE, OnTilePosition.SC, OnTilePosition.WC }
                )
            } ,
            {
                TileSchemeLayout.L1115, new TileLayoutData(
                    new int[8] { 0, 1, 2, 3, 3, 3, 3, 3  },
                    new bool[8] { false, false, false, false, true, true, true, false },
                    new Tuple<int, int, OnTilePosition>[] {
                        new Tuple<int, int, OnTilePosition>(0, 3, OnTilePosition.NC),
                        new Tuple<int, int, OnTilePosition>(1, 3, OnTilePosition.EC),
                        new Tuple<int, int, OnTilePosition>(2, 3, OnTilePosition.SC)
                    },
                    new OnTilePosition[4]{ OnTilePosition.N, OnTilePosition.E, OnTilePosition.S, OnTilePosition.WC }
                )
            } ,
            {
                TileSchemeLayout.L2231, new TileLayoutData(
                    new int[8] { 0, 1, 2, 3, 0, 1, 2, 2   },
                    new bool[8] { true, true, false, true, false, false, false, true },
                    new Tuple<int,int, OnTilePosition>[] {
                        new Tuple<int, int, OnTilePosition>(0, 1, OnTilePosition.NE),
                        new Tuple<int, int, OnTilePosition>(0, 2, OnTilePosition.NW),
                        new Tuple<int, int, OnTilePosition>(1, 2, OnTilePosition.SE),
                        new Tuple<int, int, OnTilePosition>(2, 3, OnTilePosition.WC)
                    },
                    new OnTilePosition[4]{ OnTilePosition.N, OnTilePosition.E, OnTilePosition.SC, OnTilePosition.W }
                )
            } ,

            {
                TileSchemeLayout.L2213, new TileLayoutData(
                    new int[8] { 0, 1, 2, 3, 0, 1, 3, 3   },
                    new bool[8] { true, true, false, true, false, false, true, false },
                    new Tuple<int,int, OnTilePosition>[] {
                        new Tuple<int, int, OnTilePosition>(0, 1, OnTilePosition.NE),
                        new Tuple<int, int, OnTilePosition>(0, 3, OnTilePosition.NW),
                        new Tuple<int, int, OnTilePosition>(1, 3, OnTilePosition.SE),
                        new Tuple<int, int, OnTilePosition>(2, 3, OnTilePosition.SC)
                    },
                    new OnTilePosition[4]{ OnTilePosition.N, OnTilePosition.E, OnTilePosition.S, OnTilePosition.WC }
                )
            }
        };

        /// <summary>
        /// Color of regions.
        /// </summary>
        protected readonly Dictionary<RegionType, Brush> REGION_TO_COLOR = new Dictionary<RegionType, Brush>()
        {
            { RegionType.MOUNTAIN, Brushes.Gray },
            { RegionType.GRASSLAND, Brushes.LightGreen },
            { RegionType.SEA, Brushes.LightCyan }
        };

        /// <summary>
        /// Color of the city.
        /// </summary>
        protected readonly Brush CITY_FILL = Brushes.Orange;

        /// <summary>
        /// Colors of the followers.
        /// </summary>
        protected readonly Dictionary<PlayerColor, Brush> FOLLOWER_COLOR = new Dictionary<PlayerColor, Brush>()
        {
            { PlayerColor.RED, Brushes.Red },
            { PlayerColor.GREEN, Brushes.Green },
            { PlayerColor.BLUE, Brushes.Blue },
            { PlayerColor.YELLOW, Brushes.Yellow },
            { PlayerColor.BLACK, Brushes.Black }
        };

        /// <summary>
        /// Coordinates of the follower based on its position.
        /// </summary>
        protected readonly Point[] FOLLOWER_POSITION = new Point[]
        {
            new Point(70, 10), // NE
            new Point(70, 70), // SE
            new Point(10, 70), // SW
            new Point(10, 10), // NW
            new Point(40, 10), // NC
            new Point(70, 40), // EC
            new Point(40, 70), // SC
            new Point(10, 40), // WC
            new Point(40, 40), // CC
            new Point(40, 40), // CC
            new Point(40, 40), // CC
            new Point(40, 40), // CC
            new Point(40, 0), // N
            new Point(80, 40), // E
            new Point(40, 80), // S
            new Point(0, 40)  // W
        };
        #endregion


        /// <summary>
        /// Sets layout of the tile.
        /// </summary>
        /// <param name="scheme">Scheme of the tile.</param>
        /// <param name="orientation">Orientation of the tile.</param>
        public void SetLayout(TileScheme scheme, TileOrientation orientation)
        {
            Scheme = scheme;
            Orientation = orientation;

            if (scheme != null)
            {
                App.Current.Dispatcher.Invoke(() => Draw());
            }
        }

        /// <summary>
        /// Sets coordinates of the tile.
        /// </summary>
        /// <param name="coords">Coordinates of the tile.</param>
        public void SetCoords(Coords coords)
        {
            Coords = coords;
        }

        /// <summary>
        /// Rotates the tile.
        /// </summary>
        /// <param name="angle">Angle of rotation.</param>
        public void RotateLayout(TileOrientation angle)
        {
            Orientation = Orientation.Rotate(angle);

            if (Scheme != null)
            {
                App.Current.Dispatcher.Invoke(() => Draw());
            }
        }

        /// <summary>
        /// Draws the tile layout.
        /// </summary>
        public void Draw()
        {
            // Get the layout data
            if (Scheme != null && LAYOUT_DATA.TryGetValue(Scheme.Layout, out TileLayoutData data))
            {
                int orientation = 4 - (int)Orientation;

                Border_N.Stroke = Brushes.Black;
                Border_E.Stroke = Brushes.Black;
                Border_S.Stroke = Brushes.Black;
                Border_W.Stroke = Brushes.Black;
                Border_NE.Stroke = (data.Borders[(0 + orientation) & 0b11] ? Brushes.Black : Brushes.Transparent);
                Border_SE.Stroke = (data.Borders[(1 + orientation) & 0b11] ? Brushes.Black : Brushes.Transparent);
                Border_SW.Stroke = (data.Borders[(2 + orientation) & 0b11] ? Brushes.Black : Brushes.Transparent);
                Border_NW.Stroke = (data.Borders[(3 + orientation) & 0b11] ? Brushes.Black : Brushes.Transparent);
                Border_NC_Fill_N.Stroke = (data.Borders[((0 + orientation) & 0b11) + 4] ? Brushes.Black : Brushes.Transparent);
                Border_EC_Fill_E.Stroke = (data.Borders[((1 + orientation) & 0b11) + 4] ? Brushes.Black : Brushes.Transparent);
                Border_SC_Fill_S.Stroke = (data.Borders[((2 + orientation) & 0b11) + 4] ? Brushes.Black : Brushes.Transparent);
                Border_WC_Fill_W.Stroke = (data.Borders[((3 + orientation) & 0b11) + 4] ? Brushes.Black : Brushes.Transparent);


                // Set corresponding ids to regions
                RegionShapes.Clear();

                for (int i = 0; i < Scheme.RegionCount; i++)
                {
                    RegionShapes[i] = new List<Shape>();
                }

                RegionShapes[data.RegionIds[(0 + orientation) & 0b11]].Add(Border_NC_Fill_N);
                RegionShapes[data.RegionIds[(1 + orientation) & 0b11]].Add(Border_EC_Fill_E);
                RegionShapes[data.RegionIds[(2 + orientation) & 0b11]].Add(Border_SC_Fill_S);
                RegionShapes[data.RegionIds[(3 + orientation) & 0b11]].Add(Border_WC_Fill_W);
                RegionShapes[data.RegionIds[((0 + orientation) & 0b11) + 4]].Add(Fill_NC);
                RegionShapes[data.RegionIds[((1 + orientation) & 0b11) + 4]].Add(Fill_EC);
                RegionShapes[data.RegionIds[((2 + orientation) & 0b11) + 4]].Add(Fill_SC);
                RegionShapes[data.RegionIds[((3 + orientation) & 0b11) + 4]].Add(Fill_WC);

                // Draw the regions
                for (int i = 0; i < Scheme.RegionCount; i++)
                {
                    Brush fill = REGION_TO_COLOR[Scheme.GetRegionType(i)];

                    foreach (Shape s in RegionShapes[i])
                    {
                        s.Fill = fill;
                    }
                }

                // Hide old cities
                foreach (var c in CityShapes)
                {
                    c.Visibility = Visibility.Collapsed;
                }

                CityShapes.Clear();

                // Find the cities
                for (int c = 0; c < Scheme.CityCount; c++)
                {
                    // Get the regions
                    int region1 = -1;
                    int region2 = -1;

                    int r = 0;
                    for (; r < Scheme.RegionCount; r++)
                    {
                        if (Scheme.GetNeighbouringCities(r).Contains(c))
                        {
                            region1 = r;
                            r++;
                            break;
                        }
                    }
                    for (; r < Scheme.RegionCount; r++)
                    {
                        if (Scheme.GetNeighbouringCities(r).Contains(c))
                        {
                            region2 = r;
                            r++;
                            break;
                        }
                    }

                    // Draw it on correct position
                    int position = (int)data.CityRegionData.Where(t => ((t.Item1 == region1 && t.Item2 == region2) || (t.Item2 == region1 && t.Item1 == region1))).First().Item3;

                    switch (((position + (int)Orientation) & 0b11) | (position & 0b1100))
                    {
                        case 0:
                            CityShapes.Add(City_NE);
                            break;
                        case 1:
                            CityShapes.Add(City_SE);
                            break;
                        case 2:
                            CityShapes.Add(City_SW);
                            break;
                        case 3:
                            CityShapes.Add(City_NW);
                            break;
                        case 4:
                            CityShapes.Add(City_NC);
                            break;
                        case 5:
                            CityShapes.Add(City_EC);
                            break;
                        case 6:
                            CityShapes.Add(City_SC);
                            break;
                        case 7:
                            CityShapes.Add(City_WC);
                            break;
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            CityShapes.Add(City_CC);
                            break;
                    }
                }

                // Show new cities
                foreach (var c in CityShapes)
                {
                    c.Visibility = Visibility.Visible;
                    c.Fill = CITY_FILL;
                    c.Stroke = Brushes.Black;
                }

            }
            else
            {
                // Layout not found or no scheme, nothing to be shown 

                Border_N.Stroke = Brushes.Transparent;
                Border_E.Stroke = Brushes.Transparent;
                Border_S.Stroke = Brushes.Transparent;
                Border_W.Stroke = Brushes.Transparent;
                Border_NE.Stroke = Brushes.Transparent;
                Border_SE.Stroke = Brushes.Transparent;
                Border_SW.Stroke = Brushes.Transparent;
                Border_NW.Stroke = Brushes.Transparent;
                Border_NC_Fill_N.Stroke = Brushes.Transparent;
                Border_EC_Fill_E.Stroke = Brushes.Transparent;
                Border_SC_Fill_S.Stroke = Brushes.Transparent;
                Border_WC_Fill_W.Stroke = Brushes.Transparent;

                Border_NC_Fill_N.Fill = Brushes.Transparent;
                Border_EC_Fill_E.Fill = Brushes.Transparent;
                Border_SC_Fill_S.Fill = Brushes.Transparent;
                Border_WC_Fill_W.Fill = Brushes.Transparent;
                Fill_NC.Fill = Brushes.Transparent;
                Fill_EC.Fill = Brushes.Transparent;
                Fill_SC.Fill = Brushes.Transparent;
                Fill_WC.Fill = Brushes.Transparent;

                City_NW.Fill = Brushes.Transparent;
                City_NC.Fill = Brushes.Transparent;
                City_NE.Fill = Brushes.Transparent;
                City_WC.Fill = Brushes.Transparent;
                City_CC.Fill = Brushes.Transparent;
                City_EC.Fill = Brushes.Transparent;
                City_SW.Fill = Brushes.Transparent;
                City_SC.Fill = Brushes.Transparent;
                City_SE.Fill = Brushes.Transparent;
            }
        }

        /// <summary>
        /// Places follower on the tile.
        /// </summary>
        /// <param name="color">Color of the follower.</param>
        /// <param name="regionId">Region on the tile.</param>
        public void PlaceFollower(PlayerColor color, int regionId)
        {
            int p;

            if (Scheme.Layout == TileSchemeLayout.UNKNOWN)
            {
                p = (int)OnTilePosition.CC;
            }
            else
            {
                p = (int)LAYOUT_DATA[Scheme.Layout].FollowerPositions[regionId];
            }

            var position = FOLLOWER_POSITION[(p & 0b1100) | ((p + (int)Orientation) & 0b11)];

            App.Current.Dispatcher.Invoke(() => {
                Follower.Visibility = Visibility.Visible;
                Follower.Fill = FOLLOWER_COLOR[color];
                Follower.Stroke = Brushes.Black;
                Canvas.SetLeft(Follower, position.X);
                Canvas.SetTop(Follower, position.Y);
            });
        }

        /// <summary>
        /// Removes follower from the tile.
        /// </summary>
        public void RemoveFollower()
        {
            App.Current.Dispatcher.Invoke(() => { Follower.Visibility = Visibility.Collapsed; });
        }

        /// <summary>
        /// Region was foucused by mouse.
        /// </summary>
        /// <param name="sender">Sending region object.</param>
        /// <param name="e">Event arguments.</param>
        private void Fill_MouseEnter(object sender, MouseEventArgs e)
        {
            if (RegionMouseEventEnabled)
            {
                int regionNr = RegionShapes.Where(p => p.Value.Contains(sender)).First().Key;
                RegionMouseEnter.Invoke(regionNr);
            }
        }

        /// <summary>
        /// Region was unfoucused by mouse.
        /// </summary>
        /// <param name="sender">Sending region object.</param>
        /// <param name="e">Event arguments.</param>
        private void Fill_MouseLeave(object sender, MouseEventArgs e)
        {
            if (RegionMouseEventEnabled)
            {
                int regionNr = RegionShapes.Where(p => p.Value.Contains(sender)).First().Key;
                RegionMouseExit.Invoke(regionNr);
            }
        }

        /// <summary>
        /// Region was clicked by mouse.
        /// </summary>
        /// <param name="sender">Sending region object.</param>
        /// <param name="e">Event arguments.</param>
        private void Fill_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (RegionMouseEventEnabled)
            {
                int regionNr = RegionShapes.Where(p => p.Value.Contains(sender)).First().Key;
                RegionMouseClick.Invoke(Coords, regionNr);
            }
        }


        /// <summary>
        /// Follower was foucused by mouse.
        /// </summary>
        /// <param name="sender">Sending region object.</param>
        /// <param name="e">Event arguments.</param>
        private void Follower_MouseEnter(object sender, MouseEventArgs e)
        {
            if (FollowerMouseEventEnabled)
            {
                Follower.Opacity = 0.5;
            }
        }

        /// <summary>
        /// Follower was unfoucused by mouse.
        /// </summary>
        /// <param name="sender">Sending region object.</param>
        /// <param name="e">Event arguments.</param>
        private void Follower_MouseLeave(object sender, MouseEventArgs e)
        {
            if (FollowerMouseEventEnabled)
            {
                Follower.Opacity = 1.0;
            }
        }

        /// <summary>
        /// Follower was clicked by mouse.
        /// </summary>
        /// <param name="sender">Sending region object.</param>
        /// <param name="e">Event arguments.</param>
        private void Follower_MouseLeftMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (FollowerMouseEventEnabled)
            {
                FollowerMouseClick.Invoke(Coords);
            }
        }
    }
}
