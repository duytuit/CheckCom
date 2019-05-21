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
        public Card_Check()
        {
            InitializeComponent();
            int Gio = DateTime.Now.Hour;
            GetBuaan();
            if ((8 <= Gio) && (Gio < 14))
            {
                cbBuaan.Text = "Trưa";
                foreach (BuaAn ba in buaan)
                {
                    if (ba.ten == cbBuaan.Text)
                    {
                        caanid = ba.id;
                    }
                }
                
                caan = " Trua";
            }
            else if ((14 <= Gio) && (Gio < 20))
            {
                cbBuaan.Text = "Chiều";
                foreach (BuaAn ba in buaan)
                {
                    if (ba.ten == cbBuaan.Text)
                    {
                        caanid = ba.id;
                    }
                }
               

                caan = " Chieu";
            }
            else if ((2 <= Gio) && (Gio < 8))
            {
                cbBuaan.Text = "Sáng";
                foreach (BuaAn ba in buaan)
                {
                    if (ba.ten == cbBuaan.Text)
                    {
                        caanid = ba.id;
                    }
                }
                
                caan = " Sang";
            }
            else
            {
                cbBuaan.Text = "Tối";
                foreach (BuaAn ba in buaan)
                {
                    if (ba.ten == cbBuaan.Text)
                    {
                        caanid = ba.id;
                    }
                }
                
                caan = " Toi";
            }
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
                            lbThoiGian.Text = gvdanhsach.Rows[i].Cells["thoigiandat"].Value.ToString() + " / " + gvdanhsach.Rows[i].Cells["bua"].Value.ToString();
                            var barcodeWriter = new BarcodeWriter
                            {
                                Format = BarcodeFormat.QR_CODE,
                                Options = new EncodingOptions
                                {
                                    Height = 100,
                                    Width = 100,
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
                            lbThoiGian.Text = gvdanhsach.Rows[i].Cells["thoigiandat"].Value.ToString() + " / " + gvdanhsach.Rows[i].Cells["bua"].Value.ToString();
                            var barcodeWriter = new BarcodeWriter
                            {
                                Format = BarcodeFormat.QR_CODE,
                                Options = new EncodingOptions
                                {
                                    Height = 100,
                                    Width = 100,
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
            Bitmap bitmap=new Bitmap(this.panel1.Width,this.panel1.Height);
            panel1.DrawToBitmap(bitmap, new Rectangle(0, 0, this.panel1.Width, this.panel1.Height));
            e.Graphics.DrawImage(bitmap,0,0);
        }
        private async Task<string> GetAllBuaan()
        {
            HttpClient aClient = new HttpClient();
            string astr = await aClient.GetStringAsync("http://192.84.100.207/MealOrdersAPI/api/BuaAns");
            return astr;
        }

        private void GetBuaan()
        {
            buaan.Clear();
            try
            {
                cbBuaan.Items.Clear();
                Task<string> callTask = Task.Run(() => GetAllBuaan());
                callTask.Wait();
                string astr = callTask.Result;
                buaan = JsonConvert.DeserializeObject<List<BuaAn>>(astr);
                if (buaan.Count > 0)
                {
                    foreach (BuaAn ba in buaan)
                    {
                        cbBuaan.Items.Add(ba.ten);
                    }
                }
            }
            catch (AggregateException)
            {
                MessageBox.Show("Lỗi đường truyền");
            }
        }
        private void GetDataClient()
        {
            try
            {
                string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$] order by empid asc", MyConnection);
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
                    string pathfile = Application.StartupPath + @"\CheckCom\" + dateTimePicker1.Value.ToString("MM-dd-yyyy") + caan + ".xls";
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
                                lbThoiGian.Text = drow["thoigiandat"].ToString() + " / " + drow["buaan"].ToString();
                                var barcodeWriter = new BarcodeWriter
                                {
                                    Format = BarcodeFormat.QR_CODE,
                                    Options = new EncodingOptions
                                    {
                                        Height = 100,
                                        Width = 100,
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
                                        printDocument1.DocumentName = drow["manhansu"].ToString();
                                        printDocument1.Print();
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
                                lbThoiGian.Text = drow["thoigiandat"].ToString() + " / " + drow["buaan"].ToString();
                                var barcodeWriter1 = new BarcodeWriter
                                {
                                    Format = BarcodeFormat.QR_CODE,
                                    Options = new EncodingOptions
                                    {
                                        Height = 100,
                                        Width = 100,
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
                                        printDocument1.DocumentName = drow["manhansu"].ToString();
                                        printDocument1.Print();
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
                caan = " Sang";
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
            //string khach = lvServer.SelectedItems[0].SubItems[6].Text;
            string khach = "False";
            if (khach=="False")
            {
                //lbID.Text = lvServer.SelectedItems[0].SubItems[1].Text;
                //lbTen.Text = lvServer.SelectedItems[0].SubItems[2].Text;
                //lbPhong.Text = lvServer.SelectedItems[0].SubItems[3].Text;
                //lbBan.Text = lvServer.SelectedItems[0].SubItems[4].Text;
                //lbHienTrang.Text = "Nhân Viên";
                //lbThoiGian.Text = lvServer.SelectedItems[0].SubItems[11].Text + " / " + lvServer.SelectedItems[0].SubItems[18].Text;
                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = 100,
                        Width = 100,
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
                //lbID.Text = lvServer.SelectedItems[0].SubItems[1].Text;
                //lbTen.Text = lvServer.SelectedItems[0].SubItems[2].Text;
                //lbPhong.Text = lvServer.SelectedItems[0].SubItems[3].Text;
                //lbBan.Text = lvServer.SelectedItems[0].SubItems[4].Text;
                //lbHienTrang.Text = "Khách";
                //lbThoiGian.Text = lvServer.SelectedItems[0].SubItems[11].Text + " / " + lvServer.SelectedItems[0].SubItems[18].Text;
                var barcodeWriter = new BarcodeWriter
                {
                    Format = BarcodeFormat.QR_CODE,
                    Options = new EncodingOptions
                    {
                        Height = 100,
                        Width = 100,
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
    }
}
