using ChessSharp.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessSharp.Views
{
    public class ChessPieceControl : PictureBox
    {
        private (int, string) pieceIndex;  // Backing field for the PieceIndex property
        public (int, string) PieceIndex
        {
            get => pieceIndex;
            set
            {
                if (pieceIndex != value)
                {
                    pieceIndex = value;
                    UpdatePieceImage();  // Update the image whenever the index changes
                }
            }
        }

        public ChessPiece Piece { get; set; }

        public ChessPieceControl()
        {
            this.SizeMode = PictureBoxSizeMode.StretchImage;  // Ensure the sprite fits the PictureBox
            this.Size = new Size(80, 80);

            this.MouseEnter += ChessPieceControl_MouseEnter;
            this.MouseLeave += ChessPieceControl_MouseLeave;
        }

        public void UpdatePieceImage()
        {
            (int idx, string color) = pieceIndex;
            if (pieceIndex.Item1 == -1)
            {
                this.Image = null;
                return;
            }
            Bitmap fullSprite = Properties.Resources.ChessPiecesSprite; // Load the sprite sheet
            int pieceWidth = fullSprite.Width / 6;
            int pieceHeight = fullSprite.Height / 2;
            int y = (color == "White") ? 0 : (pieceHeight - 10);
            int x = idx * pieceWidth;

            Rectangle cropArea = new Rectangle(x, y, pieceWidth, pieceHeight);
            Bitmap pieceImage = fullSprite.Clone(cropArea, fullSprite.PixelFormat);

            this.Image = pieceImage;
        }

        private void ChessPieceControl_MouseEnter(object sender, EventArgs e)
        {
            if (this.Image != null)  // Correctly refer to the Image property of the PictureBox
            {
                Cursor = Cursors.Hand;  // Change cursor to hand when hovering over a piece
            }
        }

        private void ChessPieceControl_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Default;  // Revert cursor to default when not hovering over the piece
        }

    }
}
