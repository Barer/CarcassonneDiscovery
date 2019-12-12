namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Result of place tile action.
    /// </summary>
    public class PlaceTileResponse : GameExecutionResponseBase<PlaceTileExecutionResult>
    {
        /// <inheritdoc />
        public PlaceTileResponse(PlaceTileExecutionResult executionResult, ExitCode exitCode) : base(executionResult, exitCode)
        {
        }

        /// <inheritdoc />
        public PlaceTileResponse(ServerResponse response) : base(response)
        {
            ExitCode = response.ExitCode;

            ValidationHelper.CheckType(response.Type, ServerResponseType.PlaceTile);

            if (ExitCode == ExitCode.Ok || ExitCode == ExitCode.RuleViolationError)
            {
                ValidationHelper.CheckHasValue(response.RuleViolation, "Rule violation must be specified.");

                if (response.RuleViolation.Value == RuleViolationType.Ok)
                {
                    ValidationHelper.CheckHasValue(response.Color, "Player on move must be specified.");
                    ValidationHelper.CheckHasValue(response.Tile, "Tile must be specified.");
                    ValidationHelper.CheckHasValue(response.Coords, "Tile coordinates must be specified.");
                    ValidationHelper.CheckHasValue(response.Orientation, "Tile orientation must be specified.");

                    ExecutionResult = new PlaceTileExecutionResult(response.Color.Value, response.Tile.Value, response.Coords.Value, response.Orientation.Value);
                }
                else
                {
                    ExecutionResult = new PlaceTileExecutionResult(response.RuleViolation.Value);
                }
            }
        }

        /// <inheritdoc />
        public override ServerResponse ToServerResponse()
        {
            return new ServerResponse
            {
                Type = ServerResponseType.PlaceTile,
                ExitCode = ExitCode,

                RuleViolation = ExecutionResult?.RuleViolationType,
                Color = ExecutionResult?.Color,
                Tile = ExecutionResult == null ? null : new Tile(ExecutionResult.Tile),
                Coords = ExecutionResult?.Coords,
                Orientation = ExecutionResult?.Orientation
            };
        }
    }
}
