namespace CarcassonneDiscovery.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Logic.Execution;

    /// <summary>
    /// Performs game actions.
    /// </summary>
    public class GameExecutor
    {
        /// <summary>
        /// Tries to start new game.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <returns>Game execution result.</returns>
        public StartGameExecutionResult TryStartGame(GameState state)
        {
            var stateParams = state.Params;

            if (stateParams.PlayerOrder.Length != stateParams.FollowerAmount)
            {
                return new StartGameExecutionResult(RuleViolationType.InconsistentGameParameters);
            }

            if (stateParams.TileSetParams == null)
            {
                return new StartGameExecutionResult(RuleViolationType.InconsistentGameParameters);
            }

            if (state.MovePhase != MoveWorkflow.GameStarted)
            {
                return new StartGameExecutionResult(RuleViolationType.InvalidMovePhase);
            }

            return SetStartGame(state);
        }

        /// <summary>
        /// Starts new game.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <returns>Game execution result.</returns>
        public StartGameExecutionResult SetStartGame(GameState state)
        {
            // Initialize game state
            state.CurrentPlayerIndex = -1;

            state.Grid = new Dictionary<Coords, TilePlacement>();
            state.PlacedFollowers = new Dictionary<PlayerColor, List<FollowerPlacement>>();
            state.Scores = new Dictionary<PlayerColor, int>();

            foreach (var player in state.Params.PlayerOrder)
            {
                state.PlacedFollowers.Add(player, new List<FollowerPlacement>());
                state.Scores.Add(player, 0);
            }

            state.TileSupplier = state.Params.TileSetParams.TileSupplierBuilder();

            // Place first tile
            var firstTile = state.TileSupplier.GetFirstTile();
            var firstCoords = new Coords(0, 0);
            var firstOrientation = TileOrientation.N;

            state.Grid.Add(
                firstCoords,
                new TilePlacement
                {
                    Coords = firstCoords,
                    Orientation = firstOrientation,
                    TileScheme = firstTile,
                    FollowerPlacement = null
                });

            state.MovePhase = MoveWorkflow.FirstTilePlaced;

            return new StartGameExecutionResult(firstTile, firstCoords, firstOrientation);
        }

        /// <summary>
        /// Sets player on move and tile to be played.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="color">Current player on move.</param>
        /// <param name="tile">Current tile to be placed.</param>
        /// <returns>Game execution result.</returns>
        public StartMoveExecutionResult SetStartMove(GameState state, PlayerColor color, ITileScheme tile)
        {
            state.CurrentPlayerIndex = Array.IndexOf(state.Params.PlayerOrder, color);
            state.CurrentTile = tile;
            state.MovePhase = MoveWorkflow.NextTileSelected;

            return new StartMoveExecutionResult(tile, color);
        }

        /// <summary>
        /// Tries to start next move. If next move cannot be started, an exception is thrown.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <returns>Game execution result.</returns>
        public StartMoveExecutionResult TryStartMove(GameState state)
        {
            if (state.MovePhase != MoveWorkflow.FirstTilePlaced && state.MovePhase != MoveWorkflow.MoveEnded)
            {
                return new StartMoveExecutionResult(RuleViolationType.InvalidMovePhase);
            }

            var stateTileSupplier = state.TileSupplier;

            if (stateTileSupplier.RemainingCount == 0)
            {
                return new StartMoveExecutionResult(RuleViolationType.NoTileRemaining);
            }

            var nextTile = stateTileSupplier.GetNextTile();

            var stateParams = state.Params;

            var nextPlayerIndex = state.CurrentPlayerIndex;
            nextPlayerIndex = (nextPlayerIndex + 1) % stateParams.PlayerAmount;
            var nextPlayer = stateParams.PlayerOrder[nextPlayerIndex];

            return SetStartMove(state, nextPlayer, nextTile);
        }
    }
}
