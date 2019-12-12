namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Result of pass move action.
    /// </summary>
    public class PassMoveResponse : GameExecutionResponseBase<PassMoveExecutionResult>
    {
        /// <inheritdoc />
        public PassMoveResponse(PassMoveExecutionResult executionResult, ExitCode exitCode) : base(executionResult, exitCode)
        {
        }

        /// <inheritdoc />
        public PassMoveResponse(ServerResponse response) : base(response)
        {
            ExitCode = response.ExitCode;

            ValidationHelper.CheckType(response.Type, ServerResponseType.PassMove);

            if (ExitCode == ExitCode.Ok || ExitCode == ExitCode.RuleViolationError)
            {
                ValidationHelper.CheckHasValue(response.RuleViolation, "Rule violation must be specified.");

                if (response.RuleViolation.Value == RuleViolationType.Ok)
                {
                    ValidationHelper.CheckHasValue(response.Color, "Player on move must be specified.");

                    ExecutionResult = new PassMoveExecutionResult(response.Color.Value);
                }
                else
                {
                    ExecutionResult = new PassMoveExecutionResult(response.RuleViolation.Value);
                }
            }
        }

        /// <inheritdoc />
        public override ServerResponse ToServerResponse()
        {
            return new ServerResponse
            {
                Type = ServerResponseType.PassMove,
                ExitCode = ExitCode,

                RuleViolation = ExecutionResult?.RuleViolationType,
                Color = ExecutionResult?.Color
            };
        }
    }
}
