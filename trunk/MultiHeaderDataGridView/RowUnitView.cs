//***********************************************************************
// Assembly         : testlogin
// Author           : jiamao
// Created          : 11-24-2009
//
// Last Modified By : Haofefe
// Last Modified On : 11-24-2009
// Description      : www.jiamaocode.com
//
// Copyright        : (c) 家猫编程社区. All rights reserved.
//***********************************************************************

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.Text;
using System.Windows.Forms;

namespace MultiHeaderDataGridView
{
    public partial class RowUnitView : DataGridView
    {
        public RowUnitView()
        {
            InitializeComponent();
            this.MultiSelect = true;
            this.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.AllowUserToAddRows = false;
            this.RowHeadersVisible = false;
            this.ReadOnly = true;
        }
        private int i = 1;
        private int count2 = 1;
        private int flag;
        string selectedValue;
        protected override void OnPaint(PaintEventArgs pe)
        {
            // TODO: 在此处添加自定义绘制代码

            // 调用基类 OnPaint
            base.OnPaint(pe);
        }
        
        protected override void OnCellPainting(DataGridViewCellPaintingEventArgs e)
        {
            try
            {                
                DrawCell(e);
                base.OnCellPainting(e);
            }
            catch
            { }
        }
        /// <summary>
        /// 画单元格
        /// </summary>
        /// <param name="e"></param>
        private void DrawCell(DataGridViewCellPaintingEventArgs e)
        {
            Brush gridBrush = new SolidBrush(this.GridColor);
            SolidBrush backBrush = new SolidBrush(e.CellStyle.BackColor);
            SolidBrush fontBrush = new SolidBrush(e.CellStyle.ForeColor);
            int cellheight;            
            int fontheight;
            int cellwidth;
            int fontwidth;
            int countU = 0;
            int countD = 0;
            int count = 0;
            int totalheight = 0;
            int heightU = 0;
            int heightD = 0;
            // 对第1,2列相同单元格进行合并
            if ((e.ColumnIndex == 0) && e.RowIndex != -1)
            {
                cellheight = e.CellBounds.Height;
                fontheight = (int)e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font).Height;
                cellwidth = e.CellBounds.Width;
                fontwidth = (int)e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font).Width;                
                Pen gridLinePen = new Pen(gridBrush);
                string curValue = this.Rows[e.RowIndex].Cells[0].Value == null ? "" : this.Rows[e.RowIndex].Cells[0].Value.ToString().Trim();
                string curSelected = this.CurrentRow.Cells[0].Value == null ? "" : this.CurrentRow.Cells[0].Value.ToString().Trim();
                if (!string.IsNullOrEmpty(curValue))
                {
                    for (int i = e.RowIndex; i < this.Rows.Count; i++)
                    {
                        if (this.Rows[i].Cells[0].Value.ToString().Equals(curValue))
                        {
                            this.Rows[i].Cells[0].Selected = this.Rows[e.RowIndex].Selected;
                            this.Rows[i].Selected = this.Rows[e.RowIndex].Selected;
                            countD++;
                            heightD += this.Rows[i].Height;
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (this.Rows[i].Cells[0].Value.ToString().Equals(curValue))
                        {
                            this.Rows[i].Cells[0].Selected = this.Rows[e.RowIndex].Selected;
                            this.Rows[i].Selected = this.Rows[e.RowIndex].Selected;
                            heightU += this.Rows[i].Height;
                            countU++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    heightU = heightU - e.CellBounds.Height;
                    totalheight = heightU + heightD;
                    count = countD + countU - 1;
                    if (count < 2) { return; }
                }

                if (this.Rows[e.RowIndex].Selected)
                {
                    backBrush.Color = e.CellStyle.SelectionBackColor;
                    fontBrush.Color = e.CellStyle.SelectionForeColor;
                }

                e.Graphics.FillRectangle(backBrush, e.CellBounds); //new Rectangle(e.CellBounds.X,e.CellBounds.Y-(countU-1) * cellheight,cellwidth,cellheight*countU)); 

                //e.Graphics.DrawString((String)e.Value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (countU - 1) + (cellheight * count - fontheight) / 2);
                string cellvalue = e.Value.ToString();
                if (fontwidth > cellwidth)
                {
                    for (int i = cellvalue.Length-1; i > 0; i--)
                    {
                        cellvalue = cellvalue.Substring(0, i);
                        fontwidth = (int)e.Graphics.MeasureString(cellvalue,e.CellStyle.Font).Width;
                        if (fontwidth <= cellwidth) break;
                    }
                }
                e.Graphics.DrawString(cellvalue, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - heightU + (totalheight - fontheight) / 2);
                
                if (countD == 1)
                {
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                    //count = 0;
                }

                // 画右边线
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right-1, e.CellBounds.Top, e.CellBounds.Right-1, e.CellBounds.Bottom);
                if(e.ColumnIndex==0)e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left , e.CellBounds.Top, e.CellBounds.Left , e.CellBounds.Bottom);
            
                e.Handled = true;
            }
            if ((e.ColumnIndex == 1) && e.RowIndex != -1)
            {
               
                cellheight = e.CellBounds.Height;
                fontheight = (int)e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font).Height;
                cellwidth = e.CellBounds.Width;
                fontwidth = (int)e.Graphics.MeasureString(e.Value.ToString(), e.CellStyle.Font).Width;
                Pen gridLinePen = new Pen(gridBrush);
                string curValue = this.Rows[e.RowIndex].Cells[1].Value == null ? "" : this.Rows[e.RowIndex].Cells[1].Value.ToString().Trim();
                string curSelected = this.CurrentRow.Cells[1].Value == null ? "" : this.CurrentRow.Cells[1].Value.ToString().Trim();
                if (!string.IsNullOrEmpty(curValue))
                {
                    for (int i = e.RowIndex; i < this.Rows.Count; i++)
                    {
                        if (this.Rows[i].Cells[1].Value.ToString().Equals(curValue))
                        {
                            this.Rows[i].Cells[1].Selected = this.Rows[e.RowIndex].Selected;
                            this.Rows[i].Selected = this.Rows[e.RowIndex].Selected;
                            countD++;
                            heightD += this.Rows[i].Height;
                        }
                        else
                        {
                            break;
                        }
                    }
                    for (int i = e.RowIndex; i >= 0; i--)
                    {
                        if (this.Rows[i].Cells[1].Value.ToString().Equals(curValue))
                        {
                            this.Rows[i].Cells[1].Selected = this.Rows[e.RowIndex].Selected;
                            this.Rows[i].Selected = this.Rows[e.RowIndex].Selected;
                            heightU += this.Rows[i].Height;
                            countU++;
                        }
                        else
                        {
                            break;
                        }
                    }
                    heightU = heightU - e.CellBounds.Height;
                    totalheight = heightU + heightD;
                    count = countD + countU - 1;
                    if (count < 2) { return; }
                }

                if (this.Rows[e.RowIndex].Selected)
                {
                    backBrush.Color = e.CellStyle.SelectionBackColor;
                    fontBrush.Color = e.CellStyle.SelectionForeColor;
                }

                e.Graphics.FillRectangle(backBrush, e.CellBounds); //new Rectangle(e.CellBounds.X,e.CellBounds.Y-(countU-1) * cellheight,cellwidth,cellheight*countU)); 

                //e.Graphics.DrawString((String)e.Value, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - cellheight * (countU - 1) + (cellheight * count - fontheight) / 2);
                string cellvalue = e.Value.ToString();
                if (fontwidth > cellwidth)
                {
                    for (int i = cellvalue.Length - 1; i > 0; i--)
                    {
                        cellvalue = cellvalue.Substring(0, i);
                        fontwidth = (int)e.Graphics.MeasureString(cellvalue, e.CellStyle.Font).Width;
                        if (fontwidth <= cellwidth) break;
                    }
                }
                e.Graphics.DrawString(cellvalue, e.CellStyle.Font, fontBrush, e.CellBounds.X + (cellwidth - fontwidth) / 2, e.CellBounds.Y - heightU + (totalheight - fontheight) / 2);

                if (countD == 1)
                {
                    e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Bottom - 1, e.CellBounds.Right - 1, e.CellBounds.Bottom - 1);
                    //count = 0;
                }

                // 画右边线
                e.Graphics.DrawLine(gridLinePen, e.CellBounds.Right - 1, e.CellBounds.Top, e.CellBounds.Right - 1, e.CellBounds.Bottom);
                if (e.ColumnIndex == 0) e.Graphics.DrawLine(gridLinePen, e.CellBounds.Left, e.CellBounds.Top, e.CellBounds.Left, e.CellBounds.Bottom);
                e.Handled = true;
            }
        }
        protected override void OnSelectionChanged(EventArgs e)
        {
            try
            {                
                base.OnSelectionChanged(e);
                if (this.SelectedRows.Count < 1) return;
                string curvalue = this.CurrentRow.Cells[0].Value.ToString();
                foreach(DataGridViewRow dgvr in this.SelectedRows)
                {
                    if (!dgvr.Cells[0].Value.ToString().Equals( curvalue))
                    {
                        dgvr.Selected = false;
                    }
                }
            }
            catch { }
        }
        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            base.OnCellClick(e);
            if(e.RowIndex>-1)
            this.Rows[e.RowIndex].Cells[0].Selected = true;
        }
        protected override void OnRowHeightChanged(DataGridViewRowEventArgs e)
        {
            Graphics g=Graphics.FromHwnd(this.Handle);
            float fornheight = g.MeasureString(e.Row.Cells[0].Value.ToString(), e.Row.InheritedStyle.Font).Height;
            if (e.Row.Height <= fornheight) e.Row.Height = (int)fornheight;
           
            base.OnRowHeightChanged(e);
            this.Refresh();
        }

    }
}
