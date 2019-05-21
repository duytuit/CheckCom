using CheckBaoComNew.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Drawing;
using WindowsFormsApplication1;

namespace CheckBaoComNew
{
    public partial class Form1 : Form
    {
        private string APIca = "http://localhost:3000/Ca";
       // private string APIthucdon = "http://localhost:3000/ThucDon";
       // private string APIbuaan = "http://localhost:3000/BuaAn";
        private string APICheckBaoCom = "http://localhost:3000/CheckBaoCom";
        FormTestGetAll fro = new FormTestGetAll();
        public Form1()
        {
            InitializeComponent();
            GetNhamay();
            fro.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
           
        }
        static private async Task<string> GetAllThucDon()
        {
           
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync("http://localhost:3000/ThucDon");
            return astr;
        }

        static private async Task<string> GetAllCa()
        {

            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync("http://localhost:3000/Ca");
            return astr;
        }

        static private async Task<string> GetAllbuaan()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync("http://localhost:3000/BuaAn");
            return astr;
        }

        static private async Task<string> GetAllCheckBaoCom()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync("http://localhost:3000/CheckBaoCom");
            return astr;
        }
        static private async Task<string> GetAllnhamay()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync("http://localhost:3000/NhaMay");
            return astr;
        }

         private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get ID nha may =======================
            comboBox2.Items.Clear();
            comboBox2.Text = null;
            comboBox3.Items.Clear();
            comboBox3.Text = null;
            comboBox1.Items.Clear();
            comboBox1.Text = null;
            lbtongsuatan.Text = null;
            lbsuatanconlai.Text = null;
            List<NhaMay> nm = new List<NhaMay>();
            Task<string> callTasknhamay = Task.Run(() => GetAllnhamay());
            callTasknhamay.Wait();
            string astrnhamay = callTasknhamay.Result;
            List<NhaMay> nhamay = JsonConvert.DeserializeObject<List<NhaMay>>(astrnhamay);
            nm = nhamay.Where(n=>n.tennhamay==comboBox4.Text).ToList();
            int idnhamay = nm.First().ID;
            // get ten bua an theo id nha may========

            List<BuaAn> ba = new List<BuaAn>();
            Task<string> callTask = Task.Run(() => GetAllbuaan());
            callTask.Wait();
            string astr = callTask.Result;
            List<BuaAn> buaan = JsonConvert.DeserializeObject<List<BuaAn>>(astr);
            ba = buaan.Where(b => b.nhamayid == idnhamay).ToList();
            foreach (BuaAn b in ba)
            {
                comboBox2.Items.Add(b.tenbuaan);
            }
        }
        private void GetNhamay()
        {
            Task<string> callTask = Task.Run(() => GetAllnhamay());
            callTask.Wait();
            string astr = callTask.Result;
            List<NhaMay> nhamay = JsonConvert.DeserializeObject<List<NhaMay>>(astr);

            foreach(NhaMay nm in nhamay)
            {
                comboBox4.Items.Add(nm.tennhamay);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<Ca> ca = new List<Ca>();
            Task<string> callTask = Task.Run(() => GetAllCa());
            callTask.Wait();
            string astr = callTask.Result;
            List<Ca> caan = JsonConvert.DeserializeObject<List<Ca>>(astr);
            ca = caan.Where(c => c.tenca == comboBox1.Text).ToList();
            fro.cbtenca = comboBox1.Text;
            if (ca.Count>0)
            {
                lbtongsuatan.Text = ca.First().tongsuatan.ToString();
                lbsuatanconlai.Text = ca.First().suatanconlai.ToString();
            }else
            {
                lbtongsuatan.Text = null;
                lbsuatanconlai.Text = null;
            }
          
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get ID bữa ăn =======================
            comboBox3.Items.Clear();
            comboBox1.Text = null;
            comboBox3.Text = null;
            comboBox1.Items.Clear();
            comboBox1.Text = null;
            //lbtongsuatan.Text = null;
            //lbsuatanconlai.Text = null;
            List<BuaAn> ba = new List<BuaAn>();
            Task<string> callTaskbuaan = Task.Run(() => GetAllbuaan());
            callTaskbuaan.Wait();
            string astrbuaan = callTaskbuaan.Result;
            List<BuaAn> buaan = JsonConvert.DeserializeObject<List<BuaAn>>(astrbuaan);
            ba = buaan.Where(n => n.tenbuaan == comboBox2.Text).ToList();
            int idbuaan = ba.First().ID;
            // get ten thực đơn theo id bữa ăn ===========
            List<ThucDon> don = new List<ThucDon>();
            Task<string> callTask = Task.Run(() => GetAllThucDon());
            callTask.Wait();
            string astr = callTask.Result;
            List<ThucDon> thucdon = JsonConvert.DeserializeObject<List<ThucDon>>(astr);
            don = thucdon.Where(c => c.ID == idbuaan).ToList();
            foreach (ThucDon t in don)
            {
                comboBox3.Items.Add(t.tenthucdon);
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            // get ID thực đơn =======================
            comboBox1.Items.Clear();
            comboBox1.Text = null;
            pictureBox1.Image = null;
            //lbtongsuatan.Text = null;
            //lbsuatanconlai.Text = null;
            List<ThucDon> td = new List<ThucDon>();
            Task<string> callTaskthucdon = Task.Run(() => GetAllThucDon());
            callTaskthucdon.Wait();
            string astrthucdon = callTaskthucdon.Result;
            List<ThucDon> thucdon = JsonConvert.DeserializeObject<List<ThucDon>>(astrthucdon);
            td = thucdon.Where(t => t.tenthucdon == comboBox3.Text).ToList();
            int idthucdon = td.First().ID;
            pictureBox1.Image = Image.FromFile(Application.StartupPath + @"\Image\" + td.First().picture);
            // get ten ca theo id thực đơn ===========
            List<Ca> ca = new List<Ca>();
            Task<string> callTask = Task.Run(() => GetAllCa());
            callTask.Wait();
            string astr = callTask.Result;
            List<Ca> caan = JsonConvert.DeserializeObject<List<Ca>>(astr);
            ca = caan.Where(c => c.ID == idthucdon).ToList();
            fro.cbtenthucdon = comboBox3.Text;
            foreach (Ca c in ca)
            {
                comboBox1.Items.Add(c.tenca);
            }
        }

         private void textBox1_TextChanged(object sender, EventArgs e)
        {
            List<CheckBaoCom> check = new List<CheckBaoCom>();
            Task<string> callTask = Task.Run(() => GetAllCheckBaoCom());
            callTask.Wait();
            string astr = callTask.Result;
            List<CheckBaoCom> baocom = JsonConvert.DeserializeObject<List<CheckBaoCom>>(astr);
            check = baocom.Where(c => c.manhansu == textBox1.Text).ToList();
            if(check.Count>0)
            {
                lbthongtinnhanvien.Text = check.First().manhansu.ToString() + "-" + check.First().hoten.ToString() + "-" + check.First().phong.ToString() + "-" + check.First().ban.ToString();
                lbthongbao.Text = "OK";
                lbthongbao.BackColor =Color.Green;
                UpdateCa();
                UpdateCheckBaoCom();
                fro.getthucdon_ca();
                List<Ca> ca = new List<Ca>();
                Task<string> callTask1 = Task.Run(() => GetAllCa());
                callTask1.Wait();
                string astr1 = callTask1.Result;
                List<Ca> caan = JsonConvert.DeserializeObject<List<Ca>>(astr1);
                ca = caan.Where(c => c.tenca == comboBox1.Text).ToList();
                if (ca.Count > 0)
                {
                    lbtongsuatan.Text = ca.First().tongsuatan.ToString();
                    lbsuatanconlai.Text = ca.First().suatanconlai.ToString();
                }
                else
                {
                    lbtongsuatan.Text = null;
                    lbsuatanconlai.Text = null;
                }
            }
           else
            {
                lbthongtinnhanvien.Text = null;
                lbthongbao.Text = "NG";
                lbthongbao.BackColor = Color.Red;
            }
        }
        private async void UpdateCa()
        {
            List<Ca> ca = new List<Ca>();
            Task<string> callTaskca = Task.Run(() => GetAllCa());
            callTaskca.Wait();
            string astrca = callTaskca.Result;
            List<Ca> caan = JsonConvert.DeserializeObject<List<Ca>>(astrca);
            ca = caan.Where(c => c.tenca == comboBox1.Text).ToList();
            if(ca.Count>0)
            {
                Ca canew = new Ca();
                canew.ID = ca.First().ID;
                canew.thucdonid = ca.First().thucdonid;
                canew.tenca = ca.First().tenca;
                canew.tongsuatan = ca.First().tongsuatan;
                canew.suatanconlai = ca.First().suatanconlai - 1;
                using (var client = new HttpClient())
                {
                    var serializedProduct = JsonConvert.SerializeObject(canew);
                    var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                    var result = await client.PutAsync(String.Format("{0}/{1}", APIca, canew.ID), content);
                }
            }
        }
        private async void UpdateCheckBaoCom()
        {
            // get ID thực đơn ===========================
            List<ThucDon> td = new List<ThucDon>();
            Task<string> callTaskthucdon = Task.Run(() => GetAllThucDon());
            callTaskthucdon.Wait();
            string astrthucdon = callTaskthucdon.Result;
            List<ThucDon> thucdon = JsonConvert.DeserializeObject<List<ThucDon>>(astrthucdon);
            td = thucdon.Where(t => t.tenthucdon == comboBox3.Text).ToList();
            // get ID bữa ăn   ===========================
            List<BuaAn> ba = new List<BuaAn>();
            Task<string> callTaskbuaan = Task.Run(() => GetAllbuaan());
            callTaskbuaan.Wait();
            string astrbuaan = callTaskbuaan.Result;
            List<BuaAn> buaan = JsonConvert.DeserializeObject<List<BuaAn>>(astrbuaan);
            ba = buaan.Where(t => t.tenbuaan == comboBox2.Text).ToList();
            // get all checkbaocom ========================
            List<CheckBaoCom> check = new List<CheckBaoCom>();
            Task<string> callTaskbaocom = Task.Run(() => GetAllCheckBaoCom());
            callTaskbaocom.Wait();
            string astrbaocom = callTaskbaocom.Result;
            List<CheckBaoCom> baocom = JsonConvert.DeserializeObject<List<CheckBaoCom>>(astrbaocom);
            check = baocom.Where(c => c.manhansu == textBox1.Text).ToList();
            if (check.Count > 0 && td.Count>0 && ba.Count>0)
            {
                CheckBaoCom baocomnew = new CheckBaoCom();
                baocomnew.ID = check.First().ID;
                baocomnew.empid = check.First().empid;
                baocomnew.manhansu = check.First().manhansu;
                baocomnew.hoten = check.First().hoten;
                baocomnew.phongid = check.First().phongid;
                baocomnew.banid = check.First().banid;
                baocomnew.congdoanid = check.First().congdoanid;
                baocomnew.khach = check.First().khach;
                baocomnew.ngay = check.First().ngay;
                baocomnew.thang = check.First().thang;
                baocomnew.nam = check.First().nam;
                baocomnew.taikhoandat = check.First().taikhoandat;
                baocomnew.thoigiandat = check.First().thoigiandat;
                baocomnew.sudung = 1;
                baocomnew.thoigiansudung = DateTime.Now;
                baocomnew.sosuatsudung = check.First().sosuatsudung+1;
                baocomnew.sotiendadung = check.First().sotiendadung;
                baocomnew.chot = check.First().chot;
                baocomnew.ghichu = check.First().ghichu;
                baocomnew.thucdontheobuaanid = td.First().ID;
                baocomnew.buaanid = ba.First().ID;
                baocomnew.phong = check.First().phong;
                baocomnew.ban = check.First().ban;
                baocomnew.congdoan = check.First().congdoan;
                baocomnew.dangky = check.First().dangky;
                using (var client = new HttpClient())
                {
                    var serializedProduct = JsonConvert.SerializeObject(baocomnew);
                    var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                    var result = await client.PutAsync(String.Format("{0}/{1}", APICheckBaoCom, baocomnew.ID), content);
                }
            }
        }

        private void cbSelectLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            fro.cbtenline = cbSelectLine.Text;
        }
    }
}