namespace CarcassonneDiscovery.Server
{
    using CarcassonneDiscovery.Logic;
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

            switch (result.ExitCode)
            {
                case GameExecutionRequestExitCode.Ok:
                    ServerServiceProvider.Logger.Log("Move started.", LogLevel.Normal, LogType.SimulationExecution);
                    ServerServiceProvider.ClientMessager.SendToAll(result.ExecutionResult.ToServerResponse());
                    break;

                case GameExecutionRequestExitCode.WrongSimulationState:
                    ServerServiceProvider.Logger.Log("WrongSimulationState.", LogLevel.ProgramError, LogType.SimulationExecutionError);
                    break;

                case GameExecutionRequestExitCode.Error:
                    if (result.ExecutionResult.RuleViolationType == RuleViolationType.NoTileRemaining)
                    {
                        new EndGameAction().Execute();
                    }
                    else
                    {
                        ServerServiceProvider.Logger.Log($"Error: {result.ExecutionResult.RuleViolationType}", LogLevel.ProgramError, LogType.SimulationExecutionError);
                    }

                    break;
            }
        }
    }
}
