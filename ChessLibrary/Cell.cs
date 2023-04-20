using System;
using System.Collections.Generic;

namespace XXL.Chess
{
    public class Cell
    {
        public Figure Figure { get; set; }
        public (int, int) Coordinates { get; }
        public Dictionary<(int, int), Cell> Siblings { get; } = new Dictionary<(int, int), Cell>();

        public Cell(Figure figure, (int, int) coords)
        {
            Coordinates = coords;
            this.Figure = figure;
        }
        public List<(int, int)> GetFigureLegalMoves(int currentMove)
        {
            if (Figure == null)
            {
                throw new Exception("Can't get figure legal moves because cell is empty");
            }
            return Figure.GetLegalMoves(this, currentMove);
        }
        public Cell GetSiblingCellByGlobalCoordinates((int, int) coords)
        {
            Board.AssertBoundaries(coords);
            int x = coords.Item1 - Coordinates.Item1;
            int y = coords.Item2 - Coordinates.Item2;
            if (x < -1 || x > 1 || y < -1 || y > 1)
            {
                throw new Exception("Passed coordinates are not point to any sibling of the current cell");
            }
            return Siblings[(x, y)];
        }

        public Cell GetCellByGlobalCoordinatesRecursively((int, int) coords)
        {
            Board.AssertBoundaries(coords);
            if (coords == Coordinates) return this;

            int xOffset = coords.Item1 - Coordinates.Item1;
            int yOffset = coords.Item2 - Coordinates.Item2;
            int xLocalOffset = xOffset == 0 ? 0 : xOffset > 0 ? 1 : -1;
            int yLocalOffset = yOffset == 0 ? 0 : yOffset > 0 ? 1 : -1;
            try
            {
                return GetSiblingCellByGlobalCoordinates(coords);
            }
            catch (Exception)
            {
                return Siblings[(xLocalOffset, yLocalOffset)].GetCellByGlobalCoordinatesRecursively(coords);
            }
        }

        public void IterateUp(Func<Cell, bool> func)
        {
            Iterate((coords) => (coords.Item1, coords.Item2 + 1), (coords) => coords.Item2 < 7, func);
        }

        public void IterateDown(Func<Cell, bool> func)
        {
            Iterate((coords) => (coords.Item1, coords.Item2 - 1), (coords) => coords.Item2 > 0, func);
        }

        public void IterateRight(Func<Cell, bool> func)
        {
            Iterate((coords) => (coords.Item1 + 1, coords.Item2), (coords) => coords.Item1 < 7, func);
        }

        public void IterateLeft(Func<Cell, bool> func)
        {
            Iterate((coords) => (coords.Item1 - 1, coords.Item2), (coords) => coords.Item1 > 0, func);
        }

        public void IterateUpRight(Func<Cell, bool> func)
        {
            Iterate((coords) => (coords.Item1 + 1, coords.Item2 + 1), (coords) => coords.Item1 < 7 && coords.Item2 < 7, func);
        }

        public void IterateUpLeft(Func<Cell, bool> func)
        {
            Iterate((coords) => (coords.Item1 - 1, coords.Item2 + 1), (coords) => coords.Item1 > 0 && coords.Item2 < 7, func);
        }

        public void IterateDownRight(Func<Cell, bool> func)
        {
            Iterate((coords) => (coords.Item1 + 1, coords.Item2 - 1), (coords) => coords.Item1 < 7 && coords.Item2 > 0, func);
        }

        public void IterateDownLeft(Func<Cell, bool> func)
        {
            Iterate((coords) => (coords.Item1 - 1, coords.Item2 - 1), (coords) => coords.Item1 > 0 && coords.Item2 > 0, func);
        }

        private void Iterate(Func<(int, int), (int, int)> getNextCoordinate, Func<(int, int), bool> predicate, Func<Cell, bool> func)
        {
            Cell nextCell = this;
            (int, int) nextCoords = Coordinates;
            while (predicate(nextCoords))
            {
                nextCoords = getNextCoordinate(nextCoords);
                nextCell = nextCell.GetSiblingCellByGlobalCoordinates(nextCoords);
                bool shouldStopIterating = func(nextCell);
                if (shouldStopIterating) break;
            };
        }
    }
}