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
        private string getthoigian=null;
        
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
                baocom.Clear();
                try
                {
                    Task<string> callTask = Task.Run(() => GetAllCheckBaoCom());
                    callTask.Wait();
                    string astr = callTask.Result;
                    DataTable dt = (DataTable)JsonConvert.DeserializeObject(astr, typeof(DataTable));
                    if (dt.Rows.Count > 0)
                    {
                        string info = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".txt";
                        File.Create(info);
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
                string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
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
                            dangkybosung = drow["dangkybosung"].ToString()
                        };
                        baocom.Add(ck);
                    }
                }
            }
        }

        private async void txtID_TextChanged(object sender, EventArgs e)
        {
            await Task.Delay(70);
            if(CheckData()==true)
            {
                if (!string.IsNullOrEmpty(txtID.Text))
                {
                    string info = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".txt";
                    List<CheckBaoCom> check = baocom.Where(x => x.manhansu == txtID.Text).ToList();
                    string[] lines = File.ReadAllLines(info);
                    bool checkid = false;//không
                    if(lines.Count()>0)
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
                    if (check.Count == 1)
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
                            dangkybosung = check.First().dangkybosung
                        };
                        if (check.First().sudung == "False" && checkid==false)
                        {
                            lbthongtinnv.Text = check.First().manhansu + "-" + check.First().hoten + "-" + check.First().phong + "-" + check.First().ban;
                            lbthongbao.Text = "OK";
                            checkok.Play();
                            checkok.Dispose();
                            lbthongbao.BackColor = Color.Green;
                            ck.sudung = "true";
                            ck.thoigiansudung = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                            ck.soxuatandadung = Convert.ToInt32(check.First().soxuatandadung) + 1;
                            UpdateCheckBaoCom(ck);
                            txtID.Text = null;
                            lbthoigiansudung.Text = "Thành công: " + DateTime.Now.ToString("dd/MM/yy-HH:mm:ss");
                        }
                        else
                        {
                            lbthongtinnv.Text = check.First().manhansu + "-" + check.First().hoten + "-" + check.First().phong + "-" + check.First().ban;
                            lbthongbao.Text = "NG";
                            checkng.Play();
                            checkng.Dispose();
                            lbthongbao.BackColor = Color.Yellow;
                            lbthoigiansudung.Text = "Bạn đã lấy cơm lúc: " + getthoigian;
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
        }

        private async void UpdateCheckBaoCom(CheckBaoCom ck)
        {
            string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
            string info = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".txt";
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
                            using (var writer = new StreamWriter(info, true))
                            {
                                writer.WriteLine(ck.manhansu + "-" + Convert.ToDateTime(ck.thoigiansudung).ToString("dd/MM/yy HH:mm:ss"));
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Update dữ liệu Client lỗi!");
                        }

                    }
                    else
                    {
                        try
                        {
                            using (var writer = new StreamWriter(info, true))
                            {
                                writer.WriteLine(ck.manhansu + "-" + Convert.ToDateTime(ck.thoigiansudung).ToString("dd/MM/yy HH:mm:ss") + "-NG1");
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Update dữ liệu Client lỗi!");
                        }
                    }
                }
            }
            catch (Exception)
            {

                try
                {
                    using (var writer = new StreamWriter(info, true))
                    {
                        writer.WriteLine(ck.manhansu + "-" + Convert.ToDateTime(ck.thoigiansudung).ToString("dd/MM/yy HH:mm:ss") + "-NG1");
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Update dữ liệu Client lỗi!");
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