using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckCom_Version2
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void checkCơmToolStripMenuItem_Click(object sender, EventArgs e)
        {

            CheckCom ck = new CheckCom();
           
            if (KiemTraTonTai("CheckCom") == true)
                ck.Activate();
            else
            {
                ck.MdiParent = this;
                ck.Show();
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

        private void bổSungBáoCơmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            BoSungBaoCom bs = new BoSungBaoCom();
            if (KiemTraTonTai("BoSungBaoCom") == true)
                bs.Activate();
            else
            {
                bs.MdiParent = this;
                bs.Show();
            }
        }

        private void đồngBộDữLiệuToolStripMenuItem_Click(object sender, EventArgs e)
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

        private void inThẻToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Card_Check ck = new Card_Check();
            if (KiemTraTonTai("Card Check") == true)
                ck.Activate();
            else
            {
                ck.MdiParent = this;
                ck.Show();
            }
        }

        private void hướngDẫnSửDụngToolStripMenuItem_Click(object sender, EventArgs e)
        {
            HuongDan hd = new HuongDan();
            if (KiemTraTonTai("HuongDan") == true)
                hd.Activate();
            else
            {
                hd.MdiParent = this;
                hd.Show();
            }
        }

        private void thôngTinPhầnMềmToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ThongTin tt = new ThongTin();
            if (KiemTraTonTai("ThongTin") == true)
                tt.Activate();
            else
            {
                tt.MdiParent = this;
                tt.Show();
            }
        }

        private void đồngBộFileCheckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Dongbodulieucheck tt = new Dongbodulieucheck();
            if (KiemTraTonTai("Dongbodulieucheck") == true)
                tt.Activate();
            else
            {
                tt.MdiParent = this;
                tt.Show();
            }
        }
    }
}
