using System;
using System.Collections.Generic;
using System.Linq;
#nullable enable

namespace XXL.Chess
{
    public class Game
    {
        private Player _activePlayer = Player.White;
        public Player ActivePlayer
        {
            get => _activePlayer;
        }
        private GameState _state = GameState.InProgress;
        public GameState State
        {
            get => _state;
        }

        private Board Board { get; } = new Board();

        public List<(int, int)> GetLegalMoves((int, int) coords)
        {
            Figure currentFigure = Board.GetFigure(coords);
            List<(int, int)> noMoves = new List<(int, int)>();

            if (currentFigure == null || currentFigure.Owner != _activePlayer || IsGameOver()) return noMoves;

            bool isKing = currentFigure is King;
            (int, int) activePlayerKingCoords = (-1, -1);
            List<(int, int)> moves = Board.GetLegalMoves(coords);
            if (IsCheck() && !isKing)
            {
                if (_state == GameState.DoubleCheck) return noMoves;

                Dictionary<(int, int), List<(int, int)>> nextPlayerMoves = Board.GetPlayerLegalMoves(GetNextActivePlayer());
                (int, int) attackerCoords = nextPlayerMoves
                    .Where(pair => pair.Value.Any(coords =>
                    {
                        Figure figure = Board.GetFigure(coords);
                        if (figure != null && figure.IsMain)
                        {
                            activePlayerKingCoords = coords;
                            return true;
                        }
                        return false;
                    }))
                    .Single()
                    .Key;
                Figure attacker = Board.GetFigure(attackerCoords);

                if (attacker is Knight || attacker is Pawn)
                {
                    moves = moves.Where(move => move == attackerCoords).ToList();
                }
                else
                {
                    if (activePlayerKingCoords.Item1 < 0 || activePlayerKingCoords.Item2 < 0)
                        throw new Exception("Cannot find main figure of the active player");
                    moves = moves
                        .Where(move => move == attackerCoords || currentFigure.CanDefendMainAt(move, attackerCoords, activePlayerKingCoords))
                        .ToList();
                }
            }
            else if (!isKing)
            {
                Dictionary<(int, int), List<(int, int)>> enemyLinkers =
                Board.GetPlayerLegalMovesForLinkCheck(GetNextActivePlayer()).Where(pair =>
                {
                    return pair.Value.Any(coords =>
                    {
                        Figure figure = Board.GetFigure(coords);
                        if (figure != null && figure.Color == currentFigure.Color && figure.IsMain)
                        {
                            activePlayerKingCoords = coords;
                            return true;
                        }
                        return false;
                    });
                }).ToDictionary(pair => pair.Key, pair => pair.Value);

                List<(int, int)> currentFigureLinkers = enemyLinkers.Keys
                    .Where(enemyFigureCoords => currentFigure.CanDefendMainAt(coords, enemyFigureCoords, activePlayerKingCoords))
                    .ToList();

                if (currentFigureLinkers.Count > 0)
                {
                    (int, int) linkerCoords = currentFigureLinkers[0];
                    moves = moves
                        .Where(move => move == linkerCoords || currentFigure.CanDefendMainAt(move, linkerCoords, activePlayerKingCoords))
                        .ToList();
                }
            }

            return moves;
        }

        public void MakeMove((int, int) currentCoords, (int, int) nextCoords)
        {
            _MakeMove(currentCoords, nextCoords, null);
        }

        public void MakeMove((int, int) currentCoords, (int, int) nextCoords, Type transformTo)
        {
            _MakeMove(currentCoords, nextCoords, transformTo);
        }

        private void _MakeMove((int, int) currentCoords, (int, int) nextCoords, Type? transformTo)
        {
            Figure figure = Board.GetFigure(currentCoords);
            if (figure == null || figure.Owner != _activePlayer || IsGameOver()) return;

            if (transformTo == null) Board.MoveFigure(currentCoords, nextCoords);
            else Board.MoveFigure(currentCoords, nextCoords, transformTo);

            _activePlayer = GetNextActivePlayer();
            List<(int, int)> nextPlayerMoves = Board.GetPlayerLegalMoves(GetNextActivePlayer()).Values.SelectMany(list => list).ToList();
            int mainFigureAttackersCount = nextPlayerMoves.Where(coords =>
            {
                Figure figure = Board.GetFigure(coords);
                return figure != null && figure.IsMain;
            }).Count();

            _state = GameState.InProgress;
            if (mainFigureAttackersCount == 1)
            {
                _state = GameState.Check;
            }
            else if (mainFigureAttackersCount == 2)
            {
                _state = GameState.DoubleCheck;
            }

            List<(int, int)> activePlayerMoves = Board.GetPlayerFiguresCoordinates(_activePlayer)
                .SelectMany(coords => GetLegalMoves(coords))
                .ToList();
            if (activePlayerMoves.Count == 0)
            {
                _state = _state == GameState.Check || _state == GameState.DoubleCheck ? GameState.Checkmate : GameState.Stalemate;
            }
        }

        public bool IsCheck()
        {
            return _state == GameState.Check || _state == GameState.DoubleCheck;
        }

        public bool IsGameOver()
        {
            return _state == GameState.Checkmate || _state == GameState.Stalemate;
        }

        public Dictionary<(int, int), string> GetFigurePositions()
        {
            return Board.GetFigurePositions();
        }

        public string DrawBoard()
        {
            return Board.DrawBoard();
        }

        private Player GetNextActivePlayer()
        {
            return _activePlayer == Player.White ? Player.Black : Player.White;
        }
    }
}