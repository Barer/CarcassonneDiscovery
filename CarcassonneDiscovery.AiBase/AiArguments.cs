namespace CarcassonneDiscovery.AiClient
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Arguments for the AI.
    /// </summary>
    public class AiArguments
    {
        /// <summary>
        /// IP address of the server.
        /// </summary>
        public string IP { get; set; }

        /// <summary>
        /// Port of the server.
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Name of the AI player.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Color of the player.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Default arguments for the AI player.
        /// </summary>
        public static AiArguments Default
        {
            get
            {
                return new AiArguments()
                {
                    IP = "127.0.0.1",
                    Port = 4263,
                    Name = "AiPlayer",
                    Color = PlayerColor.Red
                };
            }
        }
    }
}