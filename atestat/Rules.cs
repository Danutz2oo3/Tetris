using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace atestat
{
    public partial class Rules : Form
    {
        public Form BackToMainForm { get; set; }
        public Rules()
        {
            InitializeComponent();
            string path = "E:\\Proiecte\\atestat\\Rules.txt";
            StreamReader fis = new StreamReader(path);
            string line= fis.ReadLine();
            while(line != null)
            {
                richTextBox1.Text += line;
                line = fis.ReadLine();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.BackToMainForm.Visible=true;
            this.Close();
        }
    }
}
