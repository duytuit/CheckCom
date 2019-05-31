using CheckCom_Version2.DTOs;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace CheckCom_Version2
{
    public partial class BoSungBaoCom : Form
    {
        private string caanid;
        private List<CheckBaoCom> baocom = new List<CheckBaoCom>();
        private List<BuaAn> buaan = new List<BuaAn>();
        private string caan = null;
        private string idphong = null;
        private string idban = null;
        private string idcongdoan = null;
        public BoSungBaoCom()
        {
            InitializeComponent();
            int Gio = DateTime.Now.Hour;
            GetBuaan();
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
                cbBuaan.Text = "Bữa phụ";
                caan = " Buaphu";
            }
            else
            {
                cbBuaan.Text = "Tối";
                caan = " Toi";
            }
        }

        private void BoSungBaoCom_Load(object sender, EventArgs e)
        {
            for (int i = 0; i < gvdanhsach.Rows.Count; i++)
            {
                gvdanhsach.Rows[i].Cells[0].Value = i + 1;
            }
            gvdanhsach.ClearSelection();
        }

        private void GetCheckCom()
        {
            baocom.Clear();
            try
            {
                string pathfile = Application.StartupPath + @"\DLNS\DuLieuNhanSu.xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
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
                            manhansu = drow["maid"].ToString(),
                            hoten = drow["hoten"].ToString(),
                            phong = drow["phong"].ToString(),
                            ban = drow["ban"].ToString(),
                            congdoan = drow["congdoan"].ToString()
                        };
                        baocom.Add(ck);
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void GetBuaan()
        {
            try
            {
                string pathfile = Application.StartupPath + @"\Buaan\BuaAn.xls";
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
                            ma = drow["ma"].ToString(),
                            ten = drow["ten"].ToString(),
                            ghichu = drow["ghichu"].ToString(),
                            loaibuaanid = drow["loaibuaanid"].ToString(),
                            loaibuaan = drow["loaibuaan"].ToString()
                        };
                        cbBuaan.Items.Add(ba.ten);
                        buaan.Add(ba);
                    }
                }
            }
            catch (Exception)
            {
                // MessageBox.Show("Không có dữ liệu bữa ăn!");
            }
        }

        private void txtID_Leave(object sender, EventArgs e)
        {
            //if (txtID.Text == "")
            //{
            //    txtID.Text = "ID";
            //    txtID.ForeColor = Color.Gray;
            //    txtID.Font = new Font(txtID.Font, FontStyle.Italic);
            //}
        }

        private void txtID_Enter(object sender, EventArgs e)
        {
            //if (txtID.Text == "ID")
            //{
            //    txtID.Text = null;
            //    txtID.ForeColor = Color.Black;
            //    txtID.Font = new Font(txtID.Font, FontStyle.Regular);
            //}
        }

        private void txtTennv_Leave(object sender, EventArgs e)
        {
            if (txtTennv.Text == "")
            {
                txtTennv.Text = "Họ tên";
                txtTennv.ForeColor = Color.Gray;
                txtTennv.Font = new Font(txtTennv.Font, FontStyle.Italic);
            }
        }

        private void txtTennv_Enter(object sender, EventArgs e)
        {
            if (txtTennv.Text == "Họ tên")
            {
                txtTennv.Text = "";
                txtTennv.ForeColor = Color.Black;
                txtTennv.Font = new Font(txtTennv.Font, FontStyle.Regular);
            }
        }

        private void txtphong_Leave(object sender, EventArgs e)
        {
            if (txtphong.Text == "")
            {
                txtphong.Text = "Phòng";
                txtphong.ForeColor = Color.Gray;
                txtphong.Font = new Font(txtphong.Font, FontStyle.Italic);
            }
        }

        private void txtphong_Enter(object sender, EventArgs e)
        {
            if (txtphong.Text == "Phòng")
            {
                txtphong.Text = "";
                txtphong.ForeColor = Color.Black;
                txtphong.Font = new Font(txtphong.Font, FontStyle.Regular);
            }
        }

        private void txtban_Leave(object sender, EventArgs e)
        {
            if (txtban.Text == "")
            {
                txtban.Text = "Ban";
                txtban.ForeColor = Color.Gray;
                txtban.Font = new Font(txtban.Font, FontStyle.Italic);
            }
        }

        private void txtban_Enter(object sender, EventArgs e)
        {
            if (txtban.Text == "Ban")
            {
                txtban.Text = "";
                txtban.ForeColor = Color.Black;
                txtban.Font = new Font(txtban.Font, FontStyle.Regular);
            }
        }

        private void txtcongdoan_Leave(object sender, EventArgs e)
        {
            if (txtcongdoan.Text == "")
            {
                txtcongdoan.Text = "Công đoạn";
                txtcongdoan.ForeColor = Color.Gray;
                txtcongdoan.Font = new Font(txtcongdoan.Font, FontStyle.Italic);
            }
        }

        private void txtcongdoan_Enter(object sender, EventArgs e)
        {
            if (txtcongdoan.Text == "Công đoạn")
            {
                txtcongdoan.Text = "";
                txtcongdoan.ForeColor = Color.Black;
                txtcongdoan.Font = new Font(txtcongdoan.Font, FontStyle.Regular);
            }
        }

        private void txtLydo_Leave(object sender, EventArgs e)
        {
            if (txtLydo.Text == "")
            {
                txtLydo.Text = "Lý do bổ sung";
                txtLydo.ForeColor = Color.Gray;
                txtLydo.Font = new Font(txtLydo.Font, FontStyle.Italic);
            }
        }

        private void txtLydo_Enter(object sender, EventArgs e)
        {
            if (txtLydo.Text == "Lý do bổ sung")
            {
                txtLydo.Text = "";
                txtLydo.ForeColor = Color.Black;
                txtLydo.Font = new Font(txtLydo.Font, FontStyle.Regular);
            }
        }

        private async void txtID_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(70);
            try
            {
                HttpClient aClient = new HttpClient();
                string astr = await aClient.GetStringAsync("http://192.84.100.207/AsoftAPI/E00003/GetByCode/" + txtID.Text + "");
                Thongtinnhanvien TT = JsonConvert.DeserializeObject<Thongtinnhanvien>(astr);
                if (TT != null)
                {
                    txtTennv.Text = TT.hodem + " " + TT.ten;
                    txtTennv.ForeColor = Color.Black;
                    txtTennv.Font = new Font(txtTennv.Font, FontStyle.Regular);
                    try
                    {
                        if(TT.phong_id!=null)
                        {
                            idphong = TT.phong_id;
                            string APIphong = "http://192.84.100.207/AsoftAPI/EC0002/" + TT.phong_id + "";
                            HttpClient aClientPhong = new HttpClient();
                            string astrPhong = await aClientPhong.GetStringAsync(APIphong);
                            string dataPhong = JObject.Parse(astrPhong)["bophan_ten"].ToString();
                            if (!string.IsNullOrEmpty(dataPhong))
                            {
                                txtphong.Text = dataPhong;
                                txtphong.ForeColor = Color.Black;
                                txtphong.Font = new Font(txtphong.Font, FontStyle.Regular);
                            }
                        }
                    }
                    catch (AggregateException)
                    {
                    }
                    try
                    {
                        if(TT.ban_id!=null)
                        {
                            idban = TT.ban_id;
                            string APIban = "http://192.84.100.207/AsoftAPI/EC0002/" + TT.ban_id + "";
                            HttpClient aClientBan = new HttpClient();
                            string astrBan = await aClientBan.GetStringAsync(APIban);
                            string dataBan = JObject.Parse(astrBan)["bophan_ten"].ToString();
                            if (!string.IsNullOrEmpty(dataBan))
                            {
                                txtban.Text = dataBan;
                                txtban.ForeColor = Color.Black;
                                txtban.Font = new Font(txtban.Font, FontStyle.Regular);
                            }
                        }
                      
                    }
                    catch (AggregateException)
                    {
                    }
                    try
                    {
                        if(TT.congdoan_id!=null)
                        {
                            idcongdoan = TT.congdoan_id;
                            string APIcongdoan = "http://192.84.100.207/AsoftAPI/EC0002/" + TT.congdoan_id + "";
                            HttpClient aClientCongdoan = new HttpClient();
                            string astrCongdoan = await aClientCongdoan.GetStringAsync(APIcongdoan);
                            if (!string.IsNullOrEmpty(astrCongdoan))
                            {
                                string dataCongdoan =JObject.Parse(astrCongdoan)["bophan_ten"].ToString();
                                if (!string.IsNullOrEmpty(dataCongdoan))
                                {
                                    txtcongdoan.Text = dataCongdoan;
                                    txtcongdoan.ForeColor = Color.Black;
                                    txtcongdoan.Font = new Font(txtcongdoan.Font, FontStyle.Regular);
                                }
                            }
                        }
                    }
                    catch (AggregateException)
                    {
                    }
                }
                else
                {
                    txtID.Text = null;
                    ClearText();
                }
            }
            catch (AggregateException)
            {
                txtID.Text = null;
                ClearText();
            }
            #region faile
            //=========================================================
            //if (baocom.Count > 0)
            //{
            //    foreach (CheckBaoCom ck in baocom)
            //    {
            //        if (ck.manhansu == txtID.Text)
            //        {
            //            txtTennv.Text = ck.hoten;
            //            txtphong.Text = ck.phong;
            //            txtban.Text = ck.ban;
            //            txtcongdoan.Text = ck.congdoan;
            //            txtTennv.ForeColor = Color.Black;
            //            txtTennv.Font = new Font(txtTennv.Font, FontStyle.Regular);
            //            txtphong.ForeColor = Color.Black;
            //            txtphong.Font = new Font(txtphong.Font, FontStyle.Regular);
            //            txtban.ForeColor = Color.Black;
            //            txtban.Font = new Font(txtban.Font, FontStyle.Regular);
            //            txtcongdoan.ForeColor = Color.Black;
            //            txtcongdoan.Font = new Font(txtcongdoan.Font, FontStyle.Regular);
            //            break;
            //        }
            //    }
            //}
            #endregion

        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtID.Text) && !string.IsNullOrEmpty(txtTennv.Text) && !string.IsNullOrEmpty(txtphong.Text) && !string.IsNullOrEmpty(txtban.Text) && !string.IsNullOrEmpty(txtcongdoan.Text) && !string.IsNullOrEmpty(txtLydo.Text))
            {
                if (txtID.Text == "ID" && txtTennv.Text == "Họ tên")
                {
                    MessageBox.Show("Không để trống các trường!");
                }
                else
                {
                    try
                    {
                        string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                        FileInfo filename = new FileInfo(pathfile);
                        DataTable table = new DataTable();
                        System.Data.OleDb.OleDbConnection MyConnection;
                        MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                        MyConnection.Open();
                        OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                        oada.Fill(table);
                        MyConnection.Close();
                        bool check = false;
                        for (int j = 0; j < table.Rows.Count; j++)
                        {
                            if (txtID.Text == table.Rows[j]["manhansu"].ToString())
                            {
                                check = true;
                                break;
                            }
                        }
                        if (check == false)
                        {
                            Excel.Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
                            dynamic workbooksExcel = docExcel.Workbooks.Open(pathfile);
                            var worksheetExcel = (Excel._Worksheet)workbooksExcel.ActiveSheet;
                            int i = table.Rows.Count;
                            worksheetExcel.Cells[i + 2, 2] = txtID.Text;
                            worksheetExcel.Cells[i + 2, 3] = txtID.Text;
                            worksheetExcel.Cells[i + 2, 4] = txtTennv.Text;
                            worksheetExcel.Cells[i + 2, 5] = idphong;
                            worksheetExcel.Cells[i + 2, 6] = txtphong.Text;
                            worksheetExcel.Cells[i + 2, 7] = idban;
                            worksheetExcel.Cells[i + 2, 8] = txtban.Text;
                            worksheetExcel.Cells[i + 2, 9] = idcongdoan;
                            worksheetExcel.Cells[i + 2, 10] = txtcongdoan.Text;
                            worksheetExcel.Cells[i + 2, 11] = "FALSE";
                            worksheetExcel.Cells[i + 2, 12] = dateTimePicker1.Value.ToString();
                            worksheetExcel.Cells[i + 2, 13] = dateTimePicker1.Value.Month;
                            worksheetExcel.Cells[i + 2, 14] = dateTimePicker1.Value.Year;
                            worksheetExcel.Cells[i + 2, 16] = dateTimePicker1.Value.ToString();
                            worksheetExcel.Cells[i + 2, 17] = "FALSE";
                            worksheetExcel.Cells[i + 2, 18] = "FALSE";
                            worksheetExcel.Cells[i + 2, 20] = 0;
                            worksheetExcel.Cells[i + 2, 21] = 0;
                            worksheetExcel.Cells[i + 2, 22] = "FALSE";
                            worksheetExcel.Cells[i + 2, 23] = txtLydo.Text;
                            worksheetExcel.Cells[i + 2, 27] = caanid;
                            worksheetExcel.Cells[i + 2, 28] = cbBuaan.Text;
                            worksheetExcel.Cells[i + 2, 35] = "TRUE";
                            worksheetExcel.Cells[i + 2, 37] = "NG";
                            workbooksExcel.Save();
                            workbooksExcel.Close();
                            docExcel.Application.Quit();
                            GetBoSungToGridView();
                            for (int j = 0; j < gvdanhsach.Rows.Count; j++)
                            {
                                gvdanhsach.Rows[j].Cells[0].Value = j + 1;
                            }
                            MessageBox.Show("Thêm thành công!");
                            ClearText();
                        }
                        else
                        {
                            MessageBox.Show("Bạn đã báo cơm rồi!");
                        }
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Bạn chưa tạo dữ liệu Client!");
                    }
                }
            }
            else
            {
                MessageBox.Show("Không để trống các trường!");
            }
        }

        private void GetBoSungToGridView()
        {
            gvdanhsach.DataSource = null;
            try
            {
                string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$] where trangthai2='NG'", MyConnection);
                oada.Fill(table);
                MyConnection.Close();
                gvdanhsach.AutoGenerateColumns = false;
                gvdanhsach.DataSource = table;
                gvdanhsach.ClearSelection();
            }
            catch (Exception)
            {
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            try
            {
                string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                oada.Fill(table);
                MyConnection.Close();
                bool isSelected = false;
                string MessageBoxTitle = "Thông báo";
                string MessageBoxContent = "Bạn có muốn xóa không?";

                DialogResult dialogResult = MessageBox.Show(MessageBoxContent, MessageBoxTitle, MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    for (int i = gvdanhsach.Rows.Count - 1; i >= 0; i--)
                    {
                        isSelected = Convert.ToBoolean(gvdanhsach.Rows[i].Cells["check"].Value);
                        if (isSelected)
                        {
                            for (int j = table.Rows.Count - 1; j >= 0; j--)
                            {
                                DataRow drow = table.Rows[j];
                                if (gvdanhsach.Rows[i].Cells["manhansu"].Value.ToString() == drow["manhansu"].ToString())
                                {
                                    if (drow["sudung"].ToString() == "False")
                                    {
                                        DeleteRowExcel(j + 2);
                                    }
                                    else
                                    {
                                        MessageBox.Show("Xuất ăn đã dùng! Bạn không được xóa!");
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
                GetBoSungToGridView();
                for (int z = 0; z < gvdanhsach.Rows.Count; z++)
                {
                    gvdanhsach.Rows[z].Cells[0].Value = z + 1;
                }
                ClearText();
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn chưa tạo dữ liệu Client!");
            }
        }

        private void DeleteRowExcel(int RowExcel)
        {
            string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
            Excel._Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
            dynamic workbooksExcel = docExcel.Workbooks.Open(pathfile);
            var worksheetExcel = (Excel._Worksheet)workbooksExcel.ActiveSheet;
            ((Excel.Range)worksheetExcel.Rows[RowExcel, Missing.Value]).Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
            workbooksExcel.Save();
            workbooksExcel.Close(false);
            docExcel.Application.Quit();
        }

        private void ClearText()
        {
           
            txtTennv.Text = "Họ tên";
            txtphong.Text = "Phòng";
            txtban.Text = "Ban";
            txtcongdoan.Text = "Công đoạn";
            txtLydo.Text = "Lý do bổ sung";
            txtTennv.ForeColor = Color.Gray;
            txtTennv.Font = new Font(txtTennv.Font, FontStyle.Italic);
            txtphong.ForeColor = Color.Gray;
            txtphong.Font = new Font(txtphong.Font, FontStyle.Italic);
            txtban.ForeColor = Color.Gray;
            txtban.Font = new Font(txtban.Font, FontStyle.Italic);
            txtcongdoan.ForeColor = Color.Gray;
            txtcongdoan.Font = new Font(txtcongdoan.Font, FontStyle.Italic);
            txtLydo.ForeColor = Color.Gray;
            txtLydo.Font = new Font(txtLydo.Font, FontStyle.Italic);
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save Excel Files";

            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "Excel Files (*.xls)|*.xls|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.FileName = "Bosung" + DateTime.Now.ToString("yyMMdd-HHmm");
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo filename = new FileInfo(saveFileDialog1.FileName);
                Excel.Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
                Excel.Workbook wb = docExcel.Workbooks.Add(Type.Missing);
                Excel.Worksheet ws = (Excel.Worksheet)docExcel.ActiveSheet;

                ws.Cells[1, 1] = "TT";
                ws.Cells[1, 2] = "Mã NV";
                ws.Cells[1, 3] = "Họ Tên";
                ws.Cells[1, 4] = "Phòng";
                ws.Cells[1, 5] = "Ban";
                ws.Cells[1, 6] = "Công đoạn";
                ws.Cells[1, 7] = "Lý do bổ sung";

                var data = new object[gvdanhsach.Rows.Count, gvdanhsach.Columns.Count - 1];
                for (int row = 0; row < gvdanhsach.Rows.Count; row++)
                {
                    for (int column = 0; column <= gvdanhsach.Columns.Count - 2; column++)
                    {
                        data[row, column] = gvdanhsach.Rows[row].Cells[column].Value;
                    }
                }

                var startCell = (Excel.Range)ws.Cells[2, 1];
                var endCell = (Excel.Range)ws.Cells[gvdanhsach.Rows.Count + 1, gvdanhsach.Columns.Count - 1];
                var writeRange = ws.Range[startCell, endCell];
                ws.Columns[2].NumberFormat = "@";
                writeRange.Value2 = data;
                wb.SaveAs(filename.FullName, Excel.XlFileFormat.xlTemplate);
                wb.Close();
                docExcel.Application.Quit();
                MessageBox.Show("Thành Công!");
            }
            else
            {
                return;
            }
        }

        private void gvdanhsach_DoubleClick(object sender, EventArgs e)
        {
            gvdanhsach.ClearSelection();
            ClearText();
        }

        private void gvdanhsach_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }
            DataGridViewRow row = gvdanhsach.Rows[e.RowIndex];
            txtID.Text = row.Cells["manhansu"].Value.ToString();
            txtTennv.Text = row.Cells["hoten"].Value.ToString();
            txtphong.Text = row.Cells["phong"].Value.ToString();
            txtban.Text = row.Cells["ban"].Value.ToString();
            txtcongdoan.Text = row.Cells["congdoan"].Value.ToString();
            txtLydo.Text = row.Cells["lydo"].Value.ToString();
            txtID.ForeColor = Color.Black;
            txtID.Font = new Font(txtID.Font, FontStyle.Regular);
            txtTennv.ForeColor = Color.Black;
            txtTennv.Font = new Font(txtTennv.Font, FontStyle.Regular);
            txtphong.ForeColor = Color.Black;
            txtphong.Font = new Font(txtphong.Font, FontStyle.Regular);
            txtban.ForeColor = Color.Black;
            txtban.Font = new Font(txtban.Font, FontStyle.Regular);
            txtcongdoan.ForeColor = Color.Black;
            txtcongdoan.Font = new Font(txtcongdoan.Font, FontStyle.Regular);
            txtLydo.ForeColor = Color.Black;
            txtLydo.Font = new Font(txtLydo.Font, FontStyle.Regular);
        }

        private void btnCapnhap_Click(object sender, EventArgs e)
        {
            try
            {
                string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnectionup;
                System.Data.OleDb.OleDbCommand myCommandup = new System.Data.OleDb.OleDbCommand();
                string sqlup = null;
                MyConnectionup = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnectionup.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnectionup);
                oada.Fill(table);
                myCommandup.Connection = MyConnectionup;
                bool icheck = false;
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    if (txtID.Text == table.Rows[i]["manhansu"].ToString())
                    {
                        icheck = true;
                        if (table.Rows[i]["sudung"].ToString() == "False")
                        {
                            sqlup = "update [Sheet1$] set phong='" + txtphong.Text + "',ban='" + txtban.Text + "'  ,congdoan='" + txtcongdoan.Text + "',ghichu='" + txtLydo.Text + "' where manhansu='" + txtID.Text + "'";
                            myCommandup.CommandText = sqlup;
                            myCommandup.ExecuteNonQuery();
                            MyConnectionup.Close();
                            MessageBox.Show("Cập nhập thành công!");
                            GetBoSungToGridView();
                            for (int z = 0; z < gvdanhsach.Rows.Count; z++)
                            {
                                gvdanhsach.Rows[z].Cells[0].Value = z + 1;
                            }
                            ClearText();
                            break;
                        }
                        else
                        {
                            MessageBox.Show("Xuất ăn đã dùng! Bạn không được sửa!");
                            ClearText();
                            break;
                        }
                    }
                }
                if (icheck == false)
                {
                    MessageBox.Show("ID không tồn tại!");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Bạn chưa tạo dữ liệu Client!");
            }
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
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
            bool icheck = CheckData();
            if (icheck == true)
            {
                GetBoSungToGridView();
            }
            for (int i = 0; i < gvdanhsach.Rows.Count; i++)
            {
                gvdanhsach.Rows[i].Cells[0].Value = i + 1;
            }
            gvdanhsach.ClearSelection();
        }

        private bool CheckData()
        {
            bool kiemtrabaocom = false;
            string fileToRead = System.IO.Path.GetDirectoryName(Application.StartupPath + @"\CheckCom\");

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
    }
}