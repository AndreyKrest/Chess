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
            IterateOverAttackedCells(currentCell, collectLegalMoves);
            return legalMoves;
        }

        protected override void IterateOverAttackedCells(Cell currentCell, Func<Cell, bool> collect)
        {
            currentCell.IterateUpRight(collect);
            currentCell.IterateUpLeft(collect);
            currentCell.IterateDownRight(collect);
            currentCell.IterateDownLeft(collect);
            currentCell.IterateUp(collect);
            currentCell.IterateDown(collect);
            currentCell.IterateRight(collect);
            currentCell.IterateLeft(collect);
        }
    }
}