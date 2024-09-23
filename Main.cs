using ChessSharp.Models;
using ChessSharp.Views;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessSharp
{
    public partial class Main : Form
    {
        Label lblWhiteTimer;
        Label lblBlackTimer;
        Timer whiteTimer;
        Timer blackTimer;
        int whiteTimeLeft;
        int blackTimeLeft;

        private ChessBoard chessBoard;
        private ChessPieceControl selectedPiece = null;
        private string whoseTurn = "White";

        private static readonly Color lightColor = Color.Beige;
        private static readonly Color darkColor = Color.Brown;
        private static readonly Color borderColor = Color.SaddleBrown;
        private static readonly Color darkGray = Color.DarkGray;
        private static readonly Color lightGray = Color.LightGray;

        private static readonly int cellSize = 80;
        private static readonly int offset = 20;
        private static readonly int boardGlobalOffset = 12;

        public Main()
        {
            InitializeComponent();
            this.ClientSize = new Size(cellSize * 8 + offset * 5, cellSize * 8 + offset * 9);
            ChessPiece[,] board = new ChessPiece[8, 8];
            chessBoard = new ChessBoard(board, true);
            InitializeChessBoard(offset, cellSize);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = Color.LightGoldenrodYellow;
            InitializeTimers();
            StartTimer("White");
            ShadowTimer("Black");
        }

        private void InitializeChessBoard(int offset, int cellSize)
        {
            BoardBorder chessBoardPanel = new BoardBorder
            {
                Location = new Point(offset, offset),  // Position it to leave space for labels
                Size = new Size(cellSize * 8 + offset * 3, cellSize * 8 + offset * 3),  // Set the size to hold all chess pieces
                BorderThickness = 4,
                BorderColor = Color.Maroon,
                BackColor = Color.Linen,
                Name = "chessBoardPanel"
            };
            this.Controls.Add(chessBoardPanel);

            BoardBorder innerBorder = new BoardBorder
            {
                Location = new Point(offset + boardGlobalOffset - 2, offset + boardGlobalOffset - 2),
                Size = new Size(cellSize * 8 + 4, cellSize * 8 + 4),
                BorderThickness = 2,
                BorderColor = borderColor
            };
            chessBoardPanel.Controls.Add(innerBorder);
            chessBoardPanel.Controls.SetChildIndex(innerBorder, 3);

            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    var pieceControl = new ChessPieceControl
                    {
                        PieceIndex = DeterminePieceIndex(i, j),
                        Location = new Point(j * cellSize + offset + boardGlobalOffset, i * cellSize + offset + boardGlobalOffset),
                        Size = new Size(cellSize, cellSize),
                        Tag = new Position(i, j),
                        BackColor = CalculateColor(i, j),
                        Piece = chessBoard.Board[i, j],
                        Name = "piece" + i.ToString() + j.ToString()
                    };
                    pieceControl.Click += PieceControl_Click;
                    chessBoardPanel.Controls.Add(pieceControl);
                    chessBoardPanel.Controls.SetChildIndex(pieceControl, 0);
                }
                AddLabels(offset, cellSize, chessBoardPanel);
            }
        }

        private void InitializeTimers()
        {
            whiteTimeLeft = blackTimeLeft = 15 * 60; // 15 minutes 

            lblWhiteTimer = new Label
            {
                Text = $"{whiteTimeLeft / 60}:00",
                Location = new Point(offset, cellSize * 8 + offset * 5), // Adjust location according to your layout
                Size = new Size(200, 50),
                BackColor = Color.Ivory,
                ForeColor = Color.Black,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 24, FontStyle.Bold),
                BorderStyle = BorderStyle.FixedSingle
            };
            this.Controls.Add(lblWhiteTimer);

            lblBlackTimer = new Label
            {
                Text = $"{whiteTimeLeft / 60}:00",
                Location = new Point(cellSize * 6 + offset * 2, cellSize * 8 + offset * 5), // Adjust location accordingly
                Size = new Size(200, 50),
                BackColor = Color.Black,
                ForeColor = Color.Ivory,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Arial", 24, FontStyle.Bold),
                BorderStyle = BorderStyle.FixedSingle,
            };
            this.Controls.Add(lblBlackTimer);


            whiteTimer = new Timer { Interval = 1000 };
            blackTimer = new Timer { Interval = 1000 };

            whiteTimer.Tick += new EventHandler(WhiteTimer_Tick);
            blackTimer.Tick += new EventHandler(BlackTimer_Tick);
        }

        private (int, string) DeterminePieceIndex(int row, int column)
        {
            int idx = -1;
            string color = "";

            if (row == 0 || row == 7)
            {
                color = (row == 0) ? "Black" : "White";
                // Positions of specific pieces in the first and last rows
                switch (column)
                {
                    case 0:
                    case 7:
                        idx = 4; // Rooks
                        break;
                    case 1:
                    case 6:
                        idx = 3; // Knights
                        break;
                    case 2:
                    case 5:
                        idx = 2; // Bishops
                        break;
                    case 3:
                        idx = 1; // Queen 
                        break;
                    case 4:
                        idx = 0; // King 
                        break;
                }
            }
            else if (row == 1 || row == 6)
            {
                color = (row == 1) ? "Black" : "White";
                idx = 5; // Pawns
            }

            // If not a specific piece's position, return -1 or an index for an empty space
            return (idx, color);
        }

        private static void AddLabels(int offset, int cellSize, BoardBorder boardBorder)
        {
            Label label;
            int labelSize = 30;  // Increased size for better visibility

            // Font settings
            Font labelFont = new Font("Arial", 10, FontStyle.Bold); // Larger and bolder font

            // Adding numbers for rows on the left side only
            for (int i = 0; i < 8; i++)
            {
                label = new Label
                {
                    Text = (8 - i).ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(boardGlobalOffset, i * cellSize + offset + (cellSize - labelSize) / 2 + boardGlobalOffset),
                    Size = new Size(offset / 2, labelSize),
                    Font = labelFont
                };
                boardBorder.Controls.Add(label);
            }

            // Adding letters for columns on the bottom side only
            string columns = "abcdefgh";
            for (int j = 0; j < 8; j++)
            {
                label = new Label
                {
                    Text = columns[j].ToString(),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Location = new Point(j * cellSize + offset + boardGlobalOffset, offset + 8 * cellSize + boardGlobalOffset),
                    Size = new Size(cellSize, offset),
                    Font = labelFont
                };
                boardBorder.Controls.Add(label);
            }
        }

        private void PieceControl_Click(object sender, EventArgs e)
        {
            ChessPieceControl clickedPiece = sender as ChessPieceControl;
            if (clickedPiece != null)
            {
                HandlePieceSelection(clickedPiece);
            }
        }

        private void WhiteTimer_Tick(object sender, EventArgs e)
        {
            UpdateTimer(lblWhiteTimer, ref whiteTimeLeft);
        }

        private void BlackTimer_Tick(object sender, EventArgs e)
        {
            UpdateTimer(lblBlackTimer, ref blackTimeLeft);
        }

        private void UpdateTimer(Label timerLabel, ref int timeLeft)
        {
            if (timeLeft > 0)
            {
                timeLeft--;
                int minutes = timeLeft / 60;
                int seconds = timeLeft % 60;
                timerLabel.Text = $"{minutes:00}:{seconds:00}";
            }
            else
            {
                string winner = whoseTurn == "White" ? "Black" : "White";
                EndTheGame(winner);
            }
        }

        private void HandlePieceSelection(ChessPieceControl clickedPiece)
        {
            if (selectedPiece == null)
            {
                // Select the piece if none is currently selected and the square is not empty
                if (clickedPiece.Image != null && clickedPiece.Piece.Color == whoseTurn)
                {
                    selectedPiece = clickedPiece;
                    HighlightPiece(selectedPiece);
                }
            }
            else if (selectedPiece.PieceIndex.Item2 == clickedPiece.PieceIndex.Item2)
            {
                DeHighlightPiece(selectedPiece);
                HighlightPiece(clickedPiece);
                selectedPiece = clickedPiece;
            }
            else
            {
                // Move the selected piece to the new location if valid
                Position start = (Position)selectedPiece.Tag;
                Position end = (Position)clickedPiece.Tag;
                if (IsValidMove(selectedPiece, clickedPiece, start, end))
                {
                    // We should check that the resulting position does not contain a check to the king
                    ChessBoard mockChessBoard = new ChessBoard((ChessPiece[,])chessBoard.Board.Clone(), false);
                    mockChessBoard.Board[end.X, end.Y] = mockChessBoard.Board[start.X, start.Y];
                    mockChessBoard.Board[start.X, start.Y] = null;
                    if (!IsKingInCheck(whoseTurn, mockChessBoard))
                    {
                        // Crucial information for castling
                        if (selectedPiece.Piece is King king && king.CanCastle(chessBoard, end))
                        {

                            Position rookPosition = (start.Y < end.Y) ? new(start.X, 7) : new(start.X, 0);
                            int vector = (start.Y < end.Y) ? 1 : -1;
                            BoardBorder boardBorder = (BoardBorder)this.Controls.Find("chessBoardPanel", false)[0];
                            ChessPieceControl rookControl =
                                (ChessPieceControl)boardBorder.Controls.Find("piece" + rookPosition.X.ToString() + rookPosition.Y.ToString(), false)[0];
                            ChessPieceControl rookFinalSquare =
                                (ChessPieceControl)boardBorder.Controls.Find("piece" + start.X.ToString() + (start.Y + vector).ToString(), false)[0];
                            if (rookControl.Piece is Rook rook)
                                rook.HasMoved = true;
                            MovePiece(rookControl, rookFinalSquare);
                            MovePiece(selectedPiece, clickedPiece);

                            king.HasMoved = true;
                        }
                        else
                        {
                            if (selectedPiece.Piece is Rook rook)
                                rook.HasMoved = true;
                            MovePiece(selectedPiece, clickedPiece);
                        }
                        DeHighlightPiece(selectedPiece);
                        selectedPiece = null;
                        PassTheTurn();
                        if (IsKingInCheck(whoseTurn, chessBoard))
                        {
                            if (IsMate(chessBoard))
                            {
                                string winner = (whoseTurn == "White") ? "Black" : "White";
                                EndTheGame(winner);
                            }
                        }
                        else if (IsStalemate(chessBoard))
                            EndTheGame("Draw");

                    }
                }
                else
                {
                    // Optionally provide feedback on invalid move
                    DeHighlightPiece(selectedPiece);
                    selectedPiece = null;  // Deselect the piece
                }
            }
        }

        public static bool IsKingInCheck(string kingColor, ChessBoard chessBoard)
        {
            ChessPiece[,] Board = chessBoard.Board;
            Position? kingPosition = null;
            string opponentColor = kingColor == "White" ? "Black" : "White";

            // Getting the position of he king
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (Board[i, j] is King king && king.Color == kingColor)
                    {
                        kingPosition = new Position(i, j);
                        break;
                    }

                }
                if (kingPosition is not null)
                    break;
            }

            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    ChessPiece piece = Board[i, j];
                    if (piece != null && piece.Color == opponentColor)
                    {
                        Position startPosition = new Position(i, j);

                        if (kingPosition is null)
                            // This should never be called, but best practice is not leaving a warning
                            throw new Exception("In 'IsKingInCheck': couldn't find kings position");
                        // Check if any piece attacks a potential opposite color king's square
                        else if (piece.IsDefending(startPosition, kingPosition, chessBoard))
                        {
                            return true;
                        }
                    }

                }
            }
            return false;
        }

        private static Color CalculateColor(int i, int j)
        {
            return (i + j) % 2 == 0 ? lightColor : darkColor;
        }

        private static void HighlightPiece(ChessPieceControl piece)
        {
            piece.BackColor = Color.Gold;  // Example highlighting by changing background color
        }

        private void DeHighlightPiece(ChessPieceControl piece)
        {
            Position pos = (Position)piece.Tag;
            piece.BackColor = CalculateColor(pos.X, pos.Y);
        }


        private bool IsValidMove(ChessPieceControl from, ChessPieceControl to, Position start, Position end)
        {
            if (from.Piece != null)
            {
                if (from.Piece is King king)
                    return king.CanCastle(chessBoard, end) || king.IsValidMove(start, end, chessBoard);
                else
                    return from.Piece.IsValidMove(start, end, chessBoard);
            }
            return false;
        }

        private void MovePiece(ChessPieceControl from, ChessPieceControl to)
        {
            Position start = (Position)from.Tag;
            Position end = (Position)to.Tag;

            if (chessBoard.Board[start.X, start.Y] is Pawn pawn && (end.X == 0 || end.X == 7))
                makePromotion(from, to, pawn.Color);
            else
            {

                // Move the piece in the logical board
                chessBoard.Board[end.X, end.Y] = chessBoard.Board[start.X, start.Y];
                chessBoard.Board[start.X, start.Y] = null;

                // Update the UI
                to.PieceIndex = from.PieceIndex;
                to.Piece = from.Piece;
                to.Image = from.Image;
                to.Cursor = Cursors.Hand;

                from.PieceIndex = (-1, "");
                from.Piece = null;
                from.Image = null;
            }
        }

        private void makePromotion(ChessPieceControl from, ChessPieceControl to, string color)
        {
            Position start = (Position)from.Tag;
            Position end = (Position)to.Tag;
            using (var promotionForm = new PromotionForm(cellSize, color)
            {
                ClientSize = new Size(cellSize * 2, cellSize * 2),
                ControlBox = false,
                Text = ""
            })
            {
                var result = promotionForm.ShowDialog();
                if (result == DialogResult.OK)
                {
                    (ChessPiece chosenPiece, int pieceIndex) = promotionForm.ChosenPiece;
                    chessBoard.Board[end.X, end.Y] = chosenPiece;
                    chessBoard.Board[start.X, start.Y] = null;

                    to.PieceIndex = (pieceIndex, chosenPiece.Color);
                    to.Piece = chosenPiece;
                    to.Cursor = Cursors.Hand;

                    from.PieceIndex = (-1, "");
                    from.Piece = null;
                }
            }
        }

        private bool IsMate(ChessBoard chessBoard)
        {
            ChessPiece[,] board = chessBoard.Board;
            string kingsColor = whoseTurn;
            Position? kingsPosition = null;
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (board[i, j] is King kings && kings.Color == kingsColor)
                    {
                        kingsPosition = new Position(i, j);
                        break;
                    }
                    if (kingsPosition is not null)
                        break;
                }
            }
            if (kingsPosition is null)
                throw new Exception($"'isMate' couldn't find {kingsColor} king on the board");

            King king = (King)board[kingsPosition.X, kingsPosition.Y];
            for (int i = Math.Max(kingsPosition.X - 1, 0); i <= kingsPosition.X + 1; ++i)
            {
                for (int j = Math.Max(kingsPosition.Y - 1, 0); j <= kingsPosition.Y + 1; ++j)
                {
                    Position end = new Position(i, j);
                    if (end == kingsPosition)
                        break;
                    if (king.IsValidMove(kingsPosition, end, chessBoard))
                        return false;
                }
            }
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (!CheckAllMoves(chessBoard, i, j, kingsColor))
                        return false;
                }
            }
            return true;

        }

        private bool IsStalemate(ChessBoard chessBoard)
        {
            ChessPiece[,] board = chessBoard.Board;
            string kingsColor = whoseTurn;
            Position? kingsPosition = null;
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    if (board[i, j] is King king && king.Color == kingsColor)
                    {
                        kingsPosition = new Position(i, j);
                        break;
                    }
                    if (kingsPosition is not null)
                        break;
                }
            }
            if (kingsPosition is null)
                throw new Exception($"'isStalemate' couldn't find {kingsColor} king on the board");
            for (int i = 0; i < 8; ++i)
            {
                for (int j = 0; j < 8; ++j)
                {
                    ChessPiece piece = board[i, j];
                    if (piece is not null)
                    {
                        Position start = new Position(i, j);
                        for (int k = 0; k < 8; ++k)
                        {
                            for (int l = 0; l < 8; ++l)
                            {
                                Position end = new Position(k, l);
                                if (start != end && piece.Color == kingsColor &&
                                    piece.IsValidMove(start, end, chessBoard))
                                    return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        private bool CheckAllMoves(ChessBoard chessBoard, int x, int y, string kingsColor)
        {
            ChessPiece[,] board = chessBoard.Board;
            if (board[x, y] != null && board[x, y].Color == kingsColor && board[x, y] is not King)
            {
                ChessPiece piece = board[x, y];
                Position start = new Position(x, y);
                for (int k = 0; k < 8; ++k)
                {
                    for (int l = 0; l < 8; ++l)
                    {
                        Position end = new Position(k, l);
                        if (piece.IsValidMove(start, end, chessBoard))
                        {
                            ChessBoard mockChessBoard = new ChessBoard((ChessPiece[,])chessBoard.Board.Clone(), false);
                            mockChessBoard.Board[end.X, end.Y] = mockChessBoard.Board[start.X, start.Y];
                            mockChessBoard.Board[start.X, start.Y] = null;
                            if (!IsKingInCheck(kingsColor, mockChessBoard))
                                return false;
                        }
                    }
                }
                return true;
            }
            else
                return true;
        }

        private void PassTheTurn()
        {
            StopTimer(whoseTurn);
            ShadowTimer(whoseTurn);
            whoseTurn = (whoseTurn == "White") ? "Black" : "White";
            StartTimer(whoseTurn);
            LightTimer(whoseTurn);
        }

        private void StartTimer(string color)
        {
            if (color == "White")
                whiteTimer.Start();
            else if (color == "Black")
                blackTimer.Start();
        }
        private void StopTimer(string color)
        {
            if (color == "White")
                whiteTimer.Stop();
            else if (color == "Black")
                blackTimer.Stop();
        }

        private void ShadowTimer(string color)
        {
            if (color == "White")
            {
                lblWhiteTimer.BackColor = lightGray;
                lblWhiteTimer.ForeColor = darkGray;
            }
            else if (color == "Black")
            {
                lblBlackTimer.BackColor = darkGray;
                lblBlackTimer.ForeColor = lightGray;
            }
        }
        private void LightTimer(string color)
        {
            if (color == "White")
            {
                lblWhiteTimer.BackColor = Color.Ivory;
                lblWhiteTimer.ForeColor = Color.Black;
            }
            else if (color == "Black")
            {
                lblBlackTimer.BackColor = Color.Black;
                lblBlackTimer.ForeColor = Color.Ivory;
            }
        }

        private void EndTheGame(string winner)
        {
            whiteTimer.Stop();
            blackTimer.Stop();
            EndGameForm endGameForm = new EndGameForm(this, winner);
            this.Enabled = false;
            endGameForm.Show();
        }
    }
}
