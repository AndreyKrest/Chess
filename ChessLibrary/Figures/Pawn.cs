using System;
using System.Collections.Generic;

namespace XXL.Chess
{
    class Pawn : Figure
    {
        public Pawn(FigureColor color) : base(color, FigureConsoleRepresentation.P)
        {

        }
        public override List<(int, int)> GetLegalMoves(Cell currentCell)
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
                    if (nextCell.Coordinates.Item2 - currentCell.Coordinates.Item2 < 3)
                    {
                        legalMoves.Add(nextCell.Coordinates);
                        return false;
                    }

                }


                return true;
            }
            currentCell.IterateUp(collectLegalMoves);

            return legalMoves;
        }
    }
}
