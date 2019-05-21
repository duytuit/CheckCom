using System;
using System.Windows.Forms;

namespace CheckCom_Version2
{
    public partial class Progress : Form
    {
       
        public Progress()
        {
            InitializeComponent();
            Form test = new Form();
            test.ControlBox = false;
            test.Width = 400;
            test.Height = 65;
            test.StartPosition = FormStartPosition.CenterScreen;
            ProgressBar pg = new ProgressBar();
            pg.Location= new System.Drawing.Point(5, 0);
            pg.Width = 300;
            pg.Height = 43;
            test.Controls.Add(pg);
            Label label1 = new Label();
            label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(300, 0);
            label1.Size = new System.Drawing.Size(120, 40);
            label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            label1.Text = "100%";
            label1.Name = "labelProgress";
            test.Controls.Add(label1);
            test.Show();
            //int total = 1292;
            //float step = total / 100;
            //for (int i = 0; i < total; i++)
            //{
            //    if (i == step)
            //    {
            //        step = step + total / 100;

            //        if (pg.Value < 100)
            //        {
            //            pg.Value += 1;
            //            pg.Update();
            //        }
            //        else
            //        {
            //            test.Close();
            //        }
            //    }
            //}
        }

        private void label1_Click(object sender, EventArgs e)
        {
        }
    }
}