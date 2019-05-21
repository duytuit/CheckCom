using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Windows.Forms;
using WindowsFormsApplication1.DTOs;

namespace WindowsFormsApplication1
{
    public partial class Form3 : Form
    {
        public Form3()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Update
            CheckBaoCom ck = new CheckBaoCom()
            {
                id = "00058EF5E55743078B9D66C6864A5DB6",
                empid = null,
                manhansu = "008809",
                hoten = "Nguyễn Thị Minh Tuyết",
                phongid = null,
                phong = "MA1A",
                banid = null,
                ban = "Drilling",
                congdoanid = null,
                congdoan = null,
                khach = "false",
                ngay = DateTime.Now.ToString("2019-05-11"),
                thang = 5,
                nam = 2019,
                userid = null,
                thoigiandat = DateTime.Now.ToString("2019-05-11"),
                sudung = "true",
                dangky = "true",
                thoigiansudung = DateTime.Now.ToString("2019-05-11 HH:mm:ss"),
                soxuatandadung = 0,
                sotiendadung = 0,
                chot = "true",
                ghichu = "update123",
               // thucdontheobuaid = null,
              //  thucdontheobua = null,
              //  kieudoan = 0,
                buaanid = "275AD44F777F4C679B8F72B8F299A931",
              //  buaan = "Trưa",
              //  ca = 0,
                nhaanid = "D982F0F304104EB3A9A903CCC23958C3",
              //  nhaan = "Việt",
               // loaidouong = 0,
               // thanhtoan = 0,
               // phongrieng = 0,
                dangkybosung = "false"
            };
            UpdateCheckBaoCom(ck);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Insert
            CheckBaoCom ck = new CheckBaoCom()
            {
                empid = "1434",
                manhansu = "0001434",
                hoten = "Đinh Thị OO",
                phongid = null,
                phong = "QC5",
                banid = null,
                ban = "CSP",
                congdoanid = null,
                congdoan = "---",
                khach = "false",
                ngay = DateTime.Now.ToString("yyyy-MM-dd"),
                thang = 4,
                nam = 2019,
                //userid = "3C5AC74AC16E4C3DB8FE69CB2210A828",
                thoigiandat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                sudung = "false",
                dangky = "false",
                thoigiansudung = null,
                soxuatandadung = 0,
                sotiendadung = 0,
                chot = "true",
                ghichu = "test2",
                buaanid = "275AD44F777F4C679B8F72B8F299A931",
                nhaanid = "D982F0F304104EB3A9A903CCC23958C3",
                dangkybosung="true"
            };
            InsertCheckBaoCom(ck);
        }

        private async void UpdateCheckBaoCom(CheckBaoCom ck)
        {
            string APIbaocom = "http://192.84.100.207/MealOrdersAPI/api/DulieuBaoComs";

            using (var client = new HttpClient())
            {
                var serializedProduct = JsonConvert.SerializeObject(ck);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                var result = await client.PutAsync(String.Format("{0}/{1}", APIbaocom, ck.id), content);
            }
        }

        private async void InsertCheckBaoCom(CheckBaoCom ck)
        {
            string APIbaocom = "http://192.84.100.207/MealOrdersAPI/api/DulieuBaoComs";
            using (var client = new HttpClient())
            {
                var serializedProduct = JsonConvert.SerializeObject(ck);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(APIbaocom, content);
            }
        }
    }
}