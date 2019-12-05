namespace CarcassonneDiscovery.UserClient
{
    using System;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using System.Windows.Shapes;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Empty tile place indicator.
    /// </summary>
    internal class EmptyTileRectangle : UserControl
    {
        /// <summary>
        /// Is mouse event enabled for the rectangle?
        /// </summary>
        protected bool _MouseEventEnabled;

        /// <summary>
        /// If enabled, mouse events are invoked.
        /// </summary>
        public bool MouseEventEnabled
        {
            get => _MouseEventEnabled;
            set
            {
                _TileRectangle_MouseLeave(null, null);
                _MouseEventEnabled = value;
            }
        }

        /// <summary>
        /// Event when the region has been clicked on.
        /// </summary>
        public event Action<Coords> MouseClick = (c) => { };

        /// <summary>
        /// Coordinates of the tile.
        /// </summary>
        protected Coords Coordinates;

        /// <summary>
        /// Actual tile rectangle.
        /// </summary>
        protected Rectangle _TileRectangle;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="coords">Coordinates of the place.</param>
        public EmptyTileRectangle(Coords coords)
        {
            Coordinates = coords;

            _TileRectangle = new Rectangle()
            {
                Width = 100,
                Height = 100,
                Fill = new SolidColorBrush(new Color() { R = 255, G = 255, B = 128, A = 255 }),
                Stroke = Brushes.Transparent
            };

            Opacity = 0.5;

            _TileRectangle.MouseUp += _TileRectangle_MouseUp;
            _TileRectangle.MouseEnter += _TileRectangle_MouseEnter;
            _TileRectangle.MouseLeave += _TileRectangle_MouseLeave;

            AddChild(_TileRectangle);
        }

        /// <summary>
        /// Mouse leave event handler.
        /// </summary>
        /// <param name="sender">Sending object,</param>
        /// <param name="e">Event arguments.</param>
        private void _TileRectangle_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MouseEventEnabled)
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { Opacity = 0.5; }));
            }
        }

        /// <summary>
        /// Mouse enter event handler.
        /// </summary>
        /// <param name="sender">Sending object,</param>
        /// <param name="e">Event arguments.</param>
        private void _TileRectangle_MouseEnter(object sender, MouseEventArgs e)
        {
            if (MouseEventEnabled)
            {
                Application.Current.Dispatcher.Invoke(new Action(() => { Opacity = 1; }));
            }
        }

        /// <summary>
        /// Mouse up event handler.
        /// </summary>
        /// <param name="sender">Sending object,</param>
        /// <param name="e">Event arguments.</param>
        private void _TileRectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (MouseEventEnabled)
            {
                MouseClick.Invoke(Coordinates);
            }
        }
    }
}