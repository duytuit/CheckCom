using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckCom_Version2
{
    public partial class Hienthi : Form
    {

        public int Tong=0;
        public int Conlai=0;
        public Hienthi()
        {
            InitializeComponent();
        }
       

        private void Hienthi_Load(object sender, EventArgs e)
        {
         
        }
        public void getNumber()
        {
            string textNumberTong = string.Format("{0:000}", Tong);
            string textNumberConlai = string.Format("{0:000}", Conlai);
            Image pic1 = ImageText(textNumberTong,1);
            pictureBox1.Image = Zoom(pic1, new Size(1, 90));
            Image pic2 = ImageText(textNumberConlai,2);
            pictureBox2.Image = Zoom(pic2, new Size(1, 90));
        }
        Image Zoom(Image img,Size size)
        {
            Bitmap bmp = new Bitmap(img, img.Width + (img.Width * size.Width / 100), img.Height + (img.Height * size.Height / 100));
            Graphics g = Graphics.FromImage(bmp);
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            return bmp;
        }
        Image ImageText(string textNumber,int number)
        {
            Bitmap bitmap1 = new Bitmap(1, 1);
            Font font = new Font("Microsoft Sans Serif", 400, FontStyle.Regular, GraphicsUnit.Pixel);
            Graphics grap = Graphics.FromImage(bitmap1);
            int width = (int)grap.MeasureString(textNumber, font).Width;
            int heigth = (int)grap.MeasureString(textNumber, font).Height;
            Bitmap bitmap2 = new Bitmap(bitmap1, new Size(width, heigth));
            grap = Graphics.FromImage(bitmap2);
            if(textNumber=="000")
            {
                grap.Clear(Color.Red);
            }
            else
            {
                if(number==1)
                {
                    grap.Clear(Color.Red);
                }else
                {
                    grap.Clear(Color.Green);
                }
                
            }
            grap.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            grap.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;
            grap.DrawString(textNumber, font, new SolidBrush(Color.White), 0, 0);
            grap.Flush();
            grap.Dispose();
            Image i = (Image)bitmap2;
            return i;
        }
    }
}
