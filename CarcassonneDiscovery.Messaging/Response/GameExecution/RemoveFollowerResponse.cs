namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Result of remove follower action.
    /// </summary>
    public class RemoveFollowerResponse : GameExecutionResponseBase<RemoveFollowerExecutionResult>
    {
        /// <inheritdoc />
        public RemoveFollowerResponse(RemoveFollowerExecutionResult executionResult, ExitCode exitCode) : base(executionResult, exitCode)
        {
        }

        /// <inheritdoc />
        public RemoveFollowerResponse(ServerResponse response) : base(response)
        {
            ExitCode = response.ExitCode;

            ValidationHelper.CheckType(response.Type, ServerResponseType.RemoveFollower);

            if (ExitCode == ExitCode.Ok || ExitCode == ExitCode.RuleViolationError)
            {
                ValidationHelper.CheckHasValue(response.RuleViolation, "Rule violation must be specified.");

                if (response.RuleViolation.Value == RuleViolationType.Ok)
                {
                    ValidationHelper.CheckHasValue(response.Color, "Player on move must be specified.");
                    ValidationHelper.CheckHasValue(response.Coords, "Follower coordinates must be specified.");
                    ValidationHelper.CheckHasValue(response.Score, "Score must be specified.");

                    ExecutionResult = new RemoveFollowerExecutionResult(response.Color.Value, response.Coords.Value, response.Score.Value);
                }
                else
                {
                    ExecutionResult = new RemoveFollowerExecutionResult(response.RuleViolation.Value);
                }
            }
        }

        /// <inheritdoc />
        public override ServerResponse ToServerResponse()
        {
            return new ServerResponse
            {
                Type = ServerResponseType.RemoveFollower,
                ExitCode = ExitCode,

                RuleViolation = ExecutionResult?.RuleViolationType,
                Color = ExecutionResult?.Color,
                Coords = ExecutionResult?.Coords,
                Score = ExecutionResult?.Score
            };
        }
    }
}
