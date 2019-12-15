namespace CarcassonneDiscovery.Tools
{
    using System.Collections.Generic;
    using System.Linq;
    using CarcassonneDiscovery.Entity;

    /// <summary>
    /// Extension methods for <see cref="GameState"/>
    /// </summary>
    public static class GameStateExtensions
    {
        /// <summary>
        /// Creates copy of game state.
        /// </summary>
        /// <param name="gameState">Game state.</param>
        /// <returns>Copy.</returns>
        public static GameState Copy(this GameState gameState)
        {
            var copy = new GameState
            {
                Params = new GameParams
                {
                    FollowerAmount = gameState.Params.FollowerAmount,
                    PlayerAmount = gameState.Params.PlayerAmount,
                    PlayerOrder = (PlayerColor[])gameState.Params.PlayerOrder.Clone(),
                    TileSet = gameState.Params.TileSet
                },
                MovePhase = gameState.MovePhase,
                Grid = new Dictionary<Coords, TilePlacement>(),
                PlacedFollowers = gameState.PlacedFollowers.Keys.ToDictionary(x => x, x => new List<FollowerPlacement>()),
                Scores = new Dictionary<PlayerColor, int>(gameState.Scores),
                CurrentTile = gameState.CurrentTile.Copy(),
                CurrentPlayerIndex = gameState.CurrentPlayerIndex,
                CurrentTileCoords = gameState.CurrentTileCoords,
                TileSupplier = null
            };

            foreach (var gridKV in gameState.Grid)
            {
                var gridV = gridKV.Value;
                var copyFP = Copy(gridV.FollowerPlacement);
                var val = new TilePlacement
                {
                    Coords = gridV.Coords,
                    Orientation = gridV.Orientation,
                    TileScheme = gridV.TileScheme.Copy(),
                    FollowerPlacement = copyFP
                };
                copy.Grid.Add(gridKV.Key, val);
                copy.PlacedFollowers[copyFP.Color].Add(copyFP);
            }

            return copy;
        }

        /// <summary>
        /// Creates copy of a follower placement.
        /// </summary>
        /// <param name="followerPlacement">Follower placement.</param>
        /// <returns>Copy.</returns>
        private static FollowerPlacement Copy(FollowerPlacement followerPlacement)
        {
            return new FollowerPlacement
            {
                Color = followerPlacement.Color,
                Coords = followerPlacement.Coords,
                RegionId = followerPlacement.RegionId
            };
        }
    }
}
