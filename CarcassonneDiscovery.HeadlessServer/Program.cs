namespace CarcassonneDiscovery.Server.Headless
{
    using System.Threading;
    using System.Threading.Tasks;
    using CarcassonneDiscovery.Server;

    /// <summary>
    /// Entry point class.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Abort event.
        /// </summary>
        private static ManualResetEventSlim abortEvent = new ManualResetEventSlim();

        /// <summary>
        /// Aborts the program.
        /// </summary>
        public static void Abort()
        {
            abortEvent.Set();
        }

        /// <summary>
        /// Entry point.
        /// </summary>
        public static void Main()
        {
            var gameSimulator = new GameSimulator();
            var clientMessager = new ClientMessager();
            var logger = new Logger();
            var serverController = new ServerController();

            serverController.ActionExecuted += (sa) =>
            {
                if (sa.GetType() == typeof(AddPlayerAction))
                {
                    if (gameSimulator.PlayerCount == 2)
                    {
                        serverController.EnqueueAction(new StartGameAction());
                    }
                }
            };


            ServerServiceProvider.Init(gameSimulator, clientMessager, logger, serverController);

            abortEvent.Reset();

            ServerServiceProvider.Start();
            ServerServiceProvider.ClientMessager.StartSocket(4263);

            abortEvent.Wait();

            ServerServiceProvider.ClientMessager.StopSocket();
            ServerServiceProvider.Stop();
        }
    }
}
