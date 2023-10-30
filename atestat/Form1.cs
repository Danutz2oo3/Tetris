using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace atestat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.KeyPreview = true;
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Rules rules = new Rules();
            rules.BackToMainForm = this;
            rules.Show();
            this.Visible = false;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Info info = new Info();
            info.BackToMainForm = this;
            info.Show();
            this.Visible = false;

        }

        private void button5_Click(object sender, EventArgs e)
        {
            string message = "Don't close me please...";
            string title = "Close Game";
            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
            DialogResult result = MessageBox.Show(message, title, buttons);
            if (result == DialogResult.Yes)
            {
                this.Close();
            }
            else
            {
                this.Visible = true;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            HighScore highScore = new HighScore();
            highScore.BackToMainForm = this;    
            highScore.Show();
            this.Visible=false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Game game = new Game();
            game.BackToMainForm= this; 
            game.Show();
            this.Visible = false;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Escape)
            {
                string message = "Do you want to close this window?";
                string title = "Close Window";
                MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                DialogResult result = MessageBox.Show(message, title, buttons);
                if (result == DialogResult.Yes)
                {
                    this.Close();
                }
                else
                {
                    this.Visible = true;
                }
            }
        }
    }
}