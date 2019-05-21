using iTextSharp.text.pdf.parser;
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

namespace CheckCom_Version2
{
    public partial class HuongDan : Form
    {
        public HuongDan()
        {
            InitializeComponent();
            GetHuongDan();
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
        private void GetHuongDan()
        {
            string pathfile = Application.StartupPath + @"\Huong dan\huong dan check com1.pdf";
            //FileInfo filename = new FileInfo(pathfile);
            axFoxitCtl1.OpenFile(pathfile);
        }
    }
}
