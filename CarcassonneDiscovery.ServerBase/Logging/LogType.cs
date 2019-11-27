namespace CarcassonneDiscovery.Server
{
    /// <summary>
    /// Type of log message.
    /// </summary>
    public enum LogType
    {
        /// <summary>
        /// Log of simulation execution.
        /// </summary>
        SimulationExecution,

        /// <summary>
        /// Log of simulation execution errors.
        /// </summary>
        SimulationExecutionError,

        /// <summary>
        /// Log of messaging.
        /// </summary>
        Messaging,

        /// <summary>
        /// Log of server controller.
        /// </summary>
        ServerController
    }
}
