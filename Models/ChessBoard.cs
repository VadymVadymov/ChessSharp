namespace ChessSharp.Models
{
    public class ChessBoard
    {
        public ChessPiece[,] Board { get; private set; }

        public ChessBoard(ChessPiece[,] GivenBoard, bool mustInitialize)
        {
            // 8x8 board
            Board = GivenBoard;
            if (mustInitialize)
            {
                InitializeBoard();
            }
        }

        private void InitializeBoard()
        {
            // Initialize pawns
            for (int i = 0; i < 8; i++)
            {
                Board[1, i] = new Pawn("Black");
                Board[6, i] = new Pawn("White");
            }

            // Initialize rooks
            Board[0, 0] = new Rook("Black");
            Board[0, 7] = new Rook("Black");
            Board[7, 0] = new Rook("White");
            Board[7, 7] = new Rook("White");

            // Initialize knights
            Board[0, 1] = new Knight("Black");
            Board[0, 6] = new Knight("Black");
            Board[7, 1] = new Knight("White");
            Board[7, 6] = new Knight("White");

            // Initialize bishops
            Board[0, 2] = new Bishop("Black");
            Board[0, 5] = new Bishop("Black");
            Board[7, 2] = new Bishop("White");
            Board[7, 5] = new Bishop("White");

            // Initialize queens
            Board[0, 3] = new Queen("Black");
            Board[7, 3] = new Queen("White");

            // Initialize kings
            Board[0, 4] = new King("Black");
            Board[7, 4] = new King("White");
        }
    }
}