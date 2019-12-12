namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// End game action.
    /// </summary>
    public class EndGameAction : ServerAction
    {
        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.EndGame();

            ServerServiceProvider.Logger.LogGameExecution(result.ExitCode, result.ExecutionResult);

            if (result.ExitCode == ExitCode.Ok)
            {
                var response = new EndGameResponse(result.ExecutionResult, ExitCode.Ok).ToServerResponse();
                ServerServiceProvider.ClientMessager.SendToAll(response);
            }
        }
    }
}
