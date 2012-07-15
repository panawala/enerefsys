//***********************************************************************
// Assembly         : testlogin
// Author           : jiamao
// Created          : 11-24-2009
//
// Last Modified By : Haofefe
// Last Modified On : 11-24-2009
// Description      : www.jiamaocode.com
//
// Copyright        : (c) ��è�������. All rights reserved.
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
        /// ˮƽ����ʱ�Ƿ�ˢ�±�ͷ�����ݽ϶�ʱ���ܻ���˸����ˢ��ʱ������ʾ����
        /// </summary>
        [Description("ˮƽ����ʱ�Ƿ�ˢ�±�ͷ�����ݽ϶�ʱ���ܻ���˸����ˢ��ʱ������ʾ����")]
        public bool RefreshAtHscroll
        {
            get { return HscrollRefresh; }
            set { HscrollRefresh = value; }
        }
        /// <summary>
        /// ���캯��
        /// </summary>
        public HeaderUnitView()
        {
            InitializeComponent();
            this.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            //�����и߶���ʾģʽ            
        }

        [Description("���û��úϲ���ͷ�������")]
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


        [Description("��Ӻϲ�ʽ��Ԫ����Ƶ�����Ҫ�Ľڵ����")]
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

        [Description("������ӵ��ֶ������������")]
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
        ///���ƺϲ���ͷ
        ///</summary>
        ///<param name="node">�ϲ���ͷ�ڵ�</param>
        ///<param name="e">��ͼ������</param>
        ///<param name="level">������</param>
        ///<remarks></remarks>
        public void PaintUnitHeader(
                        TreeNode node,
                        System.Windows.Forms.DataGridViewCellPaintingEventArgs e,
                        int level)
        {
            //���ڵ�ʱ�˳��ݹ����
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

            //������
            e.Graphics.FillRectangle(backColorBrush, uhRectangle);
            
            //������
            e.Graphics.DrawLine(gridLinePen
                                , uhRectangle.Left
                                , uhRectangle.Bottom
                                , uhRectangle.Right
                                , uhRectangle.Bottom);
            //���Ҷ���
            e.Graphics.DrawLine(gridLinePen
                                , uhRectangle.Right
                                , uhRectangle.Top
                                , uhRectangle.Right
                                , uhRectangle.Bottom);
            ////д�ֶ��ı�

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
            ////�ݹ����()
            if (node.PrevNode == null)
                if (node.Parent != null)
                    PaintUnitHeader(node.Parent, e, level - 1);
        }       

        /// <summary>
        /// ��úϲ������ֶεĿ��
        /// </summary>
        /// <param name="node">�ֶνڵ�</param>
        /// <returns>�ֶο��</returns>
        /// <remarks></remarks>
        private int GetUnitHeaderWidth(TreeNode node)
        {
            //��÷���ײ��ֶεĿ��

            int uhWidth = 0;
            //�����ײ��ֶεĿ��
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
        /// ��õײ��ֶ�����
        /// </summary>
        ///' <param name="node">�ײ��ֶνڵ�</param>
        /// <returns>����</returns>
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
        /// ��õײ��ֶμ���
        /// </summary>
        /// <param name="alList">�ײ��ֶμ���</param>
        /// <param name="node">�ֶνڵ�</param>
        /// <param name="checked">�����������</param>
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
        /// ����
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
        /// �п�ȸı����д
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
        /// ��Ԫ�����(��д)
        /// </summary>
        /// <param name="e"></param>
        /// <remarks></remarks>
        protected override void OnCellPainting(System.Windows.Forms.DataGridViewCellPaintingEventArgs e)
        {
            try
            {
                //�б��ⲻ��д
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

                //���Ʊ�ͷ
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
