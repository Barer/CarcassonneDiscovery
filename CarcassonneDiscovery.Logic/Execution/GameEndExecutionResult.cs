namespace CarcassonneDiscovery.Logic.Execution
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Result of pass move action execution.
    /// </summary>
    public class GameEndExecutionResult : GameExecutionResult
    {
        /// <summary>
        /// List of removed followers and scored points.
        /// </summary>
        public List<RemoveFollowerExecutionResult> FollowerRemovements { get; set; }

        /// <summary>
        /// Constructor for correct game action.
        /// </summary>
        /// <param name="followerRemovements">List of removed followers and scored points.</param>
        public GameEndExecutionResult(List<RemoveFollowerExecutionResult> followerRemovements) : base()
        {
            FollowerRemovements = followerRemovements;
        }

        /// <inheritdoc />
        public GameEndExecutionResult(RuleViolationType type) : base(type)
        {
            if (type == RuleViolationType.Ok)
            {
                throw new InvalidOperationException($"Cannot create instance of {GetType().Name} with argument {type} using this constructor.");
            }
        }
    }
}