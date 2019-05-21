using CheckBaoComNew.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Linq;

namespace CheckBaoComNew
{
    
    public partial class FormTestGetAll : Form
    {

        public string cbtenline = null;
        public string cbtenca = null;
        public string cbtenthucdon = null;
        public FormTestGetAll()
        {
            InitializeComponent();
            getthucdon_ca();
        }

        static private async Task<string> GetAllThucDon()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync("http://localhost:3000/thucdon");
            return astr;
        }
        static private async Task<string> GetAllCa()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync("http://localhost:3000/ca");
            return astr;
        }
        public void getthucdon_ca()
        {
        
           // get thucdon
            List<ThucDon> td = new List<ThucDon>();
            Task<string> callTaskthucdon = Task.Run(() => GetAllThucDon());
            callTaskthucdon.Wait();
            string astrthucdon = callTaskthucdon.Result;
            List<ThucDon> thucdon = JsonConvert.DeserializeObject<List<ThucDon>>(astrthucdon);
            td = thucdon.Where(t => t.tenthucdon ==cbtenthucdon ).ToList();
            // get ca
            List<Ca> ca = new List<Ca>();
            Task<string> callTaskca = Task.Run(() => GetAllCa());
            callTaskca.Wait();
            string astrca = callTaskca.Result;
            List<Ca> caan = JsonConvert.DeserializeObject<List<Ca>>(astrca);
            ca = caan.Where(c => c.tenca == cbtenca).ToList();
            if (cbtenline == "Line1")
            {
                pictureBox1.Image = Image.FromFile(Application.StartupPath + @"\Image\" + td.First().picture);
                lbtongline1.Text = ca.First().tongsuatan.ToString();
                lbconlailine1.Text = ca.First().suatanconlai.ToString();
            }
            if (cbtenline == "Line2")
            {
                pictureBox2.Image = Image.FromFile(Application.StartupPath + @"\Image\" + td.First().picture);
                lbtongline2.Text = ca.First().tongsuatan.ToString();
                lbconlailine2.Text = ca.First().suatanconlai.ToString();
            }
            if (cbtenline == "Line3")
            {
                pictureBox3.Image = Image.FromFile(Application.StartupPath + @"\Image\" + td.First().picture);
                lbtongline3.Text = ca.First().tongsuatan.ToString();
                lbconlailine3.Text = ca.First().suatanconlai.ToString();
            }
            if (cbtenline == "Line4")
            {
                pictureBox4.Image = Image.FromFile(Application.StartupPath + @"\Image\" + td.First().picture);
                lbtongline4.Text = ca.First().tongsuatan.ToString();
                lbconlailine4.Text = ca.First().suatanconlai.ToString();
            }
            if (cbtenline == "Line5")
            {
                pictureBox5.Image = Image.FromFile(Application.StartupPath + @"\Image\" + td.First().picture);
                lbtongline5.Text = ca.First().tongsuatan.ToString();
                lbconlailine5.Text = ca.First().suatanconlai.ToString();
            }
            if (cbtenline == "Line6")
            {
                pictureBox6.Image = Image.FromFile(Application.StartupPath + @"\Image\" + td.First().picture);
                lbtongline6.Text = ca.First().tongsuatan.ToString();
                lbconlailine6.Text = ca.First().suatanconlai.ToString();
            }
        }
      
    }
}