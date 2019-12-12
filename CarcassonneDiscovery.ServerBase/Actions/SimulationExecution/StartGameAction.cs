namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Start game action.
    /// </summary>
    public class StartGameAction : ServerAction
    {
        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.StartGame();

            ServerServiceProvider.Logger.LogGameExecution(result.ExitCode, result.ExecutionResult);

            if (result.ExitCode == ExitCode.Ok)
            {
                var response = new StartGameResponse(result.ExecutionResult, ExitCode.Ok, result.PlayerNames).ToServerResponse();
                ServerServiceProvider.ClientMessager.SendToAll(response);
                ServerServiceProvider.ServerController.EnqueueAction(new StartMoveAction());
            }
        }
    }
}
