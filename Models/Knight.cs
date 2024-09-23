using System;

namespace ChessSharp.Models
{
    public class Knight : ChessPiece
    {
        public Knight(string color) : base(color) { }

        public override bool IsValidMove(Position start, Position end, ChessBoard chessBoard)
        {
            ChessPiece[,] board = chessBoard.Board;

            if (IsDefending(start, end, chessBoard))
            {
                // The target square can either be empty or contain an opponent's piece
                if (board[end.X, end.Y] == null || board[end.X, end.Y].Color != this.Color)
                {
                    return true;
                }
            }

            return false; // If the move does not match the "L" shape pattern
        }

        public override bool IsDefending(Position start, Position end, ChessBoard chessBoard)
        {
            // The piece is not defending itself
            if (start == end)
                return false;
            // Calculate the absolute differences in x and y coordinates
            int deltaX = Math.Abs(end.X - start.X);
            int deltaY = Math.Abs(end.Y - start.Y);

            // Knight moves in an "L" shape: two squares in one direction and one square in a perpendicular direction
            return (deltaX == 2 && deltaY == 1) || (deltaX == 1 && deltaY == 2);
        }
    }
}

