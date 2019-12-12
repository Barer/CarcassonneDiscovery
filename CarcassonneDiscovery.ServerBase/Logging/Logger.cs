namespace CarcassonneDiscovery.Server
{
    using System;
    using CarcassonneDiscovery.Logic;
    using CarcassonneDiscovery.Messaging;

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

        /// <summary>
        /// Helper method for logging game execution.
        /// </summary>
        /// <typeparam name="T">Type of execution result.</typeparam>
        /// <param name="exitCode">Exit code.</param>
        /// <param name="executionResult">Execution result.</param>
        public virtual void LogGameExecution<T>(ExitCode exitCode, T executionResult)
            where T : GameExecutionResult
        {
            switch (exitCode)
            {
                case ExitCode.Ok:
                    Log($"Executed {typeof(T)} - OK", LogLevel.Normal, LogType.SimulationExecution);
                    break;

                case ExitCode.InvalidTimeError:
                    Log($"Executed {typeof(T)} - Wrong time", LogLevel.Warning, LogType.SimulationExecutionError);
                    break;

                case ExitCode.RuleViolationError:
                    Log($"Executed {typeof(T)} - {executionResult.RuleViolationType}", LogLevel.Warning, LogType.SimulationExecutionError);
                    break;

                default:
                    Log($"Executed {typeof(T)} - invalid exit code: {exitCode}", LogLevel.ProgramError, LogType.SimulationExecutionError);
                    break;
            }
        }

        /// <summary>
        /// Helper method for logging request results.
        /// </summary>
        /// <typeparam name="T">Type of execution result.</typeparam>
        /// <param name="exitCode">Exit code.</param>
        /// <param name="action">Name of action.</param>
        /// <param name="type">Type of log.</param>
        public virtual void LogExitCode(ExitCode exitCode, string action, LogType type)
        {
            Log($"Executed {action} - {exitCode}", LogLevel.Normal, type);
        }
    }
}