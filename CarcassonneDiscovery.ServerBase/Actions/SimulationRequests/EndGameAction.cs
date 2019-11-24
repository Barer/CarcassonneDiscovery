namespace CarcassonneDiscovery.Server
{
    /// <summary>
    /// End game action.
    /// </summary>
    public class EndGameAction : ServerAction
    {
        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.EndGame();

            switch (result.ExitCode)
            {
                case GameExecutionRequestExitCode.Ok:
                    ServerServiceProvider.Logger.Log("Game ended.", LogLevel.Normal, LogType.SimulationExecution);
                    ServerServiceProvider.ClientMessager.SendToAll(new { TODO = "Todo" });
                    new StartMoveAction().Execute();
                    break;

                case GameExecutionRequestExitCode.WrongSimulationState:
                    ServerServiceProvider.Logger.Log("WrongSimulationState.", LogLevel.Warning, LogType.SimulationExecutionError);
                    break;

                case GameExecutionRequestExitCode.Error:
                    ServerServiceProvider.Logger.Log($"Error: {result.ExecutionResult.RuleViolationType}", LogLevel.Warning, LogType.SimulationExecutionError);
                    break;
            }
        }
    }
}
