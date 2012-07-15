using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnerefsysBLL.EntityData;

namespace Enerefsys
{
    public partial class SystemOptimizeResult : Form
    {
        public SystemOptimizeResult()
        {
            InitializeComponent();
        }

        private void SystemOptimizeResult_Load(object sender, EventArgs e)
        {
            try
            {
                rowUnitView1.AutoGenerateColumns = false;
                rowUnitView1.DataSource = OptimizationResultData.GetAll();
                rowUnitView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                //rowUnitView1.RowsDefaultCellStyle.SelectionBackColor = Color.Red;
                
            }
            catch (Exception ex)
            {
 
            }
       

        }

       
    }
}
