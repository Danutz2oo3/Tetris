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
    public partial class Info : Form
    {
        public Form BackToMainForm { get; set; }
        public Info()
        {
            InitializeComponent();
            /*string path = "E:\\Proiecte\\atestat\\Info.txt";
            StreamReader fis = new StreamReader(path);
            string line = fis.ReadLine();
            while (line != null)
            {
                richTextBox1.Text += line;
                line = fis.ReadLine();
            }*/
        }
        private void button1_Click_1(object sender, EventArgs e)
        {
            this.BackToMainForm.Visible = true;
            this.Close();
        }

        
    }
}
