namespace CarcassonneDiscovery.Server
{
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Main server controller.
    /// </summary>
    public class ServerController
    {
        /// <summary>
        /// Queue with actions.
        /// </summary>
        protected Queue<ServerAction> ActionQueue { get; set; } = new Queue<ServerAction>();

        /// <summary>
        /// Event when the queue has any action.
        /// </summary>
        protected ManualResetEventSlim ActionQueueEvent { get; set; } = new ManualResetEventSlim();

        /// <summary>
        /// Main thread for action execution.
        /// </summary>
        protected Thread ActionQueueThread { get; set; }

        /// <summary>
        /// Starts the server.
        /// </summary>
        public void Start()
        {
            ActionQueueThread = new Thread(Loop);
            ActionQueueThread.Start();

            ServerServiceProvider.Logger.Log("Server started.", LogLevel.Normal, LogType.ServerController);
        }

        /// <summary>
        /// Stops the server.
        /// </summary>
        public void Stop()
        {
            ActionQueueThread.Abort();
        }

        /// <summary>
        /// Main loop with actions.
        /// </summary>
        protected void Loop()
        {
            while (true)
            {
                ActionQueueEvent.Wait();

                lock (ActionQueue)
                {
                    while (ActionQueue.Count > 0)
                    {
                        var action = ActionQueue.Dequeue();
                        action.Execute();
                    }

                    ActionQueueEvent.Reset();
                }
            }
        }

        /// <summary>
        /// Adds action into the queue.
        /// </summary>
        /// <param name="action">Action to be added to the queue.</param>
        public void EnqueueAction(ServerAction action)
        {
            lock (ActionQueue)
            {
                ActionQueue.Enqueue(action);
                ActionQueueEvent.Set();
            }
        }
    }
}
