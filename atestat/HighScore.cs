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
    public partial class HighScore : Form
    {
        public Form BackToMainForm { get; set; }
        public HighScore()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.BackToMainForm.Visible = true;
            this.Close();
        }
    }
}
