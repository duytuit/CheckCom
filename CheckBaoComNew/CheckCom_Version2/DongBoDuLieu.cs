using CheckCom_Version2.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace CheckCom_Version2
{
    public partial class DongBoDuLieu : Form
    {
        private string APICheckBaoCom = null;
        private List<CheckBaoCom> baocom = new List<CheckBaoCom>();
        private List<BuaAn> buaan = new List<BuaAn>();
        private string caan = null;
        private string caanid;
        private string idnhaan;

        public DongBoDuLieu()
        {
            InitializeComponent();
            GetBuaan();
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
                cbBuaan.Text = "Bữa phụ";
                caan = " Buaphu";
            }
            else
            {
                cbBuaan.Text = "Tối";
                caan = " Toi";
            }
            GetNhaAnID();
        }

        private void GetNhaAnID()
        {
            try
            {
                string pathfile = Application.StartupPath + @"\Nhaan\NhaAn.xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                oada.Fill(table);
                MyConnection.Close();
                    if (table.Rows.Count==1)
                    {
                        idnhaan = table.Rows[0]["nhaanid"].ToString();
                    }
            }
            catch (Exception)
            {
            }
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

        private bool CheckBuaan()
        {
            bool kiemtrabuaan = false;
            string fileToRead = System.IO.Path.GetDirectoryName(Application.StartupPath + @"\Buaan\");

            DirectoryInfo dinfo = new DirectoryInfo(fileToRead);
            FileInfo[] Files = dinfo.GetFiles("*");
            foreach (FileInfo file in Files)
            {
                var path = new TestPath(file);
                if (path.ToString() == "BuaAn")
                {
                    kiemtrabuaan = true;
                    break;
                }
            }
            return kiemtrabuaan;
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

        private void kiemtratrangthai()
        {
            try
            {
                GetDataClient();
             
                btnCapNhap.Enabled = true;
            }
            catch (Exception)
            {
               
                btnCapNhap.Enabled = false;
            }
        }

        private void DongBoDuLieu_Load(object sender, EventArgs e)
        {
            lvServer.View = View.Details;
            lvServer.FullRowSelect = true;
            lvClient.View = View.Details;
            lvClient.FullRowSelect = true;
            lvDongbo.View = View.Details;
            lvDongbo.FullRowSelect = true;
        }

        private async Task<string> GetAllCheckBaoCom()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync(APICheckBaoCom);
            return astr;
        }

        private async Task<string> GetAllBuaan()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync("http://192.84.100.207/MealOrdersAPI/api/BuaAns");
            return astr;
        }

        private void GetBuaan()
        {
            buaan.Clear();
            try
            {
                cbBuaan.Items.Clear();
                Task<string> callTask = Task.Run(() => GetAllBuaan());
                callTask.Wait();
                string astr = callTask.Result;
                buaan = JsonConvert.DeserializeObject<List<BuaAn>>(astr);
                if (buaan.Count > 0)
                {
                    foreach (BuaAn ba in buaan)
                    {
                        cbBuaan.Items.Add(ba.ten);
                    }
                    bool check = CheckBuaan();
                    if (check == true)
                    {
                        // update
                        string pathfile = Application.StartupPath + @"\Buaan\BuaAn.xls";
                        Task<string> callTask1 = Task.Run(() => GetAllBuaan());
                        callTask1.Wait();
                        string astr1 = callTask1.Result;
                        DataTable dt = (DataTable)JsonConvert.DeserializeObject(astr1, typeof(DataTable));
                        Excel._Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
                        dynamic workbooksExcel = docExcel.Workbooks.Open(pathfile);
                        var worksheetExcel = (Excel._Worksheet)workbooksExcel.ActiveSheet;

                        var data = new object[dt.Rows.Count, dt.Columns.Count];
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            for (int column = 0; column <= dt.Columns.Count - 1; column++)
                            {
                                data[row, column] = dt.Rows[row][column].ToString();
                            }
                        }

                        var startCell = (Excel.Range)worksheetExcel.Cells[2, 1];
                        var endCell = (Excel.Range)worksheetExcel.Cells[dt.Rows.Count + 1, dt.Columns.Count];
                        var endCell1 = (Excel.Range)worksheetExcel.Cells[dt.Rows.Count + 10, dt.Columns.Count];
                        worksheetExcel.Range[startCell, endCell1].Clear();
                        var writeRange = worksheetExcel.Range[startCell, endCell];
                        writeRange.Value2 = data;
                        workbooksExcel.Save();
                        workbooksExcel.Close();
                        docExcel.Application.Quit();
                    }
                    else
                    {
                        // insert
                        Task<string> callTask1 = Task.Run(() => GetAllBuaan());
                        callTask1.Wait();
                        string astr1 = callTask1.Result;
                        DataTable dt = (DataTable)JsonConvert.DeserializeObject(astr1, typeof(DataTable));
                        string pathfile = Application.StartupPath + @"\Buaan\BuaAn.xls";
                        FileInfo filename = new FileInfo(pathfile);
                        Excel.Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
                        Excel.Workbook wb = docExcel.Workbooks.Add(Type.Missing);
                        Excel.Worksheet ws = (Excel.Worksheet)docExcel.ActiveSheet;
                        ws.Cells[1, 1] = "id";
                        ws.Cells[1, 2] = "ma";
                        ws.Cells[1, 3] = "ten";
                        ws.Cells[1, 4] = "ghichu";
                        ws.Cells[1, 5] = "loaibuaanid";
                        ws.Cells[1, 6] = "loaibuaan";
                        ws.Cells[1, 7] = "thutuhienthi";

                        var data = new object[dt.Rows.Count, dt.Columns.Count];
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            for (int column = 0; column <= dt.Columns.Count - 1; column++)
                            {
                                data[row, column] = dt.Rows[row][column].ToString();
                            }
                        }

                        var startCell = (Excel.Range)ws.Cells[2, 1];
                        var endCell = (Excel.Range)ws.Cells[dt.Rows.Count + 1, dt.Columns.Count];
                        var writeRange = ws.Range[startCell, endCell];
                        writeRange.Value2 = data;
                        wb.SaveAs(filename.FullName, Excel.XlFileFormat.xlExcel8);
                        wb.Close();
                        docExcel.Application.Quit();
                    }
                }
            }
            catch (Exception)
            {
                //MessageBox.Show("Lỗi đường truyền");
                cbBuaan.Items.Clear();
                bool check = CheckBuaan();
                if (check == true)
                {
                    string pathfile = Application.StartupPath + @"\Buaan\BuaAn.xls";
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
                            BuaAn ba = new BuaAn()
                            {
                                ten = drow["ten"].ToString()
                            };
                            cbBuaan.Items.Add(ba.ten);
                        }
                    }
                }
            }
        }

        private void GetBaoCom()
        {
            baocom.Clear();
            lbServer.Text = "Dữ liệu đăng ký cơm : 0";
            lvServer.Items.Clear();
            try
            {
                lbServer.Text = null;
                Task<string> callTask = Task.Run(() => GetAllCheckBaoCom());
                callTask.Wait();
                string astr = callTask.Result;
                baocom = JsonConvert.DeserializeObject<List<CheckBaoCom>>(astr);
                lbServer.Text = "Dữ liệu đăng ký cơm : " + baocom.Count.ToString();
                baocom.Sort(delegate (CheckBaoCom x, CheckBaoCom y)
                {
                    if (x.manhansu == null && y.manhansu == null) return 0;
                    else if (x.manhansu == null) return -1;
                    else if (y.manhansu == null) return 1;
                    else return x.manhansu.CompareTo(y.manhansu);
                });
                foreach (CheckBaoCom ck in baocom)
                {
                    ListViewItem lvi = new ListViewItem(ck.manhansu);
                    lvi.SubItems.Add(ck.hoten);
                    lvi.SubItems.Add(ck.phong);
                    lvi.SubItems.Add(ck.ban);
                    lvi.SubItems.Add(ck.congdoan);
                    lvi.SubItems.Add(ck.khach);
                    lvi.SubItems.Add(ck.ngay.ToString());
                    lvi.SubItems.Add(ck.thang.ToString());
                    lvi.SubItems.Add(ck.nam.ToString());
                    // lvi.SubItems.Add(ck.userid.ToString());
                    lvi.SubItems.Add(ck.thoigiandat.ToString());
                    lvi.SubItems.Add(ck.sudung);
                    lvi.SubItems.Add(ck.dangky);
                    lvi.SubItems.Add(ck.thoigiansudung);
                    lvi.SubItems.Add(ck.soxuatandadung.ToString());
                    lvi.SubItems.Add(ck.chot);
                    lvi.SubItems.Add(ck.ghichu);
                    lvi.SubItems.Add(ck.buaan);
                    lvServer.Items.Add(lvi);
                }
                if (lvServer.Items.Count == 0)
                {
                    MessageBox.Show("Chưa có dữ liệu!");
                    lvClient.Items.Clear();
                    lbClient.Text = "Dữ liệu Client : 0";
                    lvDongbo.Items.Clear();
                    lbChuadongbo.Text = "Dữ liệu chưa đồng bộ : 0";
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Lỗi đường truyền");
                lbServer.Text = "Lỗi đường truyền!";
                btnCapNhap.Enabled = false;
                btnDongBo.Enabled = false;
            }
        }

        private void lvServer_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void GetDataClient()
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
                lvClient.Items.Clear();
                lbClient.Text = null;
                lbClient.Text = "Dữ liệu Client : " + table.Rows.Count.ToString();
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
                        lvClient.Items.Add(lvi);
                    }
                }
               
                btnCapNhap.Enabled = true;
            }
            catch (Exception)
            {
                lvClient.Items.Clear();
                lbClient.Text = null;
              
                btnCapNhap.Enabled = false;
                lbClient.Text = "Dữ liệu Client : 0";
            }
        }

        private void btnCapNhap_Click(object sender, EventArgs e)
        {
            string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
            if(lvDongbo.Items.Count==0)
            {
                if (baocom.Count > 0)
                {
                    DataTable table = new DataTable();
                    System.Data.OleDb.OleDbConnection MyConnection;
                    //System.Data.OleDb.OleDbCommand myCommandup = new System.Data.OleDb.OleDbCommand();
                    //string sqlup = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                    MyConnection.Open();
                    OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$] where trangthai2='NG'", MyConnection);
                    oada.Fill(table);
                    //myCommandup.Connection = MyConnection;

                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow drow = table.Rows[i];
                        bool check1 = false;
                        if (drow.RowState != DataRowState.Deleted)
                        {
                            CheckBaoCom ck = new CheckBaoCom()
                            {
                                empid = drow["empid"].ToString(),
                                manhansu = drow["manhansu"].ToString(),
                                hoten = drow["hoten"].ToString(),
                                phongid = string.IsNullOrEmpty(drow["phongid"].ToString()) ? null : drow["phongid"].ToString(),
                                phong = string.IsNullOrEmpty(drow["phong"].ToString()) ? null : drow["phong"].ToString(),
                                banid = string.IsNullOrEmpty(drow["banid"].ToString()) ? null : drow["banid"].ToString(),
                                ban = string.IsNullOrEmpty(drow["ban"].ToString()) ? null : drow["ban"].ToString(),
                                congdoanid = string.IsNullOrEmpty(drow["congdoanid"].ToString()) ? null : drow["congdoanid"].ToString(),
                                congdoan = string.IsNullOrEmpty(drow["congdoan"].ToString()) ? null : drow["congdoan"].ToString(),
                                khach = drow["khach"].ToString(),
                                ngay = string.IsNullOrEmpty(drow["ngay"].ToString()) ? null : Convert.ToDateTime(drow["ngay"].ToString()).ToString("yyyy-MM-dd"),
                                thang = int.Parse(drow["thang"].ToString()),
                                nam = int.Parse(drow["nam"].ToString()),
                                thoigiandat = string.IsNullOrEmpty(drow["thoigiandat"].ToString()) ? null : Convert.ToDateTime(drow["thoigiandat"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                sudung = drow["sudung"].ToString(),
                                dangky = drow["dangky"].ToString(),
                                thoigiansudung = string.IsNullOrEmpty(drow["thoigiansudung"].ToString()) ? null : Convert.ToDateTime(drow["thoigiansudung"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                soxuatandadung = 0,
                                sotiendadung = 0,
                                chot = drow["chot"].ToString(),
                                ghichu = drow["ghichu"].ToString(),
                                buaanid = drow["buaanid"].ToString(),
                                nhaanid = idnhaan,
                                dangkybosung = drow["dangkybosung"].ToString()
                            };
                            check1 = Task.Run(() => InsertCheckBaoCom(ck)).Result;
                        }
                        //if (check1 == true)
                        //{
                        //    sqlup = "update [Sheet1$] set trangthai1='', trangthai2='' where manhansu='" + drow["manhansu"].ToString() + "'";
                        //    myCommandup.CommandText = sqlup;
                        //    myCommandup.ExecuteNonQuery();

                        //}
                    }
                    MyConnection.Close();
                     //GetBaoCom();
                    Task<string> callTask = Task.Run(() => GetAllCheckBaoCom());
                    callTask.Wait();
                    string astr = callTask.Result;
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(astr, typeof(DataTable));
                    Excel._Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
                    dynamic workbooksExcel = docExcel.Workbooks.Open(pathfile);
                    var worksheetExcel = (Excel._Worksheet)workbooksExcel.ActiveSheet;
                    var data = new object[dt.Rows.Count, dt.Columns.Count];
                    for (int row = 0; row < dt.Rows.Count; row++)
                    {
                        for (int column = 0; column <= dt.Columns.Count - 1; column++)
                        {
                            data[row, column] = dt.Rows[row][column].ToString();
                        }
                    }

                    var startCell = (Excel.Range)worksheetExcel.Cells[2, 1];
                    var endCell = (Excel.Range)worksheetExcel.Cells[dt.Rows.Count + 1, dt.Columns.Count];
                    //var endCell1 = (Excel.Range)worksheetExcel.Cells[dt.Rows.Count + 50, dt.Columns.Count + 1];
                    //worksheetExcel.Range[startCell, endCell1].Clear();
                    var writeRange = worksheetExcel.Range[startCell, endCell];
                    worksheetExcel.Columns[3].NumberFormat = "@";
                    worksheetExcel.Columns[19].NumberFormat = "@";
                    writeRange.Value2 = data;
                    workbooksExcel.Save();
                    workbooksExcel.Close();
                    docExcel.Application.Quit();
                    GetBaoCom();
                    kiemtratrangthai();
                }
            }
            else
            {
                MessageBox.Show("Chưa đồng bộ dữ liệu!");
            }
           
        }

        private async Task<bool> UpdateCheckBaoCom(CheckBaoCom ck)
        {
            bool check = false;
            string APIbaocom = "http://192.84.100.207/MealOrdersAPI/api/DulieuBaoComs";
            using (var client = new HttpClient())
            {
                var response = new HttpResponseMessage(HttpStatusCode.OK);
                var serializedProduct = JsonConvert.SerializeObject(ck);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                var result = await client.PutAsync(String.Format("{0}/{1}", APIbaocom, ck.id), content);
                if (result.IsSuccessStatusCode)
                {
                    check = true;
                }
            }
            return await Task.FromResult(check);
        }

        private async Task<bool> InsertCheckBaoCom(CheckBaoCom ck)
        {
            bool check = false;
            string APIbaocom = "http://192.84.100.207/MealOrdersAPI/api/DulieuBaoComs";
            using (var client = new HttpClient())
            {
                var serializedProduct = JsonConvert.SerializeObject(ck);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(APIbaocom, content);
                if (result.IsSuccessStatusCode)
                {
                    check = true;
                }
            }
            return await Task.FromResult(check);
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
            APICheckBaoCom = "http://192.84.100.207/MealOrdersAPI/api/DulieuBaoComs/" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + "/" + caanid;
            GetBaoCom();

            bool Check = CheckData();
            if (Check == true)
            {
                kiemtratrangthai();
                GetDataClientChuaUpdateServer();
            }
            else
            {
                try
                {
                    Task<string> callTask = Task.Run(() => GetAllCheckBaoCom());
                    callTask.Wait();
                    string astr = callTask.Result;
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(astr, typeof(DataTable));
                    if (dt.Rows.Count > 0)
                    {
                        string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
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
                        ws.Cells[1, 36] = "trangthai1";
                        ws.Cells[1, 37] = "trangthai2";

                        var data = new object[dt.Rows.Count, dt.Columns.Count];
                      
                        for (int row = 0; row < dt.Rows.Count; row++)
                        {
                            for (int column = 0; column <= dt.Columns.Count - 1; column++)
                            {
                                data[row, column] = dt.Rows[row][column].ToString();
                            }
                        }

                        var startCell = (Microsoft.Office.Interop.Excel.Range)ws.Cells[2, 1];
                        var endCell = (Microsoft.Office.Interop.Excel.Range)ws.Cells[dt.Rows.Count + 1, dt.Columns.Count];
                        var writeRange = ws.Range[startCell, endCell];
                        ws.Columns[3].NumberFormat = "@";
                        ws.Columns[19].NumberFormat = "@";
                        writeRange.Value2 = data;
                        wb.SaveAs(filename.FullName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8, Type.Missing, Type.Missing, Type.Missing, Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges);
                        wb.Close();
                        docExcel.Application.Quit();
                    }
                    kiemtratrangthai();
                }
                catch (Exception)
                {
                    MessageBox.Show("Chưa có dữ liệu!");
                    lvClient.Items.Clear();
                    lvDongbo.Items.Clear();
                    lbClient.Text = "Dữ liệu Client : 0";
                    lbChuadongbo.Text = "Dữ liệu chưa đồng bộ : 0";
                }
            }
        }

        private void GetDataClientChuaUpdateServer()
        {
            lvDongbo.Items.Clear();
            lbChuadongbo.Text = "Dữ liệu chưa đồng bộ : 0";
            try
            {
                string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$] where trangthai1='NG'", MyConnection);
                oada.Fill(table);
                MyConnection.Close();

                lbChuadongbo.Text = null;
                lbChuadongbo.Text = "Dữ liệu chưa đồng bộ : " + table.Rows.Count.ToString();
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
            }
            catch (Exception)
            {
                lbChuadongbo.Text = "Dữ liệu chưa đồng bộ : 0";
            }
        }

        private void btnDongBo_Click(object sender, EventArgs e)
        {
            if (baocom.Count >= 1)
            {
                string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                System.Data.OleDb.OleDbCommand myCommandup = new System.Data.OleDb.OleDbCommand();
                string sqlup = null;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$] where trangthai1='NG'", MyConnection);
                oada.Fill(table);
                myCommandup.Connection = MyConnection;
                foreach (CheckBaoCom checkcom in baocom)
                {
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow drow = table.Rows[i];
                        if (drow.RowState != DataRowState.Deleted)
                        {
                            bool check1 = false;
                            if (drow["trangthai1"].ToString() == "NG")
                            {
                                if (checkcom.manhansu == drow["manhansu"].ToString())
                                {
                                    CheckBaoCom ck = new CheckBaoCom()
                                    {
                                        id = drow["id"].ToString(),
                                        empid =string.IsNullOrEmpty(drow["empid"].ToString())?null: drow["empid"].ToString(),
                                        manhansu = drow["manhansu"].ToString(),
                                        hoten = drow["hoten"].ToString(),
                                        phongid = string.IsNullOrEmpty(drow["phongid"].ToString())?null: drow["phongid"].ToString(),
                                        phong = string.IsNullOrEmpty(drow["phong"].ToString())?null: drow["phong"].ToString(),
                                        banid = string.IsNullOrEmpty(drow["banid"].ToString())?null: drow["banid"].ToString(),
                                        ban = string.IsNullOrEmpty(drow["ban"].ToString())?null: drow["ban"].ToString(),
                                        congdoanid = string.IsNullOrEmpty(drow["congdoanid"].ToString())?null : drow["banid"].ToString(),
                                        congdoan = string.IsNullOrEmpty(drow["congdoan"].ToString())?null: drow["congdoan"].ToString(),
                                        khach = drow["khach"].ToString(),
                                        ngay = string.IsNullOrEmpty(drow["ngay"].ToString()) ? null : Convert.ToDateTime(drow["ngay"].ToString()).ToString("yyyy-MM-dd"),
                                        thang = int.Parse(drow["thang"].ToString()),
                                        nam = int.Parse(drow["nam"].ToString()),
                                        userid =string.IsNullOrEmpty(drow["userid"].ToString())?null: drow["userid"].ToString(),
                                        thoigiandat = string.IsNullOrEmpty(drow["thoigiandat"].ToString()) ? null : Convert.ToDateTime(drow["thoigiandat"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                        sudung = drow["sudung"].ToString(),
                                        dangky = drow["dangky"].ToString(),
                                        thoigiansudung =string.IsNullOrEmpty(drow["thoigiansudung"].ToString())?null: Convert.ToDateTime(drow["thoigiansudung"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                                        soxuatandadung = int.Parse(drow["soxuatandadung"].ToString()),
                                        sotiendadung = 0,
                                        chot = drow["chot"].ToString(),
                                        ghichu =string.IsNullOrEmpty(drow["ghichu"].ToString())?null: drow["ghichu"].ToString(),
                                        //thucdontheobuaid = "",
                                        buaanid = drow["buaanid"].ToString(),
                                       // kieudoan = 0,
                                        nhaanid = idnhaan,
                                        dangkybosung= drow["dangkybosung"].ToString()
                                    };
                                    check1 = Task.Run(() => UpdateCheckBaoCom(ck)).Result;
                                }
                            }
                            // Update data NG->""
                            if (check1 == true)
                            {
                                sqlup = "update [Sheet1$] set trangthai1='' where manhansu='" + drow["manhansu"].ToString() + "'";
                                myCommandup.CommandText = sqlup;
                                myCommandup.ExecuteNonQuery();
                              
                            }
                        }
                    }
                }

                MyConnection.Close();
                GetBaoCom();
                GetDataClientChuaUpdateServer();
            }
        }

        private void DeleteRowExcel(int RowExcel)
        {
            string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
            Excel._Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
            dynamic workbooksExcel = docExcel.Workbooks.Open(pathfile);
            var worksheetExcel = (Excel._Worksheet)workbooksExcel.ActiveSheet;
            Excel.Range dfd = worksheetExcel.UsedRange;

            ((Excel.Range)worksheetExcel.Rows[RowExcel, Missing.Value]).Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
            workbooksExcel.Save();
            workbooksExcel.Close(false);
            docExcel.Application.Quit();
        }
    }
}