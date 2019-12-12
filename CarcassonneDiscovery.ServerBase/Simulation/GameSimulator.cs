namespace CarcassonneDiscovery.Server
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Logic;
    using CarcassonneDiscovery.Messaging;
    using CarcassonneDiscovery.SimulationLibrary;

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

            // TODO: proper values
            GameState.Params.FollowerAmount = 4;
            GameState.Params.TileSetParams.Name = "Standard";
            GameState.Params.TileSetParams.TileSupplierBuilder = new Func<ITileSupplier>(() => new StandardTileSetSupplier());
        }

        #region Add or remove players
        /// <summary>
        /// Adds player to simulation.
        /// </summary>
        /// <param name="color">Color of the player.</param>
        /// <param name="name">Name of the player.</param>
        /// <returns>Request result.</returns>
        public AddPlayerResponse AddPlayer(PlayerColor color, string name)
        {
            if (SimulationState != SimulationWorkflow.BeforeGame)
            {
                return new AddPlayerResponse(ExitCode.InvalidTimeError);
            }

            if (color == PlayerColor.None)
            {
                return new AddPlayerResponse(ExitCode.InvalidColor);
            }

            if (Players.ContainsKey(color))
            {
                return new AddPlayerResponse(ExitCode.ColorAlreadyAdded);
            }

            name = name ?? string.Empty;

            Players.Add(color, name);

            return new AddPlayerResponse(ExitCode.Ok, color, name);
        }

        /// <summary>
        /// Removes player from simulation.
        /// </summary>
        /// <param name="color">Color of the player.</param>
        /// <returns>Request result.</returns>
        public RemovePlayerResponse RemovePlayer(PlayerColor color)
        {
            if (SimulationState != SimulationWorkflow.BeforeGame)
            {
                return new RemovePlayerResponse(ExitCode.InvalidTimeError);
            }

            if (!Players.ContainsKey(color))
            {
                return new RemovePlayerResponse(ExitCode.ColorNotAdded);
            }

            Players.Remove(color);

            return new RemovePlayerResponse(ExitCode.Ok, color);
        }

        /// <summary>
        /// Number of players in the game.
        /// </summary>
        public int PlayerCount => Players?.Count ?? 0;
        #endregion

        #region Change game parameters
        #endregion

        #region Game actions
        /// <summary>
        /// Starts the game.
        /// </summary>
        /// <returns>Request result.</returns>
        public StartGameResponse StartGame()
        {
            if (SimulationState != SimulationWorkflow.BeforeGame)
            {
                return new StartGameResponse(null, ExitCode.InvalidTimeError, null);
            }

            // TODO: player order
            var rnd = new Random();
            var playerOrder = Players.Keys.OrderBy(x => rnd.Next()).ToArray();

            GameState.Params.PlayerOrder = playerOrder;
            GameState.Params.PlayerAmount = playerOrder.Length;

            var result = Executor.TryStartGame(GameState);

            if (result.IsValid)
            {
                SimulationState = SimulationWorkflow.InGame;

                return new StartGameResponse(result, ExitCode.Ok, playerOrder.Select(c => Players[c]).ToArray());
            }

            return new StartGameResponse(result, ExitCode.RuleViolationError, null);
        }

        /// <summary>
        /// Starts the move.
        /// </summary>
        /// <returns>Request result.</returns>
        public StartMoveResponse StartMove()
        {
            if (SimulationState != SimulationWorkflow.InGame)
            {
                return new StartMoveResponse(null, ExitCode.InvalidTimeError);
            }

            var result = Executor.TryStartMove(GameState);
            var exitCode = result.IsValid ? ExitCode.Ok : ExitCode.RuleViolationError;

            return new StartMoveResponse(result, exitCode);
        }

        /// <summary>
        /// Places a tile.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="tile">Tile to be placed.</param>
        /// <param name="coords">Coordinates of the tile.</param>
        /// <param name="orientation">Orientation of the tile.</param>
        /// <returns>Request result.</returns>
        public PlaceTileResponse PlaceTile(PlayerColor color, ITileScheme tile, Coords coords, TileOrientation orientation)
        {
            if (SimulationState != SimulationWorkflow.InGame)
            {
                return new PlaceTileResponse(null, ExitCode.InvalidTimeError);
            }

            var result = Executor.TryPlaceTile(GameState, color, tile, coords, orientation);
            var exitCode = result.IsValid ? ExitCode.Ok : ExitCode.Error;

            return new PlaceTileResponse(result, exitCode);
        }

        /// <summary>
        /// Places a follower.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="coords">Coordinates of the follower.</param>
        /// <param name="regionId">Identifier of region for the follower placement.</param>
        /// <returns>Request result.</returns>
        public PlaceFollowerResponse PlaceFollower(PlayerColor color, Coords coords, int regionId)
        {
            if (SimulationState != SimulationWorkflow.InGame)
            {
                return new PlaceFollowerResponse(null, ExitCode.InvalidTimeError);
            }

            var result = Executor.TryPlaceFollower(GameState, color, coords, regionId);
            var exitCode = result.IsValid ? ExitCode.Ok : ExitCode.Error;

            return new PlaceFollowerResponse(result, exitCode);
        }

        /// <summary>
        /// Removes a follower.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <param name="coords">Coordinates of the follower.</param>
        /// <returns>Request result.</returns>
        public RemoveFollowerResponse RemoveFollower(PlayerColor color, Coords coords)
        {
            if (SimulationState != SimulationWorkflow.InGame)
            {
                return new RemoveFollowerResponse(null, ExitCode.InvalidTimeError);
            }

            var result = Executor.TryRemoveFollower(GameState, color, coords);
            var exitCode = result.IsValid ? ExitCode.Ok : ExitCode.Error;

            return new RemoveFollowerResponse(result, exitCode);
        }

        /// <summary>
        /// Passes the move.
        /// </summary>
        /// <param name="color">Color of player making the move.</param>
        /// <returns>Request result.</returns>
        public PassMoveResponse PassMove(PlayerColor color)
        {
            if (SimulationState != SimulationWorkflow.InGame)
            {
                return new PassMoveResponse(null, ExitCode.InvalidTimeError);
            }

            var result = Executor.TryPassMove(GameState, color);
            var exitCode = result.IsValid ? ExitCode.Ok : ExitCode.Error;

            return new PassMoveResponse(result, exitCode);
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        /// <returns>Request result.</returns>
        public EndGameResponse EndGame()
        {
            if (SimulationState != SimulationWorkflow.InGame)
            {
                return new EndGameResponse(null, ExitCode.InvalidTimeError);
            }

            var result = Executor.TryEndGame(GameState);

            if (result.IsValid)
            {
                SimulationState = SimulationWorkflow.AfterGame;

                return new EndGameResponse(result, ExitCode.Ok);
            }

            return new EndGameResponse(result, ExitCode.Error);
        }
        #endregion
    }
}
