namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Start move request result.
    /// </summary>
    public class StartMoveResponse : GameExecutionResponseBase<StartMoveExecutionResult>
    {
        /// <inheritdoc />
        public StartMoveResponse(StartMoveExecutionResult executionResult, ExitCode exitCode) : base(executionResult, exitCode)
        {
        }

        /// <inheritdoc />
        public StartMoveResponse(ServerResponse response) : base(response)
        {
            ExitCode = response.ExitCode;

            ValidationHelper.CheckType(response.Type, ServerResponseType.StartMove);

            if (ExitCode == ExitCode.Ok || ExitCode == ExitCode.RuleViolationError)
            {
                ValidationHelper.CheckHasValue(response.RuleViolation, "Rule violation must be specified.");

                if (response.RuleViolation.Value == RuleViolationType.Ok)
                {
                    ValidationHelper.CheckHasValue(response.Tile, "Tile must be specified.");
                    ValidationHelper.CheckHasValue(response.Color, "Player on move must be specified.");

                    ExecutionResult = new StartMoveExecutionResult(response.Tile.Value, response.Color.Value);
                }
                else
                {
                    ExecutionResult = new StartMoveExecutionResult(response.RuleViolation.Value);
                }
            }
        }

        /// <inheritdoc />
        public override ServerResponse ToServerResponse()
        {
            return new ServerResponse
            {
                Type = ServerResponseType.StartMove,
                ExitCode = ExitCode,

                RuleViolation = ExecutionResult?.RuleViolationType,
                Tile = ExecutionResult == null ? null : new Tile(ExecutionResult.Tile),
                Color = ExecutionResult?.Color
            };
        }
    }
}