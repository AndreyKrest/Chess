using System;
using System.Collections.Generic;

namespace XXL.Chess
{
    class Pawn : Figure
    {
        public Pawn(FigureColor color) : base(color, FigureConsoleRepresentation.P)
        {

        }
        public override List<(int, int)> GetLegalMoves(Cell currentCell, int currentMove)
        {
            List<(int, int)> legalMoves = new List<(int, int)>();
            bool collectLegalMoves(Cell nextCell)
            {
                if (nextCell.Figure == null)
                {
                    bool isAtStart = currentCell.Coordinates.Item2 == 1 || currentCell.Coordinates.Item2 == 6;
                    if (!isAtStart)
                    {
                        legalMoves.Add(nextCell.Coordinates);
                        return true;
                    }
                    if (Math.Abs(nextCell.Coordinates.Item2 - currentCell.Coordinates.Item2) < 3)
                    {
                        legalMoves.Add(nextCell.Coordinates);
                        return false;
                    }
                }
                return true;
            }

            bool collectLegalCaptures(Cell nextCell)
            {
                if (nextCell.Figure != null && nextCell.Figure.Color != Color)
                {
                    legalMoves.Add(nextCell.Coordinates);
                }
                if ((currentCell.Coordinates.Item2 == 3 || currentCell.Coordinates.Item2 == 4))
                {
                    (int, int) targetCoords = (nextCell.Coordinates.Item1, currentCell.Coordinates.Item2);
                    Figure targetFigure = currentCell.GetSiblingCellByGlobalCoordinates(targetCoords).Figure;
                    if (targetFigure is Pawn && targetFigure.Color != Color && targetFigure.MovesHistory.ContainsKey(currentMove - 1) && targetFigure.MovesHistory.Keys.Count == 1)
                    {
                        legalMoves.Add(nextCell.Coordinates);
                    }
                }
                return true;
            }
            if (this.Color == FigureColor.White)
            {
                currentCell.IterateUp(collectLegalMoves);
                currentCell.IterateUpRight(collectLegalCaptures);
                currentCell.IterateUpLeft(collectLegalCaptures);
            }
            else
            {
                currentCell.IterateDown(collectLegalMoves);
                currentCell.IterateDownRight(collectLegalCaptures);
                currentCell.IterateDownLeft(collectLegalCaptures);
            }


            return legalMoves;
        }

        public override void OnBeforeMove(Cell currentCell, Cell nextCell, int currentMove)
        {
            base.OnBeforeMove(currentCell, nextCell, currentMove);
            if (currentCell.Coordinates.Item1 != nextCell.Coordinates.Item1 && nextCell.Figure == null)
            {
                (int, int) targetCoords = (nextCell.Coordinates.Item1, currentCell.Coordinates.Item2);
                nextCell.GetSiblingCellByGlobalCoordinates(targetCoords).Figure = null;
            }
        }
    }
}
