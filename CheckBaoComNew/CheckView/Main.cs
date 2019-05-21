using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckView
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void checkCơmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form1 fr1 = new Form1();
            if (KiemTraTonTai("Form1") == true)
                fr1.Activate();
            else
            {
                fr1.MdiParent = this;
                fr1.Show();
            }
          
        }

        private void bổSungBáoCơmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Bosungbaocom bs = new Bosungbaocom();
            if (KiemTraTonTai("Bosungbaocom") == true)
                bs.Activate();
            else
            {
                bs.MdiParent = this;
                bs.Show();
            }
        }

        private void đồngBộToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DongBoDuLieu db = new DongBoDuLieu();
            if (KiemTraTonTai("DongBoDuLieu") == true)
                db.Activate();
            else
            {
                db.MdiParent = this;
                db.Show();
            }
            
        }
        public Boolean KiemTraTonTai(string Frmname)
        {
            foreach (Form frm in this.MdiChildren)
            {
                if (frm.Name.Equals(Frmname))
                    return true;
            }
            return false;
        }
    }
}
