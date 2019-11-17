namespace CarcassonneDiscovery.Server
{
    using System;
    using CarcassonneDiscovery.Entity;

    public class GameSimulator
    {
        protected SimulationWorkflow State;

        #region Add or remove players
        public AddPlayerRequestResult AddPlayer(PlayerColor color)
        {
            if (State != SimulationWorkflow.BeforeGame)
            {
                return new AddPlayerRequestResult
                {
                    ExitCode = AddPlayerRequestExitCode.WrongSimulationState
                };
            }

            // TODO
            throw new NotImplementedException();
        }

        public RemovePlayerRequestResult RemovePlayer(PlayerColor color)
        {
            if (State != SimulationWorkflow.BeforeGame)
            {
                return new RemovePlayerRequestResult
                {
                    ExitCode = RemovePlayerRequestExitCode.WrongSimulationState
                };
            }

            // TODO
            throw new NotImplementedException();
        }
        #endregion

        #region Change game parameters
        #endregion

        #region Game actions
        public StartGameRequestResult StartGame()
        {
            if (State != SimulationWorkflow.BeforeGame)
            {
                return new StartGameRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            // TODO
            throw new NotImplementedException();

            State = SimulationWorkflow.InGame;
        }

        public StartMoveRequestResult StartMove()
        {
            if (State != SimulationWorkflow.InGame)
            {
                return new StartMoveRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            // TODO
            throw new NotImplementedException();
        }

        public PlaceTileRequestResult PlaceTile(PlayerColor color, ITileScheme tile, Coords coords, TileOrientation orientation)
        {
            if (State != SimulationWorkflow.InGame)
            {
                return new PlaceTileRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            // TODO
            throw new NotImplementedException();
        }

        public PlaceFollowerRequestResult PlaceFollower(PlayerColor color, Coords coords, int regionId)
        {
            if (State != SimulationWorkflow.InGame)
            {
                return new PlaceFollowerRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            // TODO
            throw new NotImplementedException();
        }

        public RemoveFollowerRequestResult RemoveFollower(PlayerColor color, Coords coords)
        {
            if (State != SimulationWorkflow.InGame)
            {
                return new RemoveFollowerRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            // TODO
            throw new NotImplementedException();
        }

        public PassMoveRequestResult PassMove(PlayerColor color)
        {
            if (State != SimulationWorkflow.InGame)
            {
                return new PassMoveRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            // TODO
            throw new NotImplementedException();
        }

        public EndGameRequestResult EndGame()
        {
            if (State != SimulationWorkflow.InGame)
            {
                return new EndGameRequestResult
                {
                    ExitCode = GameExecutionRequestExitCode.WrongSimulationState
                };
            }

            // TODO
            throw new NotImplementedException();
        }
        #endregion

        public void Run()
        {

        }
    }
}
