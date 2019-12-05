namespace CarcassonneDiscovery.UserClient
{
    using System;
    using System.Windows;
    using CarcassonneDiscovery.CommunicationLibrary;
    using CarcassonneDiscovery.SimulationLibrary;

    /// <summary>
    /// Controller of the user client application.
    /// </summary>
    public class UserClientController
    {
        /// <summary>
        /// Current state of the client.
        /// </summary>
        public ClientState CurrentState { get; private set; }

        /// <summary>
        /// Socket on the client side.
        /// </summary>
        private ClientSocket _Socket;

        /// <summary>
        /// Window of the game board.
        /// </summary>
        private GameBoard _GameBoardWindow;

        /// <summary>
        /// Window of the waiting lobby.
        /// </summary>
        private WaitingLobby _WaitingLobbyWindow;

        /// <summary>
        /// Localization of the result messages.
        /// </summary>
        private GameExecutionResultLocalization MsgLocalization = new GameExecutionResultLocalization();

        /// <summary>
        /// Is the game resumed?
        /// </summary>
        private bool _ResumeMode = false;

        /// <summary>
        /// Default controller.
        /// </summary>
        public UserClientController()
        {
            CurrentState = ClientState.BEFORE_APPLICATION_START;

            _Socket = new ClientSocket();

            _WaitingLobbyWindow = new WaitingLobby(this);
            _GameBoardWindow = new GameBoard(this);

            _Socket.MessageReceived += HandleMessages;
            _Socket.ServerDisconnected += HandleDisconnection;
        }

        /// <summary>
        /// Starts the application.
        /// </summary>
        public void Start()
        {
            if (CurrentState != ClientState.BEFORE_APPLICATION_START)
            {
                throw new InvalidOperationException("Expected start of the application.");
            }

            CurrentState = ClientState.BEFORE_CONNECTING;

            _WaitingLobbyWindow.SetServerConnectingEnabled(true);
            _WaitingLobbyWindow.SetPlayerRegisteringEnabled(false);

            _WaitingLobbyWindow.Show();
        }

        #region Waiting lobby actions
        /// <summary>
        /// Tries to connect to the server.
        /// </summary>
        /// <param name="ip">IP address of the server.</param>
        /// <param name="port">Port of the server.</param>
        public void ConnectToServer(string ip, int port)
        {
            if (CurrentState != ClientState.BEFORE_CONNECTING)
            {
                throw new InvalidOperationException("Expected time to connect to the server");
            }

            _Socket.IP = ip;
            _Socket.Port = port;

            try
            {
                _Socket.Start();
            }
            catch
            {
                MessageBox.Show("Cannot connect to the server.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            CurrentState = ClientState.BEFORE_SIGNING;

            _WaitingLobbyWindow.SetServerConnectingEnabled(false);
            _WaitingLobbyWindow.SetPlayerRegisteringEnabled(true);
        }

        /// <summary>
        /// Sends connection message to the server.
        /// </summary>
        /// <param name="name">Name of the player.</param>
        /// <param name="color">Color of the player.</param>
        public void RegisterAs(string name, PlayerColor color)
        {
            _Socket.SendMessage(new PlayerRequest()
            {
                Type = PlayerRequestType.CONNECT,
                Name = name,
                Color = color
            });
        }
        #endregion

        /// <summary>
        /// Handles the messages.
        /// </summary>
        public void HandleMessages()
        {
            while (true)
            {
                ServerResponse msg = _Socket.GetNextMessage();

                if (msg == null)
                {
                    break;
                }

                switch (msg.Type)
                {
                    case ServerResponseType.EXECUTION_REQUEST_RESULT:
                        if (CurrentState == ClientState.GAME_IN_PLAY)
                        {
                            _GameBoardWindow.ResolveGameEventMessage(msg);
                        }
                        else if (CurrentState == ClientState.BEFORE_SIGNING && msg.Request != null && msg.Request.Type == PlayerRequestType.CONNECT)
                        {
                            if (msg.ExecutionResult == GameExecutionResult.OK)
                            {
                                _WaitingLobbyWindow.SetPlayerRegisteringEnabled(false);
                                CurrentState = ClientState.WAITING_FOR_START;
                                _GameBoardWindow.PlayerColor = msg.Request.Color;
                            }
                            else
                            {
                                MessageBox.Show(MsgLocalization[msg.ExecutionResult]);
                            }
                        }
                        else
                        {
                            throw new Exception("Received unexpected message.");
                        }

                        break;

                    case ServerResponseType.GAME_START:
                        CurrentState = ClientState.GAME_IN_PLAY;
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            _WaitingLobbyWindow.Close();
                            _GameBoardWindow.Start(msg.Parameters, msg.PlayerNames);
                            _GameBoardWindow.Show();
                        });
                        break;

                    case ServerResponseType.MOVE_START:
                    case ServerResponseType.TILE_PLACEMENT:
                    case ServerResponseType.FOLLOWER_PLACEMENT:
                    case ServerResponseType.FOLLOWER_REMOVEMENT:
                    case ServerResponseType.NO_FOLLOWER_PLACEMENT:
                        _GameBoardWindow.ResolveGameEventMessage(msg);
                        break;

                    case ServerResponseType.DISCONNECT:
                        HandleDisconnection();
                        break;

                    case ServerResponseType.GAME_END:
                        SocketForceClose();
                        CurrentState = ClientState.GAME_ENDED;
                        _GameBoardWindow.ResolveGameEventMessage(msg);
                        break;

                    case ServerResponseType.GAME_PAUSE:
                        SocketForceClose();
                        CurrentState = ClientState.GAME_ENDED;
                        _GameBoardWindow.ResolveGameEventMessage(msg);
                        break;

                    case ServerResponseType.GAME_HISTORY:
                        _ResumeMode = true;
                        break;

                    case ServerResponseType.GAME_RESUME:
                        _ResumeMode = false;
                        break;
                }
            }
        }

        /// <summary>
        /// Handles closing the waiting lobby window.
        /// </summary>
        public void WaitingLobbyWindowClosed()
        {
            switch (CurrentState)
            {
                case ClientState.BEFORE_CONNECTING:
                case ClientState.BEFORE_SIGNING:
                case ClientState.WAITING_FOR_START:
                    _Socket.SendMessage(new PlayerRequest() { Type = PlayerRequestType.DISCONNECT });
                    SocketForceClose();
                    Application.Current.Shutdown();
                    break;
            }
        }

        /// <summary>
        /// Handles closing the game board window.
        /// </summary>
        public void GameBoardWindowClosed()
        {
            switch (CurrentState)
            {
                case ClientState.GAME_IN_PLAY:
                    _Socket.SendMessage(new PlayerRequest() { Type = PlayerRequestType.DISCONNECT });
                    SocketForceClose();
                    Application.Current.Shutdown();
                    break;
                case ClientState.GAME_ENDED:
                    Application.Current.Shutdown();
                    break;
            }
        }

        /// <summary>
        /// Handles disconnection from the server.
        /// </summary>
        private void HandleDisconnection()
        {
            // Reset the socket
            SocketForceClose();
            _Socket = new ClientSocket();

            // Inform the player
            if (CurrentState == ClientState.BEFORE_SIGNING || CurrentState == ClientState.WAITING_FOR_START)
            {
                MessageBox.Show("Connection to the server is lost.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                _WaitingLobbyWindow.SetServerConnectingEnabled(true);
                _WaitingLobbyWindow.SetPlayerRegisteringEnabled(false);

                CurrentState = ClientState.BEFORE_CONNECTING;
            }
            else if (CurrentState == ClientState.GAME_IN_PLAY)
            {
                MessageBox.Show("Connection to the server is lost. Game has been aborted.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                CurrentState = ClientState.GAME_ENDED;
            }
        }

        /// <summary>
        /// Forces the socket to stop with no verbose.
        /// </summary>
        private void SocketForceClose()
        {
            try
            {
                _Socket.Stop();
            }
            catch
            {
            }
        }

        /// <summary>
        /// Sends message with game action request.
        /// </summary>
        /// <param name="msg">Request message.</param>
        public void SendGameMessage(PlayerRequest msg)
        {
            if (CurrentState == ClientState.GAME_IN_PLAY && !_ResumeMode)
            {
                _Socket.SendMessage(msg);
            }
        }
    }
}
