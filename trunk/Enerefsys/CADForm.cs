using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Enerefsys
{
    public partial class CADForm : Form
    {
        public CADForm()
        {
            InitializeComponent();
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "c:\\";

            openFileDialog.Filter = "文本文件|*.*|C#文件|*.cs|所有文件|*.*";

            openFileDialog.RestoreDirectory = true;

            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {

                string fName = openFileDialog.FileName;

                //Image imge = CADUtility.GetDwgImage( fName);
                Image imge = CADUtility.ShowDWG(pictureBox1.Width, pictureBox1.Height, fName);

                pictureBox1.Image = imge;

            }

        }
    }
}
