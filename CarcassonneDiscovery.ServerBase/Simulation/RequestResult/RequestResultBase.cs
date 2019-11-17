namespace CarcassonneDiscovery.Server
{
    using System;

    /// <summary>
    /// Base class for request result object classes.
    /// </summary>
    /// <typeparam name="TExitCode">Exit code enum.</typeparam>
    public abstract class RequestResultBase<TExitCode>
        where TExitCode : Enum
    {
        /// <summary>
        /// Request exit code.
        /// </summary>
        public TExitCode ExitCode { get; set; }
    }
}
