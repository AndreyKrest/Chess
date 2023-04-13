using System;
using System.Collections.Generic;

namespace XXL.Chess
{
  class Rook : Figure
  {
    public Rook(FigureColor color) : base(color, FigureConsoleRepresentation.R)
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
      currentCell.IterateUp(collectLegalMoves);
      currentCell.IterateDown(collectLegalMoves);
      currentCell.IterateRight(collectLegalMoves);
      currentCell.IterateLeft(collectLegalMoves);
      return legalMoves;
    }
  }
}