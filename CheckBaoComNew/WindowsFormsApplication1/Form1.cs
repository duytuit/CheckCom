using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication1.DTOs;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        List<CheckBaoCom> t = new List<CheckBaoCom>();
        public Form1()
        {
            InitializeComponent();
            GetBaoCom();
        }

        private async Task<string> GetAllCheckBaoCom()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync("http://192.84.100.207/MealOrdersAPI/api/BaoComBuaAn");
            return astr;
        }

        private void GetBaoCom()
        {
            Task<string> callTask = Task.Run(() => GetAllCheckBaoCom());
            callTask.Wait();
            string astr = callTask.Result;
            t = JsonConvert.DeserializeObject<List<CheckBaoCom>>(astr);
            label2.Text = t.Count.ToString();
        }

        private  void textBox1_TextChanged(object sender, EventArgs e)
        {
            //await Task.Delay(70);
            //if (!string.IsNullOrEmpty(textBox1.Text))
            //{
                
            //    List<CheckBaoCom> check = t;
            //    check = check.Where(x => x.manhansu == textBox1.Text).ToList();
            //    if (check.Count == 1)
            //    {
            //        CheckBaoCom ck = new CheckBaoCom()
            //        {
            //            id = check.First().id,
            //            empid = check.First().empid,
            //            manhansu = check.First().manhansu,
            //            hoten = check.First().hoten,
            //            phongid = check.First().phongid,
            //            phong = check.First().phong,
            //            banid = check.First().banid,
            //            ban = check.First().ban,
            //            congdoanid = check.First().congdoanid,
            //            congdoan = check.First().congdoan,
            //            khach = check.First().khach,
            //            ngay = check.First().ngay,
            //            thang = check.First().thang,
            //            nam = check.First().nam,
            //            taikhoandat = check.First().taikhoandat,
            //            thoigiandat = check.First().thoigiandat,
            //            sudung = check.First().sudung,
            //            dangky = check.First().dangky,
            //            thoigiansudung = check.First().thoigiansudung,
            //            soxuatandadung = check.First().soxuatandadung,
            //            sotiendadung = check.First().sotiendadung,
            //            chot = check.First().chot,
            //            ghichu = check.First().ghichu,
            //            thucdontheobuaid = check.First().thucdontheobuaid,
            //          //  thucdontheobua = check.First().thucdontheobua,
            //            buaanid = check.First().buaanid,
            //            buaan = check.First().buaan
            //        };
            //        if (check.First().dangky == "False")
            //        {
            //            SoundPlayer checkng = new SoundPlayer(Application.StartupPath + @"\sound\buzzer_x.wav");
            //            checkng.Play();
            //            checkng.Dispose();
            //            textBox1.Text = null;
            //            lbthongbao.Text = "NG";
            //            lbthongbao.BackColor = Color.Red;
            //        }
            //        else
            //        {
            //            if (check.First().sudung == "false")
            //            {
            //                SoundPlayer checkok = new SoundPlayer(Application.StartupPath + @"\sound\Beep_Once.wav");
            //                checkok.Play();
            //                checkok.Dispose(); 
            //                lbthongbao.Text = "OK";
            //                lbthongbao.BackColor = Color.Green;
            //                textBox1.Text = null;
            //            }
            //            else
            //            {
            //                SoundPlayer checkok = new SoundPlayer(Application.StartupPath + @"\sound\Beep_Once.wav");
            //                checkok.Play();
            //                checkok.Dispose();
            //                lbthongbao.Text = "OK";
            //                textBox1.Text = null;
            //            }
            //        }
            //    }
            //    else
            //    {
            //        SoundPlayer checkng = new SoundPlayer(Application.StartupPath + @"\sound\buzzer_x.wav");
            //        checkng.Play();
            //        checkng.Dispose();
            //        textBox1.Text = null;
            //        lbthongbao.Text = "NG";
            //        lbthongbao.BackColor = Color.Red;
            //    }
            //}
        }

        private void lbthongbao_Click(object sender, EventArgs e)
        {

        }
    }
}