using System;
using System.Collections.Generic;
using System.Linq;

namespace XXL.Chess
{
  class Board
  {
    private static readonly Dictionary<(int, int), Figure> defaultFigures = new Dictionary<(int, int), Figure>() {
        { (0, 0), new Rook(FigureColor.White) },
        { (7, 0), new Rook(FigureColor.White) },
        { (0, 7), new Rook(FigureColor.Black) },
        { (7, 7), new Rook(FigureColor.Black) },
        { (1, 0), new Knight(FigureColor.White) },
        { (6, 0), new Knight(FigureColor.White) },
        { (1, 7), new Knight(FigureColor.Black) },
        { (6, 7), new Knight(FigureColor.Black) },
        { (2, 0), new Bishop(FigureColor.White) },
        { (5, 0), new Bishop(FigureColor.White) },
        { (2, 7), new Bishop(FigureColor.Black) },
        { (5, 7), new Bishop(FigureColor.Black) },
        { (4, 0), new Queen(FigureColor.White) },
        { (4, 7), new Queen(FigureColor.Black) },
    };
    private readonly Dictionary<(int, int), Cell> cells = new Dictionary<(int, int), Cell>();

    public Board()
    {
      initCells();
      linkCells();
    }

    public static void AssertBoundaries((int, int) coords)
    {
      if (!IsInBoundaries(coords))
      {
        throw new Exception("Invalid coordinates");
      }
    }

    public static bool IsInBoundaries((int, int) coords)
    {
      return (coords.Item1 >= 0 && coords.Item1 <= 7 && coords.Item2 >= 0 && coords.Item2 <= 7);
    }

    public List<(int, int)> GetLegalMoves((int, int) coords)
    {
      return GetCell(coords).GetFigureLegalMoves();
    }

    public void MoveFigure((int, int) currentCoords, (int, int) nextCoords)
    {
      Cell currentCell = GetCell(currentCoords);
      Cell nextCell = GetCell(nextCoords);
      if (!currentCell.GetFigureLegalMoves().Contains(nextCoords))
      {
        throw new Exception("The figure can't move to the provided coordinates");
      }
      nextCell.Figure = currentCell.Figure;
      currentCell.Figure = null;
    }

    public Dictionary<(int, int), string> GetFigurePositions()
    {
      return cells.Where((item) => item.Value.Figure != null).Select((item) =>
      {
        return new KeyValuePair<(int, int), string>(
          item.Key,
          item.Value.Figure.ShortColor + item.Value.Figure.FCR.ToString()
        );
      })
      .ToDictionary((item) => item.Key, (item) => item.Value);
    }

    private void initCells()
    {
      for (int x = 0; x < 8; x++)
      {
        for (int y = 0; y < 8; y++)
        {
          (int, int) coords = (x, y);
          Board.defaultFigures.TryGetValue(coords, out Figure figure);
          cells.Add(coords, new Cell(figure, coords));
        }
      }
    }

    private void linkCells()
    {
      for (int y = 0; y < 8; y++)
      {
        for (int x = 0; x < 8; x++)
        {
          (int, int) coords = (x, y);
          Cell currentCell = cells[coords];
          for (int i = -1; i <= 1; i++)
          {
            int x1 = x + i;
            if (x1 < 0 || x1 > 7) continue;
            for (int j = -1; j <= 1; j++)
            {
              int y1 = y + j;
              if (y1 < 0 || y1 > 7 || (i == 0 && j == 0)) continue;
              (int, int) siblingCoords = (i, j);
              currentCell.Siblings.Add(siblingCoords, cells[(x1, y1)]);
            }
          }
        }
      }
    }

    public void DrawBoard()
    {
      Console.WriteLine(new string('—', 41));
      for (int y = 7; y >= 0; y--)
      {
        Console.Write("|");
        for (int x = 0; x < 8; x++)
        {
          Cell cell = GetCell((x, y));
          Console.Write(cell.Figure == null ? "    |" : $" {cell.Figure.ShortColor}{cell.Figure.FCR} |");
        }
        Console.WriteLine();
        Console.WriteLine(new string('—', 41));
      }

    }

    private Cell GetCell((int, int) coords)
    {
      try
      {
        return cells[coords];
      }
      catch (KeyNotFoundException)
      {
        throw new Exception($"Can't find cell by coordinates: {coords}");
      }
    }
  }
}