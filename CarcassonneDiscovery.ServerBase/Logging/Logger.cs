namespace CarcassonneDiscovery.Server
{
    using System;

    /// <summary>
    /// Logger of server events.
    /// </summary>
    public class Logger
    {
        /// <summary>
        /// Logs message.
        /// </summary>
        /// <param name="message">Message of log.</param>
        /// <param name="level">Level of log.</param>
        /// <param name="type">Type of log.</param>
        public virtual void Log(string message, LogLevel level, LogType type)
        {
            Console.WriteLine($"{DateTime.Now} {type} {level} : {message}");
        }
    }
}