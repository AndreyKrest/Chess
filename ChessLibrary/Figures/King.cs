using System.Linq;
using System.Collections.Generic;
using System;

namespace XXL.Chess
{
    public class King : Figure
    {
        public King(FigureColor color) : base(color, FigureConsoleRepresentation.K, true)
        {

        }
        public override List<(int, int)> GetLegalMoves(Cell currentCell, int currentMove)
        {
            List<Cell> enemyFigureCells = getEnemyFigureCells(currentCell);
            List<(int, int)> legalMoves = currentCell.Siblings
                .Where((pair) =>
                {
                    Cell sibling = pair.Value;
                    if (sibling.Figure == null)
                    {
                        bool isCellUnderAttack = enemyFigureCells.Any(cell =>
                        {
                            if (cell.Figure is King)
                            {
                                return cell.Siblings.ContainsValue(sibling);
                            }
                            return cell.Figure.GetLegalMoves(cell, currentMove).Contains(sibling.Coordinates);
                        });
                        return !isCellUnderAttack;
                    }

                    if (sibling.Figure.Color == Color) return false;

                    bool isFigureProtected = enemyFigureCells.Any(cell =>
                    {
                        if (cell.Figure is King)
                        {
                            return cell.Siblings.ContainsValue(sibling);
                        }
                        return cell.Figure.GetProtectedFigures(cell).Contains(sibling.Figure);
                    });
                    return !isFigureProtected;
                })
                .Select((pair) => pair.Value.Coordinates)
                .ToList();

            if (MovesHistory.Keys.Count == 0)
            {
                bool collectLegalCastles(Cell nextCell)
                {
                    bool isLastCell = nextCell.Coordinates.Item1 == 0 || nextCell.Coordinates.Item1 == 7;
                    if (!isLastCell) return nextCell.Figure != null;
                    if (nextCell.Figure != null && nextCell.Figure is Rook && nextCell.Figure.MovesHistory.Keys.Count == 0)
                    {
                        int offset = nextCell.Coordinates.Item1 == 0 ? -2 : 2;
                        legalMoves.Add((currentCell.Coordinates.Item1 + offset, currentCell.Coordinates.Item2));
                    }
                    return true;
                }
                currentCell.IterateLeft(collectLegalCastles);
                currentCell.IterateRight(collectLegalCastles);
            }

            return legalMoves;
        }

        public override void OnBeforeMove(Cell currentCell, Cell nextCell, int currentMove)
        {
            base.OnBeforeMove(currentCell, nextCell, currentMove);
            if (Math.Abs(currentCell.Coordinates.Item1 - nextCell.Coordinates.Item1) == 2)
            {
                bool isLeftCastling = nextCell.Coordinates.Item1 < currentCell.Coordinates.Item1;
                Cell rookCell = isLeftCastling
                    ? nextCell.GetCellByGlobalCoordinatesRecursively((0, nextCell.Coordinates.Item2))
                    : nextCell.GetCellByGlobalCoordinatesRecursively((7, nextCell.Coordinates.Item2));
                Cell targetCell = nextCell.GetSiblingCellByGlobalCoordinates(
                    (nextCell.Coordinates.Item1 + (isLeftCastling ? 1 : -1), nextCell.Coordinates.Item2)
                );
                targetCell.Figure = rookCell.Figure;
                rookCell.Figure = null;
            }
        }

        protected override void IterateOverAttackedCells(Cell currentCell, System.Func<Cell, bool> collect)
        {
            throw new Exception("This method should not be called");
        }

        private List<Cell> getEnemyFigureCells(Cell currentCell)
        {
            List<Cell> result = new List<Cell>();
            for (int y = 0; y < 7; y++)
            {
                for (int x = 0; x < 7; x++)
                {
                    (int, int) coords = (x, y);
                    Cell cell = currentCell.GetCellByGlobalCoordinatesRecursively(coords);
                    if (cell.Figure != null && cell.Figure.Color != Color)
                    {
                        result.Add(cell);
                    }
                }
            }
            return result;
        }
    }
}