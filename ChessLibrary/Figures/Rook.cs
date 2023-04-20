using System;
using System.Collections.Generic;

namespace XXL.Chess
{
    public class Rook : Figure
    {
        public Rook(FigureColor color) : base(color, FigureConsoleRepresentation.R)
        {

        }
        public override List<(int, int)> GetLegalMoves(Cell currentCell, int currentMove)
        {
            List<(int, int)> legalMoves = new List<(int, int)>();
            bool collectLegalMoves(Cell nextCell)
            {
                if (nextCell.Figure != null && (!nextCell.Figure.IsMain || nextCell.Figure.Color == Color))
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

        public override List<(int, int)> GetLegalMovesForLinkCheck(Cell currentCell, int currentMove)
        {
            List<(int, int)> legalMoves = new List<(int, int)>();
            bool isFigureSkipped = false;
            bool collectLegalMoves(Cell nextCell)
            {
                if (nextCell.Figure != null && (nextCell.Figure.Color == Color || isFigureSkipped) && (!nextCell.Figure.IsMain || nextCell.Figure.Color == Color))
                {
                    if (nextCell.Figure.Color != Color)
                    {
                        legalMoves.Add(nextCell.Coordinates);
                    }
                    return true;
                }
                if (nextCell.Figure != null && nextCell.Figure.Color != Color) isFigureSkipped = true;
                legalMoves.Add(nextCell.Coordinates);
                return false;
            }
            IterateOverAttackedCells(currentCell, collectLegalMoves);
            return legalMoves;
        }

        public override void IterateOverAttackedCells(Cell currentCell, Func<Cell, bool> collect)
        {
            currentCell.IterateUp(collect);
            currentCell.IterateDown(collect);
            currentCell.IterateRight(collect);
            currentCell.IterateLeft(collect);
        }
    }
}