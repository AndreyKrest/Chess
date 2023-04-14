using System;
using System.Collections.Generic;
using XXL.Chess;

class Program
{
    public static void Main()
    {
        Game game = new Game();
        List<(int, int)> legalMoves = game.GetLegalMoves((0, 0));
        Program.DrawBoard(game);
        game.MakeMove((0, 1), (0, 2));
        Program.DrawBoard(game);
        game.MakeMove((1, 1), (1, 3));
        Program.DrawBoard(game);
        game.MakeMove((1, 3), (1, 4));
        Program.DrawBoard(game);
        game.MakeMove((0, 6), (0, 4));
        Program.DrawBoard(game);
        game.MakeMove((1, 4), (0, 5));
        Program.DrawBoard(game);
        game.MakeMove((1, 6), (0, 5));
        Program.DrawBoard(game);


    }

    private static void DrawBoard(Game game)
    {
        Console.SetCursorPosition(0, 0);
        Console.Write(game.DrawBoard());
        Console.ReadKey();
    }
}
