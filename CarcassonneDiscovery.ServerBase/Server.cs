﻿namespace CarcassonneDiscovery.Server
{
    using System.Collections.Generic;
    using System.Threading;

    /// <summary>
    /// Main server class.
    /// </summary>
    public class Server
    {
        /// <summary>
        /// Queue with actions.
        /// </summary>
        public Queue<ServerAction> ActionQueue { get; set; }

        /// <summary>
        /// Event when the queue has any action.
        /// </summary>
        public ManualResetEventSlim ActionQueueEvent { get; set; }

        /// <summary>
        /// Main thread for action execution.
        /// </summary>
        public Thread ActionQueueThread { get; set; }

        /// <summary>
        /// Main loop with actions.
        /// </summary>
        public void Loop()
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
