namespace CarcassonneDiscovery.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Simulator of the game.
    /// </summary>
    public class GameSimulator
    {
        /// <summary>
        /// State of the simulation.
        /// </summary>
        protected SimulationWorkflow SimulationState { get; set; }

        /// <summary>
        /// Players in the game.
        /// </summary>
        protected Dictionary<PlayerColor, string> Players { get; set; }

        /// <summary>
        /// Game executor.
        /// </summary>
        protected GameExecutor Executor { get; set; }

        /// <summary>
        /// Current game state.
        /// </summary>
        protected GameState GameState { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GameSimulator()
        {
            SimulationState = SimulationWorkflow.BeforeGame;
            Players = new Dictionary<PlayerColor, string>();
            Executor = new GameExecutor();
            GameState = new GameState();
        }

        #region Add or remove players
        /// <summary>
        /// Adds player to simulation.
        /// </summary>
        /// <param name="color">Color of the player.</param>
        /// <param name="name">Name of the player.</param>
        /// <returns>Request result.</returns>
        public AddPlayerRequestResult AddPlayer(PlayerColor color, string name)
        {
            if (SimulationState != SimulationWorkflow.BeforeGame)
            {
                return new AddPlayerRequestResult
                {
                    ExitCode = AddPlayerRequestExitCode.WrongSimulationState
                };
            }

            if (color == PlayerColor.None)
            {
                return new AddPlayerRequestResult
                {
                    ExitCode = AddPlayerRequestExitCode.InvalidColor
                };
            }

            if (Players.ContainsKey(color))
            {
                return new AddPlayerRequestResult
                {
                    ExitCode = AddPlayerRequestExitCode.ColorAlreadyAdded
                };
            }

            name = name ?? string.Empty;

            Players.Add(color, name);

            return new AddPlayerRequestResult
            {
                ExitCode = AddPlayerRequestExitCode.Ok,
                Color = color,
                Name = name
            };
        }

        /// <summary>
        /// Removes player from simulation.
        /// </summary>
        /// <param name="color">Color of the player.</param>
        /// <returns>Request result.</returns>
        public RemovePlayerRequestResult RemovePlayer(PlayerColor color)
        {
            if (SimulationState != SimulationWorkflow.BeforeGame)
            {
                return new RemovePlayerRequestResult
                {
                    ExitCode = RemovePlayerRequestExitCode.WrongSimulationState
                };
            }

            if (!Players.ContainsKey(color))
            {
                return new RemovePlayerRequestResult
                {
                    ExitCode = RemovePlayerRequestExitCode.ColorNotAdded
                };
            }

            Players.Remove(color);

            return new RemovePlayerRequestResult
            {
                ExitCode = RemovePlayerRequestExitCode.Ok,
                Color = color
            };
        }
        #endregion

        #region Change game parameters
        #endregion

        #region Game actions
        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <returns>Request result.</returns>
        public StartGameRequestResult StartGame()
        {
            if (SimulationState != SimulationWorkflow.BeforeGame)
            {
                return new StartGameRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            // TODO: player order
            var rnd = new Random();
            var playerOrder = Players.Keys.OrderBy(x => rnd.Next()).ToArray();

            GameState.Params.PlayerOrder = playerOrder;

            var result = Executor.TryStartGame(GameState);

            if (result.IsValid)
            {
                SimulationState = SimulationWorkflow.InGame;

                return new StartGameRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.Ok,
                    ExecutionResult = result,
                    PlayerNames = playerOrder.Select(c => Players[c]).ToArray()
                };
            }

            return new StartGameRequestResult
            {
                ExitCode = GameExecutionRequestExitCode.Error,
                ExecutionResult = result
            };
        }

        /// <summary>
        /// Starts the move.
        /// </summary>
        /// <returns>Request result.</returns>
        public StartMoveRequestResult StartMove()
        {
            if (SimulationState != SimulationWorkflow.InGame)
            {
                return new StartMoveRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            var result = Executor.TryStartMove(GameState);

            return new StartMoveRequestResult
            {
                ExitCode = result.IsValid ? GameExecutionRequestExitCode.Ok : GameExecutionRequestExitCode.Error,
                ExecutionResult = result
            };
        }

        /// <summary>
        /// Places a tile.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="tile">Tile to be placed.</param>
        /// <param name="coords">Coordinates of the tile.</param>
        /// <param name="orientation">Orientation of the tile.</param>
        /// <returns>Request result.</returns>
        public PlaceTileRequestResult PlaceTile(PlayerColor color, ITileScheme tile, Coords coords, TileOrientation orientation)
        {
            if (SimulationState != SimulationWorkflow.InGame)
            {
                return new PlaceTileRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            var result = Executor.TryPlaceTile(GameState, color, tile, coords, orientation);

            return new PlaceTileRequestResult
            {
                ExitCode = result.IsValid ? GameExecutionRequestExitCode.Ok : GameExecutionRequestExitCode.Error,
                ExecutionResult = result
            };
        }

        /// <summary>
        /// Places a follower.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="coords">Coordinates of the follower.</param>
        /// <param name="regionId">Identifier of region for the follower placement.</param>
        /// <returns>Request result.</returns>
        public PlaceFollowerRequestResult PlaceFollower(PlayerColor color, Coords coords, int regionId)
        {
            if (SimulationState != SimulationWorkflow.InGame)
            {
                return new PlaceFollowerRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            var result = Executor.TryPlaceFollower(GameState, color, coords, regionId);

            return new PlaceFollowerRequestResult
            {
                ExitCode = result.IsValid ? GameExecutionRequestExitCode.Ok : GameExecutionRequestExitCode.Error,
                ExecutionResult = result
            };
        }

        /// <summary>
        /// Removes a follower.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="coords">Coordinates of the follower.</param>
        /// <returns>Request result.</returns>
        public RemoveFollowerRequestResult RemoveFollower(PlayerColor color, Coords coords)
        {
            if (SimulationState != SimulationWorkflow.InGame)
            {
                return new RemoveFollowerRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            var result = Executor.TryRemoveFollower(GameState, color, coords);

            return new RemoveFollowerRequestResult
            {
                ExitCode = result.IsValid ? GameExecutionRequestExitCode.Ok : GameExecutionRequestExitCode.Error,
                ExecutionResult = result
            };
        }

        /// <summary>
        /// Passes the move.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <returns>Request result.</returns>
        public PassMoveRequestResult PassMove(PlayerColor color)
        {
            if (SimulationState != SimulationWorkflow.InGame)
            {
                return new PassMoveRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            var result = Executor.TryPassMove(GameState, color);

            return new PassMoveRequestResult
            {
                ExitCode = result.IsValid ? GameExecutionRequestExitCode.Ok : GameExecutionRequestExitCode.Error,
                ExecutionResult = result
            };
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        /// <returns>Request result.</returns>
        public EndGameRequestResult EndGame()
        {
            if (SimulationState != SimulationWorkflow.InGame)
            {
                return new EndGameRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            var result = Executor.TryEndGame(GameState);

            if (result.IsValid)
            {
                SimulationState = SimulationWorkflow.AfterGame;

                return new EndGameRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.Ok,
                    ExecutionResult = result
                };
            }

            return new EndGameRequestResult
            {
                ExitCode = GameExecutionRequestExitCode.Error,
                ExecutionResult = result
            };
        }
        #endregion
    }
}
