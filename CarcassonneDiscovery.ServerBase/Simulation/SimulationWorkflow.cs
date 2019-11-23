namespace CarcassonneDiscovery.Server
{
    /// <summary>
    /// Phases of simulation.
    /// </summary>
    public enum SimulationWorkflow
    {
        /// <summary>
        /// Before game start.
        /// </summary>
        BeforeGame,

        /// <summary>
        /// Between game start and game end.
        /// </summary>
        InGame,

        /// <summary>
        /// After game end.
        /// </summary>
        AfterGame
    }
}
