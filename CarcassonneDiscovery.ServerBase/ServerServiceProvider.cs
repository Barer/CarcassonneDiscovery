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
        /// Server controller.
        /// </summary>
        public static ServerController ServerController { get; private set; }

        /// <summary>
        /// Initializer of the provider.
        /// </summary>
        /// <param name="gameSimulator">Game simulator.</param>
        /// <param name="clientMessager">Client messager.</param>
        /// <param name="logger">Event logger.</param>
        /// <param name="serverController">Server controller.</param>
        public static void Init(GameSimulator gameSimulator, ClientMessager clientMessager, Logger logger, ServerController serverController)
        {
            GameSimulator = gameSimulator;
            ClientMessager = clientMessager;
            Logger = logger;
            ServerController = serverController;
        }

        /// <summary>
        /// Starts the services.
        /// </summary>
        public static void Start()
        {
            ServerController.Start();
        }

        /// <summary>
        /// Stops the services.
        /// </summary>
        public static void Stop()
        {
            ServerController.Stop();
        }
    }
}
