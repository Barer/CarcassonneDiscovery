namespace CarcassonneDiscovery.AiClient
{
    using System;

    /// <summary>
    /// Program main file.
    /// </summary>
    public class AiProgram
    {
        /// <summary>
        /// Program entry point.
        /// </summary>
        /// <param name="player">Player object.</param>
        /// <param name="args">Command line arguments.</param>
        public static void Run<T>(T player, string[] args)
            where T : IPlayer, new()
        {
            var client = new AiClient(player);
            var parsedArgs = new ArgumentParser().Parse(args);

            Console.WriteLine($"IP: {parsedArgs.IP}");
            Console.WriteLine($"Port: {parsedArgs.Port}");
            Console.WriteLine($"Name: {parsedArgs.Name}");
            Console.WriteLine($"Color: {parsedArgs.Color}");

            try
            {
                client.Run(parsedArgs);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Console.WriteLine("Press enter to exit");
                Console.ReadLine();
            }
        }
    }
}
