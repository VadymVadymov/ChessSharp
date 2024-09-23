namespace ChessSharp.Models
{
    public abstract class ChessPiece
    {
        public string Color { get; private set; }
        public abstract bool IsValidMove(Position start, Position end, ChessBoard board);
        public abstract bool IsDefending(Position start, Position end, ChessBoard board);

        protected ChessPiece(string color)
        {
            Color = color;
        }
    }
}
