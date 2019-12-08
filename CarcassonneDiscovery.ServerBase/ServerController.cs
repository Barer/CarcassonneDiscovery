namespace CarcassonneDiscovery.Server
{
    using System;
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Main server controller.
    /// </summary>
    public class ServerController
    {
        /// <summary>
        /// Server action has been performed.
        /// </summary>
        public event Action<ServerAction> ActionExecuted = (sa) => { };

        /// <summary>
        /// Queue with actions.
        /// </summary>
        protected LinkedList<ServerAction> ActionQueue { get; set; } = new LinkedList<ServerAction>();

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
                        var action = ActionQueue.First.Value;
                        ActionQueue.RemoveFirst();

                        action.Execute();
                        ActionExecuted.Invoke(action);
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
                ActionQueue.AddLast(action);
                ActionQueueEvent.Set();
            }
        }

        /// <summary>
        /// Adds action into the queue at the first spot.
        /// </summary>
        /// <param name="action">Action to be added to the queue.</param>
        public void EnqueueActionAsFirst(ServerAction action)
        {
            lock (ActionQueue)
            {
                ActionQueue.AddFirst(action);
                ActionQueueEvent.Set();
            }
        }
    }
}
