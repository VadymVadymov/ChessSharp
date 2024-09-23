namespace ChessSharp.Models
{
    public class Rook : ChessPiece
    {
        public Rook(string color) : base(color) { }

        // Crucial for castling logic
        public bool HasMoved = false;
        public override bool IsValidMove(Position start, Position end, ChessBoard chessBoard)
        {
            ChessPiece[,] board = chessBoard.Board;

            if (IsDefending(start, end, chessBoard))
            {
                // Final position can either be empty or contain an opponent's piece
                if (board[end.X, end.Y] == null || board[end.X, end.Y].Color != this.Color)
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

            // Rook moves either horizontally or vertically
            if (start.X == end.X || start.Y == end.Y)
            {
                // Determine direction of movement
                int stepX = start.X == end.X ? 0 : (end.X > start.X ? 1 : -1);
                int stepY = start.Y == end.Y ? 0 : (end.Y > start.Y ? 1 : -1);
                int currentX = start.X + stepX;
                int currentY = start.Y + stepY;

                // Check each square along the path
                while (currentX != end.X || currentY != end.Y)
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
                // If the move isn't strictly horizontal or vertical
                return false;
            }

            return true;
        }
    }
}


