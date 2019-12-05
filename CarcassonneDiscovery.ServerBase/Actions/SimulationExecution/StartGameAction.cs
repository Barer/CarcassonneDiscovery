namespace CarcassonneDiscovery.Server
{
    /// <summary>
    /// Start game action.
    /// </summary>
    public class StartGameAction : ServerAction
    {
        /// <inheritdoc />
        public override void Execute()
        {
            var result = ServerServiceProvider.GameSimulator.StartGame();

            switch (result.ExitCode)
            {
                case GameExecutionRequestExitCode.Ok:
                    ServerServiceProvider.Logger.Log("Game started.", LogLevel.Normal, LogType.SimulationExecution);
                    var response = result.ExecutionResult.ToServerResponse();
                    response.PlayerNames = result.PlayerNames;
                    ServerServiceProvider.ClientMessager.SendToAll(response);
                    new StartMoveAction().Execute();
                    break;

                case GameExecutionRequestExitCode.WrongSimulationState:
                    ServerServiceProvider.Logger.Log("WrongSimulationState.", LogLevel.ProgramError, LogType.SimulationExecutionError);
                    break;

                case GameExecutionRequestExitCode.Error:
                    ServerServiceProvider.Logger.Log($"Error: {result.ExecutionResult.RuleViolationType}", LogLevel.ProgramError, LogType.SimulationExecutionError);
                    break;
            }
        }
    }
}
