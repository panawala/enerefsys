using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.Web.UI.WebControls;
using EnerefsysBLL.EntityData;
using EnerefsysBLL.Manager;
using System.Data.OleDb;



namespace Enerefsys
{
    public partial class ElectricPriceTable : Form
    {
        public ElectricPriceTable()
        {
            InitializeComponent();
        }


     


      
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            dataGridView1.Update();
        }

        

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string strId = dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            string price = dataGridView1.Rows[e.RowIndex].Cells["ElectronicPrice"].Value.ToString();
            ElectronicData.update(Convert.ToInt32(strId), Convert.ToDouble(price));
        }

        //private void ElectricPriceTable_Load(object sender, EventArgs e)
        //{
        //    dataGridView1.AutoGenerateColumns = false;
        //    //绑定所有的负荷信息
        //    dataGridView1.DataSource = RunManager.getAllStandardLoads();
        //}

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            if ("" == openFileDialog.FileName)
            {
                return;
            }

            string filePath = openFileDialog.FileName;
            DataTable dt = new DataTable();
            dt = CallExcel_UnitModel(filePath);
            RunManager.deleteAll();
            if (RunManager.InsertFromExcel(dt) > 1)
            {
                MessageBox.Show("数据导入成功!");
                dataGridView1.DataSource = RunManager.getAllStandardLoads();
            }
        }
        private DataTable CallExcel_UnitModel(string filepath)
        {
            try
            {
             
                OleDbConnection con = new OleDbConnection("Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + ";Extended Properties='Excel 8.0;HDR=YES;IMEX=1';");
                con.Open();
                string sql = "select * from [StandardLoad$]";//选择第一个数据SHEET
                OleDbDataAdapter adapter = new OleDbDataAdapter(sql, con);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                con.Close();
                con.Dispose();
                return dt;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString());
            }
            return null;
        }

        private void ElectricPriceTable_Load(object sender, EventArgs e)
        {
            dataGridView1.AutoGenerateColumns = false;
            //绑定所有的负荷信息
            dataGridView1.DataSource = RunManager.getAllStandardLoads();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Close();
        }



       
    }
}
