using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace Enerefsys
{
    public class RedTextRenderer :System.Windows.Forms.ToolStripRenderer
    {
        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = Color.White;
            e.TextFont = new Font("微软雅黑", 8, FontStyle.Bold);
            base.OnRenderItemText(e);
        }
    }
}
