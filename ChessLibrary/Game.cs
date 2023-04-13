using System;
using System.Collections.Generic;

namespace XXL.Chess
{
    public class Game
    {
        Board Board { get; }

        public Game()
        {
            Board = new Board();

            // Init
        }
        public List<(int, int)> GetLegalMoves((int, int) coords)
        {
            return Board.GetLegalMoves(coords);
        }

        public void MakeMove((int, int) currentCoords, (int, int) nextCoords)
        {
            Board.MoveFigure(currentCoords, nextCoords);
        }

        public Dictionary<(int, int), string> GetFigurePositions()
        {
            return Board.GetFigurePositions();
        }

        public string DrawBoard()
        {
            return Board.DrawBoard();
        }
    }
}