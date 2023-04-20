using System;
using System.Collections.Generic;

namespace XXL.Chess
{
    public abstract class Figure
    {
        public Player Owner { get; } = Player.White;
        public FigureColor Color { get; } = FigureColor.White;
        public string ShortColor { get; } = "W";
        public FigureConsoleRepresentation FCR { get; }
        public bool IsMain { get; } = false;
        public Dictionary<int, (int, int)> MovesHistory { get; } = new Dictionary<int, (int, int)>();

        protected Figure(FigureColor color, FigureConsoleRepresentation fcr)
        {
            Owner = color == FigureColor.White ? Player.White : Player.Black;
            Color = color;
            ShortColor = color == FigureColor.White ? "W" : "B";
            FCR = fcr;
        }

        protected Figure(FigureColor color, FigureConsoleRepresentation fcr, bool isMain) : this(color, fcr)
        {
            IsMain = isMain;
        }
        public abstract List<(int, int)> GetLegalMoves(Cell currentCell, int currentMove);

        public virtual List<(int, int)> GetLegalMovesForLinkCheck(Cell currentCell, int currentMove)
        {
            return GetLegalMoves(currentCell, currentMove);
        }

        public abstract void IterateOverAttackedCells(Cell currentCell, Func<Cell, bool> collect);

        public virtual List<Figure> GetProtectedFigures(Cell currentCell)
        {
            List<Figure> legalMoves = new List<Figure>();
            bool collectProtectedFigures(Cell nextCell)
            {
                if (nextCell.Figure != null && nextCell.Figure.Color == Color)
                {
                    legalMoves.Add(nextCell.Figure);
                    return true;
                }
                return false;
            }
            IterateOverAttackedCells(currentCell, collectProtectedFigures);
            return legalMoves;
        }

        public bool CanDefendMainAt((int, int) move, (int, int) attackerCoords, (int, int) kingCoords)
        {
            bool isBetween = move.Item1 >= attackerCoords.Item1 && move.Item2 >= attackerCoords.Item2
                && move.Item1 <= kingCoords.Item1 && move.Item2 <= kingCoords.Item2
                || move.Item1 <= attackerCoords.Item1 && move.Item2 <= attackerCoords.Item2
                && move.Item1 >= kingCoords.Item1 && move.Item2 >= kingCoords.Item2;
            if (!isBetween) return false;
            return (
                attackerCoords.Item1 * (move.Item2 - kingCoords.Item2)
                + move.Item1 * (kingCoords.Item2 - attackerCoords.Item2)
                + kingCoords.Item1 * (attackerCoords.Item2 - move.Item2)
            ) == 0;
        }

        public virtual void OnBeforeMove(Cell currentCell, Cell nextCell, int currentMove)
        {
            if (MovesHistory.ContainsKey(currentMove))
            {
                throw new Exception($"Same figure moved twice. Last move: {currentCell.Coordinates} -> {nextCell.Coordinates}");
            }
            MovesHistory.Add(currentMove, nextCell.Coordinates);
        }

        public virtual Figure TransformTo(Type transformTo)
        {
            throw new Exception("Figure cannot be transformed");
        }

        public virtual Type[] GetTransformationTypes()
        {
            return new Type[0];
        }
    }
}