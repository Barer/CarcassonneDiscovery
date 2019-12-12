namespace CarcassonneDiscovery.Messaging
{
    using System.Linq;
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Result of end game action.
    /// </summary>
    public class EndGameResponse : GameExecutionResponseBase<EndGameExecutionResult>
    {
        /// <inheritdoc />
        public EndGameResponse(EndGameExecutionResult executionResult, ExitCode exitCode) : base(executionResult, exitCode)
        {
        }

        /// <inheritdoc />
        public EndGameResponse(ServerResponse response) : base(response)
        {
            ExitCode = response.ExitCode;

            ValidationHelper.CheckType(response.Type, ServerResponseType.EndGame);

            if (ExitCode == ExitCode.Ok || ExitCode == ExitCode.RuleViolationError)
            {
                ValidationHelper.CheckHasValue(response.RuleViolation, "Rule violation must be specified.");

                if (response.RuleViolation.Value == RuleViolationType.Ok)
                {
                    ValidationHelper.CheckHasValue(response.FollowerRemovements, "Follower removements must be specified.");

                    var followerRemovements = response.FollowerRemovements
                        .Select(x => new RemoveFollowerResponse(x).ExecutionResult)
                        .ToList();

                    ExecutionResult = new EndGameExecutionResult(followerRemovements);
                }
                else
                {
                    ExecutionResult = new EndGameExecutionResult(response.RuleViolation.Value);
                }
            }
        }

        /// <inheritdoc />
        public override ServerResponse ToServerResponse()
        {
            return new ServerResponse
            {
                Type = ServerResponseType.EndGame,
                ExitCode = ExitCode,

                RuleViolation = ExecutionResult?.RuleViolationType,
                FollowerRemovements = ExecutionResult?.FollowerRemovements?.Select(x => new RemoveFollowerResponse(x, ExitCode.Ok).ToServerResponse()).ToArray() ?? null
            };
        }
    }
}
