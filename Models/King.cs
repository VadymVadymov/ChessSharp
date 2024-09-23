using System;

namespace ChessSharp.Models
{
    public class King : ChessPiece
    {
        public King(string color) : base(color) { }

        public bool HasMoved = false;

        public override bool IsValidMove(Position start, Position end, ChessBoard chessBoard)
        {
            ChessPiece[,] board = chessBoard.Board;

            // King can move exactly one square in any direction
            if (IsDefending(start, end, chessBoard))
            {
                // The target square can either be empty or contain an opponent's piece
                if (board[end.X, end.Y] == null || board[end.X, end.Y].Color != this.Color)
                {
                    // After move simulation to check if the new position puts the King in check
                    if (!IsPositionUnderAttack(end, this.Color, chessBoard))
                    {
                        return true;
                    }
                }
            }


            return false; // If the move does not conform to the King's movement rules or places the King in check
        }

        public override bool IsDefending(Position start, Position end, ChessBoard chessBoard)
        {
            // The piece is not defending itself
            if (start == end)
                return false;

            // Calculate the absolute differences in x and y coordinates to check if the move is within one square
            int deltaX = Math.Abs(end.X - start.X);
            int deltaY = Math.Abs(end.Y - start.Y);

            return (deltaX == 1 || deltaY == 1) && (deltaX <= 1 && deltaY <= 1);
        }

        public bool CanCastle(ChessBoard chessBoard, Position end)
        {
            if (this.HasMoved || (end.X != 0 && end.X != 7) || (end.Y != 2 && end.Y != 6))
                return false;
            ChessPiece[,] board = chessBoard.Board;

            int y_axis = this.Color == "White" ? 7 : 0;
            Position kingsPosition = new Position(y_axis, 4);
            int vector = (end.Y > kingsPosition.Y) ? 1 : -1;
            int x_axis = vector == 1 ? 7 : 0;

            if (board[y_axis, x_axis] is Rook rook1 && !rook1.HasMoved)
            {
                Position square1 = new Position(kingsPosition.X, kingsPosition.Y + vector * 1);
                Position square2 = new Position(kingsPosition.X, kingsPosition.Y + vector * 2);
                if (board[square1.X, square1.Y] == null && board[square2.X, square2.Y] == null && (vector != -1 || board[x_axis, 1] == null))
                    return
                        !IsPositionUnderAttack(square1, this.Color, chessBoard);
            }

            return false;
        }

        // This method needs to simulate enemy moves and check if any can attack the given position
        private static bool IsPositionUnderAttack(Position position, string kingColor, ChessBoard chessBoard)
        {
            ChessPiece[,] board = chessBoard.Board;
            // Check all possible enemy moves that could reach the position
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    ChessPiece piece = board[i, j];
                    Position sourcePosition = new(i, j);
                    if (piece != null && piece.Color != kingColor)
                    {
                        bool tempBool = piece.IsDefending(sourcePosition, position, chessBoard);
                        if (tempBool)
                        {
                            // An enemy piece is defending the 'position', tedy it's under attack
                            return true;
                        }
                    }
                }
            }

            return false;
        }


    }
}
