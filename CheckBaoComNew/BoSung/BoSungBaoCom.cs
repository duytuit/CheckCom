using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Reflection;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;
using System.Linq;
using System.IO;

namespace BoSung
{
    public partial class BoSungBaoCom : Form
    {
        private int IDbosung=1;
        private string IDupdate=null;
        private string pathfile = Application.StartupPath + @"\BoSung\BoSung.xls";
        private string selectionindex = null;
        public BoSungBaoCom()
        {
            InitializeComponent();
              GetAllBoSung();
        }

        private void GetAllBoSung()
        {
            List<int> IDtest = new List<int>();
            DataTable table = new DataTable();
            System.Data.OleDb.OleDbConnection MyConnection;
            MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
            MyConnection.Open();
            OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
            oada.Fill(table);
            MyConnection.Close();

            listView1.Items.Clear();
           // IDbosung = int.Parse(table.Rows.Count.ToString()) + 1;
            for (int i = 0; i < table.Rows.Count; i++)
            {
                DataRow drow = table.Rows[i];

                if (drow.RowState != DataRowState.Deleted)
                {
                    IDtest.Add(int.Parse(drow["ID"].ToString()));
                    ListViewItem lvi = new ListViewItem(drow["ID"].ToString());
                    lvi.SubItems.Add(drow["ThoiGian"].ToString());
                    lvi.SubItems.Add(drow["IDnv"].ToString());
                    lvi.SubItems.Add(drow["TenNV"].ToString());
                    lvi.SubItems.Add(drow["GhiChu"].ToString());
                    listView1.Items.Add(lvi);
                }
            }
            if(IDtest.Count>0)
            {
                IDbosung = (from x in IDtest
                            select x
                     ).Max() + 1;
            }
        }

        private void listView1_MouseClick(object sender, MouseEventArgs e)
        {
            IDupdate = listView1.SelectedItems[0].SubItems[0].Text;
            string idnhanvien = listView1.SelectedItems[0].SubItems[2].Text;
            string tennhanvien = listView1.SelectedItems[0].SubItems[3].Text;
            string ghichu = listView1.SelectedItems[0].SubItems[4].Text;
            selectionindex = (listView1.SelectedIndices[0]+2).ToString();
            txtID.Text = idnhanvien;
            txtTenNV.Text = tennhanvien;
            txtGhiChu.Text = ghichu;
            btnBoSung.Text = "Update";
            btnDelete.Enabled = true;
        }

        private void BoSungBaoCom_Load(object sender, EventArgs e)
        {

            listView1.View = View.Details;
            listView1.FullRowSelect = true;
            btnBoSung.Text = "Insert";

        }

        private void btnBoSung_Click(object sender, EventArgs e)
        {
            
            if (!string.IsNullOrEmpty(txtID.Text) && !string.IsNullOrEmpty(txtTenNV.Text) && !string.IsNullOrEmpty(txtGhiChu.Text))
            {
                if (IDupdate==null)
                {
                    System.Data.OleDb.OleDbConnection MyConnection;
                    System.Data.OleDb.OleDbCommand myCommand = new System.Data.OleDb.OleDbCommand();
                    string sql = null;
                    MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                    MyConnection.Open();
                    myCommand.Connection = MyConnection;
                    sql = "Insert into [Sheet1$] (ID,ThoiGian,IDnv,TenNV,GhiChu) values(@ID,@ThoiGian,@IDnv,@TenNV,@GhiChu)";
                    myCommand.Parameters.AddRange(new OleDbParameter[] {
                    new OleDbParameter("@ID",IDbosung),
                    new OleDbParameter("@ThoiGian",DateTime.Now.ToString()),
                    new OleDbParameter("@IDnv",txtID.Text),
                    new OleDbParameter("@TenNV",txtTenNV.Text),
                    new OleDbParameter("@GhiChu",txtGhiChu.Text)
                      });
                    myCommand.CommandText = sql;
                    myCommand.ExecuteNonQuery();
                    MyConnection.Close();
                    GetAllBoSung();
                    Clear();
                    btnBoSung.Text = "Insert";
                }
                else
                {
                    string thoigian = DateTime.Now.ToString();
                    string idnv = txtID.Text;
                    string tennv = txtTenNV.Text;
                    string gchu = txtGhiChu.Text;
                    System.Data.OleDb.OleDbConnection MyConnectionup;
                    System.Data.OleDb.OleDbCommand myCommandup = new System.Data.OleDb.OleDbCommand();
                    string sqlup = null;
                    MyConnectionup = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                    MyConnectionup.Open();
                    myCommandup.Connection = MyConnectionup;
                    sqlup = "update [Sheet1$] set ThoiGian='" + thoigian + "',IDnv='" + idnv + "'  ,TenNV='" + tennv + "',GhiChu='" + gchu + "' where ID='"+IDupdate+"'";
                    myCommandup.CommandText = sqlup;
                    myCommandup.ExecuteNonQuery();
                    MyConnectionup.Close();
                    GetAllBoSung();
                    Clear();
                    btnBoSung.Text = "Insert";
                }
            }
            else
            {
                MessageBox.Show("Nhập đầy đủ thông tin vào các trường.!");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Clear();
        }

        private void Clear()
        {
            txtID.Text = null;
            txtTenNV.Text = null;
            txtGhiChu.Text = null;
            IDupdate = null;
            selectionindex = null;
            btnBoSung.Text = "Insert";
            btnDelete.Enabled = false;
        }
        
        private void DeleteRowExcel(int RowExcel)
        {
            Excel._Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
            dynamic workbooksExcel = docExcel.Workbooks.Open(pathfile);
            var worksheetExcel = (Excel._Worksheet)workbooksExcel.ActiveSheet;
            ((Excel.Range)worksheetExcel.Rows[RowExcel, Missing.Value]).Delete(Excel.XlDeleteShiftDirection.xlShiftUp);
            workbooksExcel.Save();
            workbooksExcel.Close(false);
            docExcel.Application.Quit();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DeleteRowExcel(int.Parse(selectionindex));
            GetAllBoSung();
            Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.InitialDirectory = @"C:\";
            saveFileDialog1.Title = "Save Excel Files";

            saveFileDialog1.DefaultExt = "xls";
            saveFileDialog1.Filter = "Excel Files (*.xls)|*.xls|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                FileInfo filename = new FileInfo(saveFileDialog1.FileName);
                Excel.Application docExcel = new Microsoft.Office.Interop.Excel.Application { Visible = false };
                Excel.Workbook wb = docExcel.Workbooks.Add(Type.Missing);
                Excel.Worksheet ws = (Excel.Worksheet)docExcel.ActiveSheet;

                ws.Cells[1, 1] = "ID";
                ws.Cells[1, 2] = "ThoiGian";
                ws.Cells[1, 3] = "idnv";
                ws.Cells[1, 4] = "TenNV";
                ws.Cells[1, 5] = "GhiChu";
                int j = 2;
                DataTable table = new DataTable();
                System.Data.OleDb.OleDbConnection MyConnection;
                MyConnection = new System.Data.OleDb.OleDbConnection("provider=Microsoft.Jet.OLEDB.4.0;Data Source='" + pathfile + "';Extended Properties=Excel 8.0;");
                MyConnection.Open();
                OleDbDataAdapter oada = new OleDbDataAdapter("select * from [Sheet1$]", MyConnection);
                oada.Fill(table);
                MyConnection.Close();

                for (int i = 0; i < table.Rows.Count; i++)
                {
                    DataRow drow = table.Rows[i];

                    if (drow.RowState != DataRowState.Deleted)
                    {

                        ws.Cells[j, 1] = int.Parse(drow["ID"].ToString());
                        ws.Cells[j, 2] = drow["ThoiGian"].ToString();
                        ws.Cells[j, 3] = int.Parse(drow["idnv"].ToString());
                        ws.Cells[j, 4] = drow["TenNV"].ToString();
                        ws.Cells[j, 5] = drow["GhiChu"].ToString();
                        j++;
                    }
                }


                wb.SaveAs(filename.FullName, Excel.XlFileFormat.xlTemplate);
                wb.Close();
                docExcel.Application.Quit();
                MessageBox.Show("Thành Công!");
            }
            else
            {
                return;
            }
        }
    }
}