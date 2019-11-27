namespace CarcassonneDiscovery.Messaging
{
    using System;
    using System.Linq;
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Extensions methods for mappings from <see cref="GameExecutionResult"/> to <see cref="ServerResponse"/>
    /// </summary>
    public static class ExecutionResultMappingExtensions
    {
        /// <summary>
        /// Mapping from <see cref="StartGameExecutionResult"/> to <see cref="ServerResponse"/> message.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Server response message.</returns>
        public static ServerResponse ToServerResponse(this StartGameExecutionResult result)
        {
            return new ServerResponse
            {
                Type = ServerResponseType.StartGame,
                RuleViolation = result.RuleViolationType,
                Tile = new Tile(result.FirstTile),
                Coords = result.FirstTileCoords,
                Orientation = result.FirstTileOrientation,

                PlayerAmount = result.GameParams.PlayerAmount,
                FollowerAmount = result.GameParams.FollowerAmount,
                PlayerOrder = result.GameParams.PlayerOrder,
                TileSetName = result.GameParams.TileSetParams.Name
            };
        }

        /// <summary>
        /// Mapping from <see cref="ServerResponse"/> message to <see cref="StartGameExecutionResult"/>.
        /// </summary>
        /// <param name="msg">Server response message.</param>
        /// <returns>Game execution result.</returns>
        public static StartGameExecutionResult ToStartGameExecutionResult(this ServerResponse msg)
        {
            CheckType(msg.Type, ServerResponseType.StartGame);
            CheckHasValue(msg.RuleViolation, "Rule violation must be specified.");

            if (msg.RuleViolation.Value == RuleViolationType.Ok)
            {
                CheckHasValue(msg.Tile, "First tile must be specified.");
                CheckHasValue(msg.Coords, "First tile coordinates must be specified.");
                CheckHasValue(msg.Orientation, "First tile orientation must be specified.");

                CheckHasValue(msg.PlayerAmount, "Player amount must be specified.");
                CheckHasValue(msg.FollowerAmount, "Follower amount must be specified.");
                CheckHasValue(msg.PlayerOrder, "Player order must be specified.");
                CheckHasValue(msg.TileSetName, "Tile set name must be specified.");

                var gameParams = new GameParams
                {
                    PlayerAmount = msg.PlayerAmount.Value,
                    FollowerAmount = msg.FollowerAmount.Value,
                    PlayerOrder = msg.PlayerOrder,
                    TileSetParams = new TileSetParams
                    {
                        Name = msg.TileSetName,
                        TileSupplierBuilder = null
                    }
                };

                return new StartGameExecutionResult(gameParams, msg.Tile.Scheme, msg.Coords.Value, msg.Orientation.Value);
            }

            return new StartGameExecutionResult(msg.RuleViolation.Value);
        }

        /// <summary>
        /// Mapping from <see cref="StartMoveExecutionResult"/> to <see cref="ServerResponse"/> message.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Server response message.</returns>
        public static ServerResponse ToServerResponse(this StartMoveExecutionResult result)
        {
            return new ServerResponse
            {
                Type = ServerResponseType.PlaceFollower,
                RuleViolation = result.RuleViolationType,
                Tile = new Tile(result.Tile),
                Color = result.Color
            };
        }

        /// <summary>
        /// Mapping from <see cref="ServerResponse"/> message to <see cref="StartMoveExecutionResult"/>.
        /// </summary>
        /// <param name="msg">Server response message.</param>
        /// <returns>Game execution result.</returns>
        public static StartMoveExecutionResult ToStartMoveExecutionResult(this ServerResponse msg)
        {
            CheckType(msg.Type, ServerResponseType.StartMove);
            CheckHasValue(msg.RuleViolation, "Rule violation must be specified.");

            if (msg.RuleViolation.Value == RuleViolationType.Ok)
            {
                CheckHasValue(msg.Tile, "Tile must be specified.");
                CheckHasValue(msg.Color, "Player on move must be specified.");

                return new StartMoveExecutionResult(msg.Tile.Scheme, msg.Color.Value);
            }

            return new StartMoveExecutionResult(msg.RuleViolation.Value);
        }

        /// <summary>
        /// Mapping from <see cref="PlaceTileExecutionResult"/> to <see cref="ServerResponse"/> message.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Server response message.</returns>
        public static ServerResponse ToServerResponse(this PlaceTileExecutionResult result)
        {
            return new ServerResponse
            {
                Type = ServerResponseType.PlaceFollower,
                RuleViolation = result.RuleViolationType,
                Color = result.Color,
                Tile = new Tile(result.Tile),
                Coords = result.Coords,
                Orientation = result.Orientation
            };
        }

        /// <summary>
        /// Mapping from <see cref="ServerResponse"/> message to <see cref="PlaceTileExecutionResult"/>.
        /// </summary>
        /// <param name="msg">Server response message.</param>
        /// <returns>Game execution result.</returns>
        public static PlaceTileExecutionResult ToPlaceTileExecutionResult(this ServerResponse msg)
        {
            CheckType(msg.Type, ServerResponseType.PlaceTile);
            CheckHasValue(msg.RuleViolation, "Rule violation must be specified.");

            if (msg.RuleViolation.Value == RuleViolationType.Ok)
            {
                CheckHasValue(msg.Color, "Player on move must be specified.");
                CheckHasValue(msg.Tile, "Tile must be specified.");
                CheckHasValue(msg.Coords, "Tile coordinates must be specified.");
                CheckHasValue(msg.Orientation, "Tile orientation must be specified.");

                return new PlaceTileExecutionResult(msg.Color.Value, msg.Tile.Scheme, msg.Coords.Value, msg.Orientation.Value);
            }

            return new PlaceTileExecutionResult(msg.RuleViolation.Value);
        }

        /// <summary>
        /// Mapping from <see cref="PlaceFollowerExecutionResult"/> to <see cref="ServerResponse"/> message.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Server response message.</returns>
        public static ServerResponse ToServerResponse(this PlaceFollowerExecutionResult result)
        {
            return new ServerResponse
            {
                Type = ServerResponseType.PlaceFollower,
                RuleViolation = result.RuleViolationType,
                Color = result.Color,
                Coords = result.Coords,
                RegionId = result.RegionId
            };
        }

        /// <summary>
        /// Mapping from <see cref="ServerResponse"/> message to <see cref="PlaceFollowerExecutionResult"/>.
        /// </summary>
        /// <param name="msg">Server response message.</param>
        /// <returns>Game execution result.</returns>
        public static PlaceFollowerExecutionResult ToPlaceFollowerExecutionResult(this ServerResponse msg)
        {
            CheckType(msg.Type, ServerResponseType.PlaceFollower);
            CheckHasValue(msg.RuleViolation, "Rule violation must be specified.");

            if (msg.RuleViolation.Value == RuleViolationType.Ok)
            {
                CheckHasValue(msg.Color, "Player on move must be specified.");
                CheckHasValue(msg.Coords, "Follower coordinates must be specified.");
                CheckHasValue(msg.RegionId, "Follower region Id must be specified.");

                return new PlaceFollowerExecutionResult(msg.Color.Value, msg.Coords.Value, msg.RegionId.Value);
            }

            return new PlaceFollowerExecutionResult(msg.RuleViolation.Value);
        }

        /// <summary>
        /// Mapping from <see cref="RemoveFollowerExecutionResult"/> to <see cref="ServerResponse"/> message.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Server response message.</returns>
        public static ServerResponse ToServerResponse(this RemoveFollowerExecutionResult result)
        {
            return new ServerResponse
            {
                Type = ServerResponseType.RemoveFollower,
                RuleViolation = result.RuleViolationType,
                Color = result.Color,
                Coords = result.Coords,
                Score = result.Score
            };
        }

        /// <summary>
        /// Mapping from <see cref="ServerResponse"/> message to <see cref="RemoveFollowerExecutionResult"/>.
        /// </summary>
        /// <param name="msg">Server response message.</param>
        /// <returns>Game execution result.</returns>
        public static RemoveFollowerExecutionResult ToRemoveFollowerExecutionResult(this ServerResponse msg)
        {
            CheckType(msg.Type, ServerResponseType.RemoveFollower);
            CheckHasValue(msg.RuleViolation, "Rule violation must be specified.");

            if (msg.RuleViolation.Value == RuleViolationType.Ok)
            {
                CheckHasValue(msg.Color, "Player on move must be specified.");
                CheckHasValue(msg.Coords, "Follower coordinates must be specified.");
                CheckHasValue(msg.Score, "Score must be specified.");

                return new RemoveFollowerExecutionResult(msg.Color.Value, msg.Coords.Value, msg.Score.Value);
            }

            return new RemoveFollowerExecutionResult(msg.RuleViolation.Value);
        }

        /// <summary>
        /// Mapping from <see cref="PassMoveExecutionResult"/> to <see cref="ServerResponse"/> message.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Server response message.</returns>
        public static ServerResponse ToServerResponse(this PassMoveExecutionResult result)
        {
            return new ServerResponse
            {
                Type = ServerResponseType.PassMove,
                RuleViolation = result.RuleViolationType,
                Color = result.Color
            };
        }

        /// <summary>
        /// Mapping from <see cref="ServerResponse"/> message to <see cref="PassMoveExecutionResult"/>.
        /// </summary>
        /// <param name="msg">Server response message.</param>
        /// <returns>Game execution result.</returns>
        public static PassMoveExecutionResult ToPassMoveExecutionResult(this ServerResponse msg)
        {
            CheckType(msg.Type, ServerResponseType.PassMove);
            CheckHasValue(msg.RuleViolation, "Rule violation must be specified.");

            if (msg.RuleViolation.Value == RuleViolationType.Ok)
            {
                CheckHasValue(msg.Color, "Player on move must be specified.");

                return new PassMoveExecutionResult(msg.Color.Value);
            }

            return new PassMoveExecutionResult(msg.RuleViolation.Value);
        }

        /// <summary>
        /// Mapping from <see cref="EndGameExecutionResult"/> to <see cref="ServerResponse"/> message.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Server response message.</returns>
        public static ServerResponse ToServerResponse(this EndGameExecutionResult result)
        {
            return new ServerResponse
            {
                Type = ServerResponseType.EndGame,
                RuleViolation = result.RuleViolationType,
                FollowerRemovements = result.FollowerRemovements?.Select(x => x.ToServerResponse()).ToArray() ?? null
            };
        }

        /// <summary>
        /// Mapping from <see cref="ServerResponse"/> message to <see cref="EndGameExecutionResult"/>.
        /// </summary>
        /// <param name="msg">Server response message.</param>
        /// <returns>Game execution result.</returns>
        public static EndGameExecutionResult ToEndGameExecutionResult(this ServerResponse msg)
        {
            CheckType(msg.Type, ServerResponseType.EndGame);
            CheckHasValue(msg.RuleViolation, "Rule violation must be specified.");

            if (msg.RuleViolation.Value == RuleViolationType.Ok)
            {
                CheckHasValue(msg.FollowerRemovements, "Follower removements must be specified.");

                var followerRemovements = msg.FollowerRemovements
                    .Select(x => x.ToRemoveFollowerExecutionResult())
                    .ToList();

                return new EndGameExecutionResult(followerRemovements);
            }

            return new EndGameExecutionResult(msg.RuleViolation.Value);
        }

        #region Helper methods
        /// <summary>
        /// Checks whether nullable parameter has value.
        /// Throw <see cref="ArgumentException" /> when has no value.
        /// </summary>
        /// <typeparam name="T">Type of nullable struct.</typeparam>
        /// <param name="value">Value to be null-checked.</param>
        /// <param name="errorMsg">Error message.</param>
        private static void CheckHasValue<T>(T? value, string errorMsg = "Expected not null value.")
            where T : struct
        {
            if (!value.HasValue)
            {
                throw new ArgumentException(errorMsg);
            }
        }

        /// <summary>
        /// Checks whether nullable parameter has value.
        /// Throw <see cref="ArgumentException" /> when has no value.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="value">Value to be null-checked.</param>
        /// <param name="errorMsg">Error message.</param>
        private static void CheckHasValue<T>(T value, string errorMsg = "Expected not null value.")
            where T : class
        {
            if (value == null)
            {
                throw new ArgumentException(errorMsg);
            }
        }

        /// <summary>
        /// Checks whether a value is equal to the expected value.
        /// Throw <see cref="ArgumentException" /> when has no value.
        /// </summary>
        /// <param name="expected">Expected value.</param>
        /// <param name="actual">Actual value.</param>
        private static void CheckType(ServerResponseType expected, ServerResponseType actual)
        {
            if (expected != actual)
            {
                throw new ArgumentException($"Expected type {expected}, actually got {actual}.");
            }
        }
        #endregion
    }
}
