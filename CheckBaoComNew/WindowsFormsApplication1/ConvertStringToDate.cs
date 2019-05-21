using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class ConvertStringToDate : Form
    {
        public ConvertStringToDate()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //string somestring = textBox1.Text;
            //string newstring = somestring.Substring(0, 10);
            //MessageBox.Show(newstring);
            string filePath = Application.StartupPath + @"\FileName\checkcom.txt";
            StreamReader objReader = new StreamReader(filePath);
            string nameBaoCom = objReader.ReadLine();
            objReader.Close();

            MessageBox.Show(nameBaoCom);
        }
    }
}
