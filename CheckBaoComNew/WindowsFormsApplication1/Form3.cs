using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
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
                id = "D5B971CA8B3746488D16CA5D86F3D3E1",
                empid = "71946318-e147-4aea-8c19-861353f72f1b",
                manhansu = "007468",
                hoten = "Nguyễn Duy Tú",
                phongid = "98556f84-6d3e-42fa-a084-6b9d22839181",
                phong = "IT",
                banid = "b5c69e5b-61f9-48c4-9ccd-b8cd6f426b93",
                ban = "Phần cứng",
                congdoanid = null,
                congdoan = "---",
                khach = "false",
                ngay = DateTime.Now.ToString("yyyy-MM-dd"),
                thang = 8,
                nam = 2019,
                userid = "2B5D232BE23D44EABC26E097102D9F13",
                thoigiandat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                sudung = "false",
                dangky = "true",
                thoigiansudung = null,
                soxuatandadung = 0,
                sotiendadung = 0,
                chot = "true",
                ghichu = null,
                buaanid = "AE5D60454C19461F831F4251D63BBCA2",
                nhaanid = "D982F0F304104EB3A9A903CCC23958C3",
                dangkybosung = "false",
                nhabep = ""
            };
            bool check;
           check= Task.Run(() => UpdateCheckBaoCom(ck)).Result;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Insert
            CheckBaoCom ck = new CheckBaoCom()
            {
                empid = "71946318-e147-4aea-8c19-861353f72f1b",
                manhansu = "007468",
                hoten = "Nguyễn Duy Tú",
                phongid = "98556f84-6d3e-42fa-a084-6b9d22839181",
                phong = "IT",
                banid = "b5c69e5b-61f9-48c4-9ccd-b8cd6f426b93",
                ban = "Phần cứng",
                congdoanid = null,
                congdoan = "---",
                khach = "false",
                ngay = DateTime.Now.ToString("yyyy-MM-dd"),
                thang = DateTime.Now.Month,
                nam = DateTime.Now.Year,
                userid = "2B5D232BE23D44EABC26E097102D9F13",
                thoigiandat = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                sudung = "false",
                dangky = "true",
                thoigiansudung = null,
                soxuatandadung = 0,
                sotiendadung = 0,
                chot = "true",
                ghichu = null,
                buaanid = "AE5D60454C19461F831F4251D63BBCA2",
                nhaanid = "D982F0F304104EB3A9A903CCC23958C3",
                dangkybosung = "false",
            };
            InsertCheckBaoCom(ck);
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

        private void button3_Click(object sender, EventArgs e)
        {
            InsertNhomKy();
        }
        private async void InsertNhomKy()
        {
            List<nhomky> mk = new List<nhomky>()
            {
                new nhomky {User_id="dsfsdfdf7868dsfđfsdf",Nhomky_id=Guid.Parse("4dfa445a-ea72-e911-80fc-40a3cc3b43da"),Username="tú" },
                new nhomky {User_id="dsfsdfdfdsfđfsdf",Nhomky_id=Guid.Parse("4dfa445a-ea72-e911-80fc-40a3cc3b43da"),Username="tuấn" },
                new nhomky {User_id="dsfsdfdf74dsfđfsdf",Nhomky_id=Guid.Parse("4dfa445a-ea72-e911-80fc-40a3cc3b43da"),Username="toàn" },
                new nhomky {User_id="dsfsdfdfdsfđfsdf",Nhomky_id=Guid.Parse("4dfa445a-ea72-e911-80fc-40a3cc3b43da"),Username="huy" },
                new nhomky {User_id="dsfsdfdyuyfdsfđfsdf",Nhomky_id=Guid.Parse("4dfa445a-ea72-e911-80fc-40a3cc3b43da"),Username="hùng" },
                new nhomky {User_id="dsfsdfdfdsfđfsdf",Nhomky_id=Guid.Parse("4dfa445a-ea72-e911-80fc-40a3cc3b43da"),Username="dũng" },
                new nhomky {User_id="dsfsdfdlklfdsfđfsdf",Nhomky_id=Guid.Parse("4dfa445a-ea72-e911-80fc-40a3cc3b43da"),Username="an" },
                new nhomky {User_id="dsfsdfdfdsfđfsdf",Nhomky_id=Guid.Parse("4dfa445a-ea72-e911-80fc-40a3cc3b43da"),Username="khánh" },
                new nhomky {User_id="dsfsdfdf456dsfđfsdf",Nhomky_id=Guid.Parse("4dfa445a-ea72-e911-80fc-40a3cc3b43da"),Username="chiến" },
                new nhomky {User_id="dsfsdfd456fdsfđfsdf",Nhomky_id=Guid.Parse("4dfa445a-ea72-e911-80fc-40a3cc3b43da"),Username="nam" },
                new nhomky {User_id="dsfsdfd456fdsfđfsdf",Nhomky_id=Guid.Parse("4dfa445a-ea72-e911-80fc-40a3cc3b43da"),Username="quang" },
                new nhomky {User_id="dsfsdfd456fdsfđfsdf",Nhomky_id=Guid.Parse("4dfa445a-ea72-e911-80fc-40a3cc3b43da"),Username="châu" },

            };

            string APIbaocom = "http://localhost:50209/Api/Data3";
            using (var client = new HttpClient())
            {
                var serializedProduct = JsonConvert.SerializeObject(mk);
                var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");
                var result = await client.PostAsync(APIbaocom, content);
            }
        }
    }
}