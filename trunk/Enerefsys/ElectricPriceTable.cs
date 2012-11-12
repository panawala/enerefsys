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
using DataContext;
using EnerefsysDAL.Model;



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
        public Enerefsys enParent = null;
        public void setParent(Enerefsys parent)
        {
            enParent = parent;
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


        BackgroundWorker workerImport = new BackgroundWorker();

        string filePath = string.Empty;

        private void btnImport_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.ShowDialog();
            if ("" == openFileDialog.FileName)
            {
                return;
            }

            filePath = openFileDialog.FileName;
            DataTable dt = new DataTable();
            dt = CallExcel_UnitModel(filePath);
            RunManager.deleteAll();


            progressBar1.Maximum = dt.Rows.Count;

            workerImport.WorkerReportsProgress = true;

            //正式做事情的地方
            workerImport.DoWork += new DoWorkEventHandler(workerImport_DoWork);
            //任务完称时要做的，比如提示等等
            workerImport.ProgressChanged += new ProgressChangedEventHandler(workerImport_ProgressChanged);

            workerImport.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerImport_RunWorkerCompleted);

            workerImport.RunWorkerAsync();


            //if (RunManager.InsertFromExcel(dt) > 1)
            //{
            //    MessageBox.Show("数据导入成功!");
            //    dataGridView1.DataSource = RunManager.getAllStandardLoads();
            //}
        }

        void workerImport_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("数据导入成功!");
            dataGridView1.DataSource = RunManager.getAllStandardLoads();
        }

        void workerImport_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            //将异步任务进度的百分比赋给进度条
        }

        void workerImport_DoWork(object sender, DoWorkEventArgs e)
        {
            DataTable dt = new DataTable();
            dt = CallExcel_UnitModel(filePath);
            using (var context = new EnerefsysContext())
            {
                try
                {
                    int i=0;
                    foreach (DataRow dataRow in dt.Rows)
                    {
                        var standardLoad = new StandardLoad
                        {
                            Day = Convert.ToInt32(dataRow["Day"].ToString()),
                            Month = Convert.ToInt32(dataRow["Month"].ToString()),
                            Hour = Convert.ToInt32(dataRow["Hour"].ToString()),
                            DryTemperature = Convert.ToDouble(dataRow["DryTemperature"].ToString()),
                            WetTemperature = Convert.ToDouble(dataRow["WetTemperature"].ToString()),
                            Load = Convert.ToDouble(dataRow["Load"].ToString()),
                            EnterTemperature = Convert.ToDouble(dataRow["EnterTemperature"].ToString()),
                            ElectronicPrice = Convert.ToDouble(dataRow["ElectronicPrice"].ToString())
                        };
                        context.StandardLoads.Add(standardLoad);
                        i++;
                        workerImport.ReportProgress(i);

                        label2.Text = i.ToString() + "/" + dt.Rows.Count;

                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    return ;
                }
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
        public double kvalue = 3.8;
        private void btnSub_Click(object sender, EventArgs e)
        {
            kvalue = Convert.ToDouble(textBoxKvalue.Text);
            enParent.Kvalue = kvalue;
        }



       
    }
}
