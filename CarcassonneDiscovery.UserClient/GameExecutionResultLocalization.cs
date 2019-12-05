namespace CarcassonneDiscovery.UserClient
{
    using CarcassonneDiscovery.SimulationLibrary;

    /// <summary>
    /// Localization of game execution result messages.
    /// </summary>
    public class GameExecutionResultLocalization
    {
        /// <summary>
        /// Gets localization for execution result.
        /// </summary>
        /// <param name="result">Execution result.</param>
        /// <returns>Localized message.</returns>
        public string this[GameExecutionResult result]
        {
            get
            {
                switch (result)
                {
                    case GameExecutionResult.ERROR:
                        return "General error.";
                    case GameExecutionResult.ERR_ALREADY_CONNECTED:
                        return "You are already connected.";
                    case GameExecutionResult.ERR_ALREADY_OCCUPIED:
                        return "The region is already occupied by a follower.";
                    case GameExecutionResult.ERR_INCORRECT_COLOR:
                        return "The color is incorrect.";
                    case GameExecutionResult.ERR_INCORRECT_COORDS:
                        return "Invalid coordinates.";
                    case GameExecutionResult.ERR_NOT_ON_MOVE:
                        return "You are not on move.";
                    case GameExecutionResult.ERR_NO_FOLLOWER_AVAILABLE:
                        return "You have no follower available";
                    case GameExecutionResult.ERR_NO_SURROUNDINGS:
                        return "Tile must be placed next to other tile.";
                    case GameExecutionResult.ERR_TOO_MANY_PLAYERS:
                        return "There are already too much players.";
                    case GameExecutionResult.ERR_UNKNOWN_PLAYER:
                        return "You have to log as player first.";
                    case GameExecutionResult.ERR_WRONG_SURROUNDINGS:
                        return "The surrounding tiles has different region types.";
                    case GameExecutionResult.ERR_WRONG_TILE:
                        return "You are placing a wrong tile.";
                    case GameExecutionResult.ERR_WRONG_TIME:
                        return "You are not supposed to do that now.";                        
                    case GameExecutionResult.NONE:
                        return "There is no response.";
                    case GameExecutionResult.OK:
                        return "OK.";
                    default:
                        return "Unexpected result.";
                }
            }
        }
    }
}
