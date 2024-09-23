namespace ChessSharp.Models
{
    public class Pawn : ChessPiece
    {
        public Pawn(string color) : base(color) { }

        public override bool IsValidMove(Position start, Position end, ChessBoard chessBoard)
        {
            ChessPiece[,] board = chessBoard.Board;
            // Calculate direction factor based on the pawn's color
            int direction = Color == "White" ? -1 : 1;

            // Check forward move
            if (start.X + direction == end.X && start.Y == end.Y)
            {
                // Ensure the target square is unoccupied
                if (board[end.X, end.Y] == null)
                {
                    return true;
                }
            }

            // Check initial double forward move
            if ((Color == "White" && start.X == 6 || Color == "Black" && start.X == 1) &&
                start.X + 2 * direction == end.X && start.Y == end.Y)
            {
                // Ensure both target squares are unoccupied
                if (board[start.X + direction, start.Y] == null && board[end.X, end.Y] == null)
                {
                    return true;
                }
            }

            // Check diagonal capture
            if (start.X + direction == end.X && (start.Y + 1 == end.Y || start.Y - 1 == end.Y))
            {
                // Ensure the target square has an opposing piece
                if (board[end.X, end.Y] != null && board[end.X, end.Y].Color != this.Color)
                {
                    return true;
                }
            }

            return false;
        }

        public override bool IsDefending(Position start, Position end, ChessBoard chessBoard)
        {
            // The piece is not defending itself
            if (start == end)
                return false;

            ChessPiece[,] board = chessBoard.Board;
            int direction = this.Color == "White" ? -1 : 1;

            if (start.X + direction == end.X && (start.Y + 1 == end.Y || start.Y - 1 == end.Y))
            {
                return true;
            }
            return false;
        }

        // We need this method, as pawn is the only piece, which attack pattern is different from the moving pattern
        public bool CouldCapture(Position start, Position end, ChessBoard chessBoard)
        {
            ChessPiece[,] board = chessBoard.Board;

            // Calculate direction factor based on the pawn's color
            int direction = this.Color == "White" ? -1 : 1;

            if (start.X + direction == end.X && (start.Y + 1 == end.Y || start.Y - 1 == end.Y))
            {
                return true;
            }

            return false;
        }
    }
}
