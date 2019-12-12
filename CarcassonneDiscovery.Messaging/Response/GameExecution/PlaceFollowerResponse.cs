namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Result of place follower action.
    /// </summary>
    public class PlaceFollowerResponse : GameExecutionResponseBase<PlaceFollowerExecutionResult>
    {
        /// <inheritdoc />
        public PlaceFollowerResponse(PlaceFollowerExecutionResult executionResult, ExitCode exitCode) : base(executionResult, exitCode)
        {
        }

        /// <inheritdoc />
        public PlaceFollowerResponse(ServerResponse response) : base(response)
        {
            ExitCode = response.ExitCode;

            ValidationHelper.CheckType(response.Type, ServerResponseType.PlaceFollower);

            if (ExitCode == ExitCode.Ok || ExitCode == ExitCode.RuleViolationError)
            {
                ValidationHelper.CheckHasValue(response.RuleViolation, "Rule violation must be specified.");

                if (response.RuleViolation.Value == RuleViolationType.Ok)
                {
                    ValidationHelper.CheckHasValue(response.Color, "Player on move must be specified.");
                    ValidationHelper.CheckHasValue(response.Coords, "Follower coordinates must be specified.");
                    ValidationHelper.CheckHasValue(response.RegionId, "Follower region Id must be specified.");

                    ExecutionResult = new PlaceFollowerExecutionResult(response.Color.Value, response.Coords.Value, response.RegionId.Value);
                }
                else
                {
                    ExecutionResult = new PlaceFollowerExecutionResult(response.RuleViolation.Value);
                }
            }
        }

        /// <inheritdoc />
        public override ServerResponse ToServerResponse()
        {
            return new ServerResponse
            {
                Type = ServerResponseType.PlaceFollower,
                ExitCode = ExitCode,

                RuleViolation = ExecutionResult?.RuleViolationType,
                Color = ExecutionResult?.Color,
                Coords = ExecutionResult?.Coords,
                RegionId = ExecutionResult?.RegionId
            };
        }
    }
}
