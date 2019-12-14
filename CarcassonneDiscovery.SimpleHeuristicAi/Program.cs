namespace CarcassonneDiscovery.AiClient
{
    /// <summary>
    /// Program main file.
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        public static void Main(string[] args)
        {
            AiProgram.Run(new SimpleHeuristicAiPlayer(), args);
        }
    }
}
