namespace CarcassonneDiscovery.UserClient
{
    using System;
    using System.Collections.Generic;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Input;
    using System.Windows.Media;
    using CarcassonneDiscovery.Entity;
    using CarcassonneDiscovery.Logic;
    using CarcassonneDiscovery.Messaging;
    using CarcassonneDiscovery.Tools;

    /// <summary>
    /// Game board window.
    /// </summary>
    public partial class GameBoard : Window
    {
        /// <summary>
        /// Controller for this window.
        /// </summary>
        private UserClientController _ParentController;

        /// <summary>
        /// Color of the player.
        /// </summary>
        public PlayerColor PlayerColor { get; set; } = PlayerColor.None;

        /// <summary>
        /// Local game executor.
        /// </summary>
        private GameExecutor PrivateExecutor;

        /// <summary>
        /// Local game state.
        /// </summary>
        private GameState GameState;

        /// <summary>
        /// List of placed tiles.
        /// </summary>
        private Dictionary<Coords, TileRectangle> PlacedTiles = new Dictionary<Coords, TileRectangle>();

        /// <summary>
        /// List of empty tiles (neighbouring placed tiles).
        /// </summary>
        private Dictionary<Coords, EmptyTileRectangle> EmptyTiles = new Dictionary<Coords, EmptyTileRectangle>();

        /// <summary>
        /// List of positions where own follower is placed.
        /// </summary>
        private List<Coords> PlacedFollowerPositions = new List<Coords>();

        /// <summary>
        /// Current player on move.
        /// </summary>
        private PlayerColor CurrentOnMove = PlayerColor.None;

        /// <summary>
        /// Current tile scheme.
        /// </summary>
        private ITileScheme CurrentScheme;

        /// <summary>
        /// Current coordinates.
        /// </summary>
        private Coords CurrentCoords;

        /// <summary>
        /// List of players and their scores.
        /// </summary>
        private Dictionary<PlayerColor, ScoreBoardRecord> PlayerScoreRecords = new Dictionary<PlayerColor, ScoreBoardRecord>();

        /// <summary>
        /// Localization of the result messages.
        /// </summary>
        private RuleViolationTypeLocalization MsgLocalization = new RuleViolationTypeLocalization();

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="parentController">Controller for this window.</param>
        public GameBoard(UserClientController parentController)
        {
            InitializeComponent();

            _ParentController = parentController;

            Closed += (obj, ev) => _ParentController.GameBoardWindowClosed();
        }

        /// <summary>
        /// Starts the game board window.
        /// </summary>
        /// <param name="gameStartMsg">Message for game start.</param>
        public void Start(ServerResponse gameStartMsg)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                for (int i = 0; i < gameStartMsg.PlayerAmount; i++)
                {
                    ScoreBoardRecord record = new ScoreBoardRecord(gameStartMsg.PlayerNames[i], gameStartMsg.PlayerOrder[i])
                    {
                        Followers = gameStartMsg.FollowerAmount.Value
                    };
                    PlayerScoreRecords.Add(gameStartMsg.PlayerOrder[i], record);
                    ScoreBoardPanel.Children.Add(record);
                }

                InitializeGame(gameStartMsg);
            }));
        }

        /// <summary>
        /// Initializes the board.
        /// </summary>
        /// <param name="gameStartMsg">Message for game start.</param>
        public void InitializeGame(ServerResponse gameStartMsg)
        {
            PrivateExecutor = new GameExecutor();
            GameState = new GameState
            {
                Params = new GameParams
                {
                    FollowerAmount = gameStartMsg.FollowerAmount.Value,
                    PlayerAmount = gameStartMsg.PlayerAmount.Value,
                    PlayerOrder = gameStartMsg.PlayerOrder,
                    TileSetParams = new TileSetParams
                    {
                        Name = gameStartMsg.TileSetName
                    }
                }
            };
            ResolveGameEventMessage(gameStartMsg);
        }

        /// <summary>
        /// Resolved the game message.
        /// </summary>
        public void ResolveGameEventMessage(ServerResponse msg)
        {
            Dispatcher.Invoke(new Action(() =>
            {
                if (msg.ExitCode == ExitCode.Ok)
                {
                    lock (PrivateExecutor)
                    {
                        switch (msg.Type)
                        {
                            case ServerResponseType.StartGame:
                                var startGameResult = new StartGameResponse(msg).ExecutionResult;
                                PrivateExecutor.SetStartGame(GameState, startGameResult.FirstTile, startGameResult.FirstTileCoords, startGameResult.FirstTileOrientation);
                                PlaceTile(PlayerColor.None, startGameResult.FirstTile, startGameResult.FirstTileCoords, startGameResult.FirstTileOrientation);
                                break;

                            case ServerResponseType.StartMove:
                                var startMoveResult = new StartMoveResponse(msg).ExecutionResult;
                                PrivateExecutor.SetStartMove(GameState, startMoveResult.Color, startMoveResult.Tile);
                                MoveStart(startMoveResult.Tile, startMoveResult.Color);
                                break;

                            case ServerResponseType.PlaceTile:
                                var placeTileResult = new PlaceTileResponse(msg).ExecutionResult;
                                PrivateExecutor.SetPlaceTile(GameState, placeTileResult.Color, placeTileResult.Tile, placeTileResult.Coords, placeTileResult.Orientation);
                                PlaceTile(placeTileResult.Color, placeTileResult.Tile, placeTileResult.Coords, placeTileResult.Orientation);
                                break;

                            case ServerResponseType.PlaceFollower:
                                var placeFollowerResult = new PlaceFollowerResponse(msg).ExecutionResult;
                                PrivateExecutor.SetPlaceFollower(GameState, placeFollowerResult.Color, placeFollowerResult.Coords, placeFollowerResult.RegionId);
                                PlaceFollower(placeFollowerResult.Color, placeFollowerResult.Coords, placeFollowerResult.RegionId);
                                break;

                            case ServerResponseType.RemoveFollower:
                                var removeFollowerResult = new RemoveFollowerResponse(msg).ExecutionResult;
                                PrivateExecutor.SetRemoveFollower(GameState, removeFollowerResult.Color, removeFollowerResult.Coords, removeFollowerResult.Score);
                                RemoveFollower(removeFollowerResult.Color, removeFollowerResult.Coords, removeFollowerResult.Score);
                                break;

                            case ServerResponseType.PassMove:
                                var passMoveResult = new PassMoveResponse(msg).ExecutionResult;
                                PrivateExecutor.SetPassMove(GameState, passMoveResult.Color);
                                PassMove(passMoveResult.Color);
                                break;

                            case ServerResponseType.EndGame:
                                var endGameResult = new EndGameResponse(msg).ExecutionResult;
                                PrivateExecutor.SetEndGame(GameState);
                                foreach (var frMsg in endGameResult.FollowerRemovements)
                                {
                                    RemoveFollower(frMsg.Color, frMsg.Coords, frMsg.Score);
                                }
                                GameEnd("Game over.");
                                break;

                            default:
                                throw new Exception($"Got unexpected message: {msg}");
                        }

                    }
                }
                else if (msg.ExitCode == ExitCode.RuleViolationError)
                {
                    // TODO: Localization
                    MessageBox.Show(msg.RuleViolation.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    // TODO: Localization
                    MessageBox.Show("Unexpected: " + msg.ExitCode.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }));
        }

        /// <summary>
        /// Performs actions at the end of the game.
        /// </summary>
        /// <param name="msg">Message to be shown.</param>
        private void GameEnd(string msg)
        {
            // No clicking allowed
            foreach (var pt in PlacedTiles.Values)
            {
                pt.RegionMouseEventEnabled = false;
                pt.FollowerMouseEventEnabled = false;
            }

            foreach (var et in EmptyTiles.Values)
            {
                et.Visibility = Visibility.Collapsed;
            }

            MessageBox.Show(msg, "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// Performs actions when there is no follower placement.
        /// </summary>
        private void PassMove(PlayerColor color)
        {
            // Nothing to be done
        }

        /// <summary>
        /// Performs follower removement.
        /// </summary>
        /// <param name="color">Color of the follower.</param>
        /// <param name="coords">Coordinates of the follower.</param>
        private void RemoveFollower(PlayerColor color, Coords coords, int score)
        {
            PlacedTiles[coords].RemoveFollower();
            PlacedTiles[coords].FollowerMouseClick -= PlacedTile_FollowerMouseClick;

            if (color == PlayerColor)
            {
                PlacedFollowerPositions.Remove(coords);
            }

            PlayerScoreRecords[color].Followers++;
            PlayerScoreRecords[color].Score += score;
        }

        /// <summary>
        /// Performs follower placement.
        /// </summary>
        /// <param name="color">Color of the follower.</param>
        /// <param name="coords">Coordinates of the follower.</param>
        /// <param name="regionId">Id of region where follower is being placed.</param>
        private void PlaceFollower(PlayerColor color, Coords coords, int regionId)
        {
            PlacedTiles[coords].PlaceFollower(color, regionId);
            PlacedTiles[coords].FollowerMouseClick += PlacedTile_FollowerMouseClick;

            if (color == PlayerColor)
            {
                PlacedFollowerPositions.Add(coords);
            }

            PlayerScoreRecords[color].Followers--;
        }

        /// <summary>
        /// Performs tile placement.
        /// </summary>
        /// <param name="color">Color of the player making the move.</param>
        /// <param name="scheme">Scheme of the tile.</param>
        /// <param name="coords">Coordinates of the tile.</param>
        /// <param name="orientation">Orientation of the tile.</param>
        private void PlaceTile(PlayerColor color, ITileScheme scheme, Coords coords, TileOrientation orientation)
        {
            // Place the tile
            TileRectangle newRectangle = new TileRectangle();

            newRectangle.SetLayout(scheme, orientation);

            Canvas.SetLeft(newRectangle, coords.X * 100);
            Canvas.SetTop(newRectangle, coords.Y * 100);

            newRectangle.Coords = coords;

            TilesCanvas.Children.Add(newRectangle);

            PlacedTiles.Add(coords, newRectangle);

            // Remove empty tile and create new
            if (EmptyTiles.ContainsKey(coords))
            {
                TilesCanvas.Children.Remove(EmptyTiles[coords]);
                EmptyTiles.Remove(coords);
            }

            foreach (var or in new TileOrientation[] { TileOrientation.N, TileOrientation.E, TileOrientation.S, TileOrientation.W })
            {
                var neigh = coords.GetNeighboringCoords(or);

                if (!PlacedTiles.ContainsKey(neigh) && !EmptyTiles.ContainsKey(neigh))
                {
                    var emptyTile = new EmptyTileRectangle(neigh);
                    emptyTile.MouseClick += EmptyTile_MouseClick;
                    EmptyTiles.Add(neigh, emptyTile);

                    Canvas.SetLeft(emptyTile, neigh.X * 100);
                    Canvas.SetTop(emptyTile, neigh.Y * 100);

                    emptyTile.Visibility = Visibility.Collapsed;

                    TilesCanvas.Children.Add(emptyTile);
                }

                PassMoveBtn.IsEnabled = true;
            }

            CurrentCoords = coords;

            // If player is on move
            if (CurrentOnMove == PlayerColor)
            {
                // Activate the tile to be clicked on (placing follower)
                newRectangle.RegionMouseEventEnabled = true;
                newRectangle.RegionMouseClick += PlacedTile_RegionMouseClick;

                // Deactivate empty tiles
                foreach (var et in EmptyTiles.Values)
                {
                    et.MouseEventEnabled = false;
                    et.Visibility = Visibility.Collapsed;
                }

                // Activate the tiles with followers
                foreach (var c in PlacedFollowerPositions)
                {
                    PlacedTiles[c].FollowerMouseEventEnabled = true;
                }
            }
        }

        /// <summary>
        /// Performs action on start of the move.
        /// </summary>
        /// <param name="scheme">Scheme of the tile.</param>
        /// <param name="color">Color of the player making the move.</param>
        private void MoveStart(ITileScheme scheme, PlayerColor color)
        {
            CurrentTileRectangle.SetLayout(scheme, TileOrientation.N);

            // If player was not on move and now he is
            if (CurrentOnMove != PlayerColor && PlayerColor == color)
            {
                // Clicking enabled on empty places
                foreach (var r in EmptyTiles.Values)
                {
                    r.MouseEventEnabled = true;
                    r.Visibility = Visibility.Visible;
                }
            }
            // Else if player was on move and now he is not
            else if (CurrentOnMove == PlayerColor && PlayerColor != color)
            {
                PassMoveBtn.IsEnabled = false;

                if (PlacedTiles.TryGetValue(CurrentCoords, out TileRectangle currentTile))
                {
                    currentTile.RegionMouseEventEnabled = false;
                }

                foreach (var c in PlacedFollowerPositions)
                {
                    PlacedTiles[c].FollowerMouseEventEnabled = false;
                }
            }

            // Set on move
            foreach (var pair in PlayerScoreRecords)
            {
                pair.Value.OnMove = (pair.Key == color);
            }

            CurrentOnMove = color;
            CurrentScheme = scheme;
        }

        #region Grid movement events
        /// <summary>
        /// Previous point of mouse when moving.
        /// </summary>
        Point _LastMousePoint;

        /// <summary>
        /// Is the canvas currently being moved?
        /// </summary>
        bool _MovingCanvas = false;

        /// <summary>
        /// Current canvas zoom.
        /// </summary>
        double _Zoom = 1.0;

        private void TilesCanvasWrap_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            _LastMousePoint = e.GetPosition(this);
            _MovingCanvas = true;
        }

        private void TilesCanvasWrap_MouseMove(object sender, MouseEventArgs e)
        {
            if (_MovingCanvas)
            {
                var pt = e.GetPosition(this);
                var delta = pt - _LastMousePoint;

                Canvas.SetLeft(TilesCanvas, Canvas.GetLeft(TilesCanvas) + delta.X);
                Canvas.SetTop(TilesCanvas, Canvas.GetTop(TilesCanvas) + delta.Y);

                _LastMousePoint = pt;
            }
        }

        private void TilesCanvasWrap_MouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            _MovingCanvas = false;
        }

        private void TilesCanvasWrap_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            _Zoom *= Math.Pow(1.2, e.Delta / 40);

            TilesCanvas.LayoutTransform = new ScaleTransform(_Zoom, _Zoom);
        }
        #endregion

        #region Tiles request mouse events

        private void EmptyTile_MouseClick(Coords coords)
        {
            _ParentController.SendGameMessage(new ClientRequest()
            {
                Type = ClientRequestType.PlaceTile,
                Tile = new Tile(CurrentScheme),
                Color = PlayerColor,
                Coords = coords,
                Orientation = CurrentTileRectangle.Orientation
            });
        }

        private void PlacedTile_RegionMouseClick(Coords coords, int regionId)
        {
            _ParentController.SendGameMessage(new ClientRequest()
            {
                Type = ClientRequestType.PlaceFollower,
                Color = PlayerColor,
                Coords = coords,
                RegionId = regionId
            });
        }

        private void PlacedTile_FollowerMouseClick(Coords coords)
        {
            _ParentController.SendGameMessage(new ClientRequest()
            {
                Type = ClientRequestType.RemoveFollower,
                Color = PlayerColor,
                Coords = coords
            });
        }
        private void RotateLeftBtnClick(object sender, RoutedEventArgs e)
        {
            CurrentTileRectangle.RotateLayout(TileOrientation.W);
        }

        private void RotateRightBtnClick(object sender, RoutedEventArgs e)
        {
            CurrentTileRectangle.RotateLayout(TileOrientation.E);
        }

        private void PassMoveClick(object sender, RoutedEventArgs e)
        {
            _ParentController.SendGameMessage(new ClientRequest()
            {
                Type = ClientRequestType.PassMove,
                Color = PlayerColor
            });
        }
        #endregion

    }
}
