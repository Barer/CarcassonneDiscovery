namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Remove follower action.
    /// </summary>
    public class RemoveFollowerAction : ServerAction
    {
        /// <summary>
        /// Game action request.
        /// </summary>
        protected RemoveFollowerExecutionRequest Request { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="request">Game action request.</param>
        public RemoveFollowerAction(RemoveFollowerExecutionRequest request)
        {
            Request = request;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.RemoveFollower(Request);

            var response = new RemoveFollowerResponse(result.ExecutionResult, result.ExitCode).ToServerResponse();

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
