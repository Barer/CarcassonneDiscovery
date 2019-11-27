namespace CarcassonneDiscovery.Messaging
{
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Extensions methods for mappings from <see cref="GameExecutionResult"/> to <see cref="ServerResponse"/>
    /// </summary>
    public static class ExecutionResultMappingExtensions
    {
        /// <summary>
        /// Mapping from <see cref="StartGameExecutionResult"/> to <see cref="ServerResponse"/> DTO.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Dto.</returns>
        public static ServerResponse ToDto(this StartGameExecutionResult result)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Mapping from <see cref="StartMoveExecutionResult"/> to <see cref="ServerResponse"/> DTO.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Dto.</returns>
        public static ServerResponse ToDto(this StartMoveExecutionResult result)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Mapping from <see cref="PlaceTileExecutionResult"/> to <see cref="ServerResponse"/> DTO.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Dto.</returns>
        public static ServerResponse ToDto(this PlaceTileExecutionResult result)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Mapping from <see cref="PlaceFollowerExecutionResult"/> to <see cref="ServerResponse"/> DTO.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Dto.</returns>
        public static ServerResponse ToDto(this PlaceFollowerExecutionResult result)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Mapping from <see cref="RemoveFollowerExecutionResult"/> to <see cref="ServerResponse"/> DTO.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Dto.</returns>
        public static ServerResponse ToDto(this RemoveFollowerExecutionResult result)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Mapping from <see cref="PassMoveExecutionResult"/> to <see cref="ServerResponse"/> DTO.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Dto.</returns>
        public static ServerResponse ToDto(this PassMoveExecutionResult result)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        /// Mapping from <see cref="EndGameExecutionResult"/> to <see cref="ServerResponse"/> DTO.
        /// </summary>
        /// <param name="result">Game execution result.</param>
        /// <returns>Dto.</returns>
        public static ServerResponse ToDto(this EndGameExecutionResult result)
        {
            throw new System.NotImplementedException();
        }
    }
}
