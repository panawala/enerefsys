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
using System.Collections;
using System.Collections.Generic;
using System.Data.Sql;
using System.Text;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace MultiHeaderDataGridView
{
    public partial class HeaderUnitView : DataGridView
    {
        private TreeView[] _columnTreeView;
        private ArrayList _columnList = new ArrayList();
        private int _cellHeight = 17;

        public int CellHeight
        {
            get { return _cellHeight; }
            set { _cellHeight = value; }
        }
        private int _columnDeep = 1;

        private bool HscrollRefresh = false;
        /// <summary>
        /// 水平滚动时是否刷新表头，数据较多时可能会闪烁，不刷新时可能显示错误
        /// </summary>
        [Description("水平滚动时是否刷新表头，数据较多时可能会闪烁，不刷新时可能显示错误")]
        public bool RefreshAtHscroll
        {
            get { return HscrollRefresh; }
            set { HscrollRefresh = value; }
        }
        /// <summary>
        /// 构造函数
        /// </summary>
        public HeaderUnitView()
        {
            InitializeComponent();
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //设置列高度显示模式            
        }

        [Description("设置或获得合并表头树的深度")]
        public int ColumnDeep
        {
            get
            {
                if (this.Columns.Count == 0)
                    _columnDeep = 1;

                this.ColumnHeadersHeight = _cellHeight * _columnDeep;
                return _columnDeep;
            }

            set
            {
                if (value < 1)
                    _columnDeep = 1;
                else
                    _columnDeep = value;
                this.ColumnHeadersHeight = _cellHeight * _columnDeep;
            }
        }


        [Description("添加合并式单元格绘制的所需要的节点对象")]
        public TreeView[] ColumnTreeView
        {
            get { return _columnTreeView; }
            set
            {
                if (_columnTreeView != null)
                {
                    for (int i = 0; i <= _columnTreeView.Length - 1; i++)
                        _columnTreeView[i].Dispose();
                }
                _columnTreeView = value;
            }
        }

        [Description("设置添加的字段树的相关属性")]
        public TreeView ColumnTreeViewNode
        {
            get { return _columnTreeView[0]; }
        }

        public ArrayList NadirColumnList
        {
            get
            {
                if (_columnTreeView == null)
                    return null;

                if (_columnTreeView[0] == null)
                    return null;

                if (_columnTreeView[0].Nodes == null)
                    return null;

                if (_columnTreeView[0].Nodes.Count == 0)
                    return null;

                _columnList.Clear();
                GetNadirColumnNodes(_columnList, _columnTreeView[0].Nodes[0], false);
                return _columnList;
            }
        }


        ///<summary>
        ///绘制合并表头
        ///</summary>
        ///<param name="node">合并表头节点</param>
        ///<param name="e">绘图参数集</param>
        ///<param name="level">结点深度</param>
        ///<remarks></remarks>
        public void PaintUnitHeader(
                        TreeNode node,
                        System.Windows.Forms.DataGridViewCellPaintingEventArgs e,
                        int level)
        {
            //根节点时退出递归调用
            if (level == 0)
                return;

            RectangleF uhRectangle;
            int uhWidth;
            SolidBrush gridBrush = new SolidBrush(this.GridColor);
            SolidBrush backColorBrush = new SolidBrush(e.CellStyle.BackColor);
            Pen gridLinePen = new Pen(gridBrush);
            StringFormat textFormat = new StringFormat();


            textFormat.Alignment = StringAlignment.Center;

            uhWidth = GetUnitHeaderWidth(node);

            if( node.Nodes.Count == 0)
            {
                uhRectangle = new Rectangle(e.CellBounds.Left, 
                            e.CellBounds.Top + node.Level * _cellHeight,
                            uhWidth - 1, 
                            _cellHeight * (_columnDeep - node.Level) - 1);
            }
            else
            {   
                uhRectangle = new Rectangle(
                            e.CellBounds.Left,
                            e.CellBounds.Top + node.Level * _cellHeight,
                            uhWidth - 1,
                            _cellHeight - 1);
            }

            //画矩形
            e.Graphics.FillRectangle(backColorBrush, uhRectangle);
            
            //划底线
            e.Graphics.DrawLine(gridLinePen
                                , uhRectangle.Left
                                , uhRectangle.Bottom
                                , uhRectangle.Right
                                , uhRectangle.Bottom);
            //划右端线
            e.Graphics.DrawLine(gridLinePen
                                , uhRectangle.Right
                                , uhRectangle.Top
                                , uhRectangle.Right
                                , uhRectangle.Bottom);
            ////写字段文本

            e.Graphics.DrawString(node.Text, this.Font
                                    , new SolidBrush(e.CellStyle.ForeColor)
                                    , uhRectangle.Left + uhRectangle.Width / 2 -
                                    e.Graphics.MeasureString(node.Text, this.Font).Width / 2 - 1
                                    , uhRectangle.Top +
                                    uhRectangle.Height / 2 - e.Graphics.MeasureString(node.Text, this.Font).Height / 2);
            //Image img = new Bitmap((int)uhRectangle.Width, (int)uhRectangle.Height);
            //Graphics g = Graphics.FromImage(img);
            //g.FillRectangle(backColorBrush, uhRectangle);
            //g.DrawLine(gridLinePen
            //                    , uhRectangle.Left
            //                    , uhRectangle.Bottom
            //                    , uhRectangle.Right
            //                    , uhRectangle.Bottom);
            //g.DrawLine(gridLinePen
            //                    , uhRectangle.Right
            //                    , uhRectangle.Top
            //                    , uhRectangle.Right
            //                    , uhRectangle.Bottom);
            //g.DrawString(node.Text, e.CellStyle.Font, new SolidBrush(e.CellStyle.ForeColor), uhRectangle.Left + uhRectangle.Width / 2 - g.MeasureString(node.Text, this.Font).Width / 2 - 1, uhRectangle.Top +
            //                        uhRectangle.Height / 2 - g.MeasureString(node.Text, this.Font).Height / 2, textFormat);
            //this.Rows[e.RowIndex].Cells[e.ColumnIndex].ValueType = typeof(Image);
            //this.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = img;
            ////递归调用()
            if (node.PrevNode == null)
                if (node.Parent != null)
                    PaintUnitHeader(node.Parent, e, level - 1);
        }       

        /// <summary>
        /// 获得合并标题字段的宽度
        /// </summary>
        /// <param name="node">字段节点</param>
        /// <returns>字段宽度</returns>
        /// <remarks></remarks>
        private int GetUnitHeaderWidth(TreeNode node)
        {
            //获得非最底层字段的宽度

            int uhWidth = 0;
            //获得最底层字段的宽度
            if (node.Nodes == null)
                return this.Columns[GetColumnListNodeIndex(node)].Width;

            if (node.Nodes.Count == 0)
                return this.Columns[GetColumnListNodeIndex(node)].Width;

            for (int i = 0; i <= node.Nodes.Count - 1; i++)
            {
                uhWidth = uhWidth + GetUnitHeaderWidth(node.Nodes[i]);
            }
            return uhWidth;
        }


        /// <summary>
        /// 获得底层字段索引
        /// </summary>
        ///' <param name="node">底层字段节点</param>
        /// <returns>索引</returns>
        /// <remarks></remarks>
        private int GetColumnListNodeIndex(TreeNode node)
        {
            for (int i = 0; i <= _columnList.Count - 1; i++)
            {
                if (((TreeNode)_columnList[i]).Equals(node))
                    return i;
            }
            return -1;
        }


        /// <summary>
        /// 获得底层字段集合
        /// </summary>
        /// <param name="alList">底层字段集合</param>
        /// <param name="node">字段节点</param>
        /// <param name="checked">向上搜索与否</param>
        /// <remarks></remarks>
        private void GetNadirColumnNodes(
                        ArrayList alList,
                        TreeNode node,
                        Boolean isChecked)
        {
            if (isChecked == false)
            {
                if (node.FirstNode == null)
                {
                    alList.Add(node);
                    if (node.NextNode != null)
                    {
                        GetNadirColumnNodes(alList, node.NextNode, false);
                        return;
                    }
                    if (node.Parent != null)
                    {
                        GetNadirColumnNodes(alList, node.Parent, true);
                        return;
                    }
                }
                else
                {
                    if (node.FirstNode != null)
                    {
                        GetNadirColumnNodes(alList, node.FirstNode, false);
                        return;
                    }
                }
            }
            else
            {
                if (node.FirstNode == null)
                {
                    return;
                }
                else
                {
                    if (node.NextNode != null)
                    {
                        GetNadirColumnNodes(alList, node.NextNode, false);
                        return;
                    }

                    if (node.Parent != null)
                    {
                        GetNadirColumnNodes(alList, node.Parent, true);
                        return;
                    }
                }
            }
        }
        /// <summary>
        /// 滚动
        /// </summary>
        /// <param name="e"></param>
        protected override void OnScroll(ScrollEventArgs e)
        {
            bool scrollDirection = (e.ScrollOrientation == ScrollOrientation.HorizontalScroll);
            base.OnScroll(e);
            if (RefreshAtHscroll && scrollDirection)
                this.Refresh();
        }
        
        /// <summary>
        /// 列宽度改变的重写
        /// </summary>
        /// <param name="e"></param>
        protected override void  OnColumnWidthChanged(DataGridViewColumnEventArgs e)
        {
            Graphics g=Graphics.FromHwnd(this.Handle);
            float uwh=g.MeasureString(e.Column.HeaderText, this.Font).Width;
            if (uwh >= e.Column.Width) { e.Column.Width = Convert.ToInt16(uwh);}
            base.OnColumnWidthChanged(e);
        }
        
        /// <summary>
        /// 单元格绘制(重写)
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
        protected override void OnCellPainting(System.Windows.Forms.DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                //行标题不重写
                if (e.ColumnIndex < 0)
                {
                    base.OnCellPainting(e);
                    return;
                }

                if (_columnDeep == 1)
                {
                    base.OnCellPainting(e);
                    return;
                }

                //绘制表头
                if (e.RowIndex == -1)
                {
                    if (e.ColumnIndex >= NadirColumnList.Count) { e.Handled = true; return; }
                    PaintUnitHeader((TreeNode)NadirColumnList[e.ColumnIndex]
                                    , e
                                    , _columnDeep);
                    e.Handled = true;
                }
            }
            catch
            { }
        }
        
    }
}
