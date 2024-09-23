using System;

namespace ChessSharp.Models
{
    public class Queen : ChessPiece
    {
        public Queen(string color) : base(color) { }

        public override bool IsValidMove(Position start, Position end, ChessBoard chessBoard)
        {
            ChessPiece[,] board = chessBoard.Board;

            if (IsDefending(start, end, chessBoard))
            {
                // The destination square can either be empty or contain an opponent'
                return board[end.X, end.Y] == null || board[end.X, end.Y].Color != this.Color;
            }
            return false;
        }

        public override bool IsDefending(Position start, Position end, ChessBoard chessBoard)
        {
            // The piece is not defending itself
            if (start == end)
                return false;

            ChessPiece[,] board = chessBoard.Board;

            int deltaX = Math.Abs(end.X - start.X);
            int deltaY = Math.Abs(end.Y - start.Y);

            // Check for diagonal movement (like a bishop)
            if (deltaX == deltaY)
            {
                int stepX = end.X > start.X ? 1 : -1;
                int stepY = end.Y > start.Y ? 1 : -1;
                int currentX = start.X + stepX;
                int currentY = start.Y + stepY;

                while (currentX != end.X && currentY != end.Y)
                {
                    if (board[currentX, currentY] != null)
                    {
                        return false;
                    }
                    currentX += stepX;
                    currentY += stepY;
                }
            }
            // Check for straight movement (like a rook)
            else if (start.X == end.X || start.Y == end.Y)
            {
                int stepX = start.X == end.X ? 0 : (end.X > start.X ? 1 : -1);
                int stepY = start.Y == end.Y ? 0 : (end.Y > start.Y ? 1 : -1);
                int currentX = start.X + stepX;
                int currentY = start.Y + stepY;

                while (currentX != end.X || currentY != end.Y)
                {
                    if (board[currentX, currentY] != null)
                    {
                        return false;
                    }
                    currentX += stepX;
                    currentY += stepY;
                }
            }
            else
            {
                return false; // The move is neither straight nor diagonal
            }

            // The square is defended by the Quenn
            return true;
        }
    }

}
