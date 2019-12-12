namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Place tile action.
    /// </summary>
    public class PlaceTileAction : ServerAction
    {
        /// <summary>
        /// Game action request.
        /// </summary>
        protected PlaceTileExecutionRequest Request { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="request">Game action request.</param>
        public PlaceTileAction(PlaceTileExecutionRequest request)
        {
            Request = request;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.PlaceTile(Request.Color, Request.Tile, Request.Coords, Request.Orientation);

            var response = new PlaceTileResponse(result.ExecutionResult, result.ExitCode).ToServerResponse();

            ServerServiceProvider.Logger.LogGameExecution(result.ExitCode, result.ExecutionResult);

            if (result.ExitCode == ExitCode.Ok)
            {
                ServerServiceProvider.ClientMessager.SendToAll(response);
            }
            else
            {
                ServerServiceProvider.ClientMessager.SendToPlayer(Request.Color, response);
            }
        }
    }
}
