namespace CarcassonneDiscovery.Messaging
{
    using System;
    using System.Linq;
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Extensions methods for mappings from <see cref="ClientRequest"/> to <see cref="GameExecutionRequest"/>
    /// </summary>
    public static class ExecutionRequestMappingExtensions
    {
        /// <summary>
        /// Mapping from <see cref="PlaceTileExecutionRequest"/> to <see cref="ClientRequest"/> message.
        /// </summary>
        /// <param name="request">Game execution request.</param>
        /// <returns>Client request message.</returns>
        public static ClientRequest ToClientRequest(this PlaceTileExecutionRequest request)
        {
            return new ClientRequest
            {
                Type = ClientRequestType.PlaceTile,
                Color = request.Color,
                Tile = request.Tile,
                Coords = request.Coords,
                Orientation = request.Orientation
            };
        }

        /// <summary>
        /// Mapping from <see cref="ClientRequest"/> message to <see cref="PlaceTileExecutionRequest"/>.
        /// </summary>
        /// <param name="msg">Client request message.</param>
        /// <returns>Game execution request.</returns>
        public static PlaceTileExecutionRequest ToPlaceTileExecutionRequest(this ClientRequest msg)
        {
            CheckType(msg.Type, ClientRequestType.PlaceTile);

            CheckHasValue(msg.Color, "Player on move must be specified.");
            CheckHasValue(msg.Tile, "Tile must be specified.");
            CheckHasValue(msg.Coords, "Tile coordinates must be specified.");
            CheckHasValue(msg.Orientation, "Orientation must be specified.");

            return new PlaceTileExecutionRequest(msg.Color.Value, msg.Tile, msg.Coords.Value, msg.Orientation.Value);
        }

        /// <summary>
        /// Mapping from <see cref="PlaceFollowerExecutionRequest"/> to <see cref="ClientRequest"/> message.
        /// </summary>
        /// <param name="request">Game execution request.</param>
        /// <returns>Client request message.</returns>
        public static ClientRequest ToClientRequest(this PlaceFollowerExecutionRequest request)
        {
            return new ClientRequest
            {
                Type = ClientRequestType.PlaceFollower,
                Color = request.Color,
                Coords = request.Coords,
                RegionId = request.RegionId
            };
        }

        /// <summary>
        /// Mapping from <see cref="ClientRequest"/> message to <see cref="PlaceFollowerExecutionRequest"/>.
        /// </summary>
        /// <param name="msg">Client request message.</param>
        /// <returns>Game execution request.</returns>
        public static PlaceFollowerExecutionRequest ToPlaceFollowerExecutionRequest(this ClientRequest msg)
        {
            CheckType(msg.Type, ClientRequestType.PlaceFollower);

            CheckHasValue(msg.Color, "Player on move must be specified.");
            CheckHasValue(msg.Coords, "Tile coordinates must be specified.");
            CheckHasValue(msg.RegionId, "Region Id must be specified.");

            return new PlaceFollowerExecutionRequest(msg.Color.Value, msg.Coords.Value, msg.RegionId.Value);
        }

        /// <summary>
        /// Mapping from <see cref="RemoveFollowerExecutionRequest"/> to <see cref="ClientRequest"/> message.
        /// </summary>
        /// <param name="request">Game execution request.</param>
        /// <returns>Client request message.</returns>
        public static ClientRequest ToClientRequest(this RemoveFollowerExecutionRequest request)
        {
            return new ClientRequest
            {
                Type = ClientRequestType.PlaceFollower,
                Color = request.Color,
                Coords = request.Coords
            };
        }

        /// <summary>
        /// Mapping from <see cref="ClientRequest"/> message to <see cref="RemoveFollowerExecutionRequest"/>.
        /// </summary>
        /// <param name="msg">Client request message.</param>
        /// <returns>Game execution request.</returns>
        public static RemoveFollowerExecutionRequest ToRemoveFollowerExecutionRequest(this ClientRequest msg)
        {
            CheckType(msg.Type, ClientRequestType.PlaceFollower);

            CheckHasValue(msg.Color, "Player on move must be specified.");
            CheckHasValue(msg.Coords, "Tile coordinates must be specified.");

            return new RemoveFollowerExecutionRequest(msg.Color.Value, msg.Coords.Value);
        }

        /// <summary>
        /// Mapping from <see cref="PassMoveExecutionRequest"/> to <see cref="ClientRequest"/> message.
        /// </summary>
        /// <param name="request">Game execution request.</param>
        /// <returns>Client request message.</returns>
        public static ClientRequest ToClientRequest(this PassMoveExecutionRequest request)
        {
            return new ClientRequest
            {
                Type = ClientRequestType.PassMove,
                Color = request.Color
            };
        }

        /// <summary>
        /// Mapping from <see cref="ClientRequest"/> message to <see cref="PassMoveExecutionRequest"/>.
        /// </summary>
        /// <param name="msg">Client request message.</param>
        /// <returns>Game execution request.</returns>
        public static PassMoveExecutionRequest ToPassMoveExecutionRequest(this ClientRequest msg)
        {
            CheckType(msg.Type, ClientRequestType.PlaceFollower);

            CheckHasValue(msg.Color, "Player on move must be specified.");

            return new PassMoveExecutionRequest(msg.Color.Value);
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
        private static void CheckType(ClientRequestType expected, ClientRequestType actual)
        {
            if (expected != actual)
            {
                throw new ArgumentException($"Expected type {expected}, actually got {actual}.");
            }
        }
        #endregion
    }
}
