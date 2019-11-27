namespace CarcassonneDiscovery.Logic
{
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Request for pass move action execution.
    /// </summary>
    public class PassMoveExecutionRequest : GameExecutionRequest
    {
        /// <summary>
        /// Color of player making the move.
        /// </summary>
        public PlayerColor Color { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        public PassMoveExecutionRequest(PlayerColor color)
        {
            Color = color;
        }
    }
}
