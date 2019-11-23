namespace CarcassonneDiscovery.Server
{
    using System;
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Logic;

    /// <summary>
    /// Controller for automatic game simulation.
    /// </summary>
    public class SimulationController
    {
        #region Events
        /// <summary>
        /// Start game event.
        /// </summary>
        public event Action<StartGameExecutionResult> StartGameEvent = (rr) => { };

        /// <summary>
        /// Start move event.
        /// </summary>
        public event Action<StartMoveExecutionResult> StartMoveEvent = (rr) => { };

        /// <summary>
        /// Place tile event.
        /// </summary>
        public event Action<PlaceTileExecutionResult> PlaceTileEvent = (rr) => { };

        /// <summary>
        /// Place follower event.
        /// </summary>
        public event Action<PlaceFollowerExecutionResult> PlaceFollowerEvent = (rr) => { };

        /// <summary>
        /// Remove follower event.
        /// </summary>
        public event Action<RemoveFollowerExecutionResult> RemoveFollowerEvent = (rr) => { };

        /// <summary>
        /// Pass move event.
        /// </summary>
        public event Action<PassMoveExecutionResult> PassMoveEvent = (rr) => { };

        /// <summary>
        /// End game event
        /// </summary>
        public event Action<EndGameExecutionResult> EndGameEvent = (rr) => { };

        /// <summary>
        /// Rule violation event.
        /// </summary>
        public event Action<PlayerColor, RuleViolationType> RuleViolationEvent = (pc, rvt) => { };
        #endregion

        /// <summary>
        /// Game simulator.
        /// </summary>
        protected GameSimulator Simulator { get; set; }

        /// <summary>
        /// Log controller.
        /// </summary>
        protected ILogController LogController { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="logController">Controller for logging actions.</param>
        public SimulationController(ILogController logController)
        {
            Simulator = new GameSimulator();

            LogController = logController;
        }

        /// <summary>
        /// Tries to add player to simulation.
        /// </summary>
        /// <param name="color">Color of the player.</param>
        /// <param name="name">Name of the player.</param>
        public void AddPlayer(PlayerColor color, string name)
        {
            var result = Simulator.AddPlayer(color, name);

            switch (result.ExitCode)
            {
                case AddPlayerRequestExitCode.Ok:
                    // TODO
                    break;
                case AddPlayerRequestExitCode.WrongSimulationState:
                    // TODO
                    break;
                case AddPlayerRequestExitCode.ColorAlreadyAdded:
                case AddPlayerRequestExitCode.InvalidColor:
                case AddPlayerRequestExitCode.TooManyPlayers:
                    // TODO
                    break;
            }
        }

        /// <summary>
        /// Tries to remove player from simulation.
        /// </summary>
        /// <param name="color">Color of the player.</param>
        public void RemovePlayer(PlayerColor color)
        {
            var result = Simulator.RemovePlayer(color);

            switch (result.ExitCode)
            {
                case RemovePlayerRequestExitCode.Ok:
                    // TODO
                    break;
                case RemovePlayerRequestExitCode.WrongSimulationState:
                    // TODO
                    break;
                case RemovePlayerRequestExitCode.ColorNotAdded:
                    // TODO
                    break;
            }
        }

        /// <summary>
        /// Tries to start the game.
        /// </summary>
        public void StartGame()
        {
            var result = Simulator.StartGame();

            if (result.ExitCode == GameExecutionRequestExitCode.Ok)
            {
                StartGameEvent.Invoke(result.ExecutionResult);

                StartMove();
            }
            else
            {
                // TODO: Log error
            }
        }

        /// <summary>
        /// Tries to start the move.
        /// </summary>
        protected void StartMove()
        {
            var result = Simulator.StartMove();

            if (result.ExitCode == GameExecutionRequestExitCode.Ok)
            {
                StartMoveEvent.Invoke(result.ExecutionResult);
            }
            else if (result.ExitCode == GameExecutionRequestExitCode.Error &&
                result.ExecutionResult.RuleViolationType == RuleViolationType.NoTileRemaining)
            {
                EndGame();
            }
            else
            {
                // TODO: Log error
            }
        }

        /// <summary>
        /// Tries to place a tile.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="tile">Tile to be placed.</param>
        /// <param name="coords">Coordinates of the tile.</param>
        /// <param name="orientation">Orientation of the tile.</param>
        public void PlaceTile(PlayerColor color, ITileScheme tile, Coords coords, TileOrientation orientation)
        {
            var result = Simulator.PlaceTile(color, tile, coords, orientation);

            if (result.ExitCode == GameExecutionRequestExitCode.Ok)
            {
                PlaceTileEvent.Invoke(result.ExecutionResult);
            }
            else if (result.ExitCode == GameExecutionRequestExitCode.Error)
            {
                RuleViolationEvent.Invoke(color, result.ExecutionResult.RuleViolationType);
            }
            else if (result.ExitCode == GameExecutionRequestExitCode.WrongSimulationState)
            {
                RuleViolationEvent.Invoke(color, RuleViolationType.InvalidMovePhase);
            }
        }

        /// <summary>
        /// Tries to place a follower.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="coords">Coordinates of the follower.</param>
        /// <param name="regionId">Identifier of region for the follower placement.</param>
        public void PlaceFollower(PlayerColor color, Coords coords, int regionId)
        {
            var result = Simulator.PlaceFollower(color, coords, regionId);

            if (result.ExitCode == GameExecutionRequestExitCode.Ok)
            {
                PlaceFollowerEvent.Invoke(result.ExecutionResult);

                StartMove();
            }
            else if (result.ExitCode == GameExecutionRequestExitCode.Error)
            {
                RuleViolationEvent.Invoke(color, result.ExecutionResult.RuleViolationType);
            }
            else if (result.ExitCode == GameExecutionRequestExitCode.WrongSimulationState)
            {
                RuleViolationEvent.Invoke(color, RuleViolationType.InvalidMovePhase);
            }
        }

        /// <summary>
        /// Tries to remove a follower.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="coords">Coordinates of the follower.</param>
        public void RemoveFollower(PlayerColor color, Coords coords)
        {
            var result = Simulator.RemoveFollower(color, coords);

            if (result.ExitCode == GameExecutionRequestExitCode.Ok)
            {
                RemoveFollowerEvent.Invoke(result.ExecutionResult);

                StartMove();
            }
            else if (result.ExitCode == GameExecutionRequestExitCode.Error)
            {
                RuleViolationEvent.Invoke(color, result.ExecutionResult.RuleViolationType);
            }
            else if (result.ExitCode == GameExecutionRequestExitCode.WrongSimulationState)
            {
                RuleViolationEvent.Invoke(color, RuleViolationType.InvalidMovePhase);
            }
        }

        /// <summary>
        /// Tries to pass the move.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        public void PassMove(PlayerColor color)
        {
            var result = Simulator.PassMove(color);

            if (result.ExitCode == GameExecutionRequestExitCode.Ok)
            {
                PassMoveEvent.Invoke(result.ExecutionResult);

                StartMove();
            }
            else if (result.ExitCode == GameExecutionRequestExitCode.Error)
            {
                RuleViolationEvent.Invoke(color, result.ExecutionResult.RuleViolationType);
            }
            else if (result.ExitCode == GameExecutionRequestExitCode.WrongSimulationState)
            {
                RuleViolationEvent.Invoke(color, RuleViolationType.InvalidMovePhase);
            }
        }

        /// <summary>
        /// Tries to end the game.
        /// </summary>
        protected void EndGame()
        {
            var result = Simulator.EndGame();

            if (result.ExitCode == GameExecutionRequestExitCode.Ok)
            {
                EndGameEvent.Invoke(result.ExecutionResult);
            }
            else
            {
                // TODO: Log error
            }
        }
    }
}
