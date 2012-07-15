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



namespace WindowsFormsApplication1
{
    public partial class HourLoadForm : Form
    {
        public HourLoadForm()
        {
            InitializeComponent();
            HourLoad_Load(null,null);
            dataGridView1_dataBing();
        }

        private void HourLoad_Load(object sender, EventArgs e)
        {
            this.dataGridView1.Columns.Add("Id", "Id");
            this.dataGridView1.Columns.Add("BeginTime", "H(n)");

            this.dataGridView1.Columns.Add("EndTime", "H(n)");

            this.dataGridView1.Columns.Add("Load", "TR");

            this.dataGridView1.Columns.Add("Ratio", "%");




            for (int j = 0; j < this.dataGridView1.ColumnCount; j++)
            {

                if (j == this.dataGridView1.ColumnCount - 1)
                    this.dataGridView1.Columns[j].Width = 130;
                else
                    this.dataGridView1.Columns[j].Width = 80;

            }

            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            this.dataGridView1.ColumnHeadersHeight = this.dataGridView1.ColumnHeadersHeight * 2;

            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

            this.dataGridView1.CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView1_CellPainting);

            this.dataGridView1.Paint += new PaintEventHandler(dataGridView1_Paint);

        }
        void dataGridView1_Paint(object sender, PaintEventArgs e)
        {

            string[] monthes = { "开始时间", "结束时间", "负荷", "与尖峰负荷的比例" };

            for (int j = 0; j < 4; )
            {

                Rectangle r1 = this.dataGridView1.GetCellDisplayRectangle(j, -1, true); //get the column header cell

                r1.X += 1;

                r1.Y += 1;

                // r1.Width = r1.Width * 2 - 2;
                if (j == 3)
                    r1.Width = r1.Width;
                else
                    r1.Width = r1.Width;

                r1.Height = r1.Height / 2 - 2;

                e.Graphics.FillRectangle(new SolidBrush(this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor), r1);

                StringFormat format = new StringFormat();

                format.Alignment = StringAlignment.Center;

                format.LineAlignment = StringAlignment.Center;

                e.Graphics.DrawString(monthes[j],

                    this.dataGridView1.ColumnHeadersDefaultCellStyle.Font,

                    new SolidBrush(this.dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor),

                    r1,

                    format);

                j += 1;

            }

        }


        private void dataGridView1_dataBing()
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns["Id"].DataPropertyName = "Id";
            dataGridView1.Columns["BeginTime"].DataPropertyName = "BeginTime";
            dataGridView1.Columns["EndTime"].DataPropertyName = "EndTime";
            dataGridView1.Columns["Load"].DataPropertyName = "Load";
            dataGridView1.Columns["Ratio"].DataPropertyName = "Ratio";
            dataGridView1.Columns["Id"].Visible = false;
            dataGridView1.DataSource = EnerefsysDAL.DayLoadData.GetAllDayloads();
        }

        void dataGridView1_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {

            if (e.RowIndex == -1 && e.ColumnIndex > -1)
            {

                e.PaintBackground(e.CellBounds, false);



                Rectangle r2 = e.CellBounds;

                r2.Y += e.CellBounds.Height / 2;

                r2.Height = e.CellBounds.Height / 2;

                e.PaintContent(r2);

                e.Handled = true;

            }

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            string strId = dataGridView1.Rows[e.RowIndex].Cells["Id"].Value.ToString();
            string load = dataGridView1.Rows[e.RowIndex].Cells["Load"].Value.ToString();
            string ratio = dataGridView1.Rows[e.RowIndex].Cells["Ratio"].Value.ToString();
            EnerefsysDAL.DayLoadData.Update(Convert.ToInt32(strId), Convert.ToDouble(load), Convert.ToDouble(ratio));
        }
        
     
    }
}
