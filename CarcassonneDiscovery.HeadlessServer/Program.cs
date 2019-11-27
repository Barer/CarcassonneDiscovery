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
        private static ManualResetEventSlim AbortEvent = new ManualResetEventSlim();

        /// <summary>
        /// Aborts the program.
        /// </summary>
        public static void Abort()
        {
            AbortEvent.Set();
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
            ServerServiceProvider.Init(gameSimulator, clientMessager, logger, serverController);

            AbortEvent.Reset();

            new Task(Test).Start();

            ServerServiceProvider.Start();

            AbortEvent.Wait();

            ServerServiceProvider.Stop();
        }

        private static void Test()
        {
            ServerServiceProvider.ServerController.EnqueueAction(new StartGameAction());
        }
    }
}
