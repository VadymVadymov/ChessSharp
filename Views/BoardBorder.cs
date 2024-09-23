using System.Drawing;
using System.Windows.Forms;

namespace ChessSharp.Views
{
    public class BoardBorder : Panel
    {
        public Color BorderColor { get; set; } = Color.SaddleBrown;
        public int BorderThickness { get; set; } = 2;

        public BoardBorder()
        {
            this.DoubleBuffered = true;
            this.ResizeRedraw = true;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            int halfThickness = BorderThickness / 2;
            using (Pen pen = new Pen(BorderColor, BorderThickness))
            {
                e.Graphics.DrawRectangle(pen, halfThickness, halfThickness,
                    ClientSize.Width - BorderThickness, ClientSize.Height - BorderThickness);
            }
        }
    }
}
