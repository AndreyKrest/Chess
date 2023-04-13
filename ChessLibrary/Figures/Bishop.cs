using System;
using System.Collections.Generic;

namespace XXL.Chess
{
  class Bishop : Figure
  {
    public Bishop(FigureColor color) : base(color, FigureConsoleRepresentation.B)
    {

    }
    public override List<(int, int)> GetLegalMoves(Cell currentCell)
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
      return legalMoves;
    }
  }
}