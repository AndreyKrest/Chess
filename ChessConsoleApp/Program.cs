using System.Collections.Generic;
using XXL.Chess;

class Program
{
    public static void Main()
    {
        Game game = new Game();
        List<(int, int)> legalMoves = game.GetLegalMoves((0, 0));
        game.DrawBoard();
        game.MakeMove((0, 0), (0, 7));
        game.DrawBoard();
        game.MakeMove((1, 0), (2, 2));
        game.DrawBoard();
        game.MakeMove((2, 7), (0, 5));
        game.DrawBoard();
        game.MakeMove((4, 7), (4, 0));
        game.DrawBoard();
        game.MakeMove((5, 0), (0, 5));
        game.DrawBoard();
        // foreach ((int, int) i in legalMoves)
        // {
        //     Console.WriteLine(i);
        // }
    }
}
