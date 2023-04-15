using System.Collections.Generic;

namespace XXL.Chess
{
    public class Game
    {
        Player _activePlayer = Player.White;
        public Player ActivePlayer
        {
            get => _activePlayer;
        }
        Board Board { get; } = new Board();

        public List<(int, int)> GetLegalMoves((int, int) coords)
        {
            Figure figure = Board.GetFigure(coords);
            if (figure != null && figure.Owner != _activePlayer)
            {
                return new List<(int, int)>();
            }
            return Board.GetLegalMoves(coords);
        }

        public void MakeMove((int, int) currentCoords, (int, int) nextCoords)
        {
            Figure figure = Board.GetFigure(currentCoords);
            if (figure != null && figure.Owner != _activePlayer)
            {
                return;
            }
            Board.MoveFigure(currentCoords, nextCoords);
            ChangeActivePlayer();
        }

        public Dictionary<(int, int), string> GetFigurePositions()
        {
            return Board.GetFigurePositions();
        }

        public string DrawBoard()
        {
            return Board.DrawBoard();
        }

        private void ChangeActivePlayer()
        {
            _activePlayer = _activePlayer == Player.White ? Player.Black : Player.White;
        }
    }
}