using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckView.DTOs
{
    public class CheckBaoCom
    {
        public int id { get; set; }
        public int empid { get; set; }
        public string manhansu { get; set; }
        public string hoten { get; set; }
        public int phongid { get; set; }
        public string phong { get; set; }
        public string banid { get; set; }
        public string ban { get; set; }
        public string congdoanid { get; set; }
        public string congdoan { get; set; }
        public string khach { get; set; }
        public DateTime ngay { get; set; }
        public int thang { get; set; }
        public int nam { get; set; }
        public int taikhoandat { get; set; }
        public DateTime thoigiandat { get; set; }
        public string sudung { get; set; }
        public string dangky { get; set; }
        public string thoigiansudung { get; set; }
        public int soxuatandadung { get; set; }
        public float sotiendadung { get; set; }
        public string chot { get; set; }
        public string ghichu { get; set; }
        public string thucdontheobuaid { get; set; }
        public string thucdontheobua { get; set; }
        public int buaanid { get; set; }
        public string buaan { get; set; }
    }
}
