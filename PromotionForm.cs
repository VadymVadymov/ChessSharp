using ChessSharp.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessSharp
{
    public partial class PromotionForm : Form
    {
        public (ChessPiece, int) ChosenPiece;
        private int cellSize;
        private int offset;
        public PromotionForm(int cellSize, string color)
        {
            this.cellSize = cellSize;
            this.Size = new Size(cellSize * 2, cellSize * 2);
            this.BackColor = Color.White;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            InitializeComponent();

            Bitmap fullSprite = Properties.Resources.ChessPiecesSprite;
            int pieceWidth = fullSprite.Width / 6;
            int pieceHeight = fullSprite.Height / 2;
            int y = (color == "White") ? 0 : (pieceHeight - 10);

            Rectangle queenArea = new Rectangle(1 * pieceWidth, y, pieceWidth, pieceHeight);
            Rectangle bishopArea = new Rectangle(2 * pieceWidth, y, pieceWidth, pieceHeight);
            Rectangle knightArea = new Rectangle(3 * pieceWidth, y, pieceWidth, pieceHeight);
            Rectangle rookArea = new Rectangle(4 * pieceWidth, y, pieceWidth, pieceHeight);

            PictureBox pbxQueen = new PictureBox
            {
                Image = fullSprite.Clone(queenArea, fullSprite.PixelFormat),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(cellSize, cellSize),
                Cursor = Cursors.Hand
            };
            pbxQueen.Click += (sender, e) => { ChosenPiece = (new Queen(color), 1); this.DialogResult = DialogResult.OK; };
            pbxQueen.MouseEnter += HoverPictureBox_MouseEnter;
            pbxQueen.MouseLeave += HoverPictureBox_MouseLeave;
            this.Controls.Add(pbxQueen);

            PictureBox pbxBishop = new PictureBox
            {
                Image = fullSprite.Clone(bishopArea, fullSprite.PixelFormat),
                Location = new Point(cellSize, 0),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(cellSize, cellSize),
                Cursor = Cursors.Hand
            };
            pbxBishop.Click += (sender, e) => { ChosenPiece = (new Bishop(color), 2); this.DialogResult = DialogResult.OK; };
            pbxBishop.MouseEnter += HoverPictureBox_MouseEnter;
            pbxBishop.MouseLeave += HoverPictureBox_MouseLeave;
            this.Controls.Add(pbxBishop);

            PictureBox pbxKnight = new PictureBox
            {
                Image = fullSprite.Clone(knightArea, fullSprite.PixelFormat),
                Location = new Point(0, cellSize),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(cellSize, cellSize),
                Cursor = Cursors.Hand
            };
            pbxKnight.Click += (sender, e) => { ChosenPiece = (new Knight(color), 3); this.DialogResult = DialogResult.OK; };
            pbxKnight.MouseEnter += HoverPictureBox_MouseEnter;
            pbxKnight.MouseLeave += HoverPictureBox_MouseLeave;
            this.Controls.Add(pbxKnight);

            PictureBox pbxRook = new PictureBox
            {
                Image = fullSprite.Clone(rookArea, fullSprite.PixelFormat),
                Location = new Point(cellSize, cellSize),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(cellSize, cellSize),
                Cursor = Cursors.Hand
            };
            pbxRook.Click += (sender, e) => { ChosenPiece = (new Rook(color), 4); this.DialogResult = DialogResult.OK; };
            pbxRook.MouseEnter += HoverPictureBox_MouseEnter;
            pbxRook.MouseLeave += HoverPictureBox_MouseLeave;
            this.Controls.Add(pbxRook);
        }
        private void HoverPictureBox_MouseEnter(object sender, EventArgs e)
        {
            PictureBox pbx = sender as PictureBox;
            if (pbx != null)
            {
                pbx.BackColor = Color.PapayaWhip; // Change to a noticeable color on hover
            }
        }

        private void HoverPictureBox_MouseLeave(object sender, EventArgs e)
        {
            PictureBox pbx = sender as PictureBox;
            if (pbx != null)
            {
                pbx.BackColor = Color.White; // Revert to the original color
            }
        }
    }
}
