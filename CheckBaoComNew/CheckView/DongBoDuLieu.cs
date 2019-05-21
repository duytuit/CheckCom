using CheckView.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace CheckView
{
    public partial class DongBoDuLieu : Form
    {
        private string APICheckBaoCom = "http://192.84.100.207/MealOrdersAPI/api/BaoComBuaAn";
        private string idbuaan = null;
        private string Bua = null;
        private List<CheckBaoCom> baocom = new List<CheckBaoCom>();

        public DongBoDuLieu()
        {
            InitializeComponent();
            GetAlllistBox();
            Getbuaan();
        }

        private void GetAlllistBox()
        {
            listBox1.Items.Clear();
            string fileToRead = System.IO.Path.GetDirectoryName(Application.StartupPath + @"\CheckCom\");

            DirectoryInfo dinfo = new DirectoryInfo(fileToRead);
            FileInfo[] Files = dinfo.GetFiles("*");
            foreach (FileInfo file in Files)
            {
                listBox1.Items.Add(file);
            }
            string Item = Application.StartupPath + @"\FileName\Selectindex.txt";
            string[] items = File.ReadAllLines(Item);
            int test;
            foreach (string item in items)
            {
                test = int.Parse(item.ToString());
                if (!int.TryParse(item.ToString(), out test))
                {
                    return;
                }
                else
                {
                    listBox1.SetSelected(test, true);
                }
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string filename = listBox1.SelectedItem.ToString();
            string pathfile = Application.StartupPath + @"\CheckCom\" + filename;
            DataTable table = new DataTable();
            System.Data.OleDb.OleDbConnection MyConnection;
            MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
            MyConnection.Open();
            OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
            oada.Fill(table);
            MyConnection.Close();
            listView1.Items.Clear();
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow drow = table.Rows[i];

                if (drow.RowState != DataRowState.Deleted)
                {
                    ListViewItem lvi = new ListViewItem(drow["id"].ToString());
                    lvi.SubItems.Add(drow["manhansu"].ToString());
                    lvi.SubItems.Add(drow["hoten"].ToString());
                    lvi.SubItems.Add(drow["phong"].ToString());
                    lvi.SubItems.Add(drow["ban"].ToString());
                    lvi.SubItems.Add(drow["congdoan"].ToString());
                    lvi.SubItems.Add(drow["khach"].ToString());
                    lvi.SubItems.Add(drow["ngay"].ToString());
                    lvi.SubItems.Add(drow["thang"].ToString());
                    lvi.SubItems.Add(drow["nam"].ToString());
                    lvi.SubItems.Add(drow["taikhoandat"].ToString());
                    lvi.SubItems.Add(drow["thoigiandat"].ToString());
                    lvi.SubItems.Add(drow["sudung"].ToString());
                    lvi.SubItems.Add(drow["dangky"].ToString());
                    lvi.SubItems.Add(drow["thoigiansudung"].ToString());
                    lvi.SubItems.Add(drow["soxuatandadung"].ToString());
                    lvi.SubItems.Add(drow["chot"].ToString());
                    lvi.SubItems.Add(drow["ghichu"].ToString());
                    lvi.SubItems.Add(drow["buaan"].ToString());
                    listView1.Items.Add(lvi);
                }
            }
            string filePath = Application.StartupPath + @"\FileName\checkcom.txt";
            string filePath1 = Application.StartupPath + @"\FileName\Selectindex.txt";
            StreamWriter objWriter = new StreamWriter(filePath);
            StreamWriter objWriter1 = new StreamWriter(filePath1);
            objWriter.WriteLine(listBox1.SelectedItem.ToString());
            objWriter1.WriteLine(listBox1.SelectedIndices[0]);
            objWriter1.Close();
            objWriter.Close();
        }

        private async Task<string> GetAllCheckBaoCom()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync(APICheckBaoCom);
            return astr;
        }

        public void GetCheckBaoCom()
        {
            //Bao com==========================================
            string pathfile = Application.StartupPath + @"\CheckCom\" + DateTime.Now.ToString("yyMMdd-HHmm") + " " + Bua + ".xls";
            FileInfo filename = new FileInfo(pathfile);
            List<CheckBaoCom> check = baocom;
            if (check.Count != 0)
            {
                Excel.Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
                Excel.Workbook wb = docExcel.Workbooks.Add(Type.Missing);
                Excel.Worksheet ws = (Excel.Worksheet)docExcel.ActiveSheet;

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
                ws.Cells[1, 15] = "taikhoandat";
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
                ws.Cells[1, 26] = "buaanid";
                ws.Cells[1, 27] = "buaan";

                int i = 2;
                foreach (CheckBaoCom ck in check)
                {
                    ws.Cells[i, 1] = ck.id;
                    ws.Cells[i, 2] = ck.empid;
                    ws.Cells[i, 3] = ck.manhansu;
                    ws.Cells[i, 4] = ck.hoten;
                    ws.Cells[i, 5] = ck.phongid;
                    ws.Cells[i, 6] = ck.phong;
                    ws.Cells[i, 7] = ck.banid;
                    ws.Cells[i, 8] = ck.ban;
                    ws.Cells[i, 9] = ck.congdoanid;
                    ws.Cells[i, 10] = ck.congdoan;
                    ws.Cells[i, 11] = ck.khach;
                    ws.Cells[i, 12] = ck.ngay;
                    ws.Cells[i, 13] = ck.thang;
                    ws.Cells[i, 14] = ck.nam;
                    ws.Cells[i, 15] = ck.taikhoandat;
                    ws.Cells[i, 16] = ck.thoigiandat;
                    ws.Cells[i, 17] = ck.sudung;
                    ws.Cells[i, 18] = ck.dangky;
                    ws.Cells[i, 19] = ck.thoigiansudung;
                    ws.Cells[i, 20] = ck.soxuatandadung;
                    ws.Cells[i, 21] = ck.sotiendadung;
                    ws.Cells[i, 22] = ck.chot;
                    ws.Cells[i, 23] = ck.ghichu;
                    ws.Cells[i, 24] = ck.thucdontheobuaid;
                    ws.Cells[i, 25] = ck.thucdontheobua;
                    ws.Cells[i, 26] = ck.buaanid;
                    ws.Cells[i, 27] = ck.buaan;
                    i++;
                }
                wb.SaveAs(filename.FullName, Excel.XlFileFormat.xlExcel8);
                wb.Close();
                docExcel.Application.Quit();
                //Bua An =======================================================
                string pathfilebuaan = Application.StartupPath + @"\Buaan\BuaAn.xls";
                FileInfo filenamebuaan = new FileInfo(pathfilebuaan);
                List<BuaAn> checkbuaan = Getbuaan();

                Excel.Application docExcelbuaan = new Microsoft.Office.Interop.Excel.Application { Visible = false };
                docExcelbuaan.DisplayAlerts = false;
                Excel.Workbook wbbuaan = docExcelbuaan.Workbooks.Add(Type.Missing);
                Excel.Worksheet wsbuaan = (Excel.Worksheet)docExcelbuaan.ActiveSheet;

                wsbuaan.Cells[1, 1] = "id";
                wsbuaan.Cells[1, 2] = "ma";
                wsbuaan.Cells[1, 3] = "ten";
                wsbuaan.Cells[1, 4] = "ghichu";
                wsbuaan.Cells[1, 5] = "loaibuaanid";
                wsbuaan.Cells[1, 6] = "loaibuaanid";
                int z = 2;
                foreach (BuaAn ba in checkbuaan)
                {
                    wsbuaan.Cells[z, 1] = ba.id;
                    wsbuaan.Cells[z, 2] = ba.ma;
                    wsbuaan.Cells[z, 3] = ba.ten;
                    wsbuaan.Cells[z, 4] = ba.ghichu;
                    wsbuaan.Cells[z, 5] = ba.loaibuaanid;
                    wsbuaan.Cells[z, 6] = ba.loaibuaanid;
                    z++;
                }

                wbbuaan.SaveAs(filenamebuaan.FullName, Excel.XlFileFormat.xlExcel8);
                wbbuaan.Close(Excel.XlSaveAction.xlSaveChanges, Type.Missing, Type.Missing);
                docExcelbuaan.Application.Quit();
                GetAlllistBox();
            }
            else
            {
                MessageBox.Show("Chưa có dữ liệu!");
            }
        }

        private void btnExcel_Click(object sender, EventArgs e)
        {
            string filename = listBox1.SelectedItem.ToString();
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save Excel Files";

            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "Excel Files (*.xls)|*.xls|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo filename1 = new FileInfo(saveFileDialog1.FileName);
                Excel.Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
                Excel.Workbook wb = docExcel.Workbooks.Add(Type.Missing);
                Excel.Worksheet ws = (Excel.Worksheet)docExcel.ActiveSheet;

                ws.Cells[1, 1] = "ID";
                ws.Cells[1, 2] = "Mã NV";
                ws.Cells[1, 3] = "Họ Tên";
                ws.Cells[1, 4] = "Phòng";
                ws.Cells[1, 5] = "Ban";
                ws.Cells[1, 6] = "Công đoạn";
                ws.Cells[1, 7] = "Khách";
                ws.Cells[1, 8] = "Ngày";
                ws.Cells[1, 9] = "Tháng";
                ws.Cells[1, 10] = "Năm";
                ws.Cells[1, 11] = "Tài Khoản Đặt";
                ws.Cells[1, 12] = "Thời Gian Đặt";
                ws.Cells[1, 13] = "Sử Dụng";
                ws.Cells[1, 14] = "Đăng ký";
                ws.Cells[1, 15] = "Thời gian sử dụng";
                ws.Cells[1, 16] = "Số suất ăn đã sử dụng";
                ws.Cells[1, 17] = "Chốt";
                ws.Cells[1, 18] = "Ghi chú";
                ws.Cells[1, 19] = "Bữa ăn";
                int j = 2;

                string pathfile = Application.StartupPath + @"\CheckCom\" + filename;
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                oada.Fill(table);
                MyConnection.Close();
                listView1.Items.Clear();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow drow = table.Rows[i];

                    if (drow.RowState != DataRowState.Deleted)
                    {
                        ws.Cells[j, 1] = drow["id"].ToString();
                        ws.Cells[j, 2] = drow["manhansu"].ToString();
                        ws.Cells[j, 3] = drow["hoten"].ToString();
                        ws.Cells[j, 4] = drow["phong"].ToString();
                        ws.Cells[j, 5] = drow["ban"].ToString();
                        ws.Cells[j, 6] = drow["congdoan"].ToString();
                        ws.Cells[j, 7] = drow["khach"].ToString();
                        ws.Cells[j, 8] = drow["ngay"].ToString();
                        ws.Cells[j, 9] = drow["thang"].ToString();
                        ws.Cells[j, 10] = drow["nam"].ToString();
                        ws.Cells[j, 11] = drow["taikhoandat"].ToString();
                        ws.Cells[j, 12] = drow["thoigiandat"].ToString();
                        ws.Cells[j, 13] = drow["sudung"].ToString();
                        ws.Cells[j, 14] = drow["dangky"].ToString();
                        ws.Cells[j, 15] = drow["thoigiansudung"].ToString();
                        ws.Cells[j, 16] = drow["soxuatandadung"].ToString();
                        ws.Cells[j, 17] = drow["chot"].ToString();
                        ws.Cells[j, 18] = drow["ghichu"].ToString();
                        ws.Cells[j, 19] = drow["buaan"].ToString();
                        j++;
                    }
                }
                wb.SaveAs(filename1.FullName, Excel.XlFileFormat.xlExcel8);
                wb.Close();
                docExcel.Application.Quit();
                MessageBox.Show("Thành Công!");
                GetAlllistBox();
            }
            else
            {
                return;
            }
        }

        private void DongBoDuLieu_Load(object sender, EventArgs e)
        {
            listView1.View = View.Details;
            listView1.FullRowSelect = true;
        }

        private async void UpdateCheckBaoCom(CheckBaoCom ck)
        {
            using (var client = new HttpClient())
            {
                var serializedProduct = JsonConvert.SerializeObject(ck);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                var result = await client.PutAsync(String.Format("{0}/{1}", APICheckBaoCom, ck.id), content);
            }
        }

        private void cbBuaAn_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<BuaAn> ba = new List<BuaAn>();
            Task<string> callTask = Task.Run(() => GetAllbuaan());
            callTask.Wait();
            string astr = callTask.Result;
            List<BuaAn> buaan = JsonConvert.DeserializeObject<List<BuaAn>>(astr);
            ba = buaan.Where(x => x.ten == cbBuaAn.Text).ToList();
            idbuaan = "/" + ba.First().id.ToString();
            APICheckBaoCom = "http://192.84.100.207/MealOrdersAPI/api/BaoComBuaAn/" + DateTime.Now.ToString("MM-21-yyyy") + idbuaan;
            GetBaoCom();
            if (ba.First().ten == "Sáng")
            {
                Bua = "Sang";
            }
            if (ba.First().ten == "Trưa")
            {
                Bua = "Trua";
            }
            if (ba.First().ten == "Chiều")
            {
                Bua = "Chieu";
            }
            if (ba.First().ten == "Tối")
            {
                Bua = "Toi";
            }
        }

        static private async Task<string> GetAllbuaan()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync("http://192.84.100.207/MealOrdersAPI/api/BuaAns");
            return astr;
        }

        private List<BuaAn> Getbuaan()
        {
            List<BuaAn> buaan = new List<BuaAn>();
            try
            {
                cbBuaAn.Items.Clear();
                Task<string> callTask = Task.Run(() => GetAllbuaan());
                callTask.Wait();
                string astr = callTask.Result;
                buaan = JsonConvert.DeserializeObject<List<BuaAn>>(astr);

                foreach (BuaAn ba in buaan)
                {
                    cbBuaAn.Items.Add(ba.ten);
                }
            }
            catch (AggregateException e)
            {
                MessageBox.Show("Lỗi đường truyền!");
            }
            return buaan;
        }

        private void GetBaoCom()
        {
            try
            {
                Task<string> callTask = Task.Run(() => GetAllCheckBaoCom());
                callTask.Wait();
                string astr = callTask.Result;
                baocom = JsonConvert.DeserializeObject<List<CheckBaoCom>>(astr);
            }
            catch (AggregateException e)
            {
                MessageBox.Show("Lỗi đường truyền!");
            }
        }

        private void btnDongbolocal_Click(object sender, EventArgs e)
        {
            if (cbBuaAn.Text != null)
            {
                GetCheckBaoCom();
            }
            else
            {
                MessageBox.Show("Hãy chọn bữa ăn!");
            }
        }

        private void btnDongBo_Click_1(object sender, EventArgs e)
        {
            APICheckBaoCom = "http://192.84.100.207/MealOrdersAPI/api/BaoComBuaAn";
            try
            {
                // get name data local
                string filePath = Application.StartupPath + @"\FileName\checkcom.txt";
                StreamReader objReader = new StreamReader(filePath);
                string nameBaoCom = objReader.ReadLine();
                objReader.Close();
                string newstring = nameBaoCom.Substring(0, 6);
                string pathfile = Application.StartupPath + @"\CheckCom\" + nameBaoCom + "";
                if (DateTime.Now.ToString("yyMMdd") == newstring)
                {
                    //get data server================================================================
                    List<CheckBaoCom> ck = new List<CheckBaoCom>();
                    Task<string> callTask = Task.Run(() => GetAllCheckBaoCom());
                    callTask.Wait();
                    string astr = callTask.Result;
                    List<CheckBaoCom> check = JsonConvert.DeserializeObject<List<CheckBaoCom>>(astr);
                    ck = check.Where(x => x.sudung == "false").ToList();
                    //get data local=================================================================
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
                            if (drow["sudung"].ToString() == "True")
                            {
                                foreach (CheckBaoCom result in ck)
                                {
                                    if (int.Parse(drow["id"].ToString()) == result.id)
                                    {
                                        //thực hiện update đồng bộ data server
                                        CheckBaoCom baocom = new CheckBaoCom()
                                        {
                                            id = int.Parse(drow["id"].ToString()),
                                            empid = int.Parse(drow["empid"].ToString()),
                                            manhansu = drow["manhansu"].ToString(),
                                            hoten = drow["hoten"].ToString(),
                                            phongid = int.Parse(drow["phongid"].ToString()),
                                            phong = drow["phong"].ToString(),
                                            banid = drow["banid"].ToString(),
                                            ban = drow["ban"].ToString(),
                                            congdoanid = drow["congdoanid"].ToString(),
                                            congdoan = drow["congdoan"].ToString(),
                                            khach = drow["khach"].ToString(),
                                            ngay = DateTime.Parse(drow["ngay"].ToString()),
                                            thang = int.Parse(drow["thang"].ToString()),
                                            nam = int.Parse(drow["nam"].ToString()),
                                            taikhoandat = int.Parse(drow["taikhoandat"].ToString()),
                                            thoigiandat = DateTime.Parse(drow["thoigiandat"].ToString()),
                                            sudung = drow["sudung"].ToString(),
                                            dangky = drow["dangky"].ToString(),
                                            thoigiansudung = drow["thoigiansudung"].ToString(),
                                            soxuatandadung = int.Parse(drow["soxuatandadung"].ToString()),
                                            sotiendadung = int.Parse(drow["sotiendadung"].ToString()),
                                            chot = drow["chot"].ToString(),
                                            ghichu = drow["ghichu"].ToString(),
                                            thucdontheobuaid = drow["thucdontheobuaid"].ToString(),
                                            thucdontheobua = drow["thucdontheobua"].ToString(),
                                            buaanid = int.Parse(drow["buaanid"].ToString()),
                                            buaan = drow["buaan"].ToString(),
                                        };
                                        UpdateCheckBaoCom(baocom);
                                    }
                                }
                            }
                        }
                    }
                    MessageBox.Show("Đồng bộ thành công !");
                }
                else
                {
                    MessageBox.Show("Hãy đồng bộ dữ liệu data local mới nhất !");
                }
            }
            catch (AggregateException c)
            {
                MessageBox.Show("Lỗi đường truyền!");
            }
        }
    }
}