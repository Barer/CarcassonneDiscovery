namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// Start move action.
    /// </summary>
    public class StartMoveAction : ServerAction
    {
        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.StartMove();

            var response = new StartMoveResponse(result.ExecutionResult, result.ExitCode).ToServerResponse();

            if (result.ExitCode == ExitCode.RuleViolationError && result.ExecutionResult.RuleViolationType == Logic.RuleViolationType.NoTileRemaining)
            {
                ServerServiceProvider.ServerController.EnqueueActionAsFirst(new EndGameAction());
                return;
            }

            ServerServiceProvider.Logger.LogGameExecution(result.ExitCode, result.ExecutionResult);

            if (result.ExitCode == ExitCode.Ok)
            {
                ServerServiceProvider.ClientMessager.SendToAll(response);
                ServerServiceProvider.ServerController.EnqueueAction(new StartMoveAction());
            }
        }
    }
}
