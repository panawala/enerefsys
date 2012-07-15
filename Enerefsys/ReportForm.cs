using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;
using EnerefsysBLL.EntityData;

namespace Enerefsys
{
    public partial class ReportForm : Form
    {
        public ReportForm()
        {
            InitializeComponent();
        }

        private void ReportForm_Load(object sender, EventArgs e)
        {

            this.reportViewer1.RefreshReport();
        }

        private void btnTest_Click(object sender, EventArgs e)
        {

            DataTable dt = new DataTable("Departmentss");
            DataColumn dc1 = new DataColumn("DepartmentId", Type.GetType("System.Int32"));
            DataColumn dc2 = new DataColumn("GroupName", Type.GetType("System.String"));
            DataColumn dc5 = new DataColumn("Name", Type.GetType("System.String"));
            
            dt.Columns.Add(dc1);
            dt.Columns.Add(dc2);
            dt.Columns.Add(dc5);
            //以上代码完成了DataTable的构架，但是里面是没有任何数据的
            for (int i = 0; i < 10; i++)
            {
                DataRow dr = dt.NewRow();
                dr["DepartmentId"] = i;
                dr["GroupName"] = "group";
                dr["Name"] = "name";
                dt.Rows.Add(dr);
            }
            //填充了10条相同的记录进去




            var resultSet = OptimizationResultData.GetAll();
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Enerefsys.Report1.rdlc";
            //ReportParameter rp = new ReportParameter("content", this.textBox1.Text);
            //this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DS_Result", resultSet));
            this.reportViewer1.RefreshReport();
        }
    }
    public class cart
    {
        public string prizename { get; set; }
        public string prizeid { get; set; }
    }
}
