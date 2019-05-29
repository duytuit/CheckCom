using CheckCom_Version2.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Text;
using System.Threading;
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
        private string idnhaan;
        public CheckCom()
        {
            InitializeComponent();
            int Gio = DateTime.Now.Hour;
            GetBuaaan();

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
            //GetNhaAnID();
        }
        private void GetNhaAnID()
        {
            try
            {
                string pathfile = Application.StartupPath + @"\Nhaan\NhaAn.xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;'");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                oada.Fill(table);
                MyConnection.Close();
                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow drow = table.Rows[i];

                    if (drow.RowState != DataRowState.Deleted)
                    {
                        idnhaan = drow["nhaanid"].ToString();
                    }
                }
            }
            catch (Exception)
            {
            }
        }
        private void CheckCom_Load(object sender, EventArgs e)
        {
        }
        
        private void GetBuaaan()
        {
            try
            {
                string pathfile = Application.StartupPath + @"\Buaan\BuaAn.xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;'");
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

        private async Task<string> GetAllCheckBaoCom()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync(APICheckBaoCom);
            return astr;
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

        private void GetBaoCom()
        {
            bool icheck = CheckData();
            if (icheck == false)
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
                        wb.SaveAs(filename.FullName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8,Type.Missing,Type.Missing,Type.Missing,Type.Missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges);
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
            #region Get Com Failed
            //====================================================================
            //bool icheck = CheckData();
            //if (icheck == true)
            //{
            //    baocom.Clear();
            //    string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
            //    DataTable table = new DataTable();
            //    System.Data.OleDb.OleDbConnection MyConnection;
            //    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
            //    MyConnection.Open();
            //    OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
            //    oada.Fill(table);
            //    MyConnection.Close();

            //    for (int i = 0; i < table.Rows.Count; i++)
            //    {
            //        DataRow drow = table.Rows[i];

            //        if (drow.RowState != DataRowState.Deleted)
            //        {
            //            CheckBaoCom ck = new CheckBaoCom()
            //            {
            //                id = drow["id"].ToString(),
            //                empid = drow["empid"].ToString(),
            //                manhansu = drow["manhansu"].ToString(),
            //                hoten = drow["hoten"].ToString(),
            //                phong = drow["phong"].ToString(),
            //                banid = drow["banid"].ToString(),
            //                ban = drow["ban"].ToString(),
            //                congdoanid = drow["congdoanid"].ToString(),
            //                congdoan = drow["congdoan"].ToString(),
            //                khach = drow["khach"].ToString(),
            //                ngay = drow["ngay"].ToString(),
            //                thang = int.Parse(drow["thang"].ToString()),
            //                nam = int.Parse(drow["nam"].ToString()),
            //                userid = drow["userid"].ToString(),
            //                thoigiandat = drow["thoigiandat"].ToString(),
            //                sudung = drow["sudung"].ToString(),
            //                dangky = drow["dangky"].ToString(),
            //                thoigiansudung = drow["thoigiansudung"].ToString(),
            //                soxuatandadung = int.Parse(drow["soxuatandadung"].ToString()),
            //                sotiendadung = int.Parse(drow["sotiendadung"].ToString()),
            //                chot = drow["chot"].ToString(),
            //                ghichu = drow["ghichu"].ToString(),
            //                thucdontheobuaid = drow["thucdontheobuaid"].ToString(),
            //                thucdontheobua = drow["thucdontheobua"].ToString(),
            //                buaanid = drow["buaanid"].ToString(),
            //                buaan = drow["buaan"].ToString(),
            //                dangkybosung = drow["dangkybosung"].ToString()
            //            };
            //            baocom.Add(ck);
            //        }
            //    }
            //}
            //else
            //{
            //    baocom.Clear();
            //    try
            //    {
            //        Task<string> callTask = Task.Run(() => GetAllCheckBaoCom());
            //        callTask.Wait();
            //        string astr = callTask.Result;
            //        DataTable dt = (DataTable)JsonConvert.DeserializeObject(astr, typeof(DataTable));
            //        if (dt.Rows.Count > 0)
            //        {
            //            string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
            //            FileInfo filename = new FileInfo(pathfile);
            //            Microsoft.Office.Interop.Excel.Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
            //            Microsoft.Office.Interop.Excel.Workbook wb = docExcel.Workbooks.Add(Type.Missing);
            //            Microsoft.Office.Interop.Excel.Worksheet ws = (Microsoft.Office.Interop.Excel.Worksheet)docExcel.ActiveSheet;
            //            ws.Cells[1, 1] = "id";
            //            ws.Cells[1, 2] = "empid";
            //            ws.Cells[1, 3] = "manhansu";
            //            ws.Cells[1, 4] = "hoten";
            //            ws.Cells[1, 5] = "phongid";
            //            ws.Cells[1, 6] = "phong";
            //            ws.Cells[1, 7] = "banid";
            //            ws.Cells[1, 8] = "ban";
            //            ws.Cells[1, 9] = "congdoanid";
            //            ws.Cells[1, 10] = "congdoan";
            //            ws.Cells[1, 11] = "khach";
            //            ws.Cells[1, 12] = "ngay";
            //            ws.Cells[1, 13] = "thang";
            //            ws.Cells[1, 14] = "nam";
            //            ws.Cells[1, 15] = "userid";
            //            ws.Cells[1, 16] = "thoigiandat";
            //            ws.Cells[1, 17] = "sudung";
            //            ws.Cells[1, 18] = "dangky";
            //            ws.Cells[1, 19] = "thoigiansudung";
            //            ws.Cells[1, 20] = "soxuatandadung";
            //            ws.Cells[1, 21] = "sotiendadung";
            //            ws.Cells[1, 22] = "chot";
            //            ws.Cells[1, 23] = "ghichu";
            //            ws.Cells[1, 24] = "thucdontheobuaid";
            //            ws.Cells[1, 25] = "thucdontheobua";
            //            ws.Cells[1, 26] = "kieudoan";
            //            ws.Cells[1, 27] = "buaanid";
            //            ws.Cells[1, 28] = "buaan";
            //            ws.Cells[1, 29] = "ca";
            //            ws.Cells[1, 30] = "nhaanid";
            //            ws.Cells[1, 31] = "nhaan";
            //            ws.Cells[1, 32] = "loaidouong";
            //            ws.Cells[1, 33] = "thanhtoan";
            //            ws.Cells[1, 34] = "phongrieng";
            //            ws.Cells[1, 35] = "dangkybosung";
            //            ws.Cells[1, 36] = "trangthai1";
            //            ws.Cells[1, 37] = "trangthai2";

            //            var data = new object[dt.Rows.Count, dt.Columns.Count];
            //            for (int row = 0; row < dt.Rows.Count; row++)
            //            {
            //                for (int column = 0; column <= dt.Columns.Count - 1; column++)
            //                {
            //                    data[row, column] = dt.Rows[row][column].ToString();
            //                }
            //                DataRow drow = dt.Rows[row];

            //                if (drow.RowState != DataRowState.Deleted)
            //                {
            //                    CheckBaoCom ck = new CheckBaoCom()
            //                    {
            //                        id = drow["id"].ToString(),
            //                        empid = drow["empid"].ToString(),
            //                        manhansu = drow["manhansu"].ToString(),
            //                        hoten = drow["hoten"].ToString(),
            //                        phong = drow["phong"].ToString(),
            //                        banid = drow["banid"].ToString(),
            //                        ban = drow["ban"].ToString(),
            //                        congdoanid = drow["congdoanid"].ToString(),
            //                        congdoan = drow["congdoan"].ToString(),
            //                        khach = drow["khach"].ToString(),
            //                        ngay = drow["ngay"].ToString(),
            //                        thang = int.Parse(drow["thang"].ToString()),
            //                        nam = int.Parse(drow["nam"].ToString()),
            //                        userid = drow["userid"].ToString(),
            //                        thoigiandat = drow["thoigiandat"].ToString(),
            //                        sudung = drow["sudung"].ToString(),
            //                        dangky = drow["dangky"].ToString(),
            //                        thoigiansudung = drow["thoigiansudung"].ToString(),
            //                        soxuatandadung = int.Parse(drow["soxuatandadung"].ToString()),
            //                        sotiendadung = int.Parse(drow["sotiendadung"].ToString()),
            //                        chot = drow["chot"].ToString(),
            //                        ghichu = drow["ghichu"].ToString(),
            //                        thucdontheobuaid = drow["thucdontheobuaid"].ToString(),
            //                        thucdontheobua = drow["thucdontheobua"].ToString(),
            //                        buaanid = drow["buaanid"].ToString(),
            //                        buaan = drow["buaan"].ToString(),
            //                        dangkybosung = drow["dangkybosung"].ToString()
            //                    };
            //                    baocom.Add(ck);
            //                }
            //            }

            //            var startCell = (Microsoft.Office.Interop.Excel.Range)ws.Cells[2, 1];
            //            var endCell = (Microsoft.Office.Interop.Excel.Range)ws.Cells[dt.Rows.Count + 1, dt.Columns.Count];
            //            var writeRange = ws.Range[startCell, endCell];
            //            ws.Columns[3].NumberFormat = "@";
            //            ws.Columns[19].NumberFormat = "@";
            //            writeRange.Value2 = data;
            //            wb.SaveAs(filename.FullName, Microsoft.Office.Interop.Excel.XlFileFormat.xlExcel8);
            //            wb.Close();
            //            docExcel.Application.Quit();
            //        }
            //    }
            //    catch (AggregateException)
            //    {
            //        MessageBox.Show("Chưa có dữ liệu!");
            //    }
            //}
            #endregion
        }

        private async void txtID_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(70);
            if(CheckData()==true)
            {
                if (!string.IsNullOrEmpty(txtID.Text))
                {
                    string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                    DataTable table = new DataTable();
                    System.Data.OleDb.OleDbConnection MyConnection;
                    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'");
                    MyConnection.Open();
                    OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$] where manhansu='" + txtID.Text + "'", MyConnection);
                    oada.Fill(table);
                    MyConnection.Close();
                    if (table.Rows.Count == 1)
                    {
                        CheckBaoCom ck = new CheckBaoCom()
                        {
                            id = table.Rows[0]["id"].ToString(),
                            empid = string.IsNullOrEmpty(table.Rows[0]["empid"].ToString()) ? null : table.Rows[0]["empid"].ToString(),
                            manhansu = table.Rows[0]["manhansu"].ToString(),
                            hoten = table.Rows[0]["hoten"].ToString(),
                            phongid = string.IsNullOrEmpty(table.Rows[0]["phongid"].ToString()) ? null : table.Rows[0]["phongid"].ToString(),
                            phong = string.IsNullOrEmpty(table.Rows[0]["phong"].ToString()) ? null : table.Rows[0]["phong"].ToString(),
                            banid = string.IsNullOrEmpty(table.Rows[0]["banid"].ToString()) ? null : table.Rows[0]["banid"].ToString(),
                            ban = string.IsNullOrEmpty(table.Rows[0]["ban"].ToString()) ? null : table.Rows[0]["ban"].ToString(),
                            congdoanid = string.IsNullOrEmpty(table.Rows[0]["congdoanid"].ToString()) ? null : table.Rows[0]["congdoanid"].ToString(),
                            congdoan = string.IsNullOrEmpty(table.Rows[0]["congdoan"].ToString()) ? null : table.Rows[0]["congdoanid"].ToString(),
                            khach = table.Rows[0]["khach"].ToString(),
                            ngay = Convert.ToDateTime(table.Rows[0]["ngay"].ToString()).ToString("yyyy-MM-dd"),
                            thang = int.Parse(table.Rows[0]["thang"].ToString()),
                            nam = int.Parse(table.Rows[0]["nam"].ToString()),
                            userid = string.IsNullOrEmpty(table.Rows[0]["userid"].ToString()) ? null : table.Rows[0]["userid"].ToString(),
                            thoigiandat = Convert.ToDateTime(table.Rows[0]["thoigiandat"].ToString()).ToString("yyyy-MM-dd HH:mm:ss"),
                            sudung = table.Rows[0]["sudung"].ToString(),
                            dangky = table.Rows[0]["dangky"].ToString(),
                            sotiendadung = int.Parse(table.Rows[0]["sotiendadung"].ToString()),
                            chot = table.Rows[0]["chot"].ToString(),
                            buaanid = table.Rows[0]["buaanid"].ToString(),
                            nhaanid = table.Rows[0]["nhaanid"].ToString(),
                            dangkybosung = table.Rows[0]["dangkybosung"].ToString()
                        };
                        if (table.Rows[0]["sudung"].ToString() == "False")
                        {
                            lbthongtinnv.Text = table.Rows[0]["manhansu"].ToString() + "-" + table.Rows[0]["hoten"].ToString() + "-" + table.Rows[0]["phong"].ToString() + "-" + table.Rows[0]["ban"].ToString();
                            lbthongbao.Text = "OK";
                            checkok.Play();
                            checkok.Dispose();
                            lbthongbao.BackColor = Color.Green;
                            ck.sudung = "True";
                            ck.thoigiansudung = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            ck.soxuatandadung = Convert.ToInt32(table.Rows[0]["soxuatandadung"].ToString()) + 1;
                            UpdateCheckBaoCom(ck);
                            txtID.Text = null;
                            lbthoigiansudung.Text = "Thành công: " + DateTime.Now.ToString("dd/MM/yy-HH:mm:ss");
                        }
                        else
                        {
                            lbthongtinnv.Text = table.Rows[0]["manhansu"].ToString() + "-" + table.Rows[0]["hoten"].ToString() + "-" + table.Rows[0]["phong"].ToString() + "-" + table.Rows[0]["ban"].ToString();
                            lbthongbao.Text = "NG";
                            checkng.Play();
                            checkng.Dispose();
                            lbthongbao.BackColor = Color.Yellow;
                            lbthoigiansudung.Text = "Bạn đã lấy cơm lúc: " + table.Rows[0]["thoigiansudung"];
                            txtID.Text = null;
                        }
                    }
                    else
                    {
                        checkng.Play();
                        checkng.Dispose();
                        lbthongbao.Text = "NG";
                        lbthongbao.BackColor = Color.Red;
                        txtID.Text = null;
                        lbthongtinnv.Text = null;
                        lbthoigiansudung.Text = "Bạn chưa báo cơm. Vui lòng qua bàn đăng ký bổ sung!";
                    }
                  
                }
            }
            else
            {
                txtID.Text = null;
            }
          
            #region Check Failed
            //=============================================================================================================


            //List<CheckBaoCom> check = baocom.Where(x => x.manhansu == txtID.Text).ToList();
            //if (!string.IsNullOrEmpty(txtID.Text))
            //{
            //    if (check.Count == 1)
            //    {
            //        CheckBaoCom ck = new CheckBaoCom()
            //        {
            //            id = check.First().id,
            //            empid = string.IsNullOrEmpty(check.First().empid) ? null : check.First().empid,
            //            manhansu = check.First().manhansu,
            //            hoten = check.First().hoten,
            //            phongid = string.IsNullOrEmpty(check.First().phongid) ? null : check.First().phongid,
            //            phong = string.IsNullOrEmpty(check.First().phong)?null: check.First().phong,
            //            banid = string.IsNullOrEmpty(check.First().banid) ? null : check.First().banid,
            //            ban = string.IsNullOrEmpty(check.First().ban)?null: check.First().ban,
            //            congdoanid = string.IsNullOrEmpty(check.First().congdoanid) ? null : check.First().congdoanid,
            //            congdoan = string.IsNullOrEmpty(check.First().congdoan)?null: check.First().congdoan,
            //            khach = check.First().khach,
            //            ngay = Convert.ToDateTime(check.First().ngay).ToString("yyyy-MM-dd"),
            //            thang = check.First().thang,
            //            nam = check.First().nam,
            //            userid=string.IsNullOrEmpty(check.First().userid)?null: check.First().userid,
            //            thoigiandat = Convert.ToDateTime(check.First().thoigiandat).ToString("yyyy-MM-dd HH:mm:ss"),
            //            sudung = check.First().sudung,
            //            dangky = check.First().dangky,
            //            sotiendadung = check.First().sotiendadung,
            //            chot = check.First().chot,
            //           // ghichu = check.First().ghichu,
            //           // thucdontheobuaid = check.First().thucdontheobuaid,
            //           // thucdontheobua = check.First().thucdontheobua,
            //            buaanid = check.First().buaanid,
            //           // buaan = check.First().buaan,
            //           // kieudoan = check.First().kieudoan,
            //            nhaanid = check.First().nhaanid,
            //            dangkybosung = check.First().dangkybosung,
            //        };
            //        if (check.First().sudung.ToLower() == "false")
            //        {
            //            lbthongtinnv.Text = check.First().manhansu + "-" + check.First().hoten + "-" + check.First().phong + "-" + check.First().ban;
            //            lbthongbao.Text = "OK";
            //            checkok.Play();
            //            checkok.Dispose();
            //            lbthongbao.BackColor = Color.Green;
            //            ck.sudung = "true";
            //            ck.thoigiansudung = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //            ck.soxuatandadung = check.First().soxuatandadung + 1;
            //            UpdateCheckBaoCom(ck);
            //            txtID.Text = null;
            //            lbthoigiansudung.Text = "Thành công: " + DateTime.Now.ToString("dd/MM/yy-HH:mm:ss");
            //            GetBaoCom();
            //        }
            //        else
            //        {
            //            lbthongtinnv.Text = check.First().manhansu + "-" + check.First().hoten + "-" + check.First().phong + "-" + check.First().ban;
            //            lbthongbao.Text = "NG";
            //            checkng.Play();
            //            checkng.Dispose();
            //            lbthongbao.BackColor = Color.Yellow;
            //            lbthoigiansudung.Text = "Bạn đã lấy cơm lúc: " + check.First().thoigiansudung;
            //            txtID.Text = null;
            //        }
            //    }
            //    else
            //    {
            //        checkng.Play();
            //        checkng.Dispose();
            //        lbthongbao.Text = "NG";
            //        lbthongbao.BackColor = Color.Red;
            //        txtID.Text = null;
            //        lbthongtinnv.Text = null;
            //        lbthoigiansudung.Text = "Bạn chưa báo cơm. Vui lòng qua bàn đăng ký bổ sung!";
            //    }
            //}
            #endregion
        }

        private async void UpdateCheckBaoCom(CheckBaoCom ck)
        {
            string APIbaocom = "http://192.84.100.207/MealOrdersAPI/api/DulieuBaoComs";
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
                            string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                            DataTable table = new DataTable();
                            System.Data.OleDb.OleDbConnection MyConnectionup;
                            System.Data.OleDb.OleDbCommand myCommandup = new System.Data.OleDb.OleDbCommand();
                            string sqlup = null;
                            MyConnectionup = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;'");
                            MyConnectionup.Open();
                            myCommandup.Connection = MyConnectionup;
                            sqlup = "update [Sheet1$] set sudung=" + ck.sudung + ",thoigiansudung='" + ck.thoigiansudung + "',soxuatandadung=" + ck.soxuatandadung + " where manhansu='" + ck.manhansu + "'";
                            myCommandup.CommandText = sqlup;
                            myCommandup.ExecuteNonQuery();
                            MyConnectionup.Close();
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Update dữ liệu client lỗi!");
                        }
                    }
                    else
                    {
                        string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                        DataTable table1 = new DataTable();
                        System.Data.OleDb.OleDbConnection MyConnection;
                        MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'");
                        MyConnection.Open();
                        OleDbDataAdapter oada1 = new OleDbDataAdapter("select * from [Sheet1$] where trangthai2='NG' and manhansu='" + ck.manhansu + "'", MyConnection);
                        oada1.Fill(table1);
                        MyConnection.Close();
                        if(table1.Rows.Count==1)
                        {
                            try
                            {
                                
                                DataTable table = new DataTable();
                                System.Data.OleDb.OleDbConnection MyConnectionup;
                                System.Data.OleDb.OleDbCommand myCommandup = new System.Data.OleDb.OleDbCommand();
                                string sqlup = null;
                                MyConnectionup = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;'");
                                MyConnectionup.Open();
                                myCommandup.Connection = MyConnectionup;
                                sqlup = "update [Sheet1$] set sudung=" + ck.sudung + ",thoigiansudung='" + ck.thoigiansudung + "',soxuatandadung=" + ck.soxuatandadung + " where manhansu='" + ck.manhansu + "'";
                                myCommandup.CommandText = sqlup;
                                myCommandup.ExecuteNonQuery();
                                MyConnectionup.Close();
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Update dữ liệu client lỗi!");
                            }
                        }
                        else
                        {
                            try
                            {
                              
                                DataTable table = new DataTable();
                                System.Data.OleDb.OleDbConnection MyConnectionup;
                                System.Data.OleDb.OleDbCommand myCommandup = new System.Data.OleDb.OleDbCommand();
                                string sqlup = null;
                                MyConnectionup = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;'");
                                MyConnectionup.Open();
                                myCommandup.Connection = MyConnectionup;
                                sqlup = "update [Sheet1$] set sudung=" + ck.sudung + ",thoigiansudung='" + ck.thoigiansudung + "',soxuatandadung=" + ck.soxuatandadung + ",trangthai1='NG' where manhansu='" + ck.manhansu + "'";
                                myCommandup.CommandText = sqlup;
                                myCommandup.ExecuteNonQuery();
                                MyConnectionup.Close();
                            }
                            catch (Exception)
                            {
                                MessageBox.Show("Update dữ liệu client lỗi!");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                DataTable table1 = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;IMEX=1;'");
                MyConnection.Open();
                OleDbDataAdapter oada1 = new OleDbDataAdapter("select * from [Sheet1$] where trangthai2='NG' and manhansu='" + ck.manhansu + "'", MyConnection);
                oada1.Fill(table1);
                MyConnection.Close();
                if (table1.Rows.Count == 1)
                {
                    try
                    {

                        DataTable table = new DataTable();
                        System.Data.OleDb.OleDbConnection MyConnectionup;
                        System.Data.OleDb.OleDbCommand myCommandup = new System.Data.OleDb.OleDbCommand();
                        string sqlup = null;
                        MyConnectionup = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;'");
                        MyConnectionup.Open();
                        myCommandup.Connection = MyConnectionup;
                        sqlup = "update [Sheet1$] set sudung=" + ck.sudung + ",thoigiansudung='" + ck.thoigiansudung + "',soxuatandadung=" + ck.soxuatandadung + " where manhansu='" + ck.manhansu + "'";
                        myCommandup.CommandText = sqlup;
                        myCommandup.ExecuteNonQuery();
                        MyConnectionup.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Update dữ liệu client lỗi!");
                    }
                }
                else
                {
                    try
                    {

                        DataTable table = new DataTable();
                        System.Data.OleDb.OleDbConnection MyConnectionup;
                        System.Data.OleDb.OleDbCommand myCommandup = new System.Data.OleDb.OleDbCommand();
                        string sqlup = null;
                        MyConnectionup = new System.Data.OleDb.OleDbConnection("provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + pathfile + "';Extended Properties='Excel 12.0;HDR=YES;'");
                        MyConnectionup.Open();
                        myCommandup.Connection = MyConnectionup;
                        sqlup = "update [Sheet1$] set sudung=" + ck.sudung + ",thoigiansudung='" + ck.thoigiansudung + "',soxuatandadung=" + ck.soxuatandadung + ",trangthai1='NG' where manhansu='" + ck.manhansu + "'";
                        myCommandup.CommandText = sqlup;
                        myCommandup.ExecuteNonQuery();
                        MyConnectionup.Close();
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("Update dữ liệu client lỗi!");
                    }
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
            APICheckBaoCom = "http://192.84.100.207/MealOrdersAPI/api/DulieuBaoComs/" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + "/" + caanid;
            GetBaoCom();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            
        }
    }
}