namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Result of start game action.
    /// </summary>
    public class StartGameResponse : GameExecutionResponseBase<StartGameExecutionResult>
    {
        /// <summary>
        /// Names of players.
        /// </summary>
        public string[] PlayerNames { get; set; }

        /// <inheritdoc />
        public StartGameResponse(StartGameExecutionResult executionResult, ExitCode exitCode, string[] playerNames) : base(executionResult, exitCode)
        {
            PlayerNames = playerNames;
        }

        /// <inheritdoc />
        public StartGameResponse(ServerResponse response) : base(response)
        {
            ExitCode = response.ExitCode;
            PlayerNames = response.PlayerNames;

            ValidationHelper.CheckType(response.Type, ServerResponseType.StartGame);

            if (ExitCode == ExitCode.Ok || ExitCode == ExitCode.RuleViolationError)
            {
                ValidationHelper.CheckHasValue(response.RuleViolation, "Rule violation must be specified.");

                if (response.RuleViolation.Value == RuleViolationType.Ok)
                {
                    ValidationHelper.CheckHasValue(response.Tile, "First tile must be specified.");
                    ValidationHelper.CheckHasValue(response.Coords, "First tile coordinates must be specified.");
                    ValidationHelper.CheckHasValue(response.Orientation, "First tile orientation must be specified.");

                    ValidationHelper.CheckHasValue(response.PlayerAmount, "Player amount must be specified.");
                    ValidationHelper.CheckHasValue(response.FollowerAmount, "Follower amount must be specified.");
                    ValidationHelper.CheckHasValue(response.PlayerOrder, "Player order must be specified.");
                    ValidationHelper.CheckHasValue(response.Name, "Tile set name must be specified.");

                    var gameParams = new GameParams
                    {
                        PlayerAmount = response.PlayerAmount.Value,
                        FollowerAmount = response.FollowerAmount.Value,
                        PlayerOrder = response.PlayerOrder,
                        TileSet = response.Name
                    };

                    ExecutionResult = new StartGameExecutionResult(gameParams, response.Tile.Value, response.Coords.Value, response.Orientation.Value);
                }
                else
                {
                    ExecutionResult = new StartGameExecutionResult(response.RuleViolation.Value);
                }
            }
        }

        /// <inheritdoc />
        public override ServerResponse ToServerResponse()
        {
            return new ServerResponse
            {
                Type = ServerResponseType.StartGame,
                ExitCode = ExitCode,

                PlayerNames = PlayerNames,

                RuleViolation = ExecutionResult?.RuleViolationType,
                Tile = new Tile(ExecutionResult?.FirstTile),
                Coords = ExecutionResult?.FirstTileCoords,
                Orientation = ExecutionResult?.FirstTileOrientation,

                PlayerAmount = ExecutionResult?.GameParams.PlayerAmount,
                FollowerAmount = ExecutionResult?.GameParams.FollowerAmount,
                PlayerOrder = ExecutionResult?.GameParams.PlayerOrder,
                Name = ExecutionResult?.GameParams.TileSet
            };
        }
    }
}
