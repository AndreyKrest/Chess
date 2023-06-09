using System;
using System.Collections.Generic;

namespace XXL.Chess
{
    public class Knight : Figure
    {
        private static (int, int)[] moveVariants = { (2, 1), (2, -1), (-2, 1), (-2, -1), (1, 2), (1, -2), (-1, 2), (-1, -2) };
        public Knight(FigureColor color) : base(color, FigureConsoleRepresentation.N)
        {

        }
        public override List<(int, int)> GetLegalMoves(Cell currentCell, int currentMove)
        {
            List<(int, int)> legalMoves = new List<(int, int)>();
            IterateOverAttackedCells(currentCell, (nextCell) =>
            {
                legalMoves.Add(nextCell.Coordinates);
                return true;
            });
            return legalMoves;
        }

        public override void IterateOverAttackedCells(Cell currentCell, Func<Cell, bool> collect)
        {
            foreach ((int, int) moveVariant in moveVariants)
            {
                (int, int) nextCoords = (moveVariant.Item1 + currentCell.Coordinates.Item1, moveVariant.Item2 + currentCell.Coordinates.Item2);
                if (!Board.IsInBoundaries(nextCoords))
                {
                    continue;
                }
                Cell nextCell = currentCell.GetCellByGlobalCoordinatesRecursively(nextCoords);
                if (nextCell.Figure != null && nextCell.Figure.Color == Color)
                {
                    continue;
                }
                collect(nextCell);
            }
        }
    }
}