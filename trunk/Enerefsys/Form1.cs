using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {

        private Freezer freezer{ get; set; }
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            HourLoadForm hlf = new HourLoadForm();
            hlf.Show();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            ElectricPriceTable ept = new ElectricPriceTable();
            ept.Show();
        }

        private void btnFreezer_Click(object sender, EventArgs e)
        {
            freezer = new Freezer();
            freezer.Show();
        }

        private void btnPump_Click(object sender, EventArgs e)
        {
            WaterPump wp = new WaterPump();
            wp.Show();
        }

        private void btnCoolTower_Click(object sender, EventArgs e)
        {
            CoolTower ct = new CoolTower();
            ct.Show();

        }

        private void btn_calculate_Click(object sender, EventArgs e)
        {
            if (null == freezer || null == freezer.meList || 0 == freezer.meList.Count)
            {
                MessageBox.Show("你还没设置冷冻机参数或没有点击冷冻机页面确定按钮");
            }
            else
            {

                for (int i = 0; i < freezer.meList.Count; i++)
                {
                    MessageBox.Show("" + freezer.meList.ElementAt(i).Type.ToString());
                    MessageBox.Show(""+freezer.meList.ElementAt(i).Value.ToString());
                }
                    
            }
        }
    }
}
