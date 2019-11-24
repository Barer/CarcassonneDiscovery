namespace CarcassonneDiscovery.Server
{
    /// <summary>
    /// Provider of service classes.
    /// </summary>
    public static class ServerServiceProvider
    {
        /// <summary>
        /// Game simulator.
        /// </summary>
        public static GameSimulator GameSimulator { get; private set; }

        /// <summary>
        /// Client messager.
        /// </summary>
        public static ClientMessager ClientMessager { get; private set; }

        /// <summary>
        /// Logger.
        /// </summary>
        public static Logger Logger { get; private set; }

        /// <summary>
        /// Initializer of the provider.
        /// </summary>
        public static void Init(
            GameSimulator gameSimulator,
            ClientMessager clientMessager,
            Logger logger
            )
        {
            GameSimulator = gameSimulator;
            ClientMessager = clientMessager;
            Logger = logger;
        }
    }
}
