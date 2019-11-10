namespace CarcassonneDiscovery.Logic.Execution
{
    /// <summary>
    /// Result of game action execution.
    /// </summary>
    public abstract class GameExecutionResult
    {
        /// <summary>
        /// Has the action been performed?
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// If the action has not been performed, contains rule violation.
        /// </summary>
        public RuleViolationType RuleViolationType { get; set; }

        /// <summary>
        /// Constructor for correct game execution result.
        /// </summary>
        public GameExecutionResult()
        {
            IsValid = true;
            RuleViolationType = RuleViolationType.Ok;
        }

        /// <summary>
        /// Constructor for game execution.
        /// </summary>
        /// <param name="type">Type of rule violation.</param>
        public GameExecutionResult(RuleViolationType type)
        {
            IsValid = type == RuleViolationType.Ok;
            RuleViolationType = type;
        }
    }
}
