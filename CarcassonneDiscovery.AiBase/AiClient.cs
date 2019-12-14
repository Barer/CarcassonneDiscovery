namespace CarcassonneDiscovery.AiClient
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CarcassonneDiscovery.Messaging;

    /// <summary>
    /// AI client logic.
    /// </summary>
    public class AiClient
    {
        /// <summary>
        /// Client communication socket.
        /// </summary>
        private ClientSocket _Socket;

        /// <summary>
        /// Player logic.
        /// </summary>
        private IPlayer _Player;

        /// <summary>
        /// Has game ended?
        /// </summary>
        private bool _GameEnd = false;

        /// <summary>
        /// Is player on move?
        /// </summary>
        private bool _OnMove = false;

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <param name="player">AI player object.</param>
        public AiClient(IPlayer player)
        {
            _Socket = new ClientSocket();
            _Player = player;

            _Socket.MessageReceived += HandleMessages;
        }

        /// <summary>
        /// Handles received messages.
        /// </summary>
        private void HandleMessages()
        {
            while (!_GameEnd)
            {
                var msg = _Socket.GetNextMessage();

                if (msg == null)
                {
                    break;
                }

                Console.WriteLine($"Received message: {msg.Type}");

                switch (msg.Type)
                {
                    case ServerResponseType.StartGame:
                        var startGameRsp = new StartGameResponse(msg);
                        _Player.StartGame(startGameRsp);
                        break;

                    case ServerResponseType.PlaceFollower:
                        var placeFollowerRsp = new PlaceFollowerResponse(msg);
                        CheckIfOk(placeFollowerRsp.ExitCode);
                        _Player.PlaceFollower(placeFollowerRsp);
                        break;

                    case ServerResponseType.RemoveFollower:
                        var removeFollowerRsp = new RemoveFollowerResponse(msg);
                        CheckIfOk(removeFollowerRsp.ExitCode);
                        _Player.RemoveFollower(removeFollowerRsp);
                        break;

                    case ServerResponseType.EndGame:
                        _GameEnd = true;
                        break;

                    case ServerResponseType.StartMove:
                        var startMoveRsp = new StartMoveResponse(msg);
                        _Player.StartMove(startMoveRsp);

                        _OnMove = startMoveRsp.ExecutionResult.Color == _Player.Color;

                        if (_OnMove)
                        {
                            var timeOut = new Task(() => { Thread.Sleep(2000); });
                            timeOut.Start();

                            Console.WriteLine("On move: tile part");
                            var tilePlacement = _Player.GetMove();

                            timeOut.Wait();

                            Console.WriteLine($"Sent message: {tilePlacement.Type}");
                            _Socket.SendMessage(tilePlacement);
                        }

                        break;

                    case ServerResponseType.PassMove:
                        var passMoveRsp = new PassMoveResponse(msg);
                        CheckIfOk(passMoveRsp.ExitCode);
                        _Player.PassMove(passMoveRsp);
                        break;

                    case ServerResponseType.PlaceTile:
                        var placeTileRsp = new PlaceTileResponse(msg);
                        CheckIfOk(placeTileRsp.ExitCode);
                        _Player.PlaceTile(placeTileRsp);

                        if (_OnMove)
                        {
                            var timeOut = new Task(() => { Thread.Sleep(2000); });
                            timeOut.Start();

                            Console.WriteLine("On move: follower part");
                            var tilePlacement = _Player.GetMove();

                            timeOut.Wait();

                            Console.WriteLine($"Sent message: {tilePlacement.Type}");
                            _Socket.SendMessage(tilePlacement);
                        }

                        break;

                    case ServerResponseType.AddPlayer:
                        var addPlayerRsp = new AddPlayerResponse(msg);
                        CheckIfOk(addPlayerRsp.ExitCode);
                        _Player.Color = addPlayerRsp.Color;
                        break;
                }
            }
        }

        /// <summary>
        /// Runs the client.
        /// </summary>
        /// <param name="args">Argument of the AI client.</param>
        public void Run(AiArguments args)
        {
            // Set parameters
            _Socket.IP = args.IP;
            _Socket.Port = args.Port;
            _Player.Color = args.Color;

            // Start socket
            _Socket.Start();

            // Try to sign as the player
            _Socket.SendMessage(new ClientRequest()
            {
                Type = ClientRequestType.JoinAsPlayer,
                Name = args.Name,
                Color = args.Color
            });

            _GameEnd = false;
            while (!_GameEnd)
            {
                Thread.Sleep(1000);
            }

            _Socket.Stop();
        }

        /// <summary>
        /// Checks if result of execution is ok.
        /// </summary>
        /// <param name="exitCode">Exit code of the response.</param>
        private void CheckIfOk(ExitCode exitCode)
        {
            if (exitCode != ExitCode.Ok)
            {
                Console.WriteLine("Invalid result.");
            }
        }
    }
}
