namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Place follower action.
    /// </summary>
    public class PlaceFollowerAction : ServerAction
    {
        /// <summary>
        /// Game action request.
        /// </summary>
        protected PlaceFollowerExecutionRequest Request { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="request">Game action request.</param>
        public PlaceFollowerAction(PlaceFollowerExecutionRequest request)
        {
            Request = request;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.PlaceFollower(Request.Color, Request.Coords, Request.RegionId);

            var response = new PlaceFollowerResponse(result.ExecutionResult, result.ExitCode).ToServerResponse();

            ServerServiceProvider.Logger.LogGameExecution(result.ExitCode, result.ExecutionResult);

            if (result.ExitCode == ExitCode.Ok)
            {
                ServerServiceProvider.ClientMessager.SendToAll(response);
                ServerServiceProvider.ServerController.EnqueueAction(new StartMoveAction());
            }
            else
            {
                ServerServiceProvider.ClientMessager.SendToPlayer(Request.Color, response);
            }
        }
    }
}
