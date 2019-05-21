using CheckView.DTOs;
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

namespace CheckView
{
    public partial class Form1 : Form
    {
        private string APICheckBaoCom = null;
        private string dung = "true";
        private string sai = "false";
        private string idbuaan = null;
        private List<CheckBaoCom> baocom = new List<CheckBaoCom>();

        public Form1()
        {
            InitializeComponent();
            comboBox1.Text = "Online";
            Getbuaan();
        }

        static private async Task<string> GetAllbuaan()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync("http://192.84.100.207/MealOrdersAPI/api/BuaAns");
            return astr;
        }

        private async Task<string> GetAllCheckBaoCom()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync(APICheckBaoCom);
            return astr;
        }

        private void Getbuaan()
        {
            if (comboBox1.Text == "Online")
            {
                try
                {
                    cbBuaAn.Items.Clear();
                    Task<string> callTask = Task.Run(() => GetAllbuaan());
                    callTask.Wait();
                    string astr = callTask.Result;
                    List<BuaAn> buaan = JsonConvert.DeserializeObject<List<BuaAn>>(astr);

                    foreach (BuaAn ba in buaan)
                    {
                        cbBuaAn.Items.Add(ba.ten);
                    }
                }
                catch (AggregateException e)
                {
                    MessageBox.Show("Lỗi đường truyền!");
                }
            }
            else
            {
                string filePath = Application.StartupPath + @"\Buaan\BuaAn.xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + filePath + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                oada.Fill(table);
                MyConnection.Close();

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow drow = table.Rows[i];

                    if (drow.RowState != DataRowState.Deleted)
                    {
                        cbBuaAn.Items.Add(drow["ten"].ToString());
                    }
                }
            }
        }

        private void GetBaoCom()
        {
            if (comboBox1.Text == "Online")
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
            else
            {
                //get check com==================================================
                baocom.Clear();
                string filePath = Application.StartupPath + @"\FileName\checkcom.txt";
                StreamReader objReader = new StreamReader(filePath);
                string nameBaoCom = objReader.ReadLine();
                objReader.Close();
                //==================================================================
                string pathfile = Application.StartupPath + @"\CheckCom\" + nameBaoCom + "";
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
                        baocom.Add(ck);
                    }
                }
            }
        }

        private async void txtID_TextChanged(object sender, EventArgs e)
        {
            if(!string.IsNullOrEmpty(txtID.Text))
            {
                if (!string.IsNullOrEmpty(cbBuaAn.Text))
                {
                    if (comboBox1.Text == "Online")
                    {
                        // get name data local
                        await Task.Delay(70);
                        string filePath = Application.StartupPath + @"\FileName\checkcom.txt";
                        StreamReader objReader = new StreamReader(filePath);
                        string nameBaoCom = objReader.ReadLine();
                        objReader.Close();
                        string newstring = nameBaoCom.Substring(0, 6);
                        if (!string.IsNullOrEmpty(txtID.Text))
                        {
                            List<CheckBaoCom> check = baocom;
                            check = check.Where(x => x.manhansu == txtID.Text).ToList();
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
                                    taikhoandat = check.First().taikhoandat,
                                    thoigiandat = check.First().thoigiandat,
                                    sudung = check.First().sudung,
                                    dangky = check.First().dangky,
                                    thoigiansudung = check.First().thoigiansudung,
                                    soxuatandadung = check.First().soxuatandadung,
                                    sotiendadung = check.First().sotiendadung,
                                    chot = check.First().chot,
                                    ghichu = check.First().ghichu,
                                    thucdontheobuaid = check.First().thucdontheobuaid,
                                    thucdontheobua = check.First().thucdontheobua,
                                    buaanid = check.First().buaanid,
                                    buaan = check.First().buaan
                                };
                                if (check.First().dangky == "False")
                                {
                                    SoundPlayer checkng = new SoundPlayer(Application.StartupPath + @"\sound\buzzer_x.wav");
                                    lbThongTinNV.Text = check.First().manhansu.ToString() + "-" + check.First().hoten.ToString() + "-" + check.First().phong.ToString() + "-" + check.First().ban.ToString();
                                    checkng.Play();
                                    lbThongBao.Text = "NG";
                                    lbThongBao.BackColor = Color.Red;
                                    checkng.Dispose();
                                    txtID.Text = null;
                                    lbthoigiancheck.Text = null;
                                }
                                else
                                {
                                    if (DateTime.Now.ToString("yyMMdd") == newstring)
                                    {
                                        if (check.First().sudung == "false")
                                        {
                                            SoundPlayer checkok = new SoundPlayer(Application.StartupPath + @"\sound\Beep_Once.wav");
                                            lbThongTinNV.Text = check.First().manhansu.ToString() + "-" + check.First().hoten.ToString() + "-" + check.First().phong.ToString() + "-" + check.First().ban.ToString();
                                            lbThongBao.Text = "OK";
                                            checkok.Play();
                                            lbThongBao.BackColor = Color.Green;
                                            ck.sudung = "true";
                                            ck.thoigiansudung = DateTime.Now.ToString();
                                            ck.soxuatandadung = check.First().soxuatandadung + 1;
                                            UpdateCheckBaoCom(ck);
                                            checkok.Dispose();
                                            txtID.Text = null;
                                            lbthoigiancheck.Text = null;
                                        }
                                        else
                                        {
                                            SoundPlayer checkok = new SoundPlayer(Application.StartupPath + @"\sound\Beep_Once.wav");
                                            lbThongTinNV.Text = check.First().manhansu.ToString() + "-" + check.First().hoten.ToString() + "-" + check.First().phong.ToString() + "-" + check.First().ban.ToString();
                                            lbThongBao.Text = "OK";
                                            checkok.Play();
                                            lbThongBao.BackColor = Color.Green;
                                            checkok.Dispose();
                                            lbthoigiancheck.BackColor = Color.Red;
                                            lbthoigiancheck.Text = "Thời gian sử dụng:" + check.First().thoigiansudung;
                                            txtID.Text = null;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show("Hãy tạo dữ liệu data local mới nhất !");
                                    }
                                }
                            }
                            else
                            {
                                SoundPlayer checkng = new SoundPlayer(Application.StartupPath + @"\sound\buzzer_x.wav");
                                checkng.Play();
                                lbThongBao.Text = "NG";
                                lbThongBao.BackColor = Color.Red;
                                checkng.Dispose();
                                txtID.Text = null;
                                lbThongTinNV.Text = null;
                                lbthoigiancheck.Text = null;
                                lbthoigiancheck.BackColor = DefaultBackColor;
                            }
                        }
                    }
                }
                else
                {
                    txtID.Text = null;
                    MessageBox.Show("Hãy Chọn Bữa Ăn!");
                }
            }
          
        }

        private async void UpdateCheckBaoCom(CheckBaoCom ck)
        {
            string APIbaocom = "http://192.84.100.207/MealOrdersAPI/api/BaoComBuaAn";
            if (comboBox1.Text == "Online")
            {
                using (var client = new HttpClient())
                {
                    var serializedProduct = JsonConvert.SerializeObject(ck);
                    var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                    var result = await client.PutAsync(String.Format("{0}/{1}", APIbaocom, ck.id), content);
                }
                // Update local ==============================================

                string filePath = Application.StartupPath + @"\FileName\checkcom.txt";
                StreamReader objReader = new StreamReader(filePath);
                string nameBaoCom = objReader.ReadLine();
                objReader.Close();
                string pathfile = Application.StartupPath + @"\CheckCom\" + nameBaoCom + "";

                System.Data.OleDb.OleDbConnection MyConnectionup;
                System.Data.OleDb.OleDbCommand myCommandup = new System.Data.OleDb.OleDbCommand();
                string sqlup = null;
                MyConnectionup = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnectionup.Open();
                myCommandup.Connection = MyConnectionup;
                sqlup = "update [Sheet1$] set sudung="
                  + ck.sudung + ",thoigiansudung='"
                  + ck.thoigiansudung + "',soxuatandadung="
                  + ck.soxuatandadung + " where id="
                  + ck.id + "";

                myCommandup.CommandText = sqlup;
                myCommandup.ExecuteNonQuery();
                MyConnectionup.Close();
            }
            else
            {
                // Update local ==============================================

                string filePath = Application.StartupPath + @"\FileName\checkcom.txt";
                StreamReader objReader = new StreamReader(filePath);
                string nameBaoCom = objReader.ReadLine();
                objReader.Close();
                string pathfile = Application.StartupPath + @"\CheckCom\" + nameBaoCom + "";

                System.Data.OleDb.OleDbConnection MyConnectionup;
                System.Data.OleDb.OleDbCommand myCommandup = new System.Data.OleDb.OleDbCommand();
                string sqlup = null;
                MyConnectionup = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnectionup.Open();
                myCommandup.Connection = MyConnectionup;
                sqlup = "update [Sheet1$] set sudung="
                   + ck.sudung + ",thoigiansudung='"
                   + ck.thoigiansudung + "',soxuatandadung="
                   + ck.soxuatandadung + " where id="
                   + ck.id + "";

                myCommandup.CommandText = sqlup;
                myCommandup.ExecuteNonQuery();
                MyConnectionup.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cbBuaAn.Items.Clear();
            cbBuaAn.Text = null;
            lbThongBao.Text = null;
            lbThongBao.BackColor = DefaultBackColor;
            lbThongTinNV.Text = null;
            lbthoigiancheck.Text = null;
            lbthoigiancheck.BackColor = DefaultBackColor;
            Getbuaan();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            lbThongBao.Text = null;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtID.Text = null;
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
        }
    }
}