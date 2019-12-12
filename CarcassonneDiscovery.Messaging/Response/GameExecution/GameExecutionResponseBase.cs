namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Base class for game execution response.
    /// </summary>
    /// <typeparam name="TGameExecutionResult">Execution result type.</typeparam>
    public abstract class GameExecutionResponseBase<TGameExecutionResult> : IResponse
        where TGameExecutionResult : GameExecutionResult
    {
        /// <summary>
        /// Exit code of the response.
        /// </summary>
        public ExitCode ExitCode { get; set; }

        /// <summary>
        /// Result of execution.
        /// </summary>
        public TGameExecutionResult ExecutionResult { get; set; }

        /// <summary>
        /// Constructor from <see cref="TGameExecutionResult"/>.
        /// </summary>
        /// <param name="result">Execution result.</param>
        /// <param name="exitCode">Exit code.</param>
        public GameExecutionResponseBase(TGameExecutionResult result, ExitCode exitCode)
        {
            ExecutionResult = result;
            ExitCode = exitCode;
        }

        /// <summary>
        /// Constructor from <see cref="ServerResponse"/>.
        /// </summary>
        /// <param name="response">Response DTO.</param>
        public GameExecutionResponseBase(ServerResponse response)
        {
            // Nothing
        }

        /// <inheritdoc />
        public abstract ServerResponse ToServerResponse();
    }
}
