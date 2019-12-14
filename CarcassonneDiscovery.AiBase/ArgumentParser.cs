namespace CarcassonneDiscovery.AiClient
{
    using System;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Parse of the input program arguments.
    /// </summary>
    public class ArgumentParser
    {
        /// <summary>
        /// State of the automaton.
        /// </summary>
        private enum ArgumentParserState
        {
            /// <summary>
            /// Expecting name on the input.
            /// </summary>
            NAME,

            /// <summary>
            /// Expecting IP address on the input.
            /// </summary>
            IP,

            /// <summary>
            /// Expecting port number on the input.
            /// </summary>
            PORT,

            /// <summary>
            /// Expecting color on the input.
            /// </summary>
            COLOR,

            /// <summary>
            /// Expecting argument name on the input.
            /// </summary>
            ARGUMENT
        }

        /// <summary>
        /// Parses program arguments into AI player arguments. 
        /// </summary>
        /// <param name="args">Program arguments.</param>
        /// <returns>AI player arguments.</returns>
        public AiArguments Parse(string[] args)
        {
            var result = AiArguments.Default;

            var state = ArgumentParserState.ARGUMENT;

            bool setIP = false, setPort = false, setName = false, setColor = false;

            foreach (var arg in args)
            {
                switch (state)
                {
                    case ArgumentParserState.ARGUMENT:
                        state = ParseArgument(arg);
                        break;

                    case ArgumentParserState.IP:
                        if (setIP)
                        {
                            throw new ArgumentException("Cannot set IP address twice.");
                        }

                        result.IP = arg;
                        setIP = true;
                        state = ArgumentParserState.ARGUMENT;
                        break;

                    case ArgumentParserState.PORT:
                        if (setPort)
                        {
                            throw new ArgumentException("Cannot set port number twice.");
                        }

                        result.Port = ParsePort(arg);
                        setPort = true;
                        state = ArgumentParserState.ARGUMENT;
                        break;

                    case ArgumentParserState.NAME:
                        if (setName)
                        {
                            throw new ArgumentException("Cannot set name twice.");
                        }

                        result.Name = arg;
                        setName = true;
                        state = ArgumentParserState.ARGUMENT;
                        break;

                    case ArgumentParserState.COLOR:
                        if (setColor)
                        {
                            throw new ArgumentException("Cannot set color twice.");
                        }

                        result.Color = ParseColor(arg);
                        setColor = true;
                        state = ArgumentParserState.ARGUMENT;
                        break;
                }
            }

            if (state != ArgumentParserState.ARGUMENT)
            {
                throw new ArgumentException($"Expected argument value for {state}.");
            }

            return result;
        }

        /// <summary>
        /// Parses argument name.
        /// </summary>
        /// <param name="arg">Argument name.</param>
        /// <returns>Next expected argument.</returns>
        private ArgumentParserState ParseArgument(string arg)
        {
            switch (arg.ToLower())
            {
                case "-i":
                case "--ip":
                    return ArgumentParserState.IP;

                case "-p":
                case "--port":
                    return ArgumentParserState.PORT;

                case "-n":
                case "--name":
                    return ArgumentParserState.NAME;

                case "-c":
                case "--color":
                    return ArgumentParserState.COLOR;
            }

            throw new ArgumentException($"Unexpected argument on the input: {arg}.");
        }

        /// <summary>
        /// Parses player color.
        /// </summary>
        /// <param name="arg">Color argument.</param>
        /// <returns>Player color.</returns>
        private PlayerColor ParseColor(string arg)
        {
            switch (arg.ToUpper())
            {
                case "R":
                case "RED":
                    return PlayerColor.Red;

                case "G":
                case "GREEN":
                    return PlayerColor.Green;

                case "B":
                case "BLUE":
                    return PlayerColor.Blue;

                case "Y":
                case "YELLOW":
                    return PlayerColor.Yellow;

                case "K":
                case "BLACK":
                    return PlayerColor.Black;
            }

            throw new ArgumentException($"Unexpected color on the input: {arg}.");
        }

        /// <summary>
        /// Parses port number.
        /// </summary>
        /// <param name="arg">Port argument.</param>
        /// <returns>Port number.</returns>
        private int ParsePort(string arg)
        {
            if (int.TryParse(arg, out int result))
            {
                return result;
            }

            throw new ArgumentException($"Unexpected port number: {arg}.");
        }
    }
}
