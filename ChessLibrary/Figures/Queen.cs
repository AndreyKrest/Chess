using System;
using System.Collections.Generic;

namespace XXL.Chess
{
    public class Queen : Figure
    {
        public Queen(FigureColor color) : base(color, FigureConsoleRepresentation.Q)
        {

        }
        public override List<(int, int)> GetLegalMoves(Cell currentCell, int currentMove)
        {
            List<(int, int)> legalMoves = new List<(int, int)>();
            bool collectLegalMoves(Cell nextCell)
            {
                if (nextCell.Figure != null)
                {
                    if (nextCell.Figure.Color != Color)
                    {
                        legalMoves.Add(nextCell.Coordinates);
                    }
                    return true;
                }
                legalMoves.Add(nextCell.Coordinates);
                return false;
            }
            currentCell.IterateUpRight(collectLegalMoves);
            currentCell.IterateUpLeft(collectLegalMoves);
            currentCell.IterateDownRight(collectLegalMoves);
            currentCell.IterateDownLeft(collectLegalMoves);
            currentCell.IterateUp(collectLegalMoves);
            currentCell.IterateDown(collectLegalMoves);
            currentCell.IterateRight(collectLegalMoves);
            currentCell.IterateLeft(collectLegalMoves);
            return legalMoves;
        }
    }
}