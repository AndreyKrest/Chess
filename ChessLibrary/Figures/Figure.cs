using System.Collections.Generic;

namespace XXL.Chess
{
  abstract class Figure
  {
    public FigureColor Color { get; } = FigureColor.White;
    public string ShortColor { get; } = "W";
    public FigureConsoleRepresentation FCR { get; }

    protected Figure(FigureColor color, FigureConsoleRepresentation fcr)
    {
      Color = color;
      ShortColor = color == FigureColor.White ? "W" : "B";
      FCR = fcr;
    }
    abstract public List<(int, int)> GetLegalMoves(Cell currentCell);
  }
}