namespace CarcassonneDiscovery.AiClient
{
    using System;
    using System.Linq;
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Logic;
    using CarcassonneDiscovery.Messaging;
    using CarcassonneDiscovery.Tools;

    /// <summary>
    /// Algorithm for deciding the best move.
    /// </summary>
    public class TrivialAiPlayer : IPlayer
    {
        /// <summary>
        /// Current game state.
        /// </summary>
        private readonly GameState _GameState = new GameState();

        /// <summary>
        /// Local game executor.
        /// </summary>
        private readonly GameExecutor _Executor = new GameExecutor();

        /// <summary>
        /// Grid search helper.
        /// </summary>
        private readonly GridSearch _GridSearch = new GridSearch();

        /// <summary>
        /// Random number generator.
        /// </summary>
        private readonly Random _Random = new Random();

        /// <inheritdoc />
        public PlayerColor Color { get; set; }

        /// <inheritdoc />
        public ClientRequest GetMove()
        {
            if (_GameState.MovePhase == MoveWorkflow.NextTileSelected)
            {
                return GetTileMove();
            }
            else if (_GameState.MovePhase == MoveWorkflow.TilePlaced)
            {
                return GetFollowerMove();
            }
            else
            {
                Console.WriteLine("Invalid move phase.");
                return null;
            }
        }

        /// <summary>
        /// Gets tile part of the move.
        /// </summary>
        /// <returns>Request for the move.</returns>
        private ClientRequest GetTileMove()
        {
            var tilePlacementPossibilities = _GridSearch.GetAllTilePlacements(_GameState.Grid, _GameState.CurrentTile).ToList();

            // Select one possibility
            var selected = tilePlacementPossibilities[_Random.Next(tilePlacementPossibilities.Count)];

            return new PlaceTileExecutionRequest
            {
                Color = Color,
                Coords = selected.Coords,
                Orientation = selected.Orientation,
                Tile = _GameState.CurrentTile
            }.ToServerResponse();
        }

        /// <summary>
        /// Gets follower part of the move.
        /// </summary>
        /// <returns>Request for the move.</returns>
        private ClientRequest GetFollowerMove()
        {
            var followerMove = new PassMoveExecutionRequest
            {
                Color = Color
            }.ToServerResponse();

            // Compute best move for followers
            int bestFollowerMoveValue = -1;

            // If can place
            if (_GameState.PlacedFollowers[Color].Count() < _GameState.Params.FollowerAmount)
            {
                for (int i = 0; i < _GameState.CurrentTile.RegionAmount; i++)
                {
                    if (!_GridSearch.IsRegionOccupied(_GameState.Grid, _GameState.CurrentTileCoords, i))
                    {
                        int newScore = _GridSearch.GetScoreForFollower(_GameState.Grid, _GameState.CurrentTileCoords, i, false);

                        if (newScore > bestFollowerMoveValue)
                        {
                            bestFollowerMoveValue = newScore;
                            followerMove = new PlaceFollowerExecutionRequest
                            {
                                Color = Color,
                                Coords = _GameState.CurrentTileCoords,
                                RegionId = i
                            }.ToServerResponse();
                        }
                    }
                }
            }

            // For all own placed followers
            foreach (var fp in _GameState.PlacedFollowers[Color])
            {
                int newScore = -1;

                switch (_GameState.Grid[fp.Coords].TileScheme.GetRegionType(fp.RegionId))
                {
                    case RegionType.Mountain:
                        if (_GridSearch.IsRegionClosed(_GameState.Grid, fp.Coords, fp.RegionId))
                        {
                            newScore = 2; // If placing is for at least 2, it is good! (else get out of here)
                        }
                        else if (_GridSearch.GetScoreForFollower(_GameState.Grid, fp.Coords, fp.RegionId, false) < 2)
                        {
                            newScore = 1; // If there are too few points, get out of here
                        }
                        else
                        {
                            newScore = -1; // Else you can wait with this follower
                        }

                        break;

                    case RegionType.Sea:
                    case RegionType.Grassland:
                        if (_GridSearch.IsRegionClosed(_GameState.Grid, fp.Coords, fp.RegionId))
                        {
                            newScore = 3; // If placing is for at least 3, it is good! (else get out of here)
                        }
                        else if (_GridSearch.GetScoreForFollower(_GameState.Grid, fp.Coords, fp.RegionId, false) < 2)
                        {
                            newScore = 1; // If there are too few points, get out of here
                        }
                        else
                        {
                            newScore = -1;
                        }

                        break;
                }

                if (newScore > bestFollowerMoveValue)
                {
                    bestFollowerMoveValue = newScore;
                    followerMove = new RemoveFollowerExecutionRequest
                    {
                        Color = Color,
                        Coords = fp.Coords
                    }.ToServerResponse();
                }
            }

            return followerMove;
        }

        /// <inheritdoc />
        public void PassMove(PassMoveResponse rsp)
        {
            var executionResult = rsp.ExecutionResult;
            _Executor.SetPassMove(_GameState, executionResult.Color);
        }

        /// <inheritdoc />
        public void PlaceFollower(PlaceFollowerResponse rsp)
        {
            var executionResult = rsp.ExecutionResult;
            _Executor.SetPlaceFollower(_GameState, executionResult.Color, executionResult.Coords, executionResult.RegionId);
        }

        /// <inheritdoc />
        public void PlaceTile(PlaceTileResponse rsp)
        {
            var executionResult = rsp.ExecutionResult;
            _Executor.SetPlaceTile(_GameState, executionResult.Color, executionResult.Tile, executionResult.Coords, executionResult.Orientation);
        }

        /// <inheritdoc />
        public void RemoveFollower(RemoveFollowerResponse rsp)
        {
            var executionResult = rsp.ExecutionResult;
            _Executor.SetRemoveFollower(_GameState, executionResult.Color, executionResult.Coords, executionResult.Score);
        }

        /// <inheritdoc />
        public void StartGame(StartGameResponse rsp)
        {
            var executionResult = rsp.ExecutionResult;
            _GameState.Params = executionResult.GameParams; // TODO?
            _Executor.SetStartGame(_GameState, executionResult.FirstTile, executionResult.FirstTileCoords, executionResult.FirstTileOrientation);
        }

        /// <inheritdoc />
        public void StartMove(StartMoveResponse rsp)
        {
            var executionResult = rsp.ExecutionResult;
            _Executor.SetStartMove(_GameState, executionResult.Color, executionResult.Tile);
        }
    }
}
