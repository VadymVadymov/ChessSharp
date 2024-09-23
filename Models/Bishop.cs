using System;

namespace ChessSharp.Models
{
    public class Bishop : ChessPiece
    {
        public Bishop(string color) : base(color) { }

        public override bool IsValidMove(Position start, Position end, ChessBoard chessBoard)
        {
            ChessPiece[,] board = chessBoard.Board;
            // Bishop moves diagonally, so the absolute differences must be equal
            if (IsDefending(start, end, chessBoard))
            {
                // Final position can either be empty or contain an opponent's piece
                if (board[end.X, end.Y] == null || board[end.X, end.Y].Color != this.Color)
                {
                    return true;
                }
            }

            return false; // If the move isn't strictly diagonal
        }

        public override bool IsDefending(Position start, Position end, ChessBoard chessBoard)
        {
            ChessPiece[,] board = chessBoard.Board;

            // The piece is not defending itself
            if (start == end)
                return false;

            if (Math.Abs(end.X - start.X) == Math.Abs(end.Y - start.Y))
            {
                int stepX = end.X > start.X ? 1 : -1;
                int stepY = end.Y > start.Y ? 1 : -1;
                int currentX = start.X + stepX;
                int currentY = start.Y + stepY;

                // Check each square along the path
                while (currentX != end.X && currentY != end.Y)
                {
                    // If any square is occupied, the path is blocked
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
                return false;
            }

            return true;
        }
    }
}
