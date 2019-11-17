namespace CarcassonneDiscovery.Logic
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Tools;

    /// <summary>
    /// Performs game actions.
    /// </summary>
    public class GameExecutor
    {
        /// <summary>
        /// Grid search algorithm.
        /// </summary>
        protected GridSearch GridSearch { get; set; }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GameExecutor()
        {
            GridSearch = new GridSearch();
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
        /// Tries to start new game.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <returns>Game execution result.</returns>
        public StartGameExecutionResult TryStartGame(GameState state)
        {
            var stateParams = state.Params;

            if (stateParams.PlayerOrder.Length != stateParams.FollowerAmount)
            {
                return new StartGameExecutionResult(RuleViolationType.InconsistentGameState);
            }

            if (stateParams.TileSetParams == null)
            {
                return new StartGameExecutionResult(RuleViolationType.InconsistentGameState);
            }

            if (state.MovePhase != MoveWorkflow.GameStarted)
            {
                return new StartGameExecutionResult(RuleViolationType.InvalidMovePhase);
            }

            return SetStartGame(state);
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

        /// <summary>
        /// Places tile on the gird.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="color">Current player on move.</param>
        /// <param name="tile">Placed tile.</param>
        /// <param name="coords">Coordinates of the placed tile.</param>
        /// <param name="orientation">Orientation of the placed tile.</param>
        /// <returns>Game execution result.</returns>
        public PlaceTileExecutionResult SetPlaceTile(GameState state, PlayerColor color, ITileScheme tile, Coords coords, TileOrientation orientation)
        {
            state.Grid.Add(
                coords,
                new TilePlacement
                {
                    Coords = coords,
                    TileScheme = tile,
                    Orientation = orientation,
                    FollowerPlacement = null
                });

            state.CurrentTileCoords = coords;

            state.MovePhase = MoveWorkflow.TilePlaced;

            return new PlaceTileExecutionResult(color, tile, coords, orientation);
        }

        /// <summary>
        /// Tries to place tile on the gird.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="color">Current player on move.</param>
        /// <param name="tile">Placed tile.</param>
        /// <param name="coords">Coordinates of the placed tile.</param>
        /// <param name="orientation">Orientation of the placed tile.</param>
        /// <returns>Game execution result.</returns>
        public PlaceTileExecutionResult TryPlaceTile(GameState state, PlayerColor color, ITileScheme tile, Coords coords, TileOrientation orientation)
        {
            if (!IsPlayerOnMove(state, color))
            {
                return new PlaceTileExecutionResult(RuleViolationType.NotOnMove);
            }

            if (!IsCorrectMovePhase(state, MoveWorkflow.NextTileSelected))
            {
                return new PlaceTileExecutionResult(RuleViolationType.InvalidMovePhase);
            }

            if (!tile.TileEquals(state.CurrentTile))
            {
                return new PlaceTileExecutionResult(RuleViolationType.InvalidTile);
            }

            switch (GridSearch.CheckTilePlacement(state.Grid, tile, coords, orientation))
            {
                case GridRuleViolation.Ok:
                    return SetPlaceTile(state, color, tile, coords, orientation);
                case GridRuleViolation.IncompatibleSurroundings:
                    return new PlaceTileExecutionResult(RuleViolationType.IncompatibleSurroundings);
                case GridRuleViolation.NoNeighboringTile:
                    return new PlaceTileExecutionResult(RuleViolationType.NoNeighboringTile);
                default:
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        /// Places follower on the board.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="color">Current player on move.</param>
        /// <param name="coords">Coordinates of the tile where the follower is being placed.</param>
        /// <param name="regionId">Identifier of the region where the follower is being placed.</param>
        /// <returns>Game execution result.</returns>
        public PlaceFollowerExecutionResult SetPlaceFollower(GameState state, PlayerColor color, Coords coords, int regionId)
        {
            var placement = new FollowerPlacement
            {
                Color = color,
                Coords = coords,
                RegionId = regionId
            };

            state.PlacedFollowers[color].Add(placement);
            state.Grid[coords].FollowerPlacement = placement;

            state.MovePhase = MoveWorkflow.MoveEnded;

            return new PlaceFollowerExecutionResult(color, coords, regionId);
        }

        /// <summary>
        /// Tries to place follower on the board.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="color">Current player on move.</param>
        /// <param name="coords">Coordinates of the tile where the follower is being placed.</param>
        /// <param name="regionId">Identifier of the region where the follower is being placed.</param>
        /// <returns>Game execution result.</returns>
        public PlaceFollowerExecutionResult TryPlaceFollower(GameState state, PlayerColor color, Coords coords, int regionId)
        {
            if (!IsPlayerOnMove(state, color))
            {
                return new PlaceFollowerExecutionResult(RuleViolationType.NotOnMove);
            }

            if (!IsCorrectMovePhase(state, MoveWorkflow.TilePlaced))
            {
                return new PlaceFollowerExecutionResult(RuleViolationType.InvalidMovePhase);
            }

            if (state.PlacedFollowers[color].Count >= state.Params.FollowerAmount)
            {
                return new PlaceFollowerExecutionResult(RuleViolationType.NoFollowerAvailable);
            }

            if (state.CurrentTileCoords != coords)
            {
                return new PlaceFollowerExecutionResult(RuleViolationType.InvalidCoordinates);
            }

            if (GridSearch.IsRegionOccupied(state.Grid, coords, regionId))
            {
                return new PlaceFollowerExecutionResult(RuleViolationType.RegionOccupied);
            }

            return SetPlaceFollower(state, color, coords, regionId);
        }

        /// <summary>
        /// Removes follower form the board.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="color">Current player on move.</param>
        /// <param name="coords">Coordinates of the tile where the follower is being removed from.</param>
        /// <returns>Game execution result.</returns>
        public RemoveFollowerExecutionResult SetRemoveFollower(GameState state, PlayerColor color, Coords coords)
        {
            var placement = state.Grid[coords].FollowerPlacement;

            state.Grid[coords].FollowerPlacement = null;
            state.PlacedFollowers[color].Remove(placement);

            var score = GridSearch.GetScoreForFollower(state.Grid, coords, placement.RegionId);

            state.MovePhase = MoveWorkflow.MoveEnded;

            return new RemoveFollowerExecutionResult(color, coords, score);
        }

        /// <summary>
        /// Tries to remove follower form the board.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="color">Current player on move.</param>
        /// <param name="coords">Coordinates of the tile where the follower is being removed from.</param>
        /// <returns>Game execution result.</returns>
        public RemoveFollowerExecutionResult TryRemoveFollower(GameState state, PlayerColor color, Coords coords)
        {
            if (!IsPlayerOnMove(state, color))
            {
                return new RemoveFollowerExecutionResult(RuleViolationType.NotOnMove);
            }

            if (!IsCorrectMovePhase(state, MoveWorkflow.TilePlaced))
            {
                return new RemoveFollowerExecutionResult(RuleViolationType.InvalidMovePhase);
            }

            var fpInGrid = state.Grid.TryGetValue(coords, out var tp) ? tp.FollowerPlacement : null;
            var fpInList = state.PlacedFollowers.TryGetValue(color, out var fpList) ? fpList.FirstOrDefault(fp => fp.Coords == coords) : null;

            if (fpInGrid == null && fpInList == null)
            {
                return new RemoveFollowerExecutionResult(RuleViolationType.InvalidCoordinates);
            }

            if (fpInGrid == null || fpInList == null || !fpInGrid.Equals(fpInList))
            {
                return new RemoveFollowerExecutionResult(RuleViolationType.InconsistentGameState);
            }

            return SetRemoveFollower(state, color, coords);
        }

        /// <summary>
        /// Passes move.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="color">Current player on move.</param>
        /// <returns>Game execution result.</returns>
        public PassMoveExecutionResult SetPassMove(GameState state, PlayerColor color)
        {
            state.MovePhase = MoveWorkflow.MoveEnded;

            return new PassMoveExecutionResult(color);
        }

        /// <summary>
        /// Tries to pass move.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="color">Current player on move.</param>
        /// <returns>Game execution result.</returns>
        public PassMoveExecutionResult TryPassMove(GameState state, PlayerColor color)
        {
            if (!IsPlayerOnMove(state, color))
            {
                return new PassMoveExecutionResult(RuleViolationType.NotOnMove);
            }

            if (!IsCorrectMovePhase(state, MoveWorkflow.TilePlaced))
            {
                return new PassMoveExecutionResult(RuleViolationType.InvalidMovePhase);
            }

            return SetPassMove(state, color);
        }

        /// <summary>
        /// Ends the game.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <returns>Game execution result.</returns>
        public EndGameExecutionResult SetEndGame(GameState state)
        {
            var allPlacements = state.PlacedFollowers.SelectMany(x => x.Value).ToList();
            var allScores = new List<RemoveFollowerExecutionResult>();

            foreach (var fp in allPlacements)
            {
                var score = SetRemoveFollower(state, fp.Color, fp.Coords);
                allScores.Add(score);
            }

            state.MovePhase = MoveWorkflow.GameEnded;

            return new EndGameExecutionResult(allScores);
        }

        /// <summary>
        /// Tries to end the game.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <returns>Game execution result.</returns>
        public EndGameExecutionResult TryEndGame(GameState state)
        {
            if (!IsCorrectMovePhase(state, MoveWorkflow.MoveEnded))
            {
                return new EndGameExecutionResult(RuleViolationType.InvalidMovePhase);
            }

            if (state.TileSupplier.RemainingCount > 0)
            {
                return new EndGameExecutionResult(RuleViolationType.TilesRemaining);
            }

            return SetEndGame(state);
        }

        /// <summary>
        /// Checks whether the specified player is on move.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="color">Color of the player.</param>
        /// <returns>True if the player is on move; otherwise false.</returns>
        protected bool IsPlayerOnMove(GameState state, PlayerColor color)
        {
            return state.Params.PlayerOrder[state.CurrentPlayerIndex] == color;
        }

        /// <summary>
        /// Checks whether it is the specified move phase.
        /// </summary>
        /// <param name="state">Current game state.</param>
        /// <param name="phase">Expected move phase.</param>
        /// <returns>True if it is the specified move phase; otherwise false.</returns>
        protected bool IsCorrectMovePhase(GameState state, MoveWorkflow phase)
        {
            return state.MovePhase == phase;
        }
    }
}
