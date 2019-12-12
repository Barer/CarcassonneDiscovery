namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Pass move action.
    /// </summary>
    public class PassMoveAction : ServerAction
    {
        /// <summary>
        /// Game action request.
        /// </summary>
        protected PassMoveExecutionRequest Request { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="request">Game action request.</param>
        public PassMoveAction(PassMoveExecutionRequest request)
        {
            Request = request;
        }

        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.PassMove(Request.Color);

            var response = new PassMoveResponse(result.ExecutionResult, result.ExitCode).ToServerResponse();

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
