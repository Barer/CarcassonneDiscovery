namespace CarcassonneDiscovery.UserClient
{
    using CarcassonneDiscovery.SimulationLibrary;

    /// <summary>
    /// Player color option to be selected in waiting lobby.
    /// </summary>
    public class PlayerColorOption
    {
        /// <summary>
        /// Actual color to be selected.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Text to be shown.
        /// </summary>
        public string Text { get; set; }
    }
}
