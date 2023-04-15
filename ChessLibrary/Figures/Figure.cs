using System.Collections.Generic;

namespace XXL.Chess
{
    abstract class Figure
    {
        public Player Owner { get; } = Player.White;
        public FigureColor Color { get; } = FigureColor.White;
        public string ShortColor { get; } = "W";
        public FigureConsoleRepresentation FCR { get; }
        public Dictionary<int, (int, int)> MovesHistory { get; } = new Dictionary<int, (int, int)>();

        protected Figure(FigureColor color, FigureConsoleRepresentation fcr)
        {
            Owner = color == FigureColor.White ? Player.White : Player.Black;
            Color = color;
            ShortColor = color == FigureColor.White ? "W" : "B";
            FCR = fcr;
        }
        abstract public List<(int, int)> GetLegalMoves(Cell currentCell, int currentMove);

        public virtual void OnBeforeMove(Cell currentCell, Cell nextCell, int currentMove)
        {
            MovesHistory.Add(currentMove, nextCell.Coordinates);
        }
    }
}