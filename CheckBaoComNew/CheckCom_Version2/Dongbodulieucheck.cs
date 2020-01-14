using CheckCom_Version2.DTOs;
using CheckCom_Version2.Helper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckCom_Version2
{
    public partial class Dongbodulieucheck : Form
    {
        //private string APICheckBaoCom = null;
        private List<CheckBaoCom> baocom = new List<CheckBaoCom>();
        private List<BuaAn> buaan = new List<BuaAn>();
        private string caan = null;
        private string caanid;
        private string filecheck = null;
        private string filebuaan = null;
        private string filenhaan = null;
        private string filenhabep = null;
        private string filelog = null;
        private string idnhaan;

        private string fileApidlbc = null;
        private string fileApibuaan = null;
        private string fileApinv = null;
        private string fileApibp = null;
        public Dongbodulieucheck()
        {
            InitializeComponent();
            getPath();
            //getApi();
            GetBuaan();
            btnCapNhap.Enabled = true;
            int Gio = DateTime.Now.Hour;

            if ((8 <= Gio) && (Gio < 14))
            {
                cbBuaan.Text = "Trưa";
                caan = " Trua";
            }
            else if ((14 <= Gio) && (Gio < 20))
            {
                cbBuaan.Text = "Chiều";
                caan = " Chieu";
            }
            else if ((2 <= Gio) && (Gio < 8))
            {
                cbBuaan.Text = "Bữa nhẹ";
                caan = " Buanhe";
            }
            else
            {
                cbBuaan.Text = "Tối";
                caan = " Toi";
            }
        }
        private void GetBuaan()
        {
            buaan.Clear();
            try
            {
                string pathfile = filebuaan + "BuaAn.xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                oada.Fill(table);
                MyConnection.Close();
                cbBuaan.Items.Clear();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow drow = table.Rows[i];

                    if (drow.RowState != DataRowState.Deleted)
                    {
                        BuaAn ba = new BuaAn()
                        {
                            id = drow["id"].ToString(),
                            ten = drow["ten"].ToString()
                        };
                        cbBuaan.Items.Add(ba.ten);
                        buaan.Add(ba);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Không có dữ liệu bữa ăn!");
            }
        }
        private void getApi()
        {
            try
            {
                string path = Application.StartupPath + @"\Api.txt";
                fileApidlbc = File.ReadAllLines(path)[0];
                fileApibuaan = File.ReadAllLines(path)[1];
                fileApinv = File.ReadAllLines(path)[2];
                fileApibp = File.ReadAllLines(path)[3];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void GetNhaAnID()
        {
            try
            {
                string pathfile = filenhaan + "NhaAn.xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                oada.Fill(table);
                MyConnection.Close();
                idnhaan = table.Rows[0]["nhaanid"].ToString();
            }
            catch (Exception)
            {
            }
        }

        private void getPath()
        {
            try
            {
                string path = Application.StartupPath + @"\Path.txt";
                filecheck = File.ReadAllLines(path)[0];
                filebuaan = File.ReadAllLines(path)[1];
                filenhaan = File.ReadAllLines(path)[2];
                filenhabep = File.ReadAllLines(path)[3];
                filelog = File.ReadAllLines(path)[4];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void btnCapNhap_Click(object sender, EventArgs e)
        {
            btnCapNhap.Enabled = false;
            string fileToRead = System.IO.Path.GetDirectoryName(filecheck);

            DirectoryInfo dinfo = new DirectoryInfo(fileToRead);
            FileInfo[] Files = dinfo.GetFiles("*.xls");
            foreach (FileInfo file in Files)
            {
                for (int x = helper.FirstDayOfMonth(dtfromdate.Value).Day; x <= helper.LastDayOfMonth(dtfromdate.Value).Day; x++)
                {
                    string dlbaocom = string.Format("{0:00}", helper.FirstDayOfMonth(dtfromdate.Value).Month) + "-" + string.Format("{0:00}", x) + "-" + helper.FirstDayOfMonth(dtfromdate.Value).Year.ToString() + caan;
                    if (file.ToString().Contains(dlbaocom))
                    {
                        try
                        {
                            string pathfile = filecheck + dlbaocom;
                            string info = filecheck + dlbaocom + ".txt";
                            DataTable table = new DataTable();
                            System.Data.OleDb.OleDbConnection MyConnection;
                            System.Data.OleDb.OleDbCommand myCommandup = new System.Data.OleDb.OleDbCommand();
                            MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                            MyConnection.Open();
                            myCommandup.Connection = MyConnection;
                            OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                            oada.Fill(table);
                            try
                            {
                                string[] lines = File.ReadAllLines(info);

                                for (int i = 0; i < table.Rows.Count; i++)
                                {
                                    for (int j = 0; j < lines.Count(); j++)
                                    {
                                        if (lines[j].Split('-')[0].Contains(table.Rows[i]["manhansu"].ToString()))
                                        {
                                            string sqlup = "update [Sheet1$] set thoigiansudung='" + Convert.ToDateTime(lines[j].Split('-')[1]).ToString("yyyy-MM-dd HH:mm:ss") + "',soxuatandadung='1',bepanid='" + lines[j].Split('-')[2] + "'  where manhansu='" + table.Rows[i]["manhansu"].ToString() + "'";
                                            myCommandup.CommandText = sqlup;
                                            myCommandup.ExecuteNonQuery();

                                        }
                                    }
                                }
                                MyConnection.Close();
                            }
                            catch
                            {
                                break;
                            }
                        }
                        catch 
                        {

                            break;
                        }
                    }
                }

            }
            MessageBox.Show("Thành Công!");
            btnCapNhap.Enabled = true;
        }

        private void cbBuaan_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbBuaan.Text == "Trưa")
            {
                caan = " Trua";
            }
            else if (cbBuaan.Text == "Chiều")
            {
                caan = " Chieu";
            }
            else if (cbBuaan.Text == "Tối")
            {
                caan = " Toi";
            }
            else
            {
                caan = " Buanhe";
            }
            foreach (BuaAn ba in buaan)
            {
                if (ba.ten == cbBuaan.Text)
                {
                    caanid = ba.id;
                }
            }
            //MessageBox.Show(helper.FirstDayOfMonth(dtfromdate.Value).ToString("MM-dd-yyyy") + "-------->" + helper.LastDayOfMonth(dtfromdate.Value).ToString("MM-dd-yyyy"));
           // MessageBox.Show(helper.FirstDayOfMonth(dtfromdate.Value).Day+ "-------->" + helper.LastDayOfMonth(dtfromdate.Value).Day);
            GetAlllistBox();
        }
        private void GetAlllistBox()
        {
            dldangky.Items.Clear();
            string fileToRead = System.IO.Path.GetDirectoryName(filecheck);

            DirectoryInfo dinfo = new DirectoryInfo(fileToRead);
            FileInfo[] Files = dinfo.GetFiles("*.xls");
            foreach (FileInfo file in Files)
            {
                for (int x= helper.FirstDayOfMonth(dtfromdate.Value).Day;x<= helper.LastDayOfMonth(dtfromdate.Value).Day;x++)
                {
                    string dlbaocom = string.Format("{0:00}", helper.FirstDayOfMonth(dtfromdate.Value).Month)+"-" + string.Format("{0:00}",x)+"-" + helper.FirstDayOfMonth(dtfromdate.Value).Year.ToString() + caan;
                    if(file.ToString().Contains(dlbaocom))
                    {
                        dldangky.Items.Add(file.ToString());
                    }
                }
               
            }
           
        }

        private void dldangky_MouseClick(object sender, MouseEventArgs e)
        {
            lvdlcheckcom.Items.Clear();
            string dlngay = dldangky.SelectedItems[0].SubItems[0].Text;
           GetDataClient(dlngay);
            try
            {
                string info = filecheck + dlngay.Split('.')[0] + ".txt";
                string[] items = File.ReadAllLines(info);
                foreach (string item in items)
                {
                    lvdlcheckcom.Items.Add(item);
                }
            }
            catch
            {

                MessageBox.Show("Không có dữ liệu lấy cơm!");
            }
         
        }
        private void GetDataClient(string ngay)
        {
            try
            {
                string pathfile = filecheck + ngay;
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                oada.Fill(table);
                MyConnection.Close();
                lvDongbo.Items.Clear();
                lbClient.Text = null;
                lbClient.Text = table.Rows.Count.ToString();
                table.DefaultView.Sort = "manhansu asc";
                table = table.DefaultView.ToTable(true);
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow drow = table.Rows[i];

                    if (drow.RowState != DataRowState.Deleted)
                    {
                        ListViewItem lvi = new ListViewItem(drow["manhansu"].ToString());
                        lvi.SubItems.Add(drow["hoten"].ToString());
                        lvi.SubItems.Add(drow["phong"].ToString());
                        lvi.SubItems.Add(drow["ban"].ToString());
                        lvi.SubItems.Add(drow["congdoan"].ToString());
                        lvi.SubItems.Add(drow["khach"].ToString());
                        lvi.SubItems.Add(drow["ngay"].ToString());
                        lvi.SubItems.Add(drow["thang"].ToString());
                        lvi.SubItems.Add(drow["nam"].ToString());
                        lvi.SubItems.Add(drow["thoigiandat"].ToString());
                        lvi.SubItems.Add(drow["sudung"].ToString());
                        lvi.SubItems.Add(drow["dangky"].ToString());
                        lvi.SubItems.Add(drow["thoigiansudung"].ToString());
                        lvi.SubItems.Add(drow["soxuatandadung"].ToString());
                        lvi.SubItems.Add(drow["chot"].ToString());
                        lvi.SubItems.Add(drow["ghichu"].ToString());
                        lvi.SubItems.Add(drow["buaan"].ToString());
                        lvDongbo.Items.Add(lvi);
                    }
                }
                //StreamWriter objWriter = new StreamWriter(pathfile);
                //objWriter.WriteLine(lvdlcheckcom.SelectedItem.ToString());
                //objWriter.Close();
            }
            catch (Exception)
            {
                lvDongbo.Items.Clear();
                lbClient.Text = null;
                lbClient.Text = "0";
            }
        }

        private void lvDongbo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void Dongbodulieucheck_Load(object sender, EventArgs e)
        {
            lvDongbo.View = View.Details;
            lvDongbo.FullRowSelect = true;
        }

        private void dldangky_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }
    }
}
