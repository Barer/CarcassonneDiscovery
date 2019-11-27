namespace CarcassonneDiscovery.Logic
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Result of game end action execution.
    /// </summary>
    public class EndGameExecutionResult : GameExecutionResult
    {
        /// <summary>
        /// List of removed followers and scored points.
        /// </summary>
        public List<RemoveFollowerExecutionResult> FollowerRemovements { get; set; }

        /// <summary>
        /// Constructor for correct game action.
        /// </summary>
        /// <param name="followerRemovements">List of removed followers and scored points.</param>
        public EndGameExecutionResult(List<RemoveFollowerExecutionResult> followerRemovements) : base()
        {
            FollowerRemovements = followerRemovements;
        }

        /// <inheritdoc />
        public EndGameExecutionResult(RuleViolationType type) : base(type)
        {
            if (type == RuleViolationType.Ok)
            {
                throw new InvalidOperationException($"Cannot create instance of {GetType().Name} with argument {type} using this constructor.");
            }
        }
    }
}