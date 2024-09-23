using System;
using System.Drawing;
using System.Windows.Forms;

namespace ChessSharp
{
    public partial class EndGameForm : Form
    {
        Main mainForm;
        private static string result;
        private readonly static int size = 300;
        public EndGameForm(Main main, string winner)
        {
            mainForm = main;
            InitializeComponent();
            this.ClientSize = new Size(size, size);
            this.ControlBox = false;
            this.Text = "";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.BackColor = Color.Cornsilk;
            result = winner;
            InitializeResult();
            InitializeExitButton();
        }

        private void InitializeResult()
        {
            string text = "";
            if (result == "Draw")
                text = "The game ended in a Draw!";
            else if (result == "White")
                text = "White have won!";
            else
                text = "Black have won!";
            Label resultLabel = new Label
            {
                Size = new Size(size, size / 4),
                Location = new Point(0, 50),
                Font = new Font("Arial", 20, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleCenter,
                Text = text,
                ForeColor = Color.Black
            };
            this.Controls.Add(resultLabel);

        }

        private void InitializeExitButton()
        {
            Button exitButton = new Button
            {
                Text = "Exit",
                Size = new Size(size / 2, 50),
                Location = new Point(size / 4, 200),
                Font = new Font("Arial", 14, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.White
            };
            this.Controls.Add(exitButton);
            exitButton.Click += ExitButton_Click;
        }

        private void ExitButton_Click(object sender, EventArgs e)
        {
            mainForm.Close();
            this.Close();
        }
    }
}
