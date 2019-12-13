using CheckCom_Version2.DTOs;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZXing;
using ZXing.Common;

namespace CheckCom_Version2
{
    public partial class Card_Check : Form
    {
        private string caanid;
        private string caan = null;
        private List<BuaAn> buaan = new List<BuaAn>();
        List<Bitmap> bm = new List<Bitmap>();
        List<Bitmap> bmNew = new List<Bitmap>();
        private string filecheck = null;
        private string filebuaan = null;

        private string fileApidlbc = null;
        private string fileApibuaan = null;
        private string fileApinv = null;
        private string fileApibp = null;
        public Card_Check()
        {
            InitializeComponent();
            int Gio = DateTime.Now.Hour;
            getPath();
            getApi();
            GetBuaan();
            if ((8 <= Gio) && (Gio < 14))
            {
                cbBuaan.Text = "Trưa";
                caan = " Trua";
            }
            else if ((14 <= Gio) && (Gio < 20))
            {
                cbBuaan.Text = "Chiều";
                caan = " Chieu";
            }
            else if ((2 <= Gio) && (Gio < 8))
            {
                cbBuaan.Text = "Bữa nhẹ";
                caan = " Buanhe";
            }
            else
            {
                cbBuaan.Text = "Tối";
                caan = " Toi";
            }
        }
        private bool CheckData()
        {
            bool kiemtrabaocom = false;
            string fileToRead = System.IO.Path.GetDirectoryName(filecheck);

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
        private void getApi()
        {
            try
            {
                string path = Application.StartupPath + @"\Api.txt";
                fileApidlbc = File.ReadAllLines(path)[0];
                fileApibuaan = File.ReadAllLines(path)[1];
                fileApinv = File.ReadAllLines(path)[2];
                fileApibp = File.ReadAllLines(path)[3];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void getPath()
        {
            try
            {
                string path = Application.StartupPath + @"\Path.txt";
                filecheck = File.ReadAllLines(path)[0];
                filebuaan = File.ReadAllLines(path)[1];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        private void button1_Click(object sender, EventArgs e)
        {
            if(gvdanhsach.Rows.Count>0)
            {
                bool isSelected = false;
                for (int i = gvdanhsach.Rows.Count - 1; i >= 0; i--)
                {
                    isSelected = Convert.ToBoolean(gvdanhsach.Rows[i].Cells["check"].Value);
                  
                    if (isSelected)
                    {
                        string khach = gvdanhsach.Rows[i].Cells["khach"].Value.ToString();
                        if(khach == "False")
                        {
                            lbID.Text = gvdanhsach.Rows[i].Cells["manhansu"].Value.ToString();
                            lbTen.Text = gvdanhsach.Rows[i].Cells["hoten"].Value.ToString();
                            lbPhong.Text = gvdanhsach.Rows[i].Cells["phong"].Value.ToString();
                            lbBan.Text = gvdanhsach.Rows[i].Cells["ban"].Value.ToString();
                            lbHienTrang.Text = "Nhân Viên";
                           
                            var barcodeWriter = new BarcodeWriter
                            {
                                Format = BarcodeFormat.QR_CODE,
                                Options = new EncodingOptions
                                {
                                    Height = 70,
                                    Width = 70,
                                    Margin = 0
                                }
                            };

                            string content = lbID.Text;

                            using (var bitmap = barcodeWriter.Write(content))
                            {
                                using (var stream = new MemoryStream())
                                {
                                    bitmap.Save(stream, ImageFormat.Png);
                                    var image = Image.FromStream(stream);
                                    pictureBox1.Image = image;
                                    printDocument1.DocumentName = gvdanhsach.Rows[i].Cells["manhansu"].Value.ToString();
                                    printDocument1.Print();
                                    gvdanhsach.Rows[i].Cells["check"].Value = check.FalseValue;
                                }
                            }
                        }
                        else
                        {
                            lbID.Text = gvdanhsach.Rows[i].Cells["manhansu"].Value.ToString();
                            lbTen.Text = gvdanhsach.Rows[i].Cells["hoten"].Value.ToString();
                            lbPhong.Text = gvdanhsach.Rows[i].Cells["phong"].Value.ToString();
                            lbBan.Text = gvdanhsach.Rows[i].Cells["ban"].Value.ToString();
                            lbHienTrang.Text = "Khách";
                           
                            var barcodeWriter = new BarcodeWriter
                            {
                                Format = BarcodeFormat.QR_CODE,
                                Options = new EncodingOptions
                                {
                                    Height = 70,
                                    Width = 70,
                                    Margin = 0
                                }
                            };

                            string content = lbID.Text;

                            using (var bitmap = barcodeWriter.Write(content))
                            {
                                using (var stream = new MemoryStream())
                                {
                                    bitmap.Save(stream, ImageFormat.Png);
                                    var image = Image.FromStream(stream);
                                    pictureBox1.Image = image;
                                    printDocument1.DocumentName= gvdanhsach.Rows[i].Cells["manhansu"].Value.ToString();
                                    printDocument1.Print();
                                    gvdanhsach.Rows[i].Cells["check"].Value = check.FalseValue;
                                }
                            }
                        }
                       
                    }
                }
            }else
            {
                MessageBox.Show("Chưa có dữ liệu!");
            }
              
           
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int chieudai = 0;
            int chieurong = 0;
            int rightpage = 0;
            for (int i = 0; i < bmNew.Count; i++)
            {
                if (chieudai > e.MarginBounds.Bottom - this.panel1.Height)
                {
                    chieurong = this.panel1.Width;
                    e.Graphics.DrawImage(bmNew[i], chieurong+5, rightpage + 3, this.panel1.Width, this.panel1.Height);
                    rightpage = rightpage + this.panel1.Height;
                }
                else
                {
                    e.Graphics.DrawImage(bmNew[i], 0, chieudai + 3, this.panel1.Width, this.panel1.Height);
                    chieudai = chieudai + this.panel1.Height;
                }
            }

        }

        private void GetBuaan()
        {
            buaan.Clear();
            try
            {
                string pathfile = filebuaan + "BuaAn.xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
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
                            ten = drow["ten"].ToString()
                        };
                        cbBuaan.Items.Add(ba.ten);
                        buaan.Add(ba);
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Không có dữ liệu bữa ăn!");
            }
        }
        private void GetDataClient()
        {
            try
            {
                string pathfile = filecheck + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$] order by manhansu asc", MyConnection);
                oada.Fill(table);
                MyConnection.Close();
                gvdanhsach.DataSource = null;
                gvdanhsach.AutoGenerateColumns = false;
                gvdanhsach.DataSource = table;
                gvdanhsach.ClearSelection();
            }
            catch (Exception)
            {
                MessageBox.Show("Chưa có dữ liệu!");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtIDnhanvien.Text))
            {
                MessageBox.Show("Vui lòng nhập ID nhân viên!",
                    "QR Code Generator");
                return;
            }else
            {
                try
                {
                    string pathfile = filecheck+ dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                    DataTable table = new DataTable();
                    System.Data.OleDb.OleDbConnection MyConnectionup;
                    MyConnectionup = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                    MyConnectionup.Open();
                    OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$] where manhansu='" + txtIDnhanvien.Text + "'", MyConnectionup);
                    oada.Fill(table);
                    for (int i = 0; i < table.Rows.Count; i++)
                    {
                        DataRow drow = table.Rows[i];

                        if (drow.RowState != DataRowState.Deleted)
                        {

                            string khach = drow["khach"].ToString();
                            if (khach == "False")
                            {
                                lbID.Text = drow["manhansu"].ToString();
                                lbTen.Text = drow["hoten"].ToString();
                                lbPhong.Text = drow["phong"].ToString();
                                lbBan.Text = drow["ban"].ToString();
                                lbHienTrang.Text = "Nhân Viên";
                             
                                var barcodeWriter = new BarcodeWriter
                                {
                                    Format = BarcodeFormat.QR_CODE,
                                    Options = new EncodingOptions
                                    {
                                        Height = 70,
                                        Width = 70,
                                        Margin = 0
                                    }
                                };

                                string content = lbID.Text;

                                using (var bitmap = barcodeWriter.Write(content))
                                {
                                    using (var stream = new MemoryStream())
                                    {
                                        bitmap.Save(stream, ImageFormat.Png);
                                        var image = Image.FromStream(stream);
                                        pictureBox1.Image = image;
                                        printDocument2.DocumentName = drow["manhansu"].ToString();
                                        printDocument2.Print();
                                    }
                                }

                            }
                            else
                            {
                                lbID.Text = drow["manhansu"].ToString();
                                lbTen.Text = drow["hoten"].ToString();
                                lbPhong.Text = drow["phong"].ToString();
                                lbBan.Text = drow["ban"].ToString();
                                lbHienTrang.Text = "Nhân Viên";
                               
                                var barcodeWriter1 = new BarcodeWriter
                                {
                                    Format = BarcodeFormat.QR_CODE,
                                    Options = new EncodingOptions
                                    {
                                        Height = 70,
                                        Width = 70,
                                        Margin = 0
                                    }
                                };

                                string content1 = lbID.Text;

                                using (var bitmap = barcodeWriter1.Write(content1))
                                {
                                    using (var stream = new MemoryStream())
                                    {
                                        bitmap.Save(stream, ImageFormat.Png);
                                        var image = Image.FromStream(stream);
                                        pictureBox1.Image = image;
                                        printDocument2.DocumentName = drow["manhansu"].ToString();
                                        printDocument2.Print();
                                    }
                                }
                            }

                        }
                    }
                }
                catch (Exception)
                {
                    MessageBox.Show("Không Tìm Thấy!Hãy chọn lại ngày và bữa ăn!");
                }
            }
          
            //--------------------------------------------------------------
          
        }

        private void cbBuaan_SelectedIndexChanged(object sender, EventArgs e)
        {
            gvdanhsach.DataSource = null;
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
                caan = " Buanhe";
            }
            foreach (BuaAn ba in buaan)
            {
                if (ba.ten == cbBuaan.Text)
                {
                    caanid = ba.id;
                }
            }
            bool check = CheckData();
            if(check==true)
            {
                GetDataClient();
            }else
            {
                MessageBox.Show("Chưa có dữ liệu!");
            }
        }
        private void lvServer_MouseClick(object sender, MouseEventArgs e)
        {
            string khach = "False";
            if (khach=="False")
            {
                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = 70,
                        Width = 70,
                        Margin = 0
                    }
                };

                string content = lbID.Text;

                using (var bitmap = barcodeWriter.Write(content))
                {
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                        var image = Image.FromStream(stream);
                        pictureBox1.Image = image;
                    }
                }
            }else
            {
                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = 70,
                        Width = 70,
                        Margin = 0
                    }
                };

                string content = lbID.Text;

                using (var bitmap = barcodeWriter.Write(content))
                {
                    using (var stream = new MemoryStream())
                    {
                        bitmap.Save(stream, ImageFormat.Png);
                        var image = Image.FromStream(stream);
                        pictureBox1.Image = image;
                    }
                }
            }
           
        }

        private void btnA4_Click(object sender, EventArgs e)
        {
            if (gvdanhsach.Rows.Count > 0)
            {
                bool isSelected = false;
                for (int i = gvdanhsach.Rows.Count - 1; i >= 0; i--)
                {
                    isSelected = Convert.ToBoolean(gvdanhsach.Rows[i].Cells["check"].Value);

                    if (isSelected)
                    {
                        string khach = gvdanhsach.Rows[i].Cells["khach"].Value.ToString();
                        if (khach == "False")
                        {
                            lbID.Text = gvdanhsach.Rows[i].Cells["manhansu"].Value.ToString();
                            lbTen.Text = gvdanhsach.Rows[i].Cells["hoten"].Value.ToString();
                            lbPhong.Text = gvdanhsach.Rows[i].Cells["phong"].Value.ToString();
                            lbBan.Text = gvdanhsach.Rows[i].Cells["ban"].Value.ToString();
                            lbHienTrang.Text = "Nhân Viên";
                          
                            var barcodeWriter = new BarcodeWriter
                            {
                                Format = BarcodeFormat.QR_CODE,
                                Options = new EncodingOptions
                                {
                                    Height = 70,
                                    Width = 70,
                                    Margin = 0
                                }
                            };

                            string content = lbID.Text;

                            using (var bitmap = barcodeWriter.Write(content))
                            {
                                using (var stream = new MemoryStream())
                                {
                                    bitmap.Save(stream, ImageFormat.Png);
                                    var image = Image.FromStream(stream);
                                    pictureBox1.Image = image;
                                    //printDocument1.DocumentName = gvdanhsach.Rows[i].Cells["manhansu"].Value.ToString();
                                    //printDocument1.Print();
                                    Bitmap bitmap1 = new Bitmap(this.panel1.Width, this.panel1.Height);
                                    panel1.DrawToBitmap(bitmap1, new Rectangle(0, 0, this.panel1.Width, this.panel1.Height));
                                    //bitmap1.Save(Application.StartupPath + @"\CardNV\" + gvdanhsach.Rows[i].Cells["manhansu"].Value.ToString() + ".png", ImageFormat.Png);

                                    bm.Add(bitmap1);
                                    gvdanhsach.Rows[i].Cells["check"].Value = check.FalseValue;
                                }
                            }
                           
                        }
                        else
                        {
                            lbID.Text = gvdanhsach.Rows[i].Cells["manhansu"].Value.ToString();
                            lbTen.Text = gvdanhsach.Rows[i].Cells["hoten"].Value.ToString();
                            lbPhong.Text = gvdanhsach.Rows[i].Cells["phong"].Value.ToString();
                            lbBan.Text = gvdanhsach.Rows[i].Cells["ban"].Value.ToString();
                            lbHienTrang.Text = "Khách";
                          
                            var barcodeWriter = new BarcodeWriter
                            {
                                Format = BarcodeFormat.QR_CODE,
                                Options = new EncodingOptions
                                {
                                    Height = 70,
                                    Width = 70,
                                    Margin = 0
                                }
                            };

                            string content = lbID.Text;

                            using (var bitmap = barcodeWriter.Write(content))
                            {
                                using (var stream = new MemoryStream())
                                {
                                    bitmap.Save(stream, ImageFormat.Png);
                                    var image = Image.FromStream(stream);
                                    pictureBox1.Image = image;
                                    //printDocument1.DocumentName = gvdanhsach.Rows[i].Cells["manhansu"].Value.ToString();
                                    //printDocument1.Print();
                                    Bitmap bitmap1 = new Bitmap(this.panel1.Width, this.panel1.Height);
                                    panel1.DrawToBitmap(bitmap1, new Rectangle(0, 0, this.panel1.Width, this.panel1.Height));
                                    //bitmap1.Save(Application.StartupPath + @"\CardNV\" + gvdanhsach.Rows[i].Cells["manhansu"].Value.ToString() + ".png", ImageFormat.Png);

                                    bm.Add(bitmap1);
                                    gvdanhsach.Rows[i].Cells["check"].Value = check.FalseValue;
                                }
                            }
                        }

                    }
                }
               // MessageBox.Show(bm.Count.ToString());
                int kq1 = bm.Count / 10;
                int tong = 10;
                for (int i = 0; i < kq1; i++)
                {
                    if (tong <= bm.Count)
                    {
                        bmNew.Clear();
                        for (int j = tong - 10; j < tong; j++)
                        {
                            bmNew.Add(bm[j]);
                        }
                        printDocument1.DocumentName = "Document-" + i;
                        printDocument1.Print();
                        tong += 10;
                    }

                }
                bmNew.Clear();
                for (int z = tong - 10; z < bm.Count; z++)
                {
                    bmNew.Add(bm[z]);
                }
                printDocument1.DocumentName = "Document-End";
                printDocument1.Print();
            }
            else
            {
                MessageBox.Show("Chưa có dữ liệu!");
            }

        }

        private void printDocument2_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Bitmap bitmap = new Bitmap(this.panel1.Width, this.panel1.Height);
            panel1.DrawToBitmap(bitmap, new Rectangle(0, 0, this.panel1.Width, this.panel1.Height));
            e.Graphics.DrawImage(bitmap, 0, 0, this.panel1.Width, this.panel1.Height);
        }

       
    }
}
