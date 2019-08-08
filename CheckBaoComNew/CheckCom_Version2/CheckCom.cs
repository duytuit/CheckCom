using CheckCom_Version2.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckCom_Version2
{
    public partial class CheckCom : Form
    {
        private string APICheckBaoCom = null;
        private string caanid;
        private List<CheckBaoCom> baocom = new List<CheckBaoCom>();
        private List<BuaAn> buaan = new List<BuaAn>();
        private string caan = null;
        private SoundPlayer checkok = new SoundPlayer(Application.StartupPath + @"\sound\Beep_Once.wav");
        private SoundPlayer checkng = new SoundPlayer(Application.StartupPath + @"\sound\buzzer_x.wav");
        private string getthoigian = null;
        private string filecheck = null;
        private string filebuaan = null;
        private string filenhaan = null;
        private string filenhabep = null;
        private string idnhaan = null;
        private string nhabep = null;
        private string fileApidlbc = null;
        private string fileApibuaan = null;
        private string fileApinv = null;
        private string fileApibp = null;
        private string filelog = null;
        private Form hienthi = new Form();
        private TableLayoutPanel dynamicTableLayoutPanel = new TableLayoutPanel();
        private PictureBox picturebox1 = new PictureBox();
        private PictureBox picturebox2 = new PictureBox();
        public int Tong = 0;
        public int Conlai = 0;
        List<string> IDChuaBaoCom = new List<string>();
        public CheckCom()
        {
            InitializeComponent();
            int Gio = DateTime.Now.Hour;
            getPath();
            getApi();
            GetBuaaan();
            txtNhapSoluong.Visible = false;
            if ((8 <= Gio) && (Gio < 14))
            {
                caan = " Trua";
                cbBuaan.Text = "Trưa";
            }
            else if ((14 <= Gio) && (Gio < 20))
            {
                caan = " Chieu";
                cbBuaan.Text = "Chiều";
            }
            else if ((2 <= Gio) && (Gio < 8))
            {
                caan = " Buaphu";
                cbBuaan.Text = "Bữa phụ";
            }
            else
            {
                caan = " Toi";
                cbBuaan.Text = "Tối";
            }
            // ht.Show();
            // ht.ControlBox = false; ẩn Close

            hienthi.Width = 946;
            hienthi.Height = 594;
            hienthi.Text = "Hiển thị";
            hienthi.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            hienthi.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            //picturebox1
            picturebox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                   | System.Windows.Forms.AnchorStyles.Left)
                   | System.Windows.Forms.AnchorStyles.Right)));
            picturebox1.Location = new System.Drawing.Point(3, 3);
            picturebox1.Name = "pictureBox1";
            picturebox1.Size = new System.Drawing.Size(457, 544);
            picturebox1.TabIndex = 1;
            picturebox1.TabStop = false;
            //picturebox2
            picturebox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                         | System.Windows.Forms.AnchorStyles.Left)
                         | System.Windows.Forms.AnchorStyles.Right)));
            picturebox2.Location = new System.Drawing.Point(466, 3);
            picturebox2.Name = "pictureBox2";
            picturebox2.Size = new System.Drawing.Size(457, 544);
            picturebox2.TabIndex = 2;
            picturebox2.TabStop = false;
            // dynamicTableLayoutPanel
            dynamicTableLayoutPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
          | System.Windows.Forms.AnchorStyles.Left)
          | System.Windows.Forms.AnchorStyles.Right)));
            dynamicTableLayoutPanel.ColumnCount = 2;
            dynamicTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            dynamicTableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            dynamicTableLayoutPanel.Controls.Add(picturebox1, 0, 0);
            dynamicTableLayoutPanel.Controls.Add(picturebox2, 1, 0);
            dynamicTableLayoutPanel.Location = new System.Drawing.Point(2, 3);
            dynamicTableLayoutPanel.Name = "tableLayoutPanelHienThi";
            dynamicTableLayoutPanel.RowCount = 1;
            dynamicTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            dynamicTableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 550F));
            dynamicTableLayoutPanel.Size = new System.Drawing.Size(926, 550);
            dynamicTableLayoutPanel.TabIndex = 0;

            hienthi.Controls.Add(dynamicTableLayoutPanel);
            hienthi.ControlBox = false; //ẩn Close
            hienthi.Show();
        }

        private void CheckCom_Load(object sender, EventArgs e)
        {
            lbsosuatanconlai.Font = new Font(lbsosuatanconlai.Font.FontFamily, int.Parse(txtFontSize.Text));
            lbTong.Font = new Font(lbTong.Font.FontFamily, int.Parse(txtFontSize.Text));
        }

        private void GetNhaAnID()
        {
            try
            {
                string pathfile = filenhaan + "NhaAn.xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'");
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

        private void GetNhaBep()
        {
            try
            {
                string pathfile = filenhabep + "NhaBep.xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                oada.Fill(table);
                MyConnection.Close();
                nhabep = table.Rows[0]["tennhabep"].ToString();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        ///
        /// </summary>
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

        private void GetBuaaan()
        {
            try
            {
                string pathfile = filebuaan + "BuaAn.xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;IMEX=1'");
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

        private async Task<string> GetAllCheckBaoCom()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync(APICheckBaoCom);
            return astr;
        }

        private bool CheckData()
        {
            bool kiemtrabaocom = false;
            string fileToRead = System.IO.Path.GetDirectoryName(filecheck);

            DirectoryInfo dinfo = new DirectoryInfo(fileToRead);
            FileInfo[] Files = dinfo.GetFiles("*");
            foreach (FileInfo file in Files)
            {
                var path = new TestPath(file);
                if (path.ToString() == dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan)
                {
                    kiemtrabaocom = true;
                    break;
                }
            }
            return kiemtrabaocom;
        }

        public class TestPath
        {
            public FileInfo Original { get; private set; }

            public TestPath(FileInfo original)
            {
                Original = original;
            }

            public override string ToString()
            {
                return Path.GetFileNameWithoutExtension(Original.Name);
            }
        }

        private void GetBaoCom()
        {
            bool icheck = CheckData();
            if (icheck == false)
            {
                baocom.Clear();
                try
                {
                    Task<string> callTask = Task.Run(() => GetAllCheckBaoCom());
                    callTask.Wait();
                    string astr = callTask.Result;
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(astr, typeof(DataTable));
                    if (dt.Rows.Count > 0)
                    {
                        string info = filecheck + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".txt";
                        using (FileStream f = new FileStream(info, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            f.Close();
                        }
                        string infolog = filelog + "log-" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".txt";
                        using (FileStream f = new FileStream(infolog, FileMode.CreateNew, FileAccess.ReadWrite, FileShare.ReadWrite))
                        {
                            f.Close();
                        }
                        // File.Create(info);
                        // File.Exists(info);
                        string pathfile = filecheck + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                        FileInfo filename = new FileInfo(pathfile);
                        Microsoft.Office.Interop.Excel.Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
                        Microsoft.Office.Interop.Excel.Workbook wb = docExcel.Workbooks.Add(Type.Missing);
                        Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)docExcel.ActiveSheet;
                        ws.Cells[1, 1] = "id";
                        ws.Cells[1, 2] = "empid";
                        ws.Cells[1, 3] = "manhansu";
                        ws.Cells[1, 4] = "hoten";
                        ws.Cells[1, 5] = "phongid";
                        ws.Cells[1, 6] = "phong";
                        ws.Cells[1, 7] = "banid";
                        ws.Cells[1, 8] = "ban";
                        ws.Cells[1, 9] = "congdoanid";
                        ws.Cells[1, 10] = "congdoan";
                        ws.Cells[1, 11] = "khach";
                        ws.Cells[1, 12] = "ngay";
                        ws.Cells[1, 13] = "thang";
                        ws.Cells[1, 14] = "nam";
                        ws.Cells[1, 15] = "userid";
                        ws.Cells[1, 16] = "thoigiandat";
                        ws.Cells[1, 17] = "sudung";
                        ws.Cells[1, 18] = "dangky";
                        ws.Cells[1, 19] = "thoigiansudung";
                        ws.Cells[1, 20] = "soxuatandadung";
                        ws.Cells[1, 21] = "sotiendadung";
                        ws.Cells[1, 22] = "chot";
                        ws.Cells[1, 23] = "ghichu";
                        ws.Cells[1, 24] = "thucdontheobuaid";
                        ws.Cells[1, 25] = "thucdontheobua";
                        ws.Cells[1, 26] = "kieudoan";
                        ws.Cells[1, 27] = "buaanid";
                        ws.Cells[1, 28] = "buaan";
                        ws.Cells[1, 29] = "ca";
                        ws.Cells[1, 30] = "nhaanid";
                        ws.Cells[1, 31] = "nhaan";
                        ws.Cells[1, 32] = "loaidouong";
                        ws.Cells[1, 33] = "thanhtoan";
                        ws.Cells[1, 34] = "phongrieng";
                        ws.Cells[1, 35] = "dangkybosung";
                        ws.Cells[1, 36] = "nhabep";
                        ws.Cells[1, 37] = "trangthai2";

                        var data = new object[dt.Rows.Count, dt.Columns.Count];
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            for (int column = 0; column <= dt.Columns.Count - 1; column++)
                            {
                                data[row, column] = dt.Rows[row][column].ToString();
                            }
                            DataRow drow = dt.Rows[row];

                            if (drow.RowState != DataRowState.Deleted)
                            {
                                CheckBaoCom ck = new CheckBaoCom()
                                {
                                    id = drow["id"].ToString(),
                                    empid = string.IsNullOrEmpty(drow["empid"].ToString()) ? null : drow["empid"].ToString(),
                                    manhansu = drow["manhansu"].ToString(),
                                    hoten = drow["hoten"].ToString(),
                                    phongid = string.IsNullOrEmpty(drow["phongid"].ToString()) ? null : drow["phongid"].ToString(),
                                    phong = string.IsNullOrEmpty(drow["phong"].ToString()) ? null : drow["phong"].ToString(),
                                    banid = string.IsNullOrEmpty(drow["banid"].ToString()) ? null : drow["banid"].ToString(),
                                    ban = string.IsNullOrEmpty(drow["ban"].ToString()) ? null : drow["ban"].ToString(),
                                    congdoanid = string.IsNullOrEmpty(drow["congdoanid"].ToString()) ? null : drow["congdoanid"].ToString(),
                                    congdoan = string.IsNullOrEmpty(drow["congdoan"].ToString()) ? null : drow["congdoanid"].ToString(),
                                    khach = drow["khach"].ToString(),
                                    ngay = Convert.ToDateTime(drow["ngay"].ToString()).ToString("yyyy-MM-dd"),
                                    thang = int.Parse(drow["thang"].ToString()),
                                    nam = int.Parse(drow["nam"].ToString()),
                                    userid = string.IsNullOrEmpty(drow["userid"].ToString()) ? null : drow["userid"].ToString(),
                                    thoigiandat = Convert.ToDateTime(drow["thoigiandat"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                    sudung = drow["sudung"].ToString(),
                                    dangky = drow["dangky"].ToString(),
                                    sotiendadung = int.Parse(drow["sotiendadung"].ToString()),
                                    chot = drow["chot"].ToString(),
                                    buaanid = drow["buaanid"].ToString(),
                                    nhaanid = drow["nhaanid"].ToString(),
                                    nhaan = drow["nhaan"].ToString(),
                                    dangkybosung = drow["dangkybosung"].ToString()
                                };
                                baocom.Add(ck);
                            }
                        }
                        var startCell = (Microsoft.Office.Interop.Excel.Range)ws.Cells[2, 1];
                        var endCell = (Microsoft.Office.Interop.Excel.Range)ws.Cells[dt.Rows.Count + 1, dt.Columns.Count];
                        var writeRange = ws.Range[startCell, endCell];
                        ws.Columns[3].NumberFormat = "@";
                        ws.Columns[19].NumberFormat = "@";
                        writeRange.Value2 = data;
                        wb.SaveAs(filename.FullName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges);
                        wb.Close();
                        docExcel.Application.Quit();
                    }
                    else
                    {
                        MessageBox.Show("Chưa có dữ liệu!");
                    }
                }
                catch (AggregateException)
                {
                    MessageBox.Show("Chưa có dữ liệu!");
                }
            }
            else
            {
                baocom.Clear();
                string pathfile = filecheck + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                oada.Fill(table);
                MyConnection.Close();

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow drow = table.Rows[i];

                    if (drow.RowState != DataRowState.Deleted)
                    {
                        CheckBaoCom ck = new CheckBaoCom()
                        {
                            id = drow["id"].ToString(),
                            empid = string.IsNullOrEmpty(drow["empid"].ToString()) ? null : drow["empid"].ToString(),
                            manhansu = drow["manhansu"].ToString(),
                            hoten = drow["hoten"].ToString(),
                            phongid = string.IsNullOrEmpty(drow["phongid"].ToString()) ? null : drow["phongid"].ToString(),
                            phong = string.IsNullOrEmpty(drow["phong"].ToString()) ? null : drow["phong"].ToString(),
                            banid = string.IsNullOrEmpty(drow["banid"].ToString()) ? null : drow["banid"].ToString(),
                            ban = string.IsNullOrEmpty(drow["ban"].ToString()) ? null : drow["ban"].ToString(),
                            congdoanid = string.IsNullOrEmpty(drow["congdoanid"].ToString()) ? null : drow["congdoanid"].ToString(),
                            congdoan = string.IsNullOrEmpty(drow["congdoan"].ToString()) ? null : drow["congdoanid"].ToString(),
                            khach = drow["khach"].ToString(),
                            ngay = Convert.ToDateTime(drow["ngay"].ToString()).ToString("yyyy-MM-dd"),
                            thang = int.Parse(drow["thang"].ToString()),
                            nam = int.Parse(drow["nam"].ToString()),
                            userid = string.IsNullOrEmpty(drow["userid"].ToString()) ? null : drow["userid"].ToString(),
                            thoigiandat = Convert.ToDateTime(drow["thoigiandat"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                            sudung = drow["sudung"].ToString(),
                            dangky = drow["dangky"].ToString(),
                            sotiendadung = int.Parse(drow["sotiendadung"].ToString()),
                            chot = drow["chot"].ToString(),
                            buaanid = drow["buaanid"].ToString(),
                            nhaanid = drow["nhaanid"].ToString(),
                            nhaan = drow["nhaan"].ToString(),
                            dangkybosung = drow["dangkybosung"].ToString()
                        };
                        baocom.Add(ck);
                    }
                }
            }
        }

        private async void UpdateCheckBaoCom(CheckBaoCom ck)
        {
            string pathfile = filecheck + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
            string info = filecheck + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".txt";
            string APIbaocom = fileApidlbc;
            try
            {
                using (var client = new HttpClient())
                {
                    var serializedProduct = JsonConvert.SerializeObject(ck);
                    var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                    var result = await client.PutAsync(String.Format("{0}/{1}", APIbaocom, ck.id), content);
                    if (result.IsSuccessStatusCode)
                    {
                        try
                        {
                                using (StreamWriter writer = new StreamWriter(info,true))
                                {
                                    writer.WriteLine(ck.manhansu + "-" + Convert.ToDateTime(ck.thoigiansudung).ToString("dd/MM/yy HH:mm:ss") + "-" + ck.bepanid);
                                }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "Update dữ liệu Client lỗi!");
                        }
                    }
                    else
                    {
                        try
                        {
                                using (StreamWriter writer = new StreamWriter(info,true))
                                {
                                    writer.WriteLine(ck.manhansu + "-" + Convert.ToDateTime(ck.thoigiansudung).ToString("dd/MM/yy HH:mm:ss") + "-" + ck.bepanid + "-NG1");
                                }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message + "Update dữ liệu Client lỗi!");
                        }
                    }
                }
            }
            catch (Exception)
            {
                try
                {
                        using (StreamWriter writer = new StreamWriter(info,true))
                        {
                            writer.WriteLine(ck.manhansu + "-" + Convert.ToDateTime(ck.thoigiansudung).ToString("dd/MM/yy HH:mm:ss") + "-" + ck.bepanid + "-NG1");
                        }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + "Update dữ liệu Client lỗi!");
                }
            }
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
                caan = " Buaphu";
            }
            foreach (BuaAn ba in buaan)
            {
                if (ba.ten == cbBuaan.Text)
                {
                    caanid = ba.id;
                }
            }
            GetNhaAnID();
            GetNhaBep();
            APICheckBaoCom = fileApidlbc + dateTimePicker1.Value.ToString("MM-dd-yyyy") + "/" + caanid;
            GetBaoCom();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            txtNhapSoluong.Visible = true;
        }

        private void txtNhapSoluong_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                int x;
                bool checkx = int.TryParse(txtNhapSoluong.Text, out x);
                if (checkx)
                {
                    txtNhapSoluong.Visible = false;
                    lbsosuatanconlai.Text = txtNhapSoluong.Text;
                    lbTong.Text = txtNhapSoluong.Text;
                    Tong = Convert.ToInt32(txtNhapSoluong.Text);
                    Conlai = Convert.ToInt32(txtNhapSoluong.Text);
                    getNumber();
                    txtNhapSoluong.Text = null;
                    lbsosuatanconlai.BackColor = Color.Green;
                    lbTong.BackColor = Color.Green;
                }
                else
                {
                    MessageBox.Show("Nhập số lượng suất ăn!");
                }
            }
        }

        private async void txtID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (!string.IsNullOrEmpty(txtID.Text))
                {
                    bool checkid = false;//không
                    try
                    {
                        string info = filecheck + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".txt";
                        FileStream fs = new FileStream(info, FileMode.Open, FileAccess.Read, FileShare.Read);
                        using (StreamReader sr = new StreamReader(fs))
                        {
                            string[] lines = sr.ReadToEnd().Split('\n');
                            if (lines.Count() > 0)
                            {
                                for (int i = 0; i < lines.Count(); i++)
                                {
                                    if (lines[i].Split('-')[0].Contains(txtID.Text))
                                    {
                                        checkid = true;//có
                                        getthoigian = lines[i].Split('-')[1];
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    List<CheckBaoCom> check = baocom.Where(x => x.manhansu == txtID.Text).ToList();
                    if (check.Count >= 1)
                    {
                        CheckBaoCom ck = new CheckBaoCom()
                        {
                            id = check.First().id,
                            empid = check.First().empid,
                            manhansu = check.First().manhansu,
                            hoten = check.First().hoten,
                            phongid = check.First().phongid,
                            phong = check.First().phong,
                            banid = check.First().banid,
                            ban = check.First().ban,
                            congdoanid = check.First().congdoanid,
                            congdoan = check.First().congdoan,
                            khach = check.First().khach,
                            ngay = check.First().ngay,
                            thang = check.First().thang,
                            nam = check.First().nam,
                            userid = check.First().userid,
                            thoigiandat = check.First().thoigiandat,
                            sudung = check.First().sudung,
                            dangky = check.First().dangky,
                            sotiendadung = check.First().sotiendadung,
                            chot = check.First().chot,
                            buaanid = check.First().buaanid,
                            nhaanid = check.First().nhaanid,
                            dangkybosung = check.First().dangkybosung,
                        };
                        if (check.First().sudung == "False" && checkid == false && check.First().nhaanid == idnhaan)
                        {
                            lbthongtinnv.Text = check.First().manhansu + "-" + check.First().hoten + "-" + check.First().phong + "-" + check.First().ban;
                            lbthongbao.Text = "OK";
                            checkok.Play();
                            checkok.Dispose();
                            lbthongbao.BackColor = Color.Green;
                            ck.sudung = "true";
                            ck.bepanid = nhabep;
                            ck.thoigiansudung = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            ck.soxuatandadung = Convert.ToInt32(check.First().soxuatandadung) + 1;
                            UpdateCheckBaoCom(ck);
                            txtID.Text = null;
                            lbthoigiansudung.Text = "Thành công: " + DateTime.Now.ToString("dd/MM/yy-HH:mm:ss");

                            if (lbsosuatanconlai.Text == "0")
                            {
                                lbsosuatanconlai.BackColor = Color.Red;
                                lbTong.BackColor = Color.Red;
                            }
                            else
                            {
                                lbsosuatanconlai.Text = (int.Parse(lbsosuatanconlai.Text) - 1).ToString();
                                Conlai = Convert.ToInt32(lbsosuatanconlai.Text);
                                if (lbsosuatanconlai.Text == "0")
                                {
                                    lbsosuatanconlai.BackColor = Color.Red;
                                    lbTong.BackColor = Color.Red;
                                }
                                getNumber();
                            }
                        }
                        else
                        {
                            string infolog = filelog + "log-" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".txt";
                            lbthongtinnv.Text = check.First().manhansu + "-" + check.First().hoten + "-" + check.First().phong + "-" + check.First().ban;
                            lbthongbao.Text = "NG";
                            checkng.Play();
                            checkng.Dispose();
                            lbthongbao.BackColor = Color.Yellow;
                            if (getthoigian != null && check.First().nhaanid == idnhaan)
                            {
                                lbthoigiansudung.Text = "Bạn đã lấy cơm lúc: " + getthoigian;
                            }
                            else if (check.First().nhaanid != idnhaan)
                            {
                                if (check.First().thoigiansudung != null)
                                {
                                    lbthoigiansudung.Text = "Bạn đã sử dụng cơm tại: [" + check.First().nhaan + "] Thời gian sử dụng:[" + check.First().thoigiansudung + "]";
                                    try
                                    {
                                        using (var writer = new StreamWriter(infolog, true))
                                        {
                                            writer.WriteLine(txtID.Text + "-" + lbthoigiansudung.Text);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                                else
                                {
                                    lbthoigiansudung.Text = "Bạn đã đăng ký cơm tại: [" + check.First().nhaan + "]. Mời bạn sang [" + check.First().nhaan + "] sử dụng cơm!";
                                    try
                                    {
                                        using (var writer = new StreamWriter(infolog, true))
                                        {
                                            writer.WriteLine(txtID.Text + "-" + lbthoigiansudung.Text);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                            }
                            else
                            {
                                lbthoigiansudung.Text = "Bạn đã chưa đăng ký cơm ";
                                try
                                {
                                    using (var writer = new StreamWriter(infolog, true))
                                    {
                                        writer.WriteLine(txtID.Text);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                            }
                            txtID.Text = null;
                        }
                    }
                    else
                    {
                        string infolog = filelog + "log-" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".txt";
                        lbthoigiansudung.Text = "Bạn chưa báo cơm. Vui lòng qua bàn đăng ký bổ sung!";
                        checkng.Play();
                        checkng.Dispose();
                        lbthongbao.Text = "NG";
                        lbthongbao.BackColor = Color.Red;
                        if (txtID.Text.Length >= 6)
                        {
                            bool checkRepeatID = false;//không trùng
                            for (int i = 0; i < IDChuaBaoCom.Count; i++)
                            {
                                if (IDChuaBaoCom[i] == txtID.Text)
                                {
                                    checkRepeatID = true;//trùng
                                    break;
                                }
                            }
                            if (checkRepeatID == false)
                            {
                                try
                                {
                                    using (var writer = new StreamWriter(infolog, true))
                                    {
                                        writer.WriteLine(txtID.Text);
                                    }
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show(ex.Message);
                                }
                                if (txtID.Text.Length == 6)
                                {
                                    await Task.Run(() => ThemNhanVienBaoCom(txtID.Text));
                                }
                                IDChuaBaoCom.Add(txtID.Text);
                                txtID.Text = null;
                                lbthongtinnv.Text = null;
                                if (lbsosuatanconlai.Text == "0")
                                {
                                    lbsosuatanconlai.BackColor = Color.Red;
                                    lbTong.BackColor = Color.Red;
                                }
                                else
                                {
                                    lbsosuatanconlai.Text = (int.Parse(lbsosuatanconlai.Text) - 1).ToString();
                                    Conlai = Convert.ToInt32(lbsosuatanconlai.Text);
                                    if (lbsosuatanconlai.Text == "0")
                                    {
                                        lbsosuatanconlai.BackColor = Color.Red;
                                        lbTong.BackColor = Color.Red;
                                    }
                                    getNumber();
                                }
                            }
                        }
                        txtID.Text = null;
                        lbthongtinnv.Text = null;

                    }
                }
            }
        }

        private void txtFontSize_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                int x;
                bool checkx = int.TryParse(txtFontSize.Text, out x);
                if (checkx)
                {
                    lbsosuatanconlai.Font = new Font(lbsosuatanconlai.Font.FontFamily, int.Parse(txtFontSize.Text));
                    lbTong.Font = new Font(lbTong.Font.FontFamily, int.Parse(txtFontSize.Text));
                }
                else
                {
                    MessageBox.Show("Nhập cỡ chữ!");
                }
            }
        }

        private void CheckCom_FormClosing(object sender, FormClosingEventArgs e)
        {
            hienthi.Close();
        }
        public void getNumber()
        {
            string textNumberTong = string.Format("{0:000}", Tong);
            string textNumberConlai = string.Format("{0:000}", Conlai);
            Image pic1 = ImageText(textNumberTong, 1);
            picturebox1.Image = Zoom(pic1, new Size(1, 90));
            Image pic2 = ImageText(textNumberConlai, 2);
            picturebox2.Image = Zoom(pic2, new Size(1, 90));
        }
        Image Zoom(Image img, Size size)
        {
            Bitmap bmp = new Bitmap(img, img.Width + (img.Width * size.Width / 100), img.Height + (img.Height * size.Height / 100));
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bmp;
        }
        Image ImageText(string textNumber, int number)
        {
            Bitmap bitmap1 = new Bitmap(1, 1);
            Font font = new Font("Microsoft Sans Serif", 400, FontStyle.Regular, GraphicsUnit.Pixel);
            Graphics grap = Graphics.FromImage(bitmap1);
            int width = (int)grap.MeasureString(textNumber, font).Width;
            int heigth = (int)grap.MeasureString(textNumber, font).Height;
            Bitmap bitmap2 = new Bitmap(bitmap1, new Size(width, heigth));
            grap = Graphics.FromImage(bitmap2);
            if (textNumber == "000")
            {
                grap.Clear(Color.Red);
            }
            else
            {
                if (number == 1)
                {
                    grap.Clear(Color.Red);
                }
                else
                {
                    grap.Clear(Color.Green);
                }

            }
            grap.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            grap.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            grap.DrawString(textNumber, font, new SolidBrush(Color.White), 0, 0);
            grap.Flush();
            grap.Dispose();
            Image i = (Image)bitmap2;
            return i;
        }
        private async void ThemNhanVienBaoCom(string IDnhanvien)
        {
            CheckBaoCom ck = new CheckBaoCom()
            {
                empid = null,
                manhansu = IDnhanvien,
                hoten = null,
                phongid = null,
                phong = null,
                banid = null,
                ban = null,
                congdoanid = null,
                congdoan = null,
                khach = "false",
                ngay = DateTime.Now.ToString("yyyy-MM-dd"),
                thang = DateTime.Now.Month,
                nam = DateTime.Now.Year,
                thoigiandat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                sudung = "true",
                dangky = "false",
                thoigiansudung = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                soxuatandadung = 1,
                sotiendadung = 0,
                chot = "false",
                ghichu = "Bổ sung báo cơm",
                buaanid = caanid,
                nhaanid = idnhaan,
                dangkybosung = "true",
                bepanid = nhabep
            };
            Task<string> callTask = Task.Run(() => GetThongTinNhanVien(fileApinv + IDnhanvien));
            callTask.Wait();
            string astr = callTask.Result;
            Thongtinnhanvien TT = JsonConvert.DeserializeObject<Thongtinnhanvien>(astr);
            if (TT != null)
            {
               ck.hoten = TT.hodem + " " + TT.ten;
               ck.empid = TT.id;
                try
                {
                    if (TT.phong_id != null)
                    {
                        ck.phongid = TT.phong_id;
                        string APIphong = fileApibp + TT.phong_id;
                        Task<string> callTaskPhong = Task.Run(() => GetThongTinNhanVien(APIphong));
                        callTaskPhong.Wait();
                        string astrPhong = callTaskPhong.Result;
                        string dataPhong = JObject.Parse(astrPhong)["bophan_ten"].ToString();
                        if (!string.IsNullOrEmpty(dataPhong))
                        {
                            ck.phong = dataPhong;
                        }
                    }
                }
                catch (Exception)
                {
                }
                try
                {
                    if (TT.ban_id != null)
                    {
                        ck.banid = TT.ban_id;
                        string APIban = fileApibp + TT.ban_id;
                        Task<string> callTaskBan = Task.Run(() => GetThongTinNhanVien(APIban));
                        callTaskBan.Wait();
                        string astrBan = callTaskBan.Result;
                        string dataBan = JObject.Parse(astrBan)["bophan_ten"].ToString();
                        if (!string.IsNullOrEmpty(dataBan))
                        {
                            ck.ban = dataBan;
                        }
                    }
                }
                catch (Exception)
                {
                }
                try
                {
                    if (TT.congdoan_id != null)
                    {
                        ck.congdoanid = TT.congdoan_id;
                        string APIcongdoan = fileApibp + TT.congdoan_id;
                        Task<string> callTaskCongdoan = Task.Run(() => GetThongTinNhanVien(APIcongdoan));
                        callTaskCongdoan.Wait();
                        string astrCongdoan = callTaskCongdoan.Result;
                        if (!string.IsNullOrEmpty(astrCongdoan))
                        {
                            string dataCongdoan = JObject.Parse(astrCongdoan)["bophan_ten"].ToString();
                            if (!string.IsNullOrEmpty(dataCongdoan))
                            {
                                ck.congdoan = dataCongdoan;
                            }
                        }
                    }
                }
                catch (Exception)
                {
                }
            }

            string APIbaocom = fileApidlbc;
            using (var client = new HttpClient())
            {
                var serializedProduct = JsonConvert.SerializeObject(ck);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(APIbaocom, content);
            }
        }
        private async Task<string> GetThongTinNhanVien(string path)
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync(path);
            return astr;
        }
    }
}