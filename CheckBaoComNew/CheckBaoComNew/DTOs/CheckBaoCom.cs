using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckBaoComNew.DTOs
{
    public class CheckBaoCom
    {
        public int ID { get; set; }
        public int empid { get; set; }
        public string manhansu { get; set; }
        public string hoten { get; set; }
        public int phongid { get; set; }
        public int banid { get; set; }
        public int congdoanid { get; set; }
        public int khach { get; set; }
        public DateTime ngay { get; set; }
        public int thang { get; set; }
        public int nam { get; set; }
        public int taikhoandat { get; set; }
        public DateTime thoigiandat { get; set; }
        public byte sudung { get; set; }
        public DateTime thoigiansudung { get; set; }
        public int sosuatsudung { get; set; }
        public float sotiendadung { get; set; }
        public byte chot { get; set; }
        public string ghichu { get; set; }
        public int thucdontheobuaanid { get; set; }
        public int buaanid { get; set; }
        public string phong { get; set; }
        public string ban { get; set; }
        public string congdoan { get; set; }
        public byte dangky { get; set; }

    }
}
