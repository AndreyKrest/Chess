using System;
using System.Collections.Generic;
using System.Linq;

namespace XXL.Chess
{
    class Board
    {
        private static readonly Dictionary<(int, int), Figure> defaultFigures = new Dictionary<(int, int), Figure>() {
            { (0, 1), new Pawn(FigureColor.White) },
            { (1, 1), new Pawn(FigureColor.White) },
            { (2, 1), new Pawn(FigureColor.White) },
            { (3, 1), new Pawn(FigureColor.White) },
            { (4, 1), new Pawn(FigureColor.White) },
            { (5, 1), new Pawn(FigureColor.White) },
            { (6, 1), new Pawn(FigureColor.White) },
            { (7, 1), new Pawn(FigureColor.White) },
            { (0, 6), new Pawn(FigureColor.Black) },
            { (1, 6), new Pawn(FigureColor.Black) },
            { (2, 6), new Pawn(FigureColor.Black) },
            { (3, 6), new Pawn(FigureColor.Black) },
            { (4, 6), new Pawn(FigureColor.Black) },
            { (5, 6), new Pawn(FigureColor.Black) },
            { (6, 6), new Pawn(FigureColor.Black) },
            { (7, 6), new Pawn(FigureColor.Black) },

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
            { (3, 0), new Queen(FigureColor.White) },
            { (3, 7), new Queen(FigureColor.Black) },
            { (4, 0), new King(FigureColor.White) },
            { (4, 7), new King(FigureColor.Black) },
        };
        private int currentMove = 0;
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
            return GetCell(coords).GetFigureLegalMoves(currentMove);
        }

        public void MoveFigure((int, int) currentCoords, (int, int) nextCoords)
        {
            Cell currentCell = GetCell(currentCoords);
            Cell nextCell = GetCell(nextCoords);
            if (!currentCell.GetFigureLegalMoves(currentMove).Contains(nextCoords))
            {
                throw new Exception("The figure can't move to the provided coordinates");
            }
            currentCell.Figure.OnBeforeMove(currentCell, nextCell, currentMove);
            nextCell.Figure = currentCell.Figure;
            currentCell.Figure = null;
            currentMove++;
        }

        public void MoveFigure((int, int) currentCoords, (int, int) nextCoords, Type transformTo)
        {
            Cell currentCell = GetCell(currentCoords);
            if (!currentCell.Figure.GetTransformationTypes().Contains(transformTo))
            {
                throw new Exception($"Cannot transform figure at {currentCoords} to type {transformTo}");
            }
            MoveFigure(currentCoords, nextCoords);
            Cell nextCell = GetCell(nextCoords);
            nextCell.Figure = nextCell.Figure.TransformTo(transformTo);
        }

        public Figure GetFigure((int, int) coords)
        {
            return GetCell(coords).Figure;
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

        public string DrawBoard()
        {
            string horizontalLine = new string('—', 25) + "\n";
            string result = horizontalLine;
            for (int y = 7; y >= 0; y--)
            {
                result += "|";
                for (int x = 0; x < 8; x++)
                {
                    Cell cell = GetCell((x, y));
                    result += $"{GetFigureConsoleRepresentation(cell.Figure)}|";
                }
                result += "\n" + horizontalLine;
            }
            return result;
        }

        private string GetFigureConsoleRepresentation(Figure figure)
        {
            if (figure == null) return "  ";
            return (figure.ShortColor + figure.FCR) switch
            {
                "BK" => "\u2654 ",
                "BQ" => "\u2655 ",
                "BR" => "\u2656 ",
                "BB" => "\u2657 ",
                "BN" => "\u2658 ",
                "BP" => "\u2659 ",
                "WK" => "\u265A ",
                "WQ" => "\u265B ",
                "WR" => "\u265C ",
                "WB" => "\u265D ",
                "WN" => "\u265E ",
                "WP" => "\u265F",
                _ => " "
            };
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