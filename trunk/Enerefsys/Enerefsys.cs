using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnerefsysBLL;
using System.IO;
using EnerefsysBLL.Manager;
using EnerefsysBLL.Utility;
using EnerefsysBLL.Entity;
using EnerefsysBLL.EntityData;
using System.Reflection;

using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;
using System.Drawing.Drawing2D;
using Enerefsys.Properties; 

namespace Enerefsys
{
    public partial class Enerefsys : Form
    {

        public Enerefsys()
        {
            InitializeComponent();
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            //this.skinEngine1.SkinFile = root.FullName + "/Resources/Calmness.ssk";
            //冷冻机;
            init_label_list();
            conceal_Label(label_list);
            reponseCount = 0;
            Freezer_Load(null, null);

            //水泵配置
            init_waterpump_label_list();
            waterPump_conceal_Label(label_list);
            waterPump_reponseCount = 0;
            WaterPump_Load(null, null);
            CoolingTower_Load(null, null);

            List<string> results = PumpManager.GetPumpTypes();
            foreach (var result in results)
            {
                comboBox_PumpType.Items.Add(result);
            }

            initialPath();


            time1.Interval = 5000;
            time1.Enabled = true;
            time1.Tick += new EventHandler(timer1_Tick);
        }

        //添加一个委托
        public delegate void PassDataBetweenFormHandler(object sender, EngineWinFormEventArgs e);
        //添加一个PassDataBetweenFormHandler类型的事件
        public event PassDataBetweenFormHandler PassDataBetweenForm;
        //定义一个Trimer
        private Timer time1 = new Timer();


        private List<Label> label_list;
        private List<SubFreezer> subFreezer_list;
        private List<SubBoarder> subBoarder_list;
        private List<CoolingTower> CoolingTower_list;
        //private List<CoolingTower> CoolingTower_list;
        public List<MachineEntity> meList { get; set; }//获得冷冻值列表（类型和冷量）
        public List<MachineEntity> bhList { get; set; }//获得版换值列表（类型和冷量）
        public List<MachineEntity> ctList { get; set; }//获得冷却塔值列表
        private int labelFlag = 0;//判断是否要显示labe用
        private int reponseCount
        {
            get;
            set;
        }

        private void Freezer_Load(object sender, EventArgs e)
        {

            this.dataGridView1.Columns.Add("No", "编号");

            this.dataGridView1.Columns.Add("Name", "机器名");

            this.dataGridView1.Columns.Add("FebWin", "类型");

            this.dataGridView1.Columns.Add("FebLoss", "冷量");

            this.dataGridView1.Columns.Add("MarWin", "品牌");

            this.dataGridView1.Columns.Add("MarLoss1", "型号");

            for (int j = 0; j < this.dataGridView1.ColumnCount; j++)
            {
                this.dataGridView1.Columns[j].Width = 150;
            }

            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            this.dataGridView1.ColumnHeadersHeight = this.dataGridView1.ColumnHeadersHeight * 1;

            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

            this.dataGridView1.CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView1_CellPainting);

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
        //输入冷冻机的数量响应函数
        private void freezerNum_TextChanged(object sender, EventArgs e)
        {
            int freezerCount = 0;
            reponseCount += 1;
            if (reponseCount == 1 || labelFlag == 1)
                appear_Label(label_list);
            labelFlag = 0;
            clear_Panel();
            try
            {
                if (null != freezerNum.Text.ToString().Trim() && "" != freezerNum.Text.ToString().Trim())
                {
                    freezerCount = Int32.Parse(freezerNum.Text.ToString());
                }
                else if (!checkBoxBoard.Checked)
                {
                    labelFlag += 1;
                    conceal_Label(label_list);
                    create_Freezer_Num(freezerCount);
                    set_Freezer_Panel(subFreezer_list);
                    return;
                }
            }
            catch (Exception e1)
            {
                Console.Write("" + e1.Message);
                MessageBox.Show("请输入正确的数据类型！");
                return;
            }
            create_Freezer_Num(freezerCount);
            set_Freezer_Panel(subFreezer_list);
        }
        //隐藏标签
        private void conceal_Label(List<Label> label_list)
        {
            foreach (Label label in label_list)
            {
                label.Visible = false;
            }
        }
        //显示标签
        private void appear_Label(List<Label> label_list)
        {
            foreach (Label label in label_list)
            {
                label.Visible = true;
            }
        }
        //向列表中添加标签
        //private List<Label> add_Label_List(Label type_Label, Label cooling_Capacity_Label, Label brand_Label, Label model_Label, Label is_Frequency_Conversion_Label, Label performance_Data_Label)
        private List<Label> add_Label_List(Label type_Label, Label cooling_Capacity_Label, Label brand_Label, Label model_Label, Label amount_Label)
        {
            List<Label> temp_label_list = new List<Label>();
            temp_label_list.Add(type_Label);
            temp_label_list.Add(cooling_Capacity_Label);
            temp_label_list.Add(brand_Label);
            temp_label_list.Add(model_Label);
            temp_label_list.Add(amount_Label);
            //temp_label_list.Add(performance_Data_Label);
            return temp_label_list;
        }
        //初始化标签列表
        private void init_label_list()
        {
            label_list = add_Label_List(type_Label, cooling_Capacity_Label, brand_Label, model_Label, amount_Label);

        }
        //产生冷冻机数量
        private void create_Freezer_Num(int freezerCount)
        {
            subFreezer_list = new List<SubFreezer>();
            for (int i = 1; i <= freezerCount; i++)
            {
                subFreezer_list.Add(new SubFreezer(i));
            }
        }
        //动态显示冷冻机
        private void set_Freezer_Panel(List<SubFreezer> subFreezer_list)
        {
            int i = 1;
            foreach (SubFreezer sub_freezer in subFreezer_list)
            {
                SubFreezer temp_SubFreezer = (SubFreezer)sub_freezer;
                freezer_Panel.Controls.Add(temp_SubFreezer.freazer);
                temp_SubFreezer.type_box.Name = "type_box" + i;
                freezer_Panel.Controls.Add(temp_SubFreezer.type_box);

                temp_SubFreezer.cooling_comboBox.Name = "cooling_comboBox" + i;
                freezer_Panel.Controls.Add(temp_SubFreezer.cooling_comboBox);

                temp_SubFreezer.brand_comboBox.Name = "brand_comboBox" + i;
                freezer_Panel.Controls.Add(temp_SubFreezer.brand_comboBox);

                temp_SubFreezer.model_box.Name = "model_box" + i;
                freezer_Panel.Controls.Add(temp_SubFreezer.model_box);

                temp_SubFreezer.amount_textBox.Name = "amount_textbox" + i;
                freezer_Panel.Controls.Add(temp_SubFreezer.amount_textBox);
                //freezer_Panel.Controls.Add(temp_SubFreezer.performance_data_box);
                i++;
            }
        }
        //清除panel中冷冻机组件
        private void clear_Panel()
        {
            freezer_Panel.Controls.Clear();
        }

        //返回冷冻机类型和冷量
        private List<MachineEntity> getFreezerTypeAndCooling(List<SubFreezer> subFreezer_list)
        {
            int tempFreezerNum;
            List<MachineEntity> machineList = new List<MachineEntity>();
            if (null != freezerNum.Text.ToString().Trim() && "" != freezerNum.Text.ToString().Trim() && 0 < subFreezer_list.Count)
            {
                tempFreezerNum = Int32.Parse(freezerNum.Text.ToString().Trim());
                for (int i = 1; i <= tempFreezerNum; i++)
                {
                    SubFreezer sub_Freezer = (SubFreezer)subFreezer_list.ElementAt(i - 1);
                    if (null != sub_Freezer.cooling_comboBox.Text.ToString().Trim() && "" != sub_Freezer.cooling_comboBox.Text.ToString().Trim())
                    {
                        try
                        {
                            int icount = Convert.ToInt32(sub_Freezer.amount_textBox.Text.ToString().Trim());
                            for (int ix = 0; ix < icount; ix++)
                            {
                                MachineEntity me = new MachineEntity("冷冻机" + i, sub_Freezer.type_box.Text.ToString(), Convert.ToDouble(sub_Freezer.cooling_comboBox.Text.ToString().Trim()));
                                machineList.Add(me);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return null;
                        }
                    }
                    else
                    {
                        MessageBox.Show("请输入冷量");
                        return null;
                    }
                }
                return machineList;
            }
            else
            {
                MessageBox.Show("冷冻机台数不能为空!");
                return null;
            }
        }


        /// <summary>
        /// //板换///////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBoxBoard_CheckedChanged(object sender, EventArgs e)
        {
            int boarderCount = 0;
            reponseCount += 1;
            if (reponseCount == 1 || labelFlag == 1)
                appear_Label(label_list);
            labelFlag = 0;
            clear_Panel2();
            IsBoard = false;
            try
            {
                if (checkBoxBoard.Checked)
                {
                    boarderCount = 1;
                    IsBoard = true;
                }
                else if (null == freezerNum.Text.ToString().Trim() || "" == freezerNum.Text.ToString().Trim())
                {
                    labelFlag += 1;
                    conceal_Label(label_list);
                    create_Boarder_Num(boarderCount);
                    set_Boarder_Panel(subBoarder_list);
                    return;
                }
            }
            catch (Exception ex)
            {
                Console.Write("" + ex.Message);
                MessageBox.Show("请输入正确的数据类型！");
                return;
            }
            create_Boarder_Num(boarderCount);
            set_Boarder_Panel(subBoarder_list);
        }


        private void clear_Panel2()
        {
            boarder_Panel.Controls.Clear();
        }



        //产生板换数量
        private void create_Boarder_Num(int boarderCount)
        {
            subBoarder_list = new List<SubBoarder>();
            for (int i = 1; i <= boarderCount; i++)
            {
                subBoarder_list.Add(new SubBoarder(i));
            }
        }
        //动态显示板换
        private void set_Boarder_Panel(List<SubBoarder> subBoarder_list)
        {
            foreach (SubBoarder sub_boarder in subBoarder_list)
            {
                SubBoarder temp_SubBoarder = (SubBoarder)sub_boarder;
                boarder_Panel.Controls.Add(temp_SubBoarder.boarder);
                boarder_Panel.Controls.Add(temp_SubBoarder.addition);
                temp_SubBoarder.type_box.Name = "board_type_box";
                boarder_Panel.Controls.Add(temp_SubBoarder.type_box);
                temp_SubBoarder.cooling_comboBox.Name = "board_comboBox";
                boarder_Panel.Controls.Add(temp_SubBoarder.cooling_comboBox);
                temp_SubBoarder.brand_comboBox.Name = "board_brand_comboBox";
                boarder_Panel.Controls.Add(temp_SubBoarder.brand_comboBox);
                temp_SubBoarder.model_box.Name = "board_model_box";
                boarder_Panel.Controls.Add(temp_SubBoarder.model_box);
                temp_SubBoarder.amount_textBox.Name = "board_amount_box";
                boarder_Panel.Controls.Add(temp_SubBoarder.amount_textBox);
                //freezer_Panel.Controls.Add(temp_SubFreezer.performance_data_box);
            }
        }

        private List<MachineEntity> getBoarderTypeAndCooling(List<SubBoarder> subBoarder_list)
        {
            int tempBoarderNum;
            List<MachineEntity> machineList = new List<MachineEntity>();
            if (checkBoxBoard.Checked && 0 < subBoarder_list.Count)
            {
                tempBoarderNum = 1;
                for (int i = 1; i <= tempBoarderNum; i++)
                {
                    SubBoarder sub_Boarder = (SubBoarder)subBoarder_list.ElementAt(i - 1);
                    if (null != sub_Boarder.cooling_comboBox.Text.ToString().Trim() && "" != sub_Boarder.cooling_comboBox.Text.ToString().Trim())
                    {
                        try
                        {
                            int icount = Convert.ToInt32(sub_Boarder.amount_textBox.Text.ToString().Trim());
                            for (int ix = 0; ix < icount; ix++)
                            {
                                MachineEntity me = new MachineEntity("板换", sub_Boarder.addition.Text.ToString() + sub_Boarder.type_box.Text.ToString(), Convert.ToDouble(sub_Boarder.cooling_comboBox.Text.ToString().Trim()));
                                machineList.Add(me);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                            return null;
                        }
                    }
                    else
                    {
                        MessageBox.Show("请输入冷量");
                        return null;
                    }
                }
                return machineList;
            }
            else
            {
                return null;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////
        private void btnLoadData_Click(object sender, EventArgs e)
        {


        }
        public double boardValue = 0d;
        public int downTemperature = 0;
        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (null != subFreezer_list && 0 < subFreezer_list.Count)
            {
                meList = getFreezerTypeAndCooling(subFreezer_list);
                bhList = getBoarderTypeAndCooling(subBoarder_list);
                if (bhList != null)
                {

                    BoardCount = bhList.Count;
                    boardValue = bhList.First().Value;
                    string tempStr = bhList.First().Type;
                    tempStr = tempStr.Split(':')[1];
                    downTemperature = Convert.ToInt32(tempStr.Split('°')[0]);
                }
                else
                {
                    downTemperature = 0;
                }



                //EngineWinFormEventArgs ewfe = new EngineWinFormEventArgs(meList);
                //PassDataBetweenForm(this, ewfe);
                //this.Close();
                //this.Visible = false;
                if (meList == null)
                    return;
                dataGridView1.Rows.Clear();
                try
                {
                    for (int ix = 0; ix < subFreezer_list.Count; ix++)
                    {
                        for (int iy = 0; iy < Convert.ToInt32(subFreezer_list[ix].amount_textBox.Text); iy++)
                        {
                            int index = this.dataGridView1.Rows.Add();
                            this.dataGridView1.Rows[index].Cells[0].Value = index + 1;
                            this.dataGridView1.Rows[index].Cells[1].Value = subFreezer_list[ix].freazer.Text;
                            this.dataGridView1.Rows[index].Cells[2].Value = subFreezer_list[ix].type_box.Text;
                            this.dataGridView1.Rows[index].Cells[3].Value = subFreezer_list[ix].cooling_comboBox.Text;
                            this.dataGridView1.Rows[index].Cells[4].Value = subFreezer_list[ix].brand_comboBox.Text;
                            this.dataGridView1.Rows[index].Cells[5].Value = subFreezer_list[ix].model_box.Text;
                        }
                    }
                    if (null != subBoarder_list && 0 < subBoarder_list.Count)
                    {
                        for (int ix = 0; ix < subBoarder_list.Count; ix++)
                        {
                            for (int iy = 0; iy < Convert.ToInt32(subBoarder_list[ix].amount_textBox.Text); iy++)
                            {
                                int index = this.dataGridView1.Rows.Add();
                                this.dataGridView1.Rows[index].Cells[0].Value = index + 1;
                                this.dataGridView1.Rows[index].Cells[1].Value = subBoarder_list[ix].boarder.Text;
                                this.dataGridView1.Rows[index].Cells[2].Value = subBoarder_list[ix].addition.Text + subBoarder_list[ix].type_box.Text;
                                this.dataGridView1.Rows[index].Cells[3].Value = subBoarder_list[ix].cooling_comboBox.Text;
                                this.dataGridView1.Rows[index].Cells[4].Value = subBoarder_list[ix].brand_comboBox.Text;
                                this.dataGridView1.Rows[index].Cells[5].Value = subBoarder_list[ix].model_box.Text;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    dataGridView1.Rows.Clear();
                }
            }
            else
                MessageBox.Show("请输入数据");
        }


        /******************************************************************************************************************/
        //水泵配置WaterPump.cs
        private List<Label> waterPump_label_list;
        private List<WaterFreezer> waterPump_subFreezer_list;
        private List<WaterFreezer> waterPump_subCooler_list;
        private int waterPump_labelFlag = 0;//判断是否要显示labe用
        private int waterPump_reponseCount
        {
            get;
            set;
        }

        private void WaterPump_Load(object sender, EventArgs e)
        {
            this.dataGridView2.Columns.Add("No", "编号");

            this.dataGridView2.Columns.Add("Name", "机器名");

            this.dataGridView2.Columns.Add("FebWin", "品牌");

            this.dataGridView2.Columns.Add("FebLoss", "流量");

            this.dataGridView2.Columns.Add("MarWin", "扬程");

            this.dataGridView2.Columns.Add("MarLoss1", "功率");

            this.dataGridView2.Columns.Add("mode", "型号");

            for (int j = 0; j < this.dataGridView2.ColumnCount; j++)
            {
                this.dataGridView2.Columns[j].Width = 110;
            }

            this.dataGridView2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            this.dataGridView2.ColumnHeadersHeight = this.dataGridView2.ColumnHeadersHeight * 1;

            this.dataGridView2.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

            this.dataGridView2.CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView2_CellPainting);
        }
        void dataGridView2_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
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

        //隐藏标签
        private void waterPump_conceal_Label(List<Label> label_list)
        {
            foreach (Label label in label_list)
            {
                label.Visible = false;
            }
        }
        //显示标签
        private void waterPump_appear_Label(List<Label> label_list)
        {
            foreach (Label label in label_list)
            {
                label.Visible = true;
            }
        }
        //向列表中添加标签
        //private List<Label> add_Label_List(Label brand_Label,Label flow_label, Label lift_Label, Label power_Label, Label model_Label, Label is_Frequency_Conversion_Label, Label performance_Data_Label)
        private List<Label> add_Label_List1(Label brand_Label, Label flow_label, Label lift_Label, Label power_Label, Label model_Label, Label is_Frequency_Conversion_Label)
        {
            List<Label> temp_label_list = new List<Label>();
            temp_label_list.Add(brand_Label);
            temp_label_list.Add(flow_label);
            temp_label_list.Add(lift_Label);
            temp_label_list.Add(power_Label);
            temp_label_list.Add(model_Label);
            temp_label_list.Add(is_Frequency_Conversion_Label);
            //temp_label_list.Add(performance_Data_Label);
            return temp_label_list;
        }
        //初始化标签列表
        private void init_waterpump_label_list()
        {
            waterPump_label_list = add_Label_List1(brand_Label, flow_Label, lift_Label, power_Label, model_Label, amount_Label);

        }
        //产生冷冻或冷却水泵数量
        private void waterPump_create_Freezer_Num(int freezerCount)
        {
            waterPump_subFreezer_list = new List<WaterFreezer>();
            for (int i = 1; i <= freezerCount; i++)
            {
                waterPump_subFreezer_list.Add(new WaterFreezer(i));
            }
        }
        private void waterPump_create_Cooler_Num(int coolerCount)
        {
            waterPump_subCooler_list = new List<WaterFreezer>();
            for (int i = 1; i <= coolerCount; i++)
            {
                waterPump_subCooler_list.Add(new WaterFreezer(i));
            }
        }
        //动态显示冷冻机
        private void set_Freezer_Panel(List<WaterFreezer> subFreezer_list, Panel tempPanel)
        {
            int j = 0;
            foreach (WaterFreezer sub_freezer in subFreezer_list)
            {
                j++;
                WaterFreezer waterPump_SubFreezer = (WaterFreezer)sub_freezer;
                if (tempPanel.Name.Equals("freezingPanel"))
                    waterPump_SubFreezer.freazerAndcooler.Text = "冷冻水泵" + j;
                else
                {
                    waterPump_SubFreezer.freazerAndcooler.Text = "冷却水泵" + j;
                }
                tempPanel.Controls.Add(waterPump_SubFreezer.freazerAndcooler);

                waterPump_SubFreezer.brand_comboBox.Name = "brand_comboBox" + j;
                tempPanel.Controls.Add(waterPump_SubFreezer.brand_comboBox);

                waterPump_SubFreezer.flow_textBox.Name = "flow_comboBox" + j;
                tempPanel.Controls.Add(waterPump_SubFreezer.flow_textBox);

                waterPump_SubFreezer.lift_textBox.Name = "lift_comboBox" + j;
                tempPanel.Controls.Add(waterPump_SubFreezer.lift_textBox);

                waterPump_SubFreezer.power_textBox.Name = "power_comboBox" + j;
                tempPanel.Controls.Add(waterPump_SubFreezer.power_textBox);

                waterPump_SubFreezer.model_textBox.Name = "model_comboBox" + j;
                tempPanel.Controls.Add(waterPump_SubFreezer.model_textBox);

                waterPump_SubFreezer.amount_textBox.Name = "type_comboBox" + j;
                tempPanel.Controls.Add(waterPump_SubFreezer.amount_textBox);

                //if(j<4&&tempPanel.Name.Equals("freezingPanel"))
                //tempPanel.Controls.Add(waterPump_SubFreezer.performance_data_button);

            }
        }
        //清楚panel中冷冻水泵和冷却水泵组件
        private void waterPump_clear_Panel(Panel tempPanel)
        {
            tempPanel.Controls.Clear();
        }


        //动态显示冷冻机
        private void set_WaterPumpFreezer_Panel(List<WaterFreezer> subFreezer_list, Panel tempPanel)
        {
            int j = 0;
            foreach (WaterFreezer sub_freezer in subFreezer_list)
            {
                j++;
                WaterFreezer waterPump_SubFreezer = (WaterFreezer)sub_freezer;
                if (tempPanel.Name.Equals("freezingPanel"))
                    waterPump_SubFreezer.freazerAndcooler.Text = "冷冻水泵" + j;
                else
                {
                    waterPump_SubFreezer.freazerAndcooler.Text = "冷却水泵" + j;
                }
                tempPanel.Controls.Add(waterPump_SubFreezer.freazerAndcooler);

                waterPump_SubFreezer.brand_comboBox.Name = "brand_comboBox" + j;
                tempPanel.Controls.Add(waterPump_SubFreezer.brand_comboBox);

                waterPump_SubFreezer.flow_textBox.Name = "flow_comboBox" + j;
                tempPanel.Controls.Add(waterPump_SubFreezer.flow_textBox);

                waterPump_SubFreezer.lift_textBox.Name = "lift_comboBox" + j;
                tempPanel.Controls.Add(waterPump_SubFreezer.lift_textBox);

                waterPump_SubFreezer.power_textBox.Name = "power_comboBox" + j;
                tempPanel.Controls.Add(waterPump_SubFreezer.power_textBox);

                waterPump_SubFreezer.model_textBox.Name = "model_comboBox" + j;
                tempPanel.Controls.Add(waterPump_SubFreezer.model_textBox);

                waterPump_SubFreezer.amount_textBox.Name = "type_comboBox" + j;
                tempPanel.Controls.Add(waterPump_SubFreezer.amount_textBox);

                //if (j < 4 && tempPanel.Name.Equals("freezingPanel"))
                //    tempPanel.Controls.Add(temp_SubFreezer.performance_data_button);

            }
        }


        private void freezingNum_TextChanged(object sender, EventArgs e)
        {
            int freezerCount = 0;
            waterPump_reponseCount += 1;
            if (waterPump_reponseCount == 1 || waterPump_reponseCount == 1)
                waterPump_appear_Label(label_list);
            waterPump_labelFlag = 0;
            waterPump_clear_Panel(freezingPanel);
            try
            {
                if (null != freezingNum.Text.ToString().Trim() && "" != freezingNum.Text.ToString().Trim())
                {
                    freezerCount = Int32.Parse(freezingNum.Text.ToString());
                }
                else if (null == coolingNum.Text.ToString().Trim() && "" == coolingNum.Text.ToString().Trim())
                {
                    waterPump_labelFlag += 1;
                    waterPump_conceal_Label(label_list);
                    waterPump_create_Freezer_Num(freezerCount);
                    set_WaterPumpFreezer_Panel(waterPump_subFreezer_list, freezingPanel);
                    return;
                }
            }
            catch (Exception e1)
            {
                Console.Write("" + e1.Message);
                MessageBox.Show("请输入正确的数据类型！");
                return;

            }
            waterPump_create_Freezer_Num(freezerCount);
            set_WaterPumpFreezer_Panel(waterPump_subFreezer_list, freezingPanel);
        }

        private void coolingNum_TextChanged(object sender, EventArgs e)
        {
            int coolerCount = 0;
            waterPump_reponseCount += 1;
            if (waterPump_reponseCount == 1 || waterPump_labelFlag == 1)
                waterPump_appear_Label(label_list);
            waterPump_labelFlag = 0;
            waterPump_clear_Panel(coolingPanel);
            try
            {
                if (null != coolingNum.Text.ToString().Trim() && "" != coolingNum.Text.ToString().Trim())
                {
                    coolerCount = Int32.Parse(coolingNum.Text.ToString());
                }
                else if (null == freezerNum.Text.ToString().Trim() && "" == freezerNum.Text.ToString().Trim())
                {
                    waterPump_labelFlag += 1;
                    waterPump_conceal_Label(label_list);
                    waterPump_create_Cooler_Num(coolerCount);
                    set_WaterPumpFreezer_Panel(waterPump_subCooler_list, coolingPanel);
                    return;
                }
            }
            catch (Exception e1)
            {
                Console.Write("" + e1.Message);
                MessageBox.Show("请输入正确的数据类型！");
                return;

            }
            waterPump_create_Cooler_Num(coolerCount);
            set_WaterPumpFreezer_Panel(waterPump_subCooler_list, coolingPanel);
        }

        private void btnSubmit_Click_1(object sender, EventArgs e)
        {
            if ((null != waterPump_subFreezer_list && 0 < waterPump_subFreezer_list.Count) || (null != waterPump_subCooler_list && 0 < waterPump_subCooler_list.Count))
            {
                //EngineWinFormEventArgs ewfe = new EngineWinFormEventArgs(meList);
                //PassDataBetweenForm(this, ewfe);
                //this.Close();
                //this.Visible = false;
                dataGridView2.Rows.Clear();
                try
                {
                    if (null != waterPump_subFreezer_list && 0 < waterPump_subFreezer_list.Count)
                    {
                        for (int ix = 0; ix < waterPump_subFreezer_list.Count; ix++)
                        {
                            for (int iy = 0; iy < Convert.ToInt32(waterPump_subFreezer_list[ix].amount_textBox.Text); iy++)
                            {
                                int index = this.dataGridView2.Rows.Add();
                                this.dataGridView2.Rows[index].Cells[0].Value = index + 1;
                                this.dataGridView2.Rows[index].Cells[1].Value = waterPump_subFreezer_list[ix].freazerAndcooler.Text;
                                this.dataGridView2.Rows[index].Cells[2].Value = waterPump_subFreezer_list[ix].brand_comboBox.Text;
                                this.dataGridView2.Rows[index].Cells[3].Value = waterPump_subFreezer_list[ix].flow_textBox.Text;
                                this.dataGridView2.Rows[index].Cells[4].Value = waterPump_subFreezer_list[ix].lift_textBox.Text;
                                this.dataGridView2.Rows[index].Cells[5].Value = waterPump_subFreezer_list[ix].power_textBox.Text;
                                this.dataGridView2.Rows[index].Cells[6].Value = waterPump_subFreezer_list[ix].model_textBox.Text;
                                //Convert.ToInt32(waterPump_subFreezer_list[ix].flow_textBox.Text);
                                //Convert.ToInt32(waterPump_subFreezer_list[ix].lift_textBox.Text);
                                //Convert.ToInt32(waterPump_subFreezer_list[ix].power_textBox.Text);
                            }

                        }
                    }
                    if (null != waterPump_subCooler_list && 0 < waterPump_subCooler_list.Count)
                    {
                        for (int ix = 0; ix < waterPump_subCooler_list.Count; ix++)
                        {
                            for (int iy = 0; iy < Convert.ToInt32(waterPump_subCooler_list[ix].amount_textBox.Text); iy++)
                            {
                                int index = this.dataGridView2.Rows.Add();
                                this.dataGridView2.Rows[index].Cells[0].Value = index + 1;
                                this.dataGridView2.Rows[index].Cells[1].Value = waterPump_subCooler_list[ix].freazerAndcooler.Text;
                                this.dataGridView2.Rows[index].Cells[2].Value = waterPump_subCooler_list[ix].brand_comboBox.Text;
                                this.dataGridView2.Rows[index].Cells[3].Value = waterPump_subCooler_list[ix].flow_textBox.Text;
                                this.dataGridView2.Rows[index].Cells[4].Value = waterPump_subCooler_list[ix].lift_textBox.Text;
                                this.dataGridView2.Rows[index].Cells[5].Value = waterPump_subCooler_list[ix].power_textBox.Text;
                                this.dataGridView2.Rows[index].Cells[6].Value = waterPump_subCooler_list[ix].model_textBox.Text;
                                //Convert.ToInt32(waterPump_subCooler_list[ix].flow_textBox.Text);
                                //Convert.ToInt32(waterPump_subCooler_list[ix].lift_textBox.Text);
                                //Convert.ToInt32(waterPump_subCooler_list[ix].power_textBox.Text);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    dataGridView2.Rows.Clear();
                }
            }
            else
                MessageBox.Show("请输入数据");
        }


        //添加一个委托
        //  public delegate void PassDataBetweenFormHandler(object sender, PumpWinFormEventArgs e);
        //添加一个PassDataBetweenFormHandler类型的事件
        //  public event PassDataBetweenFormHandler PassDataBetweenForm;
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //传递给父窗口选择类型
            //  PumpWinFormEventArgs pwfa = new PumpWinFormEventArgs(comboBox_PumpType.Text);
            //  PassDataBetweenForm(this, pwfa);
            this.Dispose();
        }
        /***********************************************************************************************************/
        //Form1.cs界面
        private Freezer freezer { get; set; }
        private void button1_Click(object sender, EventArgs e)
        {
            HourLoadForm hlf = new HourLoadForm();
            hlf.Show();

        }
        public double Kvalue = 3.8d;
        private void button2_Click(object sender, EventArgs e)
        {
            ElectricPriceTable ept = new ElectricPriceTable();
            ept.setParent(this);
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
                    MessageBox.Show("" + freezer.meList.ElementAt(i).Value.ToString());
                }

            }
        }
        /**********************************************************************************************************/
        public List<MachineEntity> caculate_meList = new List<MachineEntity>();//获得冷冻值列表（类型和冷量）

        //事件
        void freezer_PassDataBetweenForm(object sender, EngineWinFormEventArgs e)
        {
            caculate_meList = e.meList;
        }

        void wp_PassDataBetweenForm(object sender, PumpWinFormEventArgs e)
        {
            pumpType = e.PumpType;
        }
        public string pumpType { get; set; }

        public double TemperRange { get; set; }
        //冷却类型
        public string CoolingType = "一对一";
        //冷冻类型
        public string FreezeType = "一对一";
        //分别保存当前最佳组合，当前最优结果，和当前每台主机的负荷率
        private List<MachineEntity> meMin = new List<MachineEntity>();
        private double minResult = double.MaxValue;
        private double minSolute = double.MaxValue;
        private double percentValue = 0;
        //private List<MachineEntity> meMin = new List<MachineEntity>();
        //private double minResult = double.MaxValue;
        //private double minSolute = double.MaxValue;
        //private double percentValue = 0;
        private List<MachineEntity> machineList = new List<MachineEntity>();
        public bool IsSwap { get; set; }
        public bool IsNormal { get; set; }



        private void btnShow_Click(object sender, EventArgs e)
        {
            meMin.Clear();
            minResult = double.MaxValue;
            minSolute = double.MaxValue;
            percentValue = 0;
            IsNormal = false;
            IsSwap = false;
            FreezeType = "一对一";
            CoolingType = "一对一";
            //温差设置为5度
            TemperRange = 5;

            double temperature = Convert.ToDouble(textBox_Temperature.Text);
            if (temperature < downTemperature)
                IsBoard = true;
            else
                IsBoard = false;

            if (!string.IsNullOrEmpty(textBox_TemperRange.Text))
                TemperRange = Convert.ToInt32(textBox_TemperRange.Text);


            string type = strCoolingTowerStyle;
            if (string.IsNullOrEmpty(type))
            {
                MessageBox.Show("请配置冷却塔！");
                return;
            }
            if (type.Equals("常规"))
            {
                coolingPower = iCoolingTowerKW;
            }
            else if (type.Equals("高低速"))
            {
                coolingPower = iCoolingTowerKW2;
                if (iCoolingTowerT1 > temperature)
                    coolingPower = 0;
                if (iCoolingTowerT2 < temperature)
                    coolingPower = iCoolingTowerKW3;
            }
            else if (type.Equals("变频"))
            {
                coolingPower = iCoolingTowerKW2;
                if (iCoolingTowerT1 > temperature)
                    coolingPower = 0;
                if (iCoolingTowerT2 < temperature)
                    coolingPower = iCoolingTowerKW3;
            }






            //coolingPower = Convert.ToInt32(textBox_CoolingPower.Text.ToString());
            GetOptimizationResult(meList, Convert.ToDouble(textBox_Load.Text), Convert.ToDouble(textBox_Temperature.Text));


            //如果是板换，则散热塔的功率等于板换数量
            if (IsBoard)
                coolingPower = coolingPower * BoardCount;
            else
            {
                coolingPower = coolingPower * meMin.Count;
            }

            minResult = enginePower + freezePumpPower + lengquePower + coolingPower;


            string picName = string.Empty;
            //如果是板换
            if (IsBoard)
            {
                //使用主机的数量
                int enginetotalCount = meList.Count;
                if (enginetotalCount < 6)
                    picName += enginetotalCount.ToString();
                else
                    picName += "N";

                if (BoardCount > 2)
                    picName += "2.jpg";
                else
                    picName += BoardCount + ".jpg";
            }
            //如果不是板换
            else
            {
                //使用主机的数量
                int enginetotalCount = meMin.Count;
                if (enginetotalCount < 6)
                    picName += enginetotalCount.ToString();
                else
                    picName += "N";

                picName += "0.jpg";
            }

            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            //pictureBox_Result.ImageLocation = root.FullName + "/Resources/result.jpg";
            pictureBox_Result.ImageLocation = root.FullName + "/Resources/" + picName;



            showMessage();



            //for (int i = 0; i < 24; i++)
            //{
            //    double time = i + 0.5;
            //    string timeStr = i + ":00-" + (i + 1) + ":00";
            //    double load = EnerefsysDAL.DayLoadData.GetItemByTime("2012-05-09", time.ToString());
            //    GetOptimizationResult(meList, load, Convert.ToDouble(textBox_Temperature.Text));
            //    foreach (var memin in meMin)
            //    {
            //        EnerefsysDAL.OptimizationResultData.Insert("2012-05-09", timeStr, Convert.ToDouble(textBox_Temperature.Text), load, memin.Type, memin.Value
            //            , percentValue, memin.Value * percentValue, minSolute, minResult);
            //    }
            //}
        }

        private void showMessage()
        {
            addStrToBox("在室外温度为" + textBox_Temperature.Text + "摄氏度，", textBox_Message);
            addStrToBox("系统负荷为" + textBox_Load.Text + "kw的工况下，", textBox_Message);
            addStrToBox("经过Enerefsys计算得出的最优算法控制如下：", textBox_Message);
            if (!IsBoard)
            {
                addStrToBox("主机组合如下：", textBox_Message);
                addStrToBox("--------", textBox_Message);
                foreach (var me in meMin)
                {
                    string machineResult = "类型：" + me.Type + ";";
                    addStrToBox(machineResult, textBox_Message);
                    machineResult = me.Value + "KW * " + String.Format("{0:F}", percentValue * 100) + "%=" + String.Format("{0:F}", me.Value * percentValue) + "KW.";
                    addStrToBox(machineResult, textBox_Message);
                }
                addStrToBox("--------", textBox_Message);
                string minPowerStr = "系统最低功率为：" + String.Format("{0:F}", minResult) + "KW.";
                addStrToBox(minPowerStr, textBox_Message);
                addStrToBox("此时流量为：" + String.Format("{0:F}", minSolute * 100) + "%.", textBox_Message);
                addStrToBox("各主机负荷率为：" + String.Format("{0:F}", percentValue * 100) + "%.", textBox_Message);
                addStrToBox("主机能耗为：" + String.Format("{0:F}", enginePower) + "KW.", textBox_Message);
                addStrToBox("冷冻水泵能耗为：" + String.Format("{0:F}", freezePumpPower) + "KW.", textBox_Message);
                //addStrToBox("冷却水泵能耗为：" + String.Format("{0:F}", minResult - freezePumpPower - enginePower - coolingPower) + "KW.", textBox_Message);
                addStrToBox("冷却水泵能耗为：" + String.Format("{0:F}", lengquePower) + "KW.", textBox_Message);

                addStrToBox("冷却塔能耗为：" + String.Format("{0:F}", coolingPower) + "KW.", textBox_Message);
                addStrToBox("最小流量公式为：W(x)=" + String.Format("{0:F}", threeOption) + "x^3+" +
                    String.Format("{0:F}", a) + "x^2+" + String.Format("{0:F}", b) + "x+" + String.Format("{0:F}", c) + ".", textBox_Message);
            }
            else
            {
                addStrToBox("板换组合如下：", textBox_Message);
                addStrToBox("--------", textBox_Message);
                int load = Convert.ToInt32(textBox_Load.Text);
                percentValue = load / (BoardCount * boardValue);
                addStrToBox("共需要" + BoardCount + "种板换，每种板换为" + boardValue + "KW * " + String.Format("{0:F}", percentValue * 100) + "%", textBox_Message);

                addStrToBox("--------", textBox_Message);
                string minPowerStr = "系统最低功率为：" + String.Format("{0:F}", minResult) + "KW.";
                addStrToBox(minPowerStr, textBox_Message);
                addStrToBox("此时流量为：" + String.Format("{0:F}", minSolute * 100) + "%.", textBox_Message);
                //addStrToBox("各主机负荷率为：" + String.Format("{0:F}", percentValue * 100) + "%.", textBox_Message);
                //addStrToBox("主机能耗为：" + String.Format("{0:F}", enginePower) + "KW.", textBox_Message);
                addStrToBox("冷冻水泵能耗为：" + String.Format("{0:F}", freezePumpPower) + "KW.", textBox_Message);
                //addStrToBox("冷却水泵能耗为：" + String.Format("{0:F}", minResult - freezePumpPower - enginePower - coolingPower) + "KW.", textBox_Message);
                addStrToBox("冷却水泵能耗为：" + String.Format("{0:F}", lengquePower) + "KW.", textBox_Message);

                addStrToBox("冷却塔能耗为：" + String.Format("{0:F}", coolingPower) + "KW.", textBox_Message);
                addStrToBox("最小流量公式为：W(x)=" + String.Format("{0:F}", threeOption) + "x^3+" +
                    String.Format("{0:F}", a) + "x^2+" + String.Format("{0:F}", b) + "x+" + String.Format("{0:F}", c) + ".", textBox_Message);
            }

        }
        private void addStrToBox(string str, TextBox rtbox)
        {
            rtbox.Text += str;
            rtbox.Text += "\r\n";
        }

        private void GetOptimizationResult(List<MachineEntity> machineList, double load, double temperature)
        {
            DealWithCalculate(temperature, load, machineList);
        }


        /// <summary>
        /// 冷冻功率
        /// </summary>
        public double freezePumpPower = 0;
        /// <summary>
        /// 主机功率
        /// </summary>
        public double enginePower = 0;
        /// <summary>
        /// 冷却塔功率
        /// </summary>
        public double coolingPower = 94.0d;


        public int fullFlow = 320;

        //得到一定温度下，一台主机的能耗的函数
        private List<double> getFormulaByEntity(string type, double load, double temperature)
        {
            //得到一定温度下，特定类型和特定负荷下的主机能耗关于流量的函数的二次系数
            List<double> results = EngineManager.GetParamsByType(temperature, load, type);
            return results;
        }

        private void btnOptimizeResult_Click(object sender, EventArgs e)
        {
            SystemOptimizeResult sor = new SystemOptimizeResult();
            sor.Show();
        }




        //常规算法：
        //根据总负荷，按照从vsd到csd的顺序，按照从大到小的顺序选择主机，流量100%，计算能耗
        public List<MachineEntity> OrderMachiList(List<MachineEntity> list, double load)
        {
            List<MachineEntity> tmplist = new List<MachineEntity>();
            Sort(ref list);
            int index = 0;
            double total = 0.0;
            while ((total < load) && (index < list.Count))
            {
                tmplist.Add(list[index]);
                total += list[index++].Value;
            }
            return tmplist;
        }
        private void Sort(ref List<MachineEntity> list)
        {
            MachineEntity tmp;
            for (int i = 0; i < list.Count; i++)
            {
                for (int j = i; j < list.Count; j++)
                {
                    if (Small(list[j], list[i]))
                    {
                        tmp = list[i];
                        list[i] = list[j];
                        list[j] = tmp;
                    }
                }
            }
        }
        private bool Small(MachineEntity obj1, MachineEntity obj2)
        {
            if (obj1.Type == obj2.Type)
                return obj1.Value > obj2.Value ? true : false;
            else
            {
                if (obj1.Type == "VSD")
                    return true;
                else
                    return false;
            }
        }
        /// <summary>
        /// 是否为板换
        /// </summary>
        private bool IsBoard = false;
        /// <summary>
        /// 板换数量
        /// </summary>
        private int BoardCount = 0;
        double threeOption = 0d;
        double a = 0d;
        double b = 0d;
        double c = 0d;
        //在一定温度下得到一个主机组合的最低能耗，及对应的流量
        private SoluteResult getMinByConsist(List<MachineEntity> mes, double temperature, double load)
        {
            /***********************************************************************************/
            //主机功率的计算公式
            /***********************************************************************************/

            threeOption = 0d;
            a = 0d;
            b = 0d;
            c = 0d;
            int engineCount = 0;

            double tempthreeoption = 0d;
            double tempa = 0d;
            double tempb = 0d;
            double tempc = 0d;

            if (!IsBoard)
            {
                double sumLoad2 = 0;
                foreach (var me in mes)
                {
                    sumLoad2 += me.Value;
                }
                //求得每台主机的负荷率，每台主机运行的负荷除以总负荷相等
                double percentValue1 = load / sumLoad2;

                foreach (var me in mes)
                {
                    //得到每台特定类型的主机在一定温度，一定负荷下的关于流量的二次项系数
                    List<double> results = getFormulaByEntity(me.Type, me.Value * percentValue1, temperature);
                    //threeOption += results[0];
                    //a += results[1];
                    //b += results[2];
                    //c += results[3];
                    a += results[0];
                    b += results[1];
                    c += results[2];
                }
                tempthreeoption = threeOption;
                tempa = a;
                tempb = b;
                tempc = c;

                //string pumpType = PumpType.Text;
                engineCount = mes.Count;
            }


            //如果使用板换，去除主机,主机数量当成板换数量

            if (IsBoard)
            {
                tempthreeoption = 0d;
                tempa = 0d;
                tempb = 0d;
                tempc = 0d;
                //主机数量即为板换数量
                int countmodel = Convert.ToInt32(load) % Convert.ToInt32(boardValue);
                if (countmodel == 0)
                    BoardCount = Convert.ToInt32(load) / Convert.ToInt32(boardValue);
                else
                    BoardCount = Convert.ToInt32(load) / Convert.ToInt32(boardValue) + 1;
                engineCount = BoardCount;
            }


            /***********************************************************************************/
            //冷却水泵的计算公式
            /***********************************************************************************/
            if (CoolingType.Equals("并联"))
            {
                double constantNumber = fullFlow * engineCount;
                double constantNumber2 = constantNumber * constantNumber;
                double constantNumber3 = constantNumber2 * constantNumber;
                //从数据库得到二次项系数
                List<double> doubleParams = PumpManager.GetParamsByType("1");

                //对水泵公式中的自变量进行变换，影响到二次方程的a,b,c
                threeOption += doubleParams[0] * constantNumber3;
                a += doubleParams[1] * constantNumber2;
                b += doubleParams[2] * constantNumber;
                c += doubleParams[3];
                //tempa = doubleParams[0] * constantNumber2;
                //tempb = doubleParams[1] * constantNumber;
                //tempc = doubleParams[2];
            }

            if (CoolingType.Equals("一对一"))
            {
                fullFlow = Convert.ToInt32(PumpInfoData.getFlow("1"));
                //从数据库得到二次项系数
                List<double> doubleParams = PumpManager.GetParamsByType("1");
                //对水泵公式中的自变量进行变换，影响到二次方程的a,b,c
                threeOption += doubleParams[0] * fullFlow * fullFlow * fullFlow * engineCount;
                a += doubleParams[1] * fullFlow * fullFlow * engineCount;
                b += doubleParams[2] * fullFlow * engineCount;
                c += doubleParams[3] * engineCount;

            }


            //求出使得能耗最低的解，即流量的百分比
            double solute = Utility.GetMinSolute(threeOption, a, b, c);

            List<double> doubleParamss = PumpManager.GetParamsByType("1");
            lengquePower = doubleParamss[0] * fullFlow * fullFlow * fullFlow * engineCount * solute * solute * solute +
                doubleParamss[1] * fullFlow * fullFlow * engineCount * solute * solute +
                doubleParamss[2] * fullFlow * engineCount * solute +
                 doubleParamss[3] * engineCount;

            //if (solute < 0.45)
            // solute = 0.45;
            double result = threeOption * solute * solute * solute + a * solute * solute + b * solute + c;

            //enginePower = tempthreeoption * solute * solute * solute + tempa * solute * solute + tempb * solute + tempc;

            enginePower = tempa * solute * solute + tempb * solute + tempc;

            return new SoluteResult(result, solute);
        }

        private double lengquePower = 0d;
        /// <summary>
        /// 主体方法
        /// </summary>
        /// <param name="temperature"></param>
        /// <param name="load"></param>
        /// <param name="machineEntities"></param>
        public void DealWithCalculate(double temperature, double load, List<MachineEntity> machineEntities)
        {
            /*****************************************************************************/
            //根据界面的板换信息，得到最终的总负荷
            /*****************************************************************************/
            double swapPower = 0;
            int swapCount = 0;
            //此处为总负荷，由界面录入，
            if (IsSwap)
                load = load - swapCount * swapPower;



            /*****************************************************************************/
            //如果是常规算法。则机器按照从vsd到csd的顺序，按照从大到小的顺序选择主机
            /*****************************************************************************/
            if (IsNormal)
            {
                minResult = 0;
                // 加工machineResult, 排序...
                machineEntities = OrderMachiList(machineEntities, load);
                meMin = machineEntities;

                double a = 0, b = 0, c = 0;
                double sumLoad2 = 0;
                foreach (var me in machineEntities)
                {
                    sumLoad2 += me.Value;
                }
                //求得每台主机的负荷率，每台主机运行的负荷除以总负荷相等
                double percentValue1 = load / sumLoad2;
                foreach (var me in machineEntities)
                {
                    //得到每台特定类型的主机在一定温度，一定负荷下的关于流量的二次项系数
                    List<double> results = getFormulaByEntity(me.Type, me.Value * percentValue1, temperature);
                    a += results[0];
                    b += results[1];
                    c += results[2];
                }
                percentValue = percentValue1;
                enginePower += a + b + c;

                int engineCount = machineEntities.Count;
                //冷却水泵,按照并联处理
                List<double> doubleParamsCool = PumpManager.GetParamsByType("1");
                lengquePower += doubleParamsCool[0] * fullFlow * fullFlow * fullFlow * engineCount * engineCount * engineCount
                    + doubleParamsCool[1] * fullFlow * fullFlow * engineCount * engineCount
                    + doubleParamsCool[2] * fullFlow * engineCount
                    + doubleParamsCool[3] * engineCount;
                //冷冻水泵
                List<double> doubleParamsFreeze = PumpManager.GetParamsByType("2");
                freezePumpPower += doubleParamsFreeze[0] * 125 * 125 * 125 * engineCount * engineCount * engineCount
                    + doubleParamsFreeze[1] * 125 * 125 * engineCount * engineCount
                    + doubleParamsFreeze[2] * 125 * engineCount
                    + doubleParamsFreeze[3] * engineCount;

                coolingPower += machineEntities.Count * coolingPower;

                minResult += enginePower;
                minResult += lengquePower;
                minResult += freezePumpPower;
                minResult += coolingPower;

                minSolute = 1.0;
                return;
            }

            /*****************************************************************************/
            //如果是优化算法。根据主机的选择和冷却水泵和冷却水泵的并联和一对一进行
            /*****************************************************************************/
            if (!IsBoard)
            {
                List<double> doubleList = new List<double>();
                if (machineEntities == null)
                {
                    MessageBox.Show("请配置冷冻机！");
                    return;
                }

                for (int i = 0; i < machineEntities.Count; i++)
                {
                    doubleList.Add(machineEntities[i].Value);
                }

                //判断总负荷是否成立
                double sumLoad = 0;
                foreach (var me in machineEntities)
                {
                    sumLoad += me.Value;
                }

                if (sumLoad < load)
                {
                    //MessageBox.Show("总负荷过大，所提供主机不足");
                    //将总负荷赋给所需负荷
                    load = sumLoad;
                    //return;
                }

                //根据数量得到最终组合
                List<List<int>> consist = Utility.GetConsist(doubleList, load);
                foreach (var con in consist)
                {
                    //申请一个组合的列表
                    List<MachineEntity> machineResult = new List<MachineEntity>();

                    //对一个组合中的数字进行轮询
                    foreach (var val in con)
                    {
                        //将每一个脚码添加到结果里面
                        machineResult.Add(machineEntities[val]);
                    }
                    //以上得到一个组合，接下来对其求最小值
                    SoluteResult sr = getMinByConsist(machineResult, temperature, load);
                    /***********************************************************************************/
                    //冷冻水泵的计算公式
                    /***********************************************************************************/
                    freezePumpPower = 0;
                    if (FreezeType.Equals("一对一"))
                    {
                        double minValue = double.MaxValue;
                        foreach (var me in machineResult)
                        {
                            if (minValue > me.Value)
                                minValue = me.Value;
                        }
                        if (4.187 * TemperRange * 125 < minValue)
                        {
                            //MessageBox.Show("总负荷过大，所提供冷冻水泵不足");
                            return;
                        }

                        //从数据库得到二次项系数
                        //一对一：一台水泵对应一台主机
                        List<double> doubleParams = PumpManager.GetParamsByType("2");
                        fullFlow = Convert.ToInt32(PumpInfoData.getFlow("2"));
                        //如果是满足板换条件
                        if (IsBoard)
                        {
                            double curflow = load * 3.6 / (4.187 * TemperRange);
                            if (curflow < fullFlow * 0.3)
                                curflow = fullFlow * 0.3;
                            double curPower = doubleParams[0] * curflow * curflow * curflow + doubleParams[1] * curflow * curflow + doubleParams[2] * curflow + doubleParams[3];
                            curPower = curPower * BoardCount;
                            freezePumpPower += curPower;
                        }
                        else
                        {
                            //此处相当于功率乘以主机数量
                            foreach (var me in machineResult)
                            {
                                //此处公式修改
                                //double curflow = me.Value / (4.187 * TemperRange);
                                double curflow = load * 3.6 / (4.187 * TemperRange);
                                if (curflow < fullFlow * 0.3)
                                    curflow = fullFlow * 0.3;
                                double curPower = doubleParams[0] * curflow * curflow * curflow + doubleParams[1] * curflow * curflow + doubleParams[2] * curflow + doubleParams[3];
                                freezePumpPower += curPower;
                            }
                        }

                    }
                    if (FreezeType.Equals("并联"))
                    {
                        if (4.187 * TemperRange * 125 * machineResult.Count < load)
                        {
                            MessageBox.Show("总负荷过大，所提供冷冻水泵不足");
                            return;
                        }

                        //从数据库得到二次项系数
                        List<double> doubleParams = PumpManager.GetParamsByType("2");
                        double curflow = load / (4.187 * TemperRange * machineResult.Count);
                        double curPower = doubleParams[0] * curflow * curflow * curflow + doubleParams[1] * curflow * curflow + doubleParams[2] * curflow + doubleParams[3];
                        freezePumpPower += curPower;
                    }
                    //得到最终结果,并且加上冷却塔功率
                    //double coolingPower = 0;
                    sr = new SoluteResult(sr.Result + freezePumpPower + coolingPower, sr.Solute);

                    /***********************************************************************************/
                    //判断某个组合的的最小功率是不是在所有组合中最小
                    /***********************************************************************************/
                    if (sr.Result < minResult)
                    {
                        //如果是最小的，则将最小值赋值为当前组合的最小值
                        minResult = sr.Result;
                        //保存取得最小能耗时的流量
                        minSolute = sr.Solute;
                        //此处保存最小的主机组合
                        meMin = machineResult;
                    }
                    //如果是板换不进行循环
                    if (IsBoard)
                        break;
                }
                //循环结束得到最小的主机组合，及最小值


                double sumLoad1 = 0;
                foreach (var me in meMin)
                {
                    sumLoad1 += me.Value;
                }
                //求得每台主机的负荷率，每台主机运行的负荷除以总负荷相等
                percentValue = load / sumLoad1;
            }
            else
            {
                //以上得到一个组合，接下来对其求最小值
                SoluteResult sr = getMinByConsist(null, temperature, load);
                /***********************************************************************************/
                //冷冻水泵的计算公式
                /***********************************************************************************/
                freezePumpPower = 0;
                if (FreezeType.Equals("一对一"))
                {
                    //从数据库得到二次项系数
                    //一对一：一台水泵对应一台主机
                    List<double> doubleParams = PumpManager.GetParamsByType("2");
                    fullFlow = Convert.ToInt32(PumpInfoData.getFlow("2"));
                    //如果是满足板换条件
                    if (IsBoard)
                    {
                        double curflow = load * 3.6 / (4.187 * TemperRange);
                        if (curflow < fullFlow * 0.3)
                            curflow = fullFlow * 0.3;
                        double curPower = doubleParams[0] * curflow * curflow * curflow + doubleParams[1] * curflow * curflow + doubleParams[2] * curflow + doubleParams[3];
                        curPower = curPower * BoardCount;
                        freezePumpPower += curPower;
                    }
                }

                //得到最终结果,并且加上冷却塔功率
                //double coolingPower = 0;
                sr = new SoluteResult(sr.Result + freezePumpPower + coolingPower, sr.Solute);

                /***********************************************************************************/
                //判断某个组合的的最小功率是不是在所有组合中最小
                /***********************************************************************************/
                if (sr.Result < minResult)
                {
                    //如果是最小的，则将最小值赋值为当前组合的最小值
                    minResult = sr.Result;
                    //保存取得最小能耗时的流量
                    minSolute = sr.Solute;
                    //此处保存最小的主机组合
                    //meMin = machineResult;
                }


                //循环结束得到最小的主机组合，及最小值


                //double sumLoad1 = 0;
                //foreach (var me in meMin)
                //{
                //    sumLoad1 += me.Value;
                //}
                ////求得每台主机的负荷率，每台主机运行的负荷除以总负荷相等
                //percentValue = load / sumLoad1;



            }

        }


        private void createNewProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void openExistingProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = "d:\\";

            openFileDialog.Filter = "Ener文件(*.xml)|*.xml";

            openFileDialog.RestoreDirectory = true;

            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                openProject(openFileDialog.FileName);
                isFlag = true;
            }
        }

        /**********************************************************************************************************/
        //窗口实体类部分

        //产生冷冻机类
        class SubFreezer
        {
            public Label freazer
            {
                get;
                set;
            }
            public ComboBox type_box
            {
                get;
                set;
            }
            public TextBox cooling_comboBox
            {
                get;
                set;
            }
            public ComboBox brand_comboBox
            {
                get;
                set;
            }
            public TextBox model_box
            {
                get;
                set;
            }
            public TextBox amount_textBox
            {
                get;
                set;
            }
            //public ComboBox performance_data_box
            //{
            //    get;
            //    set;
            //}
            public SubFreezer(int i)
            {
                freazer = new Label();
                type_box = new ComboBox();
                cooling_comboBox = new TextBox();
                brand_comboBox = new ComboBox();
                model_box = new TextBox();
                amount_textBox = new TextBox();
                amount_textBox.Width = 20;
                amount_textBox.MaxLength = 2;
                //performance_data_box = new ComboBox();
                setComponentAttribute(i);
                setComponetLocation(i);
            }

            public void setComponentAttribute(int i)
            {
                freazer.Name = "freezer_label" + i;
                type_box.Name = "freezer_type_box" + i;
                type_box.Items.Add("CSD");
                type_box.Items.Add("VSD");
                type_box.SelectedItem = "VSD";
                type_box.DropDownStyle = ComboBoxStyle.DropDownList;
                cooling_comboBox.Name = "freezer_cooling_comboBox" + i;
                brand_comboBox.Name = "freezer_brand_comboBox" + i;
                brand_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                brand_comboBox.Items.Add("York");
                brand_comboBox.Items.Add("Carrier");
                brand_comboBox.Items.Add("McQuay");
                brand_comboBox.Items.Add("TRANE");
                model_box.Name = "freezer_model_box" + i;
                amount_textBox.Name = "freezer_amount_checkBox" + i;
                //performance_data_box.Name = "performance_data_box" + i;
            }

            public void setComponetLocation(int i)
            {
                freazer.Location = new Point(19, 20 + (i - 1) * 39);
                freazer.Text = "冷冻机" + i;
                type_box.Location = new Point(118, 17 + (i - 1) * 39);
                type_box.Width = 100;
                type_box.Height = 21;
                cooling_comboBox.Location = new Point(235, 17 + (i - 1) * 39);
                cooling_comboBox.Width = 100;
                cooling_comboBox.Height = 21;
                brand_comboBox.Location = new Point(363, 17 + (i - 1) * 39);
                brand_comboBox.Width = 100;
                brand_comboBox.Height = 21;
                model_box.Location = new Point(502, 17 + (i - 1) * 39);
                model_box.Width = 100;
                model_box.Height = 21;
                amount_textBox.Location = new Point(669, 17 + (i - 1) * 39);
                //performance_data_box.Location = new Point(773, 17 + (i - 1) * 39);
            }
        }

        //产生板换类
        class SubBoarder
        {
            public Label boarder
            {
                get;
                set;
            }
            public Label addition
            {
                get;
                set;
            }
            public ComboBox type_box
            {
                get;
                set;
            }
            public TextBox cooling_comboBox
            {
                get;
                set;
            }
            public ComboBox brand_comboBox
            {
                get;
                set;
            }
            public TextBox model_box
            {
                get;
                set;
            }
            public TextBox amount_textBox
            {
                get;
                set;
            }
            //public ComboBox performance_data_box
            //{
            //    get;
            //    set;
            //}
            public SubBoarder(int i)
            {
                boarder = new Label();
                boarder.Width = 50;
                addition = new Label();
                addition.Width = 60;
                type_box = new ComboBox();
                cooling_comboBox = new TextBox();
                brand_comboBox = new ComboBox();
                model_box = new TextBox();
                amount_textBox = new TextBox();
                amount_textBox.Width = 20;
                amount_textBox.MaxLength = 2;
                //performance_data_box = new ComboBox();
                setComponentAttribute(i);
                setComponetLocation(i);
            }

            public void setComponentAttribute(int i)
            {
                boarder.Name = "boarder_label" + i;
                addition.Name = "boarder_addition_label" + i;
                type_box.Name = "boarder_type_box" + i;
                type_box.Items.Add("5°");
                type_box.Items.Add("6°");
                type_box.Items.Add("7°");
                type_box.Items.Add("8°");
                type_box.Items.Add("9°");
                type_box.Items.Add("10°");
                type_box.Items.Add("11°");
                type_box.Items.Add("12°");
                type_box.Items.Add("13°");
                type_box.Items.Add("14°");
                type_box.Items.Add("15°");
                type_box.DropDownStyle = ComboBoxStyle.DropDownList;
                cooling_comboBox.Name = "boarder_cooling_comboBox" + i;
                brand_comboBox.Name = "boarder_brand_comboBox" + i;
                model_box.Name = "boarder_model_box" + i;
                amount_textBox.Name = "boarder_amount_textBox" + i;
                //performance_data_box.Name = "performance_data_box" + i;
            }

            public void setComponetLocation(int i)
            {
                boarder.Location = new Point(19, 20 + (i - 1) * 39);
                boarder.Text = "板换";
                addition.Location = new Point(90, 20 + (i - 1) * 39);
                addition.Text = "温度低于:";
                type_box.Location = new Point(158, 17 + (i - 1) * 39);
                type_box.Width = 50;
                type_box.Height = 21;
                cooling_comboBox.Location = new Point(235, 17 + (i - 1) * 39);
                cooling_comboBox.Width = 100;
                cooling_comboBox.Height = 21;
                brand_comboBox.Location = new Point(363, 17 + (i - 1) * 39);
                brand_comboBox.Width = 100;
                brand_comboBox.Height = 21;
                model_box.Location = new Point(502, 17 + (i - 1) * 39);
                model_box.Width = 100;
                model_box.Height = 21;
                amount_textBox.Location = new Point(669, 17 + (i - 1) * 39);
                //performance_data_box.Location = new Point(773, 17 + (i - 1) * 39);
            }
        }


        //产生水泵配置类
        class WaterFreezer
        {
            public Label freazerAndcooler
            {
                get;
                set;
            }
            public ComboBox brand_comboBox
            {
                get;
                set;
            }
            public TextBox model_textBox
            {
                get;
                set;
            }
            public TextBox flow_textBox
            {
                get;
                set;
            }
            public TextBox lift_textBox
            {
                get;
                set;
            }
            public TextBox power_textBox
            {
                get;
                set;
            }
            public TextBox amount_textBox
            {
                get;
                set;
            }
            //public Button performance_data_button
            //{
            //    get;
            //    set;
            //}
            public WaterFreezer(int i)
            {
                freazerAndcooler = new Label();
                brand_comboBox = new ComboBox();
                model_textBox = new TextBox();
                amount_textBox = new TextBox();
                //performance_data_button = new Button();
                flow_textBox = new TextBox();
                lift_textBox = new TextBox();
                power_textBox = new TextBox();
                setComponentAttribute(i);
                setComponetLocation(i);
            }

            public void setComponentAttribute(int i)
            {
                freazerAndcooler.Name = "freazerAndcooler" + i;
                brand_comboBox.Name = "brand_comboBox" + i;
                brand_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                brand_comboBox.Items.Add("格兰富");
                brand_comboBox.Items.Add("ITT LOWARA");
                brand_comboBox.Items.Add("威乐");
                brand_comboBox.Items.Add("凯士比KSB");
                brand_comboBox.Items.Add("东方EAST");
                brand_comboBox.Items.Add("博山");
                brand_comboBox.Items.Add("肯富来KENFLO");
                brand_comboBox.Text = brand_comboBox.Items[0].ToString();
                model_textBox.Name = "model_textBox" + i;
                amount_textBox.Name = "amount_textBox" + i;
                //performance_data_button.Name = "performance_data_button" + i;
                flow_textBox.Name = "flow_textBox" + i;
                lift_textBox.Name = "lift_textBox" + i;
                power_textBox.Name = "power_textBox" + i;
            }

            public void setComponetLocation(int i)
            {
                freazerAndcooler.Location = new Point(15, 17 + (i - 1) * 35);
                freazerAndcooler.Width = 68;
                freazerAndcooler.Height = 12;
                brand_comboBox.Location = new Point(112, 15 + (i - 1) * 35);
                brand_comboBox.Width = 76;
                brand_comboBox.Height = 20;
                flow_textBox.Location = new Point(234, 15 + (i - 1) * 35);
                flow_textBox.Width = 76;
                flow_textBox.Height = 20;
                lift_textBox.Location = new Point(359, 15 + (i - 1) * 35);
                lift_textBox.Width = 76;
                lift_textBox.Height = 20;
                power_textBox.Location = new Point(483, 15 + (i - 1) * 35);
                power_textBox.Width = 76;
                power_textBox.Height = 20;
                model_textBox.Location = new Point(608, 15 + (i - 1) * 35);
                model_textBox.Width = 76;
                model_textBox.Height = 20;
                amount_textBox.Location = new Point(733, 15 + (i - 1) * 35);
                amount_textBox.Width = 20;
                amount_textBox.Height = 20;
                //performance_data_button.Location = new Point(700, 15 + (i - 1) * 35);
                //performance_data_button.Width = 60;
                //performance_data_button.Height = 23;
                //performance_data_button.Text = "调用";
            }
        }

        class CoolingTower
        {
            public Label LBCoolingTower
            {
                get;
                set;
            }
            public TextBox brand_textBox
            {
                get;
                set;
            }
            public ComboBox type_comboBox
            {
                get;
                set;
            }
            public TextBox throughput_textBox
            {
                get;
                set;
            }
            public TextBox temperature_textBox
            {
                get;
                set;
            }
            public TextBox power_textBox
            {
                get;
                set;
            }
            public TextBox amount_textBox
            {
                get;
                set;
            }
            public Label lb1
            {
                get;
                set;
            }
            public Label lb2
            {
                get;
                set;
            }
            public Label lb3
            {
                get;
                set;
            }
            public Label lb4
            {
                get;
                set;
            }
            public Label lb5
            {
                get;
                set;
            }
            public Label lb_Unit_HZ_B
            {
                get;
                set;
            }
            public Label lb_Unit_HZ_C
            {
                get;
                set;
            }
            public Label lb_Unit_KW_A
            {
                get;
                set;
            }
            public Label lb_Unit_KW_B
            {
                get;
                set;
            }
            public Label lb_Unit_KW_C
            {
                get;
                set;
            }
            public TextBox tbx_HZ_A
            {
                get;
                set;
            }
            public TextBox tbx_HZ_B
            {
                get;
                set;
            }
            public TextBox tbx_HZ_C
            {
                get;
                set;
            }
            public TextBox tbx_KW_A
            {
                get;
                set;
            }
            public TextBox tbx_KW_B
            {
                get;
                set;
            }
            public TextBox tbx_KW_C
            {
                get;
                set;
            }
            public TextBox tbx_T1
            {
                get;
                set;
            }
            public TextBox tbx_T2
            {
                get;
                set;
            }
            //public ComboBox performance_data_box
            //{
            //    get;
            //    set;
            //}
            public CoolingTower(int i)
            {
                LBCoolingTower = new Label();
                LBCoolingTower.Width = 50;
                brand_textBox = new TextBox();
                type_comboBox = new ComboBox();
                throughput_textBox = new TextBox();
                temperature_textBox = new TextBox();
                power_textBox = new TextBox();
                amount_textBox = new TextBox();
                amount_textBox.Width = 20;
                amount_textBox.MaxLength = 2;
                lb1 = new Label();
                lb2 = new Label();
                lb3 = new Label();
                lb4 = new Label();
                lb5 = new Label();
                lb_Unit_HZ_B = new Label();
                lb_Unit_HZ_C = new Label();
                lb_Unit_KW_A = new Label();
                lb_Unit_KW_B = new Label();
                lb_Unit_KW_C = new Label();
                tbx_HZ_A = new TextBox();
                tbx_HZ_B = new TextBox();
                tbx_HZ_C = new TextBox();
                tbx_KW_A = new TextBox();
                tbx_KW_B = new TextBox();
                tbx_KW_C = new TextBox();
                tbx_T1 = new TextBox();
                tbx_T2 = new TextBox();

                //performance_data_box = new ComboBox();
                setComponentAttribute(i);
                setComponetLocation(i);
            }

            public void setComponentAttribute(int i)
            {
                LBCoolingTower.Name = "CoolingTower" + i;
                LBCoolingTower.Text = "冷却塔";
                brand_textBox.Name = "brand_comboBox" + i;
                type_comboBox.Name = "type_comboBox" + i;
                type_comboBox.Items.Add("常规");
                type_comboBox.Items.Add("高低速");
                type_comboBox.Items.Add("变频");
                type_comboBox.DropDownStyle = ComboBoxStyle.DropDownList;
                type_comboBox.SelectedIndex = 0;
                type_comboBox.SelectedIndexChanged += new EventHandler(type_comboBox_SelectedIndexChanged);
                throughput_textBox.Name = "throughput_textBox" + i;
                temperature_textBox.Name = "temperature_textBox" + i;
                power_textBox.Name = "power_textBox" + i;
                amount_textBox.Name = "boarder_amount_textBox" + i;
                amount_textBox.Text = "1";
                //performance_data_box.Name = "performance_data_box" + i;
                lb1.Text = "根据温度 T1：";
                lb1.Width = 85;
                lb2.Text = "℃, T2：";
                lb2.Width = 55;
                lb3.Text = "℃ 选择能耗，当t < T1时";
                lb3.Width = 150;
                tbx_HZ_A.Text = "不开冷却塔";
                tbx_HZ_A.Width = 70;
                tbx_HZ_A.Enabled = false;
                tbx_KW_A.Text = "0";
                tbx_KW_A.Width = 30;
                tbx_KW_A.Enabled = false;

                lb4.Text = "当T1 < t < T2时";
                lb4.Width = 100;
                tbx_HZ_B.Text = "开低速";
                tbx_HZ_B.Width = 45;
                tbx_KW_B.Width = 30;

                lb5.Text = "当t > T2时";
                lb5.Width = 80;
                tbx_HZ_C.Text = "开高速";
                tbx_HZ_C.Width = 45;
                tbx_KW_C.Width = 30;

                lb_Unit_HZ_B.Text = "HZ";
                lb_Unit_HZ_B.Width = 20;
                lb_Unit_HZ_C.Text = "HZ";
                lb_Unit_HZ_C.Width = 20;
                lb_Unit_KW_A.Text = "KW";
                lb_Unit_KW_A.Width = 20;
                lb_Unit_KW_B.Text = "KW";
                lb_Unit_KW_B.Width = 20;
                lb_Unit_KW_C.Text = "KW";
                lb_Unit_KW_C.Width = 20;
                tbx_T1.Width = 20;
                tbx_T1.MaxLength = 2;
                tbx_T2.Width = 20;
                tbx_T2.MaxLength = 2;

                power_textBox.Enabled = true;
                lb1.Visible = false;
                lb2.Visible = false;
                lb3.Visible = false;
                lb4.Visible = false;
                lb5.Visible = false;
                lb_Unit_HZ_B.Visible = false;
                lb_Unit_HZ_C.Visible = false;
                lb_Unit_KW_A.Visible = false;
                lb_Unit_KW_B.Visible = false;
                lb_Unit_KW_C.Visible = false;
                tbx_HZ_A.Visible = false;
                tbx_HZ_B.Visible = false;
                tbx_HZ_C.Visible = false;
                tbx_KW_A.Visible = false;
                tbx_KW_B.Visible = false;
                tbx_KW_C.Visible = false;
                tbx_T1.Visible = false;
                tbx_T2.Visible = false;
            }

            public void type_comboBox_SelectedIndexChanged(object sender, EventArgs e)
            {
                if (type_comboBox.SelectedIndex == 0)
                {
                    power_textBox.Enabled = true;
                    lb1.Visible = false;
                    lb2.Visible = false;
                    lb3.Visible = false;
                    lb4.Visible = false;
                    lb5.Visible = false;
                    lb_Unit_HZ_B.Visible = false;
                    lb_Unit_HZ_C.Visible = false;
                    lb_Unit_KW_A.Visible = false;
                    lb_Unit_KW_B.Visible = false;
                    lb_Unit_KW_C.Visible = false;
                    tbx_HZ_A.Visible = false;
                    tbx_HZ_B.Visible = false;
                    tbx_HZ_C.Visible = false;
                    tbx_KW_A.Visible = false;
                    tbx_KW_B.Visible = false;
                    tbx_KW_C.Visible = false;
                    tbx_T1.Visible = false;
                    tbx_T2.Visible = false;
                }
                else if (type_comboBox.SelectedIndex == 1)
                {
                    power_textBox.Text = "";
                    power_textBox.Enabled = false;
                    lb1.Visible = true;
                    lb2.Visible = true;
                    lb3.Visible = true;
                    lb3.Text = "选择能耗，当t < T1时";
                    lb4.Visible = true;
                    lb5.Visible = true;
                    lb_Unit_HZ_B.Visible = false;
                    lb_Unit_HZ_C.Visible = false;
                    lb_Unit_KW_A.Visible = true;
                    lb_Unit_KW_B.Visible = true;
                    lb_Unit_KW_B.Text = "KW";
                    lb_Unit_KW_B.Width = 20;
                    lb_Unit_KW_C.Visible = true;
                    tbx_HZ_A.Visible = true;
                    tbx_HZ_B.Visible = true;
                    tbx_HZ_B.Text = "开低速";
                    tbx_HZ_B.Enabled = false;
                    tbx_HZ_C.Visible = true;
                    tbx_HZ_C.Text = "开高速";
                    tbx_HZ_C.Enabled = false;
                    tbx_KW_A.Visible = true;
                    tbx_KW_B.Visible = true;
                    tbx_KW_B.Text = "";
                    tbx_KW_B.Enabled = true;
                    tbx_KW_C.Visible = true;
                    tbx_T1.Visible = true;
                    tbx_T2.Visible = true;
                }
                else
                {
                    power_textBox.Text = "";
                    power_textBox.Enabled = false;
                    lb1.Visible = true;
                    lb2.Visible = true;
                    lb3.Visible = true;
                    lb3.Text = "判断能耗，当t < T1时";
                    lb4.Visible = true;
                    lb5.Visible = true;
                    lb_Unit_HZ_B.Visible = true;
                    lb_Unit_HZ_C.Visible = true;
                    lb_Unit_KW_A.Visible = true;
                    lb_Unit_KW_B.Visible = true;
                    lb_Unit_KW_B.Text = "KW (*该结果由计算获得)";
                    lb_Unit_KW_B.Width = 200;
                    lb_Unit_KW_C.Visible = true;
                    tbx_HZ_A.Visible = true;
                    tbx_HZ_B.Visible = true;
                    tbx_HZ_B.Text = "30";
                    tbx_HZ_B.Enabled = true;
                    tbx_HZ_C.Text = "50";
                    tbx_HZ_C.Enabled = false;
                    tbx_HZ_C.Visible = true;
                    tbx_KW_A.Visible = true;
                    tbx_KW_B.Visible = true;
                    tbx_KW_B.Enabled = false;
                    tbx_KW_C.Visible = true;
                    tbx_T1.Visible = true;
                    tbx_T2.Visible = true;
                }
            }

            public void setComponetLocation(int i)
            {
                LBCoolingTower.Location = new Point(15, 20 + (i - 1) * 35);
                LBCoolingTower.Width = 68;
                LBCoolingTower.Height = 12;
                brand_textBox.Location = new Point(112, 17 + (i - 1) * 35);
                brand_textBox.Width = 76;
                brand_textBox.Height = 20;
                type_comboBox.Location = new Point(234, 17 + (i - 1) * 35);
                type_comboBox.Width = 76;
                type_comboBox.Height = 20;
                throughput_textBox.Location = new Point(359, 17 + (i - 1) * 35);
                throughput_textBox.Width = 76;
                throughput_textBox.Height = 20;
                power_textBox.Location = new Point(560, 17 + (i - 1) * 35);
                power_textBox.Width = 76;
                power_textBox.Height = 20;
                temperature_textBox.Location = new Point(474, 17 + (i - 1) * 35);
                temperature_textBox.Width = 36;
                temperature_textBox.Height = 20;
                amount_textBox.Location = new Point(683, 17 + (i - 1) * 35);
                amount_textBox.Width = 20;
                amount_textBox.Height = 20;
                lb1.Location = new Point(15, 60 + (i - 1) * 35);
                tbx_T1.Location = new Point(100, 57 + (i - 1) * 35);
                lb2.Location = new Point(125, 60 + (i - 1) * 35);
                tbx_T2.Location = new Point(180, 57 + (i - 1) * 35);
                lb3.Location = new Point(205, 60 + (i - 1) * 35);
                tbx_HZ_A.Location = new Point(400, 57 + (i - 1) * 35);
                tbx_KW_A.Location = new Point(550, 57 + (i - 1) * 35);
                lb_Unit_KW_A.Location = new Point(580, 60 + (i - 1) * 35);

                lb4.Location = new Point(265, 100 + (i - 1) * 35);
                tbx_HZ_B.Location = new Point(400, 97 + (i - 1) * 35);
                lb_Unit_HZ_B.Location = new Point(450, 100 + (i - 1) * 35);
                tbx_KW_B.Location = new Point(550, 97 + (i - 1) * 35);
                lb_Unit_KW_B.Location = new Point(580, 100 + (i - 1) * 35);

                lb5.Location = new Point(265, 140 + (i - 1) * 35);
                tbx_HZ_C.Location = new Point(400, 137 + (i - 1) * 35);
                lb_Unit_HZ_C.Location = new Point(450, 140 + (i - 1) * 35);
                tbx_KW_C.Location = new Point(550, 137 + (i - 1) * 35);
                lb_Unit_KW_C.Location = new Point(580, 140 + (i - 1) * 35);
            }
        }



        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Filter = "Ener文件(*.xml)|*.xml";
            saveFileDialog.FilterIndex = 2;
            saveFileDialog.RestoreDirectory = true;
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                saveProject(saveFileDialog.FileName);
            }
        }

        private void saveProject(string xml)
        {
            //string xml = "D:/prj.xml";
            FileStream fs = new FileStream(xml, FileMode.Create);
            StreamWriter sw = new StreamWriter(fs);
            //开始写入
            sw.Write("<?xml version=\"1.0\" encoding=\"utf-8\"?>");
            sw.Write("<Project />");
            //清空缓冲区
            sw.Flush();
            //关闭流
            sw.Close();
            fs.Close();

            /**********************************************************************************************/
            //插入环境变量配置
            /**********************************************************************************************/
            XmlHelper.Insert(xml, "/Project", "EnvironmentVariable", "", "");

            //建筑物制冷类型
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable", "BuildingCooling", "", "");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/BuildingCooling", "BuildingType", "", comboBox1.Text);
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/BuildingCooling", "BuildingArea", "", textBox1.Text);
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/BuildingCooling", "PeakLoad", "", textBox1.Text);
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/BuildingCooling", "CoolingStart", "", domainUpDown1.Text);
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/BuildingCooling", "CoolingEnd", "", domainUpDown2.Text);
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/BuildingCooling", "HoursPerDay", "", textBox3.Text);
            //建筑物制冷设计
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable", "CoolingDesign", "", "");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign", "Freeze", "", "");
            //冷冻
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign/Freeze", "InTemperature", "", textBox4.Text);
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign/Freeze", "BackTemperature", "", textBox5.Text);
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign/Freeze", "TotalFlow", "", textBox6.Text);
            //冷却日温度
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign", "CoolingDay", "", "");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign/CoolingDay", "InTemperature", "", textBox7.Text);
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign/CoolingDay", "OutTemperature", "", textBox8.Text);
            //冷却夜温度
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign", "CoolingNight", "", "");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign/CoolingNight", "InTemperature", "", textBox9.Text);



            //电价配置
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable", "ProperPrice", "", "");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/ProperPrice", "ProjectPlace", "", comboBox2.Text);
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/ProperPrice", "PriceType", "", comboBox3.Text);
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/ProperPrice", "PowerLevel", "", comboBox4.Text);


            //外部环境配置
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable", "OuterEnvironment", "", "");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/OuterEnvironment", "WetBallTemperature", "", textBox10.Text);
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/OuterEnvironment", "DayDryBallTemperature", "", textBox11.Text);
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/OuterEnvironment", "NightDryBallTemperature", "", textBox12.Text);

            /**********************************************************************************************/
            //插入冷冻机配置
            /**********************************************************************************************/
            XmlHelper.Insert(xml, "/Project", "FreezeConfiguration", "", "");
            XmlHelper.Insert(xml, "/Project/FreezeConfiguration", "FreezerCount", "", freezerNum.Text);
            //此处如果板换点过，设置为1，未点设置为0
            if (checkBoxBoard.Checked)
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration", "BoardCount", "", "1");
            else
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration", "BoardCount", "", "0");


            //冷冻机动态获取控件，已经成功
            for (int i = 0; i < Convert.ToInt32(freezerNum.Text); i++)
            {
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration", "FreezeMachine", "id", i.ToString());
                string strTypeBox = "type_box" + (i + 1);
                ComboBox temp_type_box = (ComboBox)GetControl(strTypeBox, freezer_Panel);
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "Type", "", temp_type_box.Text);
                string strCooling_comboBox = "cooling_comboBox" + (i + 1);
                TextBox temp_Cooling_comboBox = (TextBox)GetControl(strCooling_comboBox, freezer_Panel);
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "FreezePower", "", temp_Cooling_comboBox.Text);
                string strBrand_comboBox = "brand_comboBox" + (i + 1);
                ComboBox brand_comboBox = (ComboBox)GetControl(strBrand_comboBox, freezer_Panel);
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "Brand", "", brand_comboBox.Text);
                string strModel_box = "model_box" + (i + 1);
                TextBox temp_model_box = (TextBox)GetControl(strModel_box, freezer_Panel);
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "Model", "", temp_model_box.Text);
                string strIs_Frequency_Conversion_checkBox = "amount_textbox" + (i + 1);
                TextBox temp_is_Frequency_Conversion_checkBox = (TextBox)GetControl(strIs_Frequency_Conversion_checkBox, freezer_Panel);
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "Frequency", "", temp_is_Frequency_Conversion_checkBox.Text);
                string strPerformance_data_box = "performance_data_box" + (i + 1);
                //ComboBox temp_performance_data_box = (ComboBox)GetControl(strPerformance_data_box, freezer_Panel);
                //MessageBox.Show(temp_performance_data_box.Name);
            }
            if (checkBoxBoard.Checked)
            {
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration", "Board", "", "");
                ComboBox temp_type_box = (ComboBox)GetControl("board_type_box", boarder_Panel);
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/Board", "BoardTemperature", "", temp_type_box.Text);

                TextBox temp_Cooling_comboBox = (TextBox)GetControl("board_comboBox", boarder_Panel);
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/Board", "BoardPower", "", temp_Cooling_comboBox.Text);

                ComboBox brand_comboBox = (ComboBox)GetControl("board_brand_comboBox", boarder_Panel);
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/Board", "BoardBrand", "", brand_comboBox.Text);

                TextBox temp_model_box = (TextBox)GetControl("board_model_box", boarder_Panel);
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/Board", "BoardModel", "", temp_model_box.Text);

                TextBox temp_is_Frequency_Conversion_checkBox = (TextBox)GetControl("board_amount_box", boarder_Panel);
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/Board", "BoardNo", "", temp_is_Frequency_Conversion_checkBox.Text);

            }

            /*XmlHelper.Insert(xml, "/Project", "FreezeConfiguration", "", "");
            for (int i = 0; i < Convert.ToInt32(freezerNum.Text); i++)
            {
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration", "FreezeMachine", "id", i.ToString());
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "Type", "", "this is type");
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "FreezePower", "", "this is type");
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "Brand", "", "this is type");
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "Model", "", "this is type");
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "Frequency", "", "this is type");
            }*/


            /**********************************************************************************************/
            //插入水泵配置
            /**********************************************************************************************/
            //冰冻水泵动态获取控件，已经成功
            XmlHelper.Insert(xml, "/Project", "PumpConfiguration", "", "");
            XmlHelper.Insert(xml, "/Project/PumpConfiguration", "FreezePumpCount", "", freezingNum.Text);
            XmlHelper.Insert(xml, "/Project/PumpConfiguration", "CoolingPumpCount", "", coolingNum.Text);
            for (int i = 0; i < Convert.ToInt32(freezingNum.Text); i++)
            {
                XmlHelper.Insert(xml, "/Project/PumpConfiguration", "FreezePump", "id", i.ToString());
                string strbrand_comboBox = "brand_comboBox" + (i + 1);
                ComboBox temp_brand_comboBox = (ComboBox)GetControl(strbrand_comboBox, freezingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Brand", "", temp_brand_comboBox.Text);


                string strflow_comboBox = "flow_comboBox" + (i + 1);
                TextBox temp_flow_comboBox = (TextBox)GetControl(strflow_comboBox, freezingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Flow", "", temp_flow_comboBox.Text);

                string strlift_comboBox = "lift_comboBox" + (i + 1);
                TextBox temp_lift_comboBox = (TextBox)GetControl(strlift_comboBox, freezingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Distance", "", temp_lift_comboBox.Text);

                string strpower_comboBox = "power_comboBox" + (i + 1);
                TextBox temp_power_comboBox = (TextBox)GetControl(strpower_comboBox, freezingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Power", "", temp_power_comboBox.Text);

                string strmodel_comboBox = "model_comboBox" + (i + 1);
                TextBox temp_model_comboBox = (TextBox)GetControl(strmodel_comboBox, freezingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Type", "", temp_model_comboBox.Text);


                string strtype_comboBox = "type_comboBox" + (i + 1);
                TextBox temp_type_comboBox = (TextBox)GetControl(strtype_comboBox, freezingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Count", "", temp_type_comboBox.Text);

            }

            for (int i = 0; i < Convert.ToInt32(coolingNum.Text); i++)
            {
                XmlHelper.Insert(xml, "/Project/PumpConfiguration", "CoolingPump", "id", i.ToString());
                string strbrand_comboBox = "brand_comboBox" + (i + 1);
                ComboBox temp_brand_comboBox = (ComboBox)GetControl(strbrand_comboBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Brand", "", temp_brand_comboBox.Text);


                string strflow_comboBox = "flow_comboBox" + (i + 1);
                TextBox temp_flow_comboBox = (TextBox)GetControl(strflow_comboBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Flow", "", temp_flow_comboBox.Text);

                string strlift_comboBox = "lift_comboBox" + (i + 1);
                TextBox temp_lift_comboBox = (TextBox)GetControl(strlift_comboBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Distance", "", temp_lift_comboBox.Text);

                string strpower_comboBox = "power_comboBox" + (i + 1);
                TextBox temp_power_comboBox = (TextBox)GetControl(strpower_comboBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Power", "", temp_power_comboBox.Text);

                string strmodel_comboBox = "model_comboBox" + (i + 1);
                TextBox temp_model_comboBox = (TextBox)GetControl(strmodel_comboBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Type", "", temp_model_comboBox.Text);


                string strtype_comboBox = "type_comboBox" + (i + 1);
                TextBox temp_type_comboBox = (TextBox)GetControl(strtype_comboBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Count", "", temp_type_comboBox.Text);
            }



            /**********************************************************************************************/
            //插入冷却配置
            /**********************************************************************************************/
            XmlHelper.Insert(xml, "/Project", "CoolingConfiguration", "", "");
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration", "CoolingTower", "", "");


            TextBox deng_brand_textbox = (TextBox)GetControl("deng_brand_textbox", coolingtowerpanel);
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "Brand", "", deng_brand_textbox.Text);

            ComboBox deng_type_combox = (ComboBox)GetControl("deng_type_combobox", coolingtowerpanel);
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "Type", "", deng_type_combox.Text);

            TextBox deng_throughput_textbox = (TextBox)GetControl("deng_throughput_textbox", coolingtowerpanel);
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "Flow", "", deng_throughput_textbox.Text);

            TextBox deng_temperature_textbox = (TextBox)GetControl("deng_temperature_textbox", coolingtowerpanel);
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "Temperature", "", deng_temperature_textbox.Text);

            TextBox deng_power_textbox = (TextBox)GetControl("deng_power_textbox", coolingtowerpanel);
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "Power", "", deng_power_textbox.Text);

            TextBox deng_amount_textbox = (TextBox)GetControl("deng_amount_textbox", coolingtowerpanel);
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "Count", "", deng_amount_textbox.Text);


            TextBox deng_t1_textbox = (TextBox)GetControl("deng_t1_textbox", coolingtowerpanel);
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "TF", "", deng_t1_textbox.Text);

            TextBox deng_t2_textbox = (TextBox)GetControl("deng_t2_textbox", coolingtowerpanel);
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "TS", "", deng_t2_textbox.Text);


            TextBox deng_tfs_kw_textbox = (TextBox)GetControl("deng_tfs_kw_textbox", coolingtowerpanel);
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "PFS", "", deng_tfs_kw_textbox.Text);

            TextBox deng_ts_textbox = (TextBox)GetControl("deng_ts_textbox", coolingtowerpanel);
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "PS", "", deng_ts_textbox.Text);

            TextBox deng_tfs_textbox = (TextBox)GetControl("deng_tfs_textbox", coolingtowerpanel);
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "PowerTFS", "", deng_tfs_textbox.Text);

            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "PowerTS", "", deng_ts_textbox.Text);


            //XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "Frequency", "", coolTower_cb.Text);

            /**********************************************************************************************/
            //插入结果配置
            /**********************************************************************************************/
            XmlHelper.Insert(xml, "/Project", "Result", "", "");
            XmlHelper.Insert(xml, "/Project/Result", "RoomTemperature", "", textBox_Temperature.Text);
            XmlHelper.Insert(xml, "/Project/Result", "SystemLoad", "", textBox_Load.Text);

        }

        private void openProject(string path)
        {
            XmlHelper xh = new XmlHelper();


            /**********************************************************************************************/
            //插入环境变量配置
            /**********************************************************************************************/
            //string path = "D:\\prj.xml";

            //建筑物制冷类型

            comboBox1.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/BuildingCooling/BuildingType", "");

            textBox1.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/BuildingCooling/BuildingArea", "");
            textBox2.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/BuildingCooling/PeakLoad", "");
            domainUpDown1.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/BuildingCooling/CoolingStart", "");
            domainUpDown2.Text = (XmlHelper.Read(path, "/Project/EnvironmentVariable/BuildingCooling/CoolingEnd", ""));
            textBox3.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/BuildingCooling/HoursPerDay", "");

            //冷冻

            textBox4.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/CoolingDesign/Freeze/InTemperature", "");
            textBox5.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/CoolingDesign/Freeze/BackTemperature", "");
            textBox6.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/BuildingCooling/TotalFlow", "");

            //冷却日温度

            textBox7.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/CoolingDesign/CoolingDay/InTemperature", "");
            textBox8.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/CoolingDesign/Freeze/OutTemperature", "");



            //冷却夜温度

            textBox9.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/CoolingDesign/CoolingNight/InTemperature", "");



            //电价配置


            comboBox2.Text = (XmlHelper.Read(path, "/Project/EnvironmentVariable/ProperPrice/ProjectPlace", ""));
            comboBox3.Text = (XmlHelper.Read(path, "/Project/EnvironmentVariable/ProperPrice/PriceType", ""));
            comboBox4.Text = (XmlHelper.Read(path, "/Project/EnvironmentVariable/ProperPrice/PriceType", ""));



            //外部环境配置

            textBox10.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/OuterEnvironment/OuterEnvironment", "");
            textBox11.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/OuterEnvironment/DayDryBallTemperature", "");
            textBox12.Text = XmlHelper.Read(path, "/Project/EnvironmentVariable/OuterEnvironment/NightDryBallTemperature", "");

            /**********************************************************************************************/
            //得到冷冻机变量
            /**********************************************************************************************/
            freezerNum.Text = XmlHelper.Read(path, "/Project/FreezeConfiguration/FreezerCount", "");
            freezerNum_TextChanged(null, null);

            string boardCount = XmlHelper.Read(path, "/Project/FreezeConfiguration/FreezerCount", "");
            if (boardCount == "1")
                checkBoxBoard.Checked = true;
            else
                checkBoxBoard.Checked = false;
            checkBoxBoard_CheckedChanged(null, null);

            for (int i = 0; i < Convert.ToInt32(freezerNum.Text); i++)
            {
                string strTypeBox = "type_box" + (i + 1);
                ComboBox temp_type_box = (ComboBox)GetControl(strTypeBox, freezer_Panel);
                temp_type_box.Text = XmlHelper.Read(path, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]/Type", "");

                string strCooling_comboBox = "cooling_comboBox" + (i + 1);
                TextBox temp_Cooling_comboBox = (TextBox)GetControl(strCooling_comboBox, freezer_Panel);
                temp_Cooling_comboBox.Text = XmlHelper.Read(path, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]/FreezePower", "");

                string strBrand_comboBox = "brand_comboBox" + (i + 1);
                ComboBox brand_comboBox = (ComboBox)GetControl(strBrand_comboBox, freezer_Panel);
                brand_comboBox.Text = XmlHelper.Read(path, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]/Brand", "");

                string strModel_box = "model_box" + (i + 1);
                TextBox temp_model_box = (TextBox)GetControl(strModel_box, freezer_Panel);
                temp_model_box.Text = XmlHelper.Read(path, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]/Model", "");

                string strIs_Frequency_Conversion_checkBox = "amount_textbox" + (i + 1);
                TextBox temp_is_Frequency_Conversion_checkBox = (TextBox)GetControl(strIs_Frequency_Conversion_checkBox, freezer_Panel);
                temp_is_Frequency_Conversion_checkBox.Text = XmlHelper.Read(path, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]/Frequency", "");
            }

            if (checkBoxBoard.Checked)
            {
                XmlHelper.Read(path, "/Project/FreezeConfiguration/Board", "");
                ComboBox temp_type_box = (ComboBox)GetControl("board_type_box", boarder_Panel);
                temp_type_box.Text = XmlHelper.Read(path, "/Project/FreezeConfiguration/Board/BoardTemperature", "");

                TextBox temp_Cooling_comboBox = (TextBox)GetControl("board_comboBox", boarder_Panel);
                temp_Cooling_comboBox.Text = XmlHelper.Read(path, "/Project/FreezeConfiguration/Board/BoardPower", "");

                ComboBox brand_comboBox = (ComboBox)GetControl("board_brand_comboBox", boarder_Panel);
                brand_comboBox.Text = XmlHelper.Read(path, "/Project/FreezeConfiguration/Board/BoardBrand", "");

                TextBox temp_model_box = (TextBox)GetControl("board_model_box", boarder_Panel);
                temp_model_box.Text = XmlHelper.Read(path, "/Project/FreezeConfiguration/Board/BoardModel", "");

                TextBox temp_is_Frequency_Conversion_checkBox = (TextBox)GetControl("board_amount_box", boarder_Panel);
                temp_is_Frequency_Conversion_checkBox.Text = XmlHelper.Read(path, "/Project/FreezeConfiguration/Board/BoardNo", "");
            }


            freezingNum.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePumpCount", "");
            coolingNum.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/CoolingPumpCount", "");
            freezingNum_TextChanged(null, null);
            coolingNum_TextChanged(null, null);
            for (int i = 0; i < Convert.ToInt32(freezingNum.Text); i++)
            {
                string strbrand_comboBox = "brand_comboBox" + (i + 1);
                ComboBox temp_brand_comboBox = (ComboBox)GetControl(strbrand_comboBox, freezingPanel);
                temp_brand_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Brand", "");


                string strflow_comboBox = "flow_comboBox" + (i + 1);
                TextBox temp_flow_comboBox = (TextBox)GetControl(strflow_comboBox, freezingPanel);
                temp_flow_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Flow", "");

                string strlift_comboBox = "lift_comboBox" + (i + 1);
                TextBox temp_lift_comboBox = (TextBox)GetControl(strlift_comboBox, freezingPanel);
                temp_lift_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Distance", "");

                string strpower_comboBox = "power_comboBox" + (i + 1);
                TextBox temp_power_comboBox = (TextBox)GetControl(strpower_comboBox, freezingPanel);
                temp_power_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Power", "");

                string strmodel_comboBox = "model_comboBox" + (i + 1);
                TextBox temp_model_comboBox = (TextBox)GetControl(strmodel_comboBox, freezingPanel);
                temp_model_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Type", "");


                string strtype_comboBox = "type_comboBox" + (i + 1);
                TextBox temp_type_comboBox = (TextBox)GetControl(strtype_comboBox, freezingPanel);
                temp_type_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Count", "");

            }
            for (int i = 0; i < Convert.ToInt32(coolingNum.Text); i++)
            {
                string strbrand_comboBox = "brand_comboBox" + (i + 1);
                ComboBox temp_brand_comboBox = (ComboBox)GetControl(strbrand_comboBox, coolingPanel);
                temp_brand_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]/Brand", "");


                string strflow_comboBox = "flow_comboBox" + (i + 1);
                TextBox temp_flow_comboBox = (TextBox)GetControl(strflow_comboBox, coolingPanel);
                temp_flow_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]/Flow", "");

                string strlift_comboBox = "lift_comboBox" + (i + 1);
                TextBox temp_lift_comboBox = (TextBox)GetControl(strlift_comboBox, coolingPanel);
                temp_lift_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]/Distance", "");

                string strpower_comboBox = "power_comboBox" + (i + 1);
                TextBox temp_power_comboBox = (TextBox)GetControl(strpower_comboBox, coolingPanel);
                temp_power_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]/Power", "");

                string strmodel_comboBox = "model_comboBox" + (i + 1);
                TextBox temp_model_comboBox = (TextBox)GetControl(strmodel_comboBox, coolingPanel);
                temp_model_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]/Type", "");


                string strtype_comboBox = "type_comboBox" + (i + 1);
                TextBox temp_type_comboBox = (TextBox)GetControl(strtype_comboBox, coolingPanel);
                temp_type_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]/Count", "");


            }


            /**********************************************************************************************/
            //插入冷却配置
            /**********************************************************************************************/
            tabPage4_Enter(null, null);

            ComboBox deng_type_combox = (ComboBox)GetControl("deng_type_combobox", coolingtowerpanel);
            deng_type_combox.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/Type", "");
            temp_CoolingTower.type_comboBox_SelectedIndexChanged(null, null);

            TextBox deng_brand_textbox = (TextBox)GetControl("deng_brand_textbox", coolingtowerpanel);
            deng_brand_textbox.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/Brand", "");

            TextBox deng_throughput_textbox = (TextBox)GetControl("deng_throughput_textbox", coolingtowerpanel);
            deng_throughput_textbox.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/Flow", "");
            TextBox deng_temperature_textbox = (TextBox)GetControl("deng_temperature_textbox", coolingtowerpanel);
            deng_temperature_textbox.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/Temperature", "");

            TextBox deng_power_textbox = (TextBox)GetControl("deng_power_textbox", coolingtowerpanel);
            deng_power_textbox.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/Power", "");

            TextBox deng_amount_textbox = (TextBox)GetControl("deng_amount_textbox", coolingtowerpanel);
            deng_amount_textbox.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/Count", "");

            TextBox deng_t1_textbox = (TextBox)GetControl("deng_t1_textbox", coolingtowerpanel);
            deng_t1_textbox.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/TF", "");

            TextBox deng_t2_textbox = (TextBox)GetControl("deng_t2_textbox", coolingtowerpanel);
            deng_t2_textbox.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/TS", "");

            TextBox deng_tfs_kw_textbox = (TextBox)GetControl("deng_tfs_kw_textbox", coolingtowerpanel);
            deng_tfs_kw_textbox.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/PFS", "");

            TextBox deng_ts_textbox = (TextBox)GetControl("deng_ts_textbox", coolingtowerpanel);
            deng_ts_textbox.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/PS", "");

            TextBox deng_tfs_textbox = (TextBox)GetControl("deng_tfs_textbox", coolingtowerpanel);
            deng_tfs_textbox.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/PowerTFS", "");
            deng_tfs_textbox.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/PowerTS", "");


            /**********************************************************************************************/
            //插入结果配置
            /**********************************************************************************************/

            textBox_Temperature.Text = XmlHelper.Read(path, "/Project/Result/RoomTemperature", "");

            textBox_Load.Text = XmlHelper.Read(path, "/Project/Result/SystemLoad", "");

        }

        private Control GetControl(string name, Panel panel)
        {
            foreach (Control c in panel.Controls)
            {
                if (c.Name == name)
                {
                    return c;
                }
            }
            return null;
        }

        private void Enerefsys_Load(object sender, EventArgs e)
        {
            Control.CheckForIllegalCrossThreadCalls = false;
            this.reportViewer1.RefreshReport();
            fullFlow = Convert.ToInt32(PumpInfoData.getFlow("2"));

            menuStrip1.RenderMode = ToolStripRenderMode.ManagerRenderMode;
            menuStrip1.Renderer = new CustomProfessionalRenderer(Color.Gray);


            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            btnTab1.BackgroundImage = Image.FromFile(root.FullName + "/Resources/huanjingbianliang2.jpg");
            btnTab2.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengshuijizu1.jpg");
            btnTab3.BackgroundImage = Image.FromFile(root.FullName + "/Resources/shuibeng1.jpg");
            btnTab4.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengqueta1.jpg");
            btnTab5.BackgroundImage = Image.FromFile(root.FullName + "/Resources/changguijieguo1.jpg");
            btnTab6.BackgroundImage = Image.FromFile(root.FullName + "/Resources/youhua1.jpg");
            btnTab7.BackgroundImage = Image.FromFile(root.FullName + "/Resources/jisuan1.jpg");
            btnTab8.BackgroundImage = Image.FromFile(root.FullName + "/Resources/baobiao1.jpg");

        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            var resultSet = RunManager.getRunResults();
            this.reportViewer1.Reset();
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Enerefsys.Report1.rdlc";
            //ReportParameter rp = new ReportParameter("content", this.textBox1.Text);
            //this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DS_Result", resultSet));
            this.reportViewer1.RefreshReport();
        }

        string workerFileName = string.Empty;
        private void btnLoadEngineData_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel|*.xls|Excel|*.xlsx|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //得到excel中的所有sheet名字，然后循环得到数据模拟表达式
                List<string> sheets = Utility.GetSheetNames(openFileDialog.FileName);

                int sheetsCount = sheets.Count();
                //int iStep = 0;
                progressBar.Maximum = sheetsCount;
                progressBar.Value = 0;

               // EngineParam ep = new EngineParam(progressBar, "vsd", openFileDialog.FileName);

                //DealData(ep);

                workerFileName = openFileDialog.FileName;
                mWorker.WorkerReportsProgress = true;
                mWorker.WorkerSupportsCancellation = true;

                mWorker.DoWork += new DoWorkEventHandler(mWorker_DoWork);
                mWorker.ProgressChanged += new ProgressChangedEventHandler(mWorker_ProgressChanged);
                mWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mWorker_RunWorkerCompleted);
                mWorker.RunWorkerAsync();
                completed.Text = "处理中...";



            }
        }
        BackgroundWorker mWorker = new BackgroundWorker();

        //private void DealData(EngineParam engineParam)
        //{
        //    BackgroundWorker mWorker = new BackgroundWorker();
        //    mWorker.WorkerReportsProgress = true;
        //    mWorker.WorkerSupportsCancellation = true;
        //    if (engineParam.ProgressBar == progressBar)
        //    {
        //        mWorker.DoWork += new DoWorkEventHandler(mWorker_DoWork);
        //        mWorker.ProgressChanged += new ProgressChangedEventHandler(mWorker_ProgressChanged);
        //        mWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mWorker_RunWorkerCompleted);
        //        mWorker.RunWorkerAsync(engineParam);
        //        completed.Text = "处理中...";
        //    }
        //    else if (engineParam.ProgressBar == progressBarPump)
        //    {
        //        mWorker.DoWork += new DoWorkEventHandler(mWorker_DoWorkPump);
        //        mWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(worker_ProgressChangedPump);
        //        mWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompletedPump);
        //        mWorker.RunWorkerAsync(engineParam);
        //        completedPump.Text = "处理中...";
        //    }


        //}


        void mWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            completed.Text = "处理完毕";
        }

        void mWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;
            completed.Text = progressBar.Value + "/" + progressBar.Maximum;
        }
        
        void mWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //BackgroundWorker worker = (BackgroundWorker)sender;
            //EngineParam ep = (EngineParam)e.Argument;
            //string fileName = ep.FileName;
            //ProgressBar progressBar = ep.ProgressBar;
            //string engineType = ep.EngineType;
            string engineType = "vsd";

            //得到excel中的所有sheet名字，然后循环得到数据模拟表达式
            //List<string> sheets = Utility.GetSheetNames(workerFileName);
            List<string> sheets = Utility.GetSheetNames(workerFileName);
            //删除所有数据
            int rc = EngineManager.DeleteByType(engineType);

            int iStep = 0;
            foreach (var sheet in sheets)
            {
                //Fit.Test test = new Fit.Test();
                //BVPF.BVPF test = new BVPF.BVPF();
                BVQF.BVQF test = new BVQF.BVQF();
                //MathWorks.MATLAB.NET.Arrays.MWArray mArray = test.MultiPolyfit(fileName, sheet);
                //MathWorks.MATLAB.NET.Arrays.MWArray mArray = test.BiVariablePolyFit(fileName, sheet);
                //MathWorks.MATLAB.NET.Arrays.MWArray mArray = test.BiVariableQuandricsFit(fileName, sheet);
                MathWorks.MATLAB.NET.Arrays.MWArray mArray = test.BiVariableQuandricsFit(workerFileName, sheet);
                MathWorks.MATLAB.NET.Arrays.MWNumericArray mmArray = mArray as MathWorks.MATLAB.NET.Arrays.MWNumericArray;
                Array array = mmArray.ToArray();
                //int ret = EngineManager.Insert((array.GetValue(0, 0)), array.GetValue(0, 1), array.GetValue(0, 2), array.GetValue(0, 3), array.GetValue(0, 4), array.GetValue(0, 5), array.GetValue(0, 6), sheet, engineType);
                int ret = EngineManager.Insert((array.GetValue(0, 0)), array.GetValue(1, 0), array.GetValue(2, 0), array.GetValue(3, 0), array.GetValue(4, 0), array.GetValue(5, 0), sheet, engineType);
                if (ret == 1)
                {
                    iStep++;
                    //worker.ReportProgress(iStep);
                    mWorker.ReportProgress(iStep);
                }
            }
        }

        BackgroundWorker mWorkerPump = new BackgroundWorker();
        private void btnLoadDataPump_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Excel|*.xls|Excel|*.xlsx|所有文件|*.*";
            openFileDialog.RestoreDirectory = true;
            openFileDialog.FilterIndex = 1;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                //得到excel中的所有sheet名字，然后循环得到数据模拟表达式
                List<string> sheets = Utility.GetSheetNames(openFileDialog.FileName);

                int sheetsCount = sheets.Count();
                progressBarPump.Maximum = sheetsCount;
                progressBarPump.Value = 0;
                //EngineParam ep = new EngineParam(progressBarPump, "", openFileDialog.FileName);

                //DealData(ep);

                workerPumpFileName = openFileDialog.FileName;
                mWorkerPump.WorkerReportsProgress = true;
                mWorkerPump.WorkerSupportsCancellation = true;

                mWorkerPump.DoWork += new DoWorkEventHandler(mWorker_DoWorkPump);
                mWorkerPump.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(worker_ProgressChangedPump);
                mWorkerPump.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompletedPump);
                mWorkerPump.RunWorkerAsync();
                completedPump.Text = "处理中...";

            }
        }


        string workerPumpFileName = string.Empty;

        void worker_RunWorkerCompletedPump(object sender, RunWorkerCompletedEventArgs e)
        {
            completedPump.Text = "处理完毕";
        }

        void worker_ProgressChangedPump(object sender, ProgressChangedEventArgs e)
        {
            progressBarPump.Value = e.ProgressPercentage;
            completedPump.Text = progressBarPump.Value + "/" + progressBarPump.Maximum;
        }

        void mWorker_DoWorkPump(object sender, DoWorkEventArgs e)
        {
            try
            {
                //BackgroundWorker worker = (BackgroundWorker)sender;
                //EngineParam ep = (EngineParam)e.Argument;
                //string fileName = ep.FileName;


                //得到excel中的所有sheet名字，然后循环得到数据模拟表达式,sheet名代表水泵类型
                //List<string> sheets = Utility.GetSheetNames(fileName);
                List<string> sheets = Utility.GetSheetNames(workerPumpFileName);
                PumpManager.DeletePump();

                int iStep = 0;
                foreach (var sheet in sheets)
                {
                    Fit.Test test = new Fit.Test();

                    //根据dll得到excel中的数据，并插入数据库
                    //MathWorks.MATLAB.NET.Arrays.MWArray mwArray = test.GetFND(fileName, sheet);
                    MathWorks.MATLAB.NET.Arrays.MWArray mwArray = test.GetFND(workerPumpFileName, sheet);
                    MathWorks.MATLAB.NET.Arrays.MWNumericArray mmwArray = mwArray as MathWorks.MATLAB.NET.Arrays.MWNumericArray;
                    Array warray = mmwArray.ToArray();
                    double[,] cc = (double[,])warray;

                    int j_count = warray.Length / 3;

                    for (int j = 0; j < j_count; j++)
                    {
                        PumpManager.InsertPumpInfo(warray.GetValue(j, 0), warray.GetValue(j, 1), sheet, warray.GetValue(j, 2));
                        //如果是2，得到满载流量
                        if (sheet.Equals("2"))
                            fullFlow = Convert.ToInt32(warray.GetValue(j, 2).ToString());
                    }
                    SVPF.SVPF svpf = new SVPF.SVPF();
                    //根据dll得到拟合出来的二次项系数
                    //MathWorks.MATLAB.NET.Arrays.MWArray mArray = svpf.SingleVariablePolyFit(fileName, sheet);
                    MathWorks.MATLAB.NET.Arrays.MWArray mArray = svpf.SingleVariablePolyFit(workerPumpFileName, sheet);
                    MathWorks.MATLAB.NET.Arrays.MWNumericArray mmArray = mArray as MathWorks.MATLAB.NET.Arrays.MWNumericArray;
                    Array array = mmArray.ToArray();
                    int ret = PumpManager.Insert((array.GetValue(0, 0)), array.GetValue(0, 1), array.GetValue(0, 2), array.GetValue(0, 3), sheet);
                    if (ret == 1)
                    {
                        iStep++;
                        mWorkerPump.ReportProgress(iStep);
                    }
                }
            }
            catch (Exception ee)
            {
                MessageBox.Show(ee.Message);
            }

        }





        //String s1 = @"../../newImages/1/1.jpg";
        //String s2 = @"../../newImages/1/2.jpg";
        //String s3 = @"../../newImages/1/3.jpg";
        //String s4 = @"../../newImages/1/4.jpg";
        //String s5 = @"../../newImages/1/6.jpg";

        //String s31 = @"../../newImages/3/0.jpg";
        //String s32 = @"../../newImages/3/1.jpg";
        //String s33 = @"../../newImages/3/2.jpg";
        //String s34 = @"../../newImages/3/4.jpg";
        //String s35 = @"../../newImages/3/a.jpg";

        //String s21 = @"../../newImages/2/1.jpg";
        //String s22 = @"../../newImages/2/2.jpg";
        //String s23 = @"../../newImages/2/3.jpg";
        //String s24 = @"../../newImages/2/e_1.jpg";
        //String s25 = @"../../newImages/2/2.jpg";

        public void initialPath()
        {
            string exePath = System.IO.Directory.GetCurrentDirectory();
            DirectoryInfo directory = new DirectoryInfo(exePath);
            dir = directory.Parent.Parent.FullName;
        }
        String dir = String.Empty;
        int count = 1;

        private void timer1_Tick(object sender, EventArgs e)
        {
            //string exePath = System.IO.Directory.GetCurrentDirectory();
            //DirectoryInfo directory = new DirectoryInfo(exePath);
            //String dir= directory.Parent.Parent.FullName;


            if (!string.IsNullOrEmpty(dir))
            {
                String s1 = dir + @"/newImages/1/1.jpg";
                String s2 = dir + @"/newImages/1/2.jpg";
                String s3 = dir + @"/newImages/1/3.jpg";
                String s4 = dir + @"/newImages/1/4.jpg";
                String s5 = dir + @"/newImages/1/6.jpg";

                String s31 = dir + @"/newImages/3/0.jpg";
                String s32 = dir + @"/newImages/3/1.jpg";
                String s33 = dir + @"/newImages/3/2.jpg";
                String s34 = dir + @"/newImages/3/4.jpg";
                String s35 = dir + @"/newImages/3/a.jpg";

                String s21 = dir + @"/newImages/2/1.jpg";
                String s22 = dir + @"/newImages/2/2.jpg";
                String s23 = dir + @"/newImages/2/3.jpg";
                String s24 = dir + @"/newImages/2/e_1.jpg";
                String s25 = dir + @"/newImages/2/4.jpg";
                String s26 = dir + @"/newImages/2/5.jpg";

                String s41 = dir + @"/newImages/4/1.jpg";
                String s42 = dir + @"/newImages/4/2.jpg";
                String s43 = dir + @"/newImages/4/3.jpg";


                if (count == 6)
                    count = 1;
                switch (count)
                {
                    case 1:
                        {

                            

                            Stream stream1 = File.Open(s1, FileMode.Open);
                            pictureBox1.Image = Image.FromStream(stream1);
                            stream1.Close();
                            Stream stream2 = File.Open(s2, FileMode.Open);
                            pictureBox2.Image = Image.FromStream(stream2);
                            stream2.Close();
   

                            Stream stream3 = File.Open(s21, FileMode.Open);
                            pictureBox3.Image = Image.FromStream(stream3);
                            stream3.Close();
                            Stream stream4 = File.Open(s22, FileMode.Open);
                            pictureBox4.Image = Image.FromStream(stream4);
                            stream4.Close();

                            Stream stream5 = File.Open(s31, FileMode.Open);
                            pictureBox5.Image = Image.FromStream(stream5);
                            stream5.Close();
                            Stream stream6 = File.Open(s32, FileMode.Open);
                            pictureBox6.Image = Image.FromStream(stream6);
                            stream6.Close();


                            Stream stream7 = File.Open(s41, FileMode.Open);
                            pictureBox7.Image = Image.FromStream(stream7);
                            stream7.Close();
                            Stream stream8 = File.Open(s42, FileMode.Open);
                            pictureBox8.Image = Image.FromStream(stream8);
                            stream8.Close();


                            count++;
                        }
                        break;
                    case 2:
                     
                        {

                            Stream stream1 = File.Open(s3, FileMode.Open);
                            pictureBox1.Image = Image.FromStream(stream1);
                            stream1.Close();
                            Stream stream2 = File.Open(s4, FileMode.Open);
                            pictureBox2.Image = Image.FromStream(stream2);
                            stream2.Close();

                            Stream stream3 = File.Open(s23, FileMode.Open);
                            pictureBox3.Image = Image.FromStream(stream3);
                            stream3.Close();
                            Stream stream4 = File.Open(s24, FileMode.Open);
                            pictureBox4.Image = Image.FromStream(stream4);
                            stream4.Close();


                            Stream stream5 = File.Open(s33, FileMode.Open);
                            pictureBox5.Image = Image.FromStream(stream5);
                            stream5.Close();
                            Stream stream6 = File.Open(s34, FileMode.Open);
                            pictureBox6.Image = Image.FromStream(stream6);
                            stream6.Close();


                            Stream stream7 = File.Open(s42, FileMode.Open);
                            pictureBox7.Image = Image.FromStream(stream7);
                            stream7.Close();
                            Stream stream8 = File.Open(s43, FileMode.Open);
                            pictureBox8.Image = Image.FromStream(stream8);
                            stream8.Close();

                            count++;
                        }
                        break;
                    case 3:
                        //pictureBox1.Image = Image.FromFile(s1);
                        //pictureBox2.Image = Image.FromFile(s5);

                        //pictureBox3.Image = Image.FromFile(s21);
                        //pictureBox4.Image = Image.FromFile(s25);

                        //pictureBox5.Image = Image.FromFile(s31);
                        //pictureBox6.Image = Image.FromFile(s35);
                        {
                            Stream stream1 = File.Open(s1, FileMode.Open);
                            pictureBox1.Image = Image.FromStream(stream1);
                            stream1.Close();
                            Stream stream2 = File.Open(s5, FileMode.Open);
                            pictureBox2.Image = Image.FromStream(stream2);
                            stream2.Close();

                            Stream stream3 = File.Open(s26, FileMode.Open);
                            pictureBox3.Image = Image.FromStream(stream3);
                            stream3.Close();
                            Stream stream4 = File.Open(s25, FileMode.Open);
                            pictureBox4.Image = Image.FromStream(stream4);
                            stream4.Close();


                            Stream stream5 = File.Open(s31, FileMode.Open);
                            pictureBox5.Image = Image.FromStream(stream5);
                            stream5.Close();
                            Stream stream6 = File.Open(s35, FileMode.Open);
                            pictureBox6.Image = Image.FromStream(stream6);
                            stream6.Close();



                            Stream stream7 = File.Open(s43, FileMode.Open);
                            pictureBox7.Image = Image.FromStream(stream7);
                            stream7.Close();
                            Stream stream8 = File.Open(s41, FileMode.Open);
                            pictureBox8.Image = Image.FromStream(stream8);
                            stream8.Close();


                            count++;
                        }

                        break;
                    case 4:
                        //pictureBox1.Image = Image.FromFile(s2);
                        //pictureBox2.Image = Image.FromFile(s1);

                        //pictureBox3.Image = Image.FromFile(s22);
                        //pictureBox4.Image = Image.FromFile(s21);

                        //pictureBox5.Image = Image.FromFile(s32);
                        //pictureBox6.Image = Image.FromFile(s31);
                        {
                            Stream stream1 = File.Open(s2, FileMode.Open);
                            pictureBox1.Image = Image.FromStream(stream1);
                            stream1.Close();
                            Stream stream2 = File.Open(s1, FileMode.Open);
                            pictureBox2.Image = Image.FromStream(stream2);
                            stream2.Close();

                            Stream stream3 = File.Open(s25, FileMode.Open);
                            pictureBox3.Image = Image.FromStream(stream3);
                            stream3.Close();
                            Stream stream4 = File.Open(s21, FileMode.Open);
                            pictureBox4.Image = Image.FromStream(stream4);
                            stream4.Close();

                            Stream stream5 = File.Open(s32, FileMode.Open);
                            pictureBox5.Image = Image.FromStream(stream5);
                            stream5.Close();
                            Stream stream6 = File.Open(s31, FileMode.Open);
                            pictureBox6.Image = Image.FromStream(stream6);
                            stream6.Close();



                            Stream stream7 = File.Open(s42, FileMode.Open);
                            pictureBox7.Image = Image.FromStream(stream7);
                            stream7.Close();
                            Stream stream8 = File.Open(s43, FileMode.Open);
                            pictureBox8.Image = Image.FromStream(stream8);
                            stream8.Close();

                            count++;
                        }
                        break;
                    case 5:
                        //pictureBox1.Image = Image.FromFile(s4);
                        //pictureBox1.Image = Image.FromFile(s5);

                        //pictureBox3.Image = Image.FromFile(s24);
                        //pictureBox4.Image = Image.FromFile(s25);

                        //pictureBox5.Image = Image.FromFile(s34);
                        //pictureBox6.Image = Image.FromFile(s35);
                        {
                            Stream stream1 = File.Open(s4, FileMode.Open);
                            pictureBox1.Image = Image.FromStream(stream1);
                            stream1.Close();
                            Stream stream2 = File.Open(s5, FileMode.Open);
                            pictureBox2.Image = Image.FromStream(stream2);
                            stream2.Close();

                            Stream stream3 = File.Open(s22, FileMode.Open);
                            pictureBox3.Image = Image.FromStream(stream3);
                            stream3.Close();
                            Stream stream4 = File.Open(s25, FileMode.Open);
                            pictureBox4.Image = Image.FromStream(stream4);
                            stream4.Close();

                            Stream stream5 = File.Open(s34, FileMode.Open);
                            pictureBox5.Image = Image.FromStream(stream5);
                            stream5.Close();
                            Stream stream6 = File.Open(s35, FileMode.Open);
                            pictureBox6.Image = Image.FromStream(stream6);
                            stream6.Close();

                            Stream stream7 = File.Open(s43, FileMode.Open);
                            pictureBox7.Image = Image.FromStream(stream7);
                            stream7.Close();
                            Stream stream8 = File.Open(s41, FileMode.Open);
                            pictureBox8.Image = Image.FromStream(stream8);
                            stream8.Close();


                            count++;
                        }
                        break;
                }
            }

        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.Text.Equals("大系统"))
            {
                //fullFlow = 1332;
                //coolingPower = 94.0d;
            }
            else if (comboBox7.Text.Equals("小系统"))
            {
                //fullFlow = 320;
                //coolingPower = 15.5d;
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textBox_Message.Text = "";
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void 出品介绍ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Description desp = new Description();
            desp.ShowDialog();
            desp.Dispose();
        }

        private void 帮助文档ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Help help = new Help();
            help.ShowDialog();
            help.Dispose();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Description_ACPE m_window = new Description_ACPE();
            m_window.ShowDialog();
            m_window.Dispose();
        }

        class EngineParam
        {
            public string EngineType { get; set; }
            public string FileName { get; set; }
            public ProgressBar ProgressBar { get; set; }
            public EngineParam(ProgressBar progressbar, string engineType, string fileName)
            {
                ProgressBar = progressbar;
                EngineType = engineType;
                FileName = fileName;
            }
        }

        bool isFlag = false;

        private void tabPage4_Enter(object sender, EventArgs e)
        {
            if (!isFlag)
            {
                int coolingtowerCount = 0;
                reponseCount += 1;
                if (reponseCount == 1 || labelFlag == 1)
                    appear_Label(label_list);
                labelFlag = 0;
                clear_CoolingTowerPanel();
                try
                {
                    coolingtowerCount = 1;
                    labelFlag += 1;
                    conceal_Label(label_list);
                    create_CoolingTower_Num(coolingtowerCount);
                    set_CoolingTower_Panel(CoolingTower_list);
                }
                catch (Exception ex)
                {
                    Console.Write("" + ex.Message);
                    MessageBox.Show("请输入正确的数据类型！");
                    return;
                }
            }
        }
        private void clear_CoolingTowerPanel()
        {
            coolingtowerpanel.Controls.Clear();
        }
        //产生冷却塔数量
        private void create_CoolingTower_Num(int coolingtowerCount)
        {
            CoolingTower_list = new List<CoolingTower>();
            for (int i = 1; i <= coolingtowerCount; i++)
            {
                CoolingTower_list.Add(new CoolingTower(i));
            }
        }
        CoolingTower temp_CoolingTower = new CoolingTower(1);
        //动态显示冷却塔
        private void set_CoolingTower_Panel(List<CoolingTower> CoolingTower_list)
        {
            foreach (CoolingTower myCoolingTower in CoolingTower_list)
            {
                //此处修改
                temp_CoolingTower = (CoolingTower)myCoolingTower;

                temp_CoolingTower.amount_textBox.Name = "deng_amount_textbox";
                coolingtowerpanel.Controls.Add(temp_CoolingTower.amount_textBox);

                temp_CoolingTower.brand_textBox.Name = "deng_brand_textbox";
                coolingtowerpanel.Controls.Add(temp_CoolingTower.brand_textBox);


                coolingtowerpanel.Controls.Add(temp_CoolingTower.LBCoolingTower);

                temp_CoolingTower.power_textBox.Name = "deng_power_textbox";
                coolingtowerpanel.Controls.Add(temp_CoolingTower.power_textBox);

                temp_CoolingTower.temperature_textBox.Name = "deng_temperature_textbox";
                coolingtowerpanel.Controls.Add(temp_CoolingTower.temperature_textBox);

                temp_CoolingTower.throughput_textBox.Name = "deng_throughput_textbox";
                coolingtowerpanel.Controls.Add(temp_CoolingTower.throughput_textBox);

                temp_CoolingTower.type_comboBox.Name = "deng_type_combobox";
                coolingtowerpanel.Controls.Add(temp_CoolingTower.type_comboBox);

                coolingtowerpanel.Controls.Add(temp_CoolingTower.lb1);
                coolingtowerpanel.Controls.Add(temp_CoolingTower.lb2);
                coolingtowerpanel.Controls.Add(temp_CoolingTower.lb3);
                coolingtowerpanel.Controls.Add(temp_CoolingTower.lb4);
                coolingtowerpanel.Controls.Add(temp_CoolingTower.lb5);
                coolingtowerpanel.Controls.Add(temp_CoolingTower.lb_Unit_HZ_B);
                coolingtowerpanel.Controls.Add(temp_CoolingTower.lb_Unit_HZ_C);
                coolingtowerpanel.Controls.Add(temp_CoolingTower.lb_Unit_KW_A);
                coolingtowerpanel.Controls.Add(temp_CoolingTower.lb_Unit_KW_B);
                coolingtowerpanel.Controls.Add(temp_CoolingTower.lb_Unit_KW_C);
                coolingtowerpanel.Controls.Add(temp_CoolingTower.tbx_HZ_A);

                temp_CoolingTower.tbx_HZ_B.Name = "deng_tfs_textbox";
                coolingtowerpanel.Controls.Add(temp_CoolingTower.tbx_HZ_B);
                coolingtowerpanel.Controls.Add(temp_CoolingTower.tbx_HZ_C);
                coolingtowerpanel.Controls.Add(temp_CoolingTower.tbx_KW_A);

                temp_CoolingTower.tbx_KW_B.Name = "deng_tfs_kw_textbox";
                coolingtowerpanel.Controls.Add(temp_CoolingTower.tbx_KW_B);

                temp_CoolingTower.tbx_KW_C.Name = "deng_ts_textbox";
                coolingtowerpanel.Controls.Add(temp_CoolingTower.tbx_KW_C);

                temp_CoolingTower.tbx_T1.Name = "deng_t1_textbox";
                coolingtowerpanel.Controls.Add(temp_CoolingTower.tbx_T1);
                temp_CoolingTower.tbx_T2.Name = "deng_t2_textbox";
                coolingtowerpanel.Controls.Add(temp_CoolingTower.tbx_T2);
                //freezer_Panel.Controls.Add(temp_SubFreezer.performance_data_box);
            }
        }
        public string strCoolingTowerStyle;
        public int iCoolingTowerT1 = 0;
        public int iCoolingTowerT2 = 0;
        public int iCoolingTowerKW = 0;
        public int iCoolingTowerHZ1 = 0;
        public int iCoolingTowerHZ2 = 0;
        public int iCoolingTowerHZ3 = 0;
        public int iCoolingTowerKW1 = 0;
        public double iCoolingTowerKW2 = 0;
        public int iCoolingTowerKW3 = 0;
        private void button8_Click(object sender, EventArgs e)
        {
            if (null != CoolingTower_list && 0 < CoolingTower_list.Count)
            {
                //ctList = getCoolingTowerTypeAndCooling(CoolingTower_list);
                //EngineWinFormEventArgs ewfe = new EngineWinFormEventArgs(meList);
                //PassDataBetweenForm(this, ewfe);
                //this.Close();
                //this.Visible = false;
                dataGridView3.Rows.Clear();
                try
                {
                    for (int ix = 0; ix < CoolingTower_list.Count; ix++)
                    {
                        for (int iy = 0; iy < Convert.ToInt32(CoolingTower_list[ix].amount_textBox.Text); iy++)
                        {
                            int index = this.dataGridView3.Rows.Add();
                            this.dataGridView3.Rows[index].Cells[0].Value = index + 1;
                            this.dataGridView3.Rows[index].Cells[1].Value = CoolingTower_list[ix].LBCoolingTower.Text;
                            this.dataGridView3.Rows[index].Cells[2].Value = CoolingTower_list[ix].brand_textBox.Text;
                            this.dataGridView3.Rows[index].Cells[3].Value = CoolingTower_list[ix].type_comboBox.Text;
                            this.dataGridView3.Rows[index].Cells[4].Value = CoolingTower_list[ix].throughput_textBox.Text;
                            this.dataGridView3.Rows[index].Cells[5].Value = CoolingTower_list[ix].temperature_textBox.Text;
                            this.dataGridView3.Rows[index].Cells[6].Value = CoolingTower_list[ix].power_textBox.Text;
                        }
                        if (CoolingTower_list[ix].type_comboBox.SelectedIndex == 0)
                        {
                            strCoolingTowerStyle = CoolingTower_list[ix].type_comboBox.Text;
                            iCoolingTowerKW = Convert.ToInt32(CoolingTower_list[ix].power_textBox.Text);
                        }
                        else if (CoolingTower_list[ix].type_comboBox.SelectedIndex == 1)
                        {
                            strCoolingTowerStyle = CoolingTower_list[ix].type_comboBox.Text;
                            iCoolingTowerT1 = Convert.ToInt32(CoolingTower_list[ix].tbx_T1.Text);
                            iCoolingTowerT2 = Convert.ToInt32(CoolingTower_list[ix].tbx_T2.Text);
                            iCoolingTowerKW2 = Convert.ToInt32(CoolingTower_list[ix].tbx_KW_B.Text);
                            iCoolingTowerKW3 = Convert.ToInt32(CoolingTower_list[ix].tbx_KW_C.Text);

                        }
                        else if (CoolingTower_list[ix].type_comboBox.SelectedIndex == 2)
                        {
                            strCoolingTowerStyle = CoolingTower_list[ix].type_comboBox.Text;
                            iCoolingTowerT1 = Convert.ToInt32(CoolingTower_list[ix].tbx_T1.Text);
                            iCoolingTowerT2 = Convert.ToInt32(CoolingTower_list[ix].tbx_T2.Text);
                            iCoolingTowerHZ2 = Convert.ToInt32(CoolingTower_list[ix].tbx_HZ_B.Text);
                            iCoolingTowerHZ3 = Convert.ToInt32(CoolingTower_list[ix].tbx_HZ_C.Text);
                            iCoolingTowerKW3 = Convert.ToInt32(CoolingTower_list[ix].tbx_KW_C.Text);
                            double d = Convert.ToDouble(iCoolingTowerHZ2) / Convert.ToDouble(iCoolingTowerHZ3);
                            //iCoolingTowerKW2 = Convert.ToInt32(d * d * d * iCoolingTowerKW3);
                            iCoolingTowerKW2 = d * d * d * iCoolingTowerKW3;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    dataGridView1.Rows.Clear();
                }
            }
            else
                MessageBox.Show("请输入数据");
        }
        private void CoolingTower_Load(object sender, EventArgs e)
        {

            this.dataGridView3.Columns.Add("No", "编号");

            this.dataGridView3.Columns.Add("Name", "机器名");

            this.dataGridView3.Columns.Add("FebWin", "品牌");

            this.dataGridView3.Columns.Add("FebLoss", "类型");

            this.dataGridView3.Columns.Add("MarWin", "流量");

            this.dataGridView3.Columns.Add("MarLoss1", "温度");

            this.dataGridView3.Columns.Add("MarLoss2", "功率");

            for (int j = 0; j < this.dataGridView3.ColumnCount; j++)
            {
                this.dataGridView3.Columns[j].Width = 107;
            }

            this.dataGridView3.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            this.dataGridView3.ColumnHeadersHeight = this.dataGridView3.ColumnHeadersHeight * 1;

            this.dataGridView3.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

        }

        private void btnCal_Click(object sender, EventArgs e)
        {
            var standardLoads = RunManager.getAllStandardLoads();
            RunManager.deleteallRunResults();
            foreach (var standardLoad in standardLoads)
            {

                if (standardLoad.Load <= 0)
                {
                    RunManager.InsertIntoRunResult(standardLoad, 0, 0);
                    continue;
                }

                meMin.Clear();
                minResult = double.MaxValue;
                minSolute = double.MaxValue;
                percentValue = 0;
                IsNormal = false;
                IsSwap = false;
                FreezeType = "一对一";
                CoolingType = "一对一";
                //温差设置为5度
                TemperRange = 5;

                double temperature = standardLoad.WetTemperature + Kvalue;
                if (temperature < downTemperature)
                    IsBoard = true;
                else
                    IsBoard = false;


                string type = strCoolingTowerStyle;
                if (type.Equals("常规"))
                {
                    coolingPower = iCoolingTowerKW;
                }
                else if (type.Equals("高低速"))
                {
                    coolingPower = iCoolingTowerKW2;
                    if (iCoolingTowerT1 > temperature)
                        coolingPower = 0;
                    if (iCoolingTowerT2 < temperature)
                        coolingPower = iCoolingTowerKW3;
                }
                else if (type.Equals("变频"))
                {
                    coolingPower = iCoolingTowerKW2;
                    if (iCoolingTowerT1 > temperature)
                        coolingPower = 0;
                    if (iCoolingTowerT2 < temperature)
                        coolingPower = iCoolingTowerKW3;
                }

                //coolingPower = Convert.ToInt32(textBox_CoolingPower.Text.ToString());
                GetOptimizationResult(meList, standardLoad.Load, standardLoad.WetTemperature + Kvalue);


                //如果是板换，则散热塔的功率等于板换数量
                if (IsBoard)
                    coolingPower = coolingPower * BoardCount;
                else
                {
                    coolingPower = coolingPower * meMin.Count;
                }

                minResult = enginePower + freezePumpPower + lengquePower + coolingPower;

                //GetOptimizationResult(meList, standardLoad.Load, standardLoad.WetTemperature + Kvalue);

                RunManager.InsertIntoRunResult(standardLoad, minResult, minResult * standardLoad.ElectronicPrice);
            }
        }

        private void pictureBox_Result_Click(object sender, EventArgs e)
        {

        }

        private void tabPage7_Click(object sender, EventArgs e)
        {

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            var resultSet = RunManager.getRunResultsByCon(comboBox_Month.Text,
                comboBox_Day.Text,
                comboBox_Hour.Text,
                textBox_Dry.Text,
                textBox_Wet.Text,
                textBox_CoolLoad.Text,
                textBox_Temperature.Text,
                textBox_Power.Text,
                textBox_Price.Text,
                textBox_TotalPrice.Text);
            this.reportViewer1.Reset();
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Enerefsys.Report1.rdlc";
            //ReportParameter rp = new ReportParameter("content", this.textBox1.Text);
            //this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DS_Result", resultSet));
            this.reportViewer1.RefreshReport();
        }

        private void btnShowOptim_Click(object sender, EventArgs e)
        {
            var resultSet = RunManager.getRunResults();
            rowUnitView1.AutoGenerateColumns = false;
            rowUnitView1.DataSource = resultSet;
        }

        /// <summary>
        /// 汇报计算情况的工作者线程
        /// </summary>
        BackgroundWorker workerCal = new BackgroundWorker();
        BackgroundWorker workerCalOptim = new BackgroundWorker();


        private void btnNormalCal_Click(object sender, EventArgs e)
        {

            var standardLoads = RunManager.getAllStandardLoads();

            progressBar1.Maximum = standardLoads.Count;

            workerCal.WorkerReportsProgress = true;

            //正式做事情的地方
            workerCal.DoWork += new DoWorkEventHandler(workerCal_DoWork);

            //任务完称时要做的，比如提示等等
            workerCal.ProgressChanged += new ProgressChangedEventHandler(workerCal_ProgressChanged);

            workerCal.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerCal_RunWorkerCompleted);

            workerCal.RunWorkerAsync();
        }

        void workerCal_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("计算完成！");
        }

        void workerCal_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar1.Value = e.ProgressPercentage;
            //将异步任务进度的百分比赋给进度条
        }

        void workerCal_DoWork(object sender, DoWorkEventArgs e)
        {
            var standardLoads = RunManager.getAllStandardLoads();
            RunManager.deleteallNormalRunResults();
            int i = 0;
            foreach (var standardLoad in standardLoads)
            {
                i++;
                if (standardLoad.Load <= 0)
                {
                    RunManager.InsertIntoNormalRunResult(standardLoad, 0, 0);
                    workerCal.ReportProgress(i);
                    label60.Text = i.ToString() + "/" + standardLoads.Count;
                    continue;
                }

                meMin.Clear();
                minResult = double.MaxValue;
                minSolute = double.MaxValue;
                percentValue = 0;
                IsNormal = true;
                IsSwap = false;
                FreezeType = "一对一";
                CoolingType = "一对一";
                //温差设置为5度
                TemperRange = 5;

                double temperature = standardLoad.WetTemperature + Kvalue;
                if (temperature < downTemperature)
                    IsBoard = true;
                else
                    IsBoard = false;


                string type = strCoolingTowerStyle;
                if (type.Equals("常规"))
                {
                    coolingPower = iCoolingTowerKW;
                }
                else if (type.Equals("高低速"))
                {
                    coolingPower = iCoolingTowerKW2;
                    if (iCoolingTowerT1 > temperature)
                        coolingPower = 0;
                    if (iCoolingTowerT2 < temperature)
                        coolingPower = iCoolingTowerKW3;
                }
                else if (type.Equals("变频"))
                {
                    coolingPower = iCoolingTowerKW2;
                    if (iCoolingTowerT1 > temperature)
                        coolingPower = 0;
                    if (iCoolingTowerT2 < temperature)
                        coolingPower = iCoolingTowerKW3;
                }

                //coolingPower = Convert.ToInt32(textBox_CoolingPower.Text.ToString());
                GetOptimizationResult(meList, standardLoad.Load, standardLoad.WetTemperature + Kvalue);


                //如果是板换，则散热塔的功率等于板换数量
                if (IsBoard)
                    coolingPower = coolingPower * BoardCount;
                else
                {
                    coolingPower = coolingPower * meMin.Count;
                }

                minResult = enginePower + freezePumpPower + lengquePower + coolingPower;

                RunManager.InsertIntoNormalRunResult(standardLoad, minResult, minResult * standardLoad.ElectronicPrice);


                //工作者回发报告进度信息
                workerCal.ReportProgress(i);
                label60.Text = i.ToString() + "/" + standardLoads.Count;
            }
        }

        private void btnNormalShow_Click(object sender, EventArgs e)
        {
            var resultSet = RunManager.getNormalRunResults();
            rowUnitView2.AutoGenerateColumns = false;
            rowUnitView2.DataSource = resultSet;
        }


        //优化计算
        private void btnCalOptim_Click(object sender, EventArgs e)
        {
            var standardLoads = RunManager.getAllStandardLoads();

            progressBar2.Maximum = standardLoads.Count;

            workerCalOptim.WorkerReportsProgress = true;

            //正式做事情的地方
            workerCalOptim.DoWork += new DoWorkEventHandler(workerCalOptim_DoWork);

            //任务完称时要做的，比如提示等等
            workerCalOptim.ProgressChanged += new ProgressChangedEventHandler(workerCalOptim_ProgressChanged);

            workerCalOptim.RunWorkerCompleted += new RunWorkerCompletedEventHandler(workerCalOptim_RunWorkerCompleted);

            workerCalOptim.RunWorkerAsync();
        }

        void workerCalOptim_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            MessageBox.Show("计算完成！");
        }

        void workerCalOptim_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar2.Value = e.ProgressPercentage;
            //将异步任务进度的百分比赋给进度条
        }

        void workerCalOptim_DoWork(object sender, DoWorkEventArgs e)
        {
            var standardLoads = RunManager.getAllStandardLoads();
            RunManager.deleteallRunResults();
            int i = 0;
            foreach (var standardLoad in standardLoads)
            {
                i++;
                if (standardLoad.Load <= 0)
                {
                    RunManager.InsertIntoRunResult(standardLoad, 0, 0);
                    workerCalOptim.ReportProgress(i);
                    label62.Text = i.ToString() + "/" + standardLoads.Count;
                    continue;
                }

                meMin.Clear();
                minResult = double.MaxValue;
                minSolute = double.MaxValue;
                percentValue = 0;
                IsNormal = false;
                IsSwap = false;
                FreezeType = "一对一";
                CoolingType = "一对一";
                //温差设置为5度
                TemperRange = 5;

                double temperature = standardLoad.WetTemperature + Kvalue;
                if (temperature < downTemperature)
                    IsBoard = true;
                else
                    IsBoard = false;


                string type = strCoolingTowerStyle;
                if (type.Equals("常规"))
                {
                    coolingPower = iCoolingTowerKW;
                }
                else if (type.Equals("高低速"))
                {
                    coolingPower = iCoolingTowerKW2;
                    if (iCoolingTowerT1 > temperature)
                        coolingPower = 0;
                    if (iCoolingTowerT2 < temperature)
                        coolingPower = iCoolingTowerKW3;
                }
                else if (type.Equals("变频"))
                {
                    coolingPower = iCoolingTowerKW2;
                    if (iCoolingTowerT1 > temperature)
                        coolingPower = 0;
                    if (iCoolingTowerT2 < temperature)
                        coolingPower = iCoolingTowerKW3;
                }

                //coolingPower = Convert.ToInt32(textBox_CoolingPower.Text.ToString());
                GetOptimizationResult(meList, standardLoad.Load, standardLoad.WetTemperature + Kvalue);


                //如果是板换，则散热塔的功率等于板换数量
                if (IsBoard)
                    coolingPower = coolingPower * BoardCount;
                else
                {
                    coolingPower = coolingPower * meMin.Count;
                }

                minResult = enginePower + freezePumpPower + lengquePower + coolingPower;

                //插入到优化结果列表中
                RunManager.InsertIntoRunResult(standardLoad, minResult, minResult * standardLoad.ElectronicPrice);


                //工作者回发报告进度信息
                workerCalOptim.ReportProgress(i);
                label62.Text = i.ToString() + "/" + standardLoads.Count;
            }
        }





        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataGridViewToExcel(rowUnitView1);
        }




        public void DataGridViewToExcel(DataGridView dgv)
        {

            //申明保存对话框   
            SaveFileDialog dlg = new SaveFileDialog();
            //默然文件后缀   
            dlg.DefaultExt = "xls ";
            //文件后缀列表   
            dlg.Filter = "EXCEL文件(*.XLS)|*.xls ";
            //默然路径是系统当前路径   
            dlg.InitialDirectory = Directory.GetCurrentDirectory();
            //打开保存对话框   
            if (dlg.ShowDialog() == DialogResult.Cancel) return;
            //返回文件路径   
            string fileNameString = dlg.FileName;
            //验证strFileName是否为空或值无效   
            if (fileNameString.Trim() == " ")
            { return; }
            //定义表格内数据的行数和列数   
            int rowscount = dgv.Rows.Count;
            int colscount = dgv.Columns.Count;
            //行数必须大于0   
            if (rowscount <= 0)
            {
                MessageBox.Show("没有数据可供保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //列数必须大于0   
            if (colscount <= 0)
            {
                MessageBox.Show("没有数据可供保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //行数不可以大于65536   
            if (rowscount > 65536)
            {
                MessageBox.Show("数据记录数太多(最多不能超过65536条)，不能保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //列数不可以大于255   
            if (colscount > 255)
            {
                MessageBox.Show("数据记录行数太多，不能保存 ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //验证以fileNameString命名的文件是否存在，如果存在删除它   
            FileInfo file = new FileInfo(fileNameString);
            if (file.Exists)
            {
                try
                {
                    file.Delete();
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message, "删除失败 ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }
            Excel.Application objExcel = null;
            Excel.Workbook objWorkbook = null;
            Excel.Worksheet objsheet = null;
            try
            {
                //申明对象   
                objExcel = new Microsoft.Office.Interop.Excel.Application();
                objWorkbook = objExcel.Workbooks.Add(Missing.Value);
                objsheet = (Excel.Worksheet)objWorkbook.ActiveSheet;
                //设置EXCEL不可见   
                objExcel.Visible = false;

                //向Excel中写入表格的表头   
                int displayColumnsCount = 1;
                for (int i = 0; i <= dgv.ColumnCount - 1; i++)
                {
                    if (dgv.Columns[i].Visible == true)
                    {
                        objExcel.Cells[1, displayColumnsCount] = dgv.Columns[i].HeaderText.Trim();
                        displayColumnsCount++;
                    }
                }
                //设置进度条   
                //tempProgressBar.Refresh();   
                //tempProgressBar.Visible   =   true;   
                //tempProgressBar.Minimum=1;   
                //tempProgressBar.Maximum=dgv.RowCount;   
                //tempProgressBar.Step=1;   
                //向Excel中逐行逐列写入表格中的数据   
                for (int row = 0; row <= dgv.RowCount - 1; row++)
                {
                    //tempProgressBar.PerformStep();   

                    displayColumnsCount = 1;
                    for (int col = 0; col < colscount; col++)
                    {
                        if (dgv.Columns[col].Visible == true)
                        {
                            try
                            {
                                objExcel.Cells[row + 2, displayColumnsCount] = dgv.Rows[row].Cells[col].Value.ToString().Trim();
                                displayColumnsCount++;
                            }
                            catch (Exception)
                            {

                            }

                        }
                    }
                }
                //隐藏进度条   
                //tempProgressBar.Visible   =   false;   
                //保存文件   
                objWorkbook.SaveAs(fileNameString, Missing.Value, Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Excel.XlSaveAsAccessMode.xlShared, Missing.Value, Missing.Value, Missing.Value,
                        Missing.Value, Missing.Value);
            }
            catch (Exception error)
            {
                MessageBox.Show(error.Message, "警告 ", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            finally
            {
                //关闭Excel应用   
                if (objWorkbook != null) objWorkbook.Close(Missing.Value, Missing.Value, Missing.Value);
                if (objExcel.Workbooks != null) objExcel.Workbooks.Close();
                if (objExcel != null) objExcel.Quit();

                objsheet = null;
                objWorkbook = null;
                objExcel = null;
            }
            MessageBox.Show(fileNameString + "\n\n导出完毕! ", "提示 ", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void button6_Click(object sender, EventArgs e)
        {
            DataGridViewToExcel(rowUnitView2);
        }

        private bool isFirst = true;
        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {

            try
            {
                System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
                System.IO.DirectoryInfo root = directory.Parent.Parent;

                //this.skinEngine1.SkinFile = root.FullName + "/Resources/Calmness.ssk";
                string imagePath = root.FullName + "/Resources/huanjingbianliang.jpg";
                //tabPage标签图片
                switch (e.Index)
                {
                    case 0:
                        imagePath = root.FullName + "/Resources/huanjingbianliang.jpg";
                        break;
                    case 1:
                        imagePath = root.FullName + "/Resources/lengshuijizu.jpg";
                        break;
                    case 2:
                        imagePath = root.FullName + "/Resources/shuibeng.jpg";
                        break;
                    case 3:
                        imagePath = root.FullName + "/Resources/lengqueta.jpg";
                        break;
                    case 4:
                        imagePath = root.FullName + "/Resources/changgui.jpg";
                        break;
                    case 5:
                        imagePath = root.FullName + "/Resources/youhua.jpg";
                        break;
                    case 6:
                        imagePath = root.FullName + "/Resources/jisuan.jpg";
                        break;
                    case 7:
                        imagePath = root.FullName + "/Resources/baobiao.jpg";
                        break;

                }

                Bitmap image = new Bitmap(imagePath);
                if (isFirst)
                {
                    Bitmap backimage = new Bitmap(root.FullName + "/Resources/beijing.png");
                    e.Graphics.DrawImage(backimage, 0, 0, tabControl1.Width, tabControl1.Height);
                    isFirst = false;
                }

                //e.Graphics.DrawImage(backimage, 0, 0, tabControl1.Width, tabControl1.Height); 

                Rectangle myTabRect = this.tabControl1.GetTabRect(e.Index);

                ////=============================================
                //使用图片
                Bitmap bt = new Bitmap(image);
                Point p5 = new Point(myTabRect.X, myTabRect.Y);
                e.Graphics.DrawImage(bt, p5);
                //先添加TabPage属性   
                //新建一个StringFormat对象，用于对标签文字的布局设置 

                //StringFormat StrFormat = new StringFormat();
                //StrFormat.Alignment = StringAlignment.Center;// 设置文字水平方向居中     
                //StrFormat.LineAlignment = StringAlignment.Far;

                //e.Graphics.DrawString(tabControl1.TabPages[e.Index].Text, this.Font, SystemBrushes.ControlText, myTabRect, StrFormat);
                e.Graphics.Dispose();
            }
            catch (Exception)
            { }
        }

        private void tabControl1_Resize(object sender, EventArgs e)
        {
            isFirst = true;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;

            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            btnTab1.BackgroundImage = Image.FromFile(root.FullName + "/Resources/huanjingbianliang2.jpg");
            btnTab2.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengshuijizu1.jpg");
            btnTab3.BackgroundImage = Image.FromFile(root.FullName + "/Resources/shuibeng1.jpg");
            btnTab4.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengqueta1.jpg");
            btnTab5.BackgroundImage = Image.FromFile(root.FullName + "/Resources/changguijieguo1.jpg");
            btnTab6.BackgroundImage = Image.FromFile(root.FullName + "/Resources/youhua1.jpg");
            btnTab7.BackgroundImage = Image.FromFile(root.FullName + "/Resources/jisuan1.jpg");
            btnTab8.BackgroundImage = Image.FromFile(root.FullName + "/Resources/baobiao1.jpg");

        }

        private void btnTab2_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;

            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            btnTab1.BackgroundImage = Image.FromFile(root.FullName + "/Resources/huanjingbianliang1.jpg");
            btnTab2.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengshuijizu2.jpg");
            btnTab3.BackgroundImage = Image.FromFile(root.FullName + "/Resources/shuibeng1.jpg");
            btnTab4.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengqueta1.jpg");
            btnTab5.BackgroundImage = Image.FromFile(root.FullName + "/Resources/changguijieguo1.jpg");
            btnTab6.BackgroundImage = Image.FromFile(root.FullName + "/Resources/youhua1.jpg");
            btnTab7.BackgroundImage = Image.FromFile(root.FullName + "/Resources/jisuan1.jpg");
            btnTab8.BackgroundImage = Image.FromFile(root.FullName + "/Resources/baobiao1.jpg");
        }

        private void btnTab3_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 2;
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            btnTab1.BackgroundImage = Image.FromFile(root.FullName + "/Resources/huanjingbianliang1.jpg");
            btnTab2.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengshuijizu1.jpg");
            btnTab3.BackgroundImage = Image.FromFile(root.FullName + "/Resources/shuibeng2.jpg");
            btnTab4.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengqueta1.jpg");
            btnTab5.BackgroundImage = Image.FromFile(root.FullName + "/Resources/changguijieguo1.jpg");
            btnTab6.BackgroundImage = Image.FromFile(root.FullName + "/Resources/youhua1.jpg");
            btnTab7.BackgroundImage = Image.FromFile(root.FullName + "/Resources/jisuan1.jpg");
            btnTab8.BackgroundImage = Image.FromFile(root.FullName + "/Resources/baobiao1.jpg");
        }

        private void btnTab4_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 3;
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            btnTab1.BackgroundImage = Image.FromFile(root.FullName + "/Resources/huanjingbianliang1.jpg");
            btnTab2.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengshuijizu1.jpg");
            btnTab3.BackgroundImage = Image.FromFile(root.FullName + "/Resources/shuibeng1.jpg");
            btnTab4.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengqueta2.jpg");
            btnTab5.BackgroundImage = Image.FromFile(root.FullName + "/Resources/changguijieguo1.jpg");
            btnTab6.BackgroundImage = Image.FromFile(root.FullName + "/Resources/youhua1.jpg");
            btnTab7.BackgroundImage = Image.FromFile(root.FullName + "/Resources/jisuan1.jpg");
            btnTab8.BackgroundImage = Image.FromFile(root.FullName + "/Resources/baobiao1.jpg");
        }

        private void btnTab5_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 4;
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            btnTab1.BackgroundImage = Image.FromFile(root.FullName + "/Resources/huanjingbianliang1.jpg");
            btnTab2.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengshuijizu1.jpg");
            btnTab3.BackgroundImage = Image.FromFile(root.FullName + "/Resources/shuibeng1.jpg");
            btnTab4.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengqueta1.jpg");
            btnTab5.BackgroundImage = Image.FromFile(root.FullName + "/Resources/changguijieguo2.jpg");
            btnTab6.BackgroundImage = Image.FromFile(root.FullName + "/Resources/youhua1.jpg");
            btnTab7.BackgroundImage = Image.FromFile(root.FullName + "/Resources/jisuan1.jpg");
            btnTab8.BackgroundImage = Image.FromFile(root.FullName + "/Resources/baobiao1.jpg");
        }

        private void btnTab6_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 5;
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            btnTab1.BackgroundImage = Image.FromFile(root.FullName + "/Resources/huanjingbianliang1.jpg");
            btnTab2.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengshuijizu1.jpg");
            btnTab3.BackgroundImage = Image.FromFile(root.FullName + "/Resources/shuibeng1.jpg");
            btnTab4.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengqueta1.jpg");
            btnTab5.BackgroundImage = Image.FromFile(root.FullName + "/Resources/changguijieguo1.jpg");
            btnTab6.BackgroundImage = Image.FromFile(root.FullName + "/Resources/youhua2.jpg");
            btnTab7.BackgroundImage = Image.FromFile(root.FullName + "/Resources/jisuan1.jpg");
            btnTab8.BackgroundImage = Image.FromFile(root.FullName + "/Resources/baobiao1.jpg");
        }

        private void btnTab7_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 6;
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            btnTab1.BackgroundImage = Image.FromFile(root.FullName + "/Resources/huanjingbianliang1.jpg");
            btnTab2.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengshuijizu1.jpg");
            btnTab3.BackgroundImage = Image.FromFile(root.FullName + "/Resources/shuibeng1.jpg");
            btnTab4.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengqueta1.jpg");
            btnTab5.BackgroundImage = Image.FromFile(root.FullName + "/Resources/changguijieguo1.jpg");
            btnTab6.BackgroundImage = Image.FromFile(root.FullName + "/Resources/youhua1.jpg");
            btnTab7.BackgroundImage = Image.FromFile(root.FullName + "/Resources/jisuan2.jpg");
            btnTab8.BackgroundImage = Image.FromFile(root.FullName + "/Resources/baobiao1.jpg");
        }

        private void btnTab8_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 7;
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            btnTab1.BackgroundImage = Image.FromFile(root.FullName + "/Resources/huanjingbianliang1.jpg");
            btnTab2.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengshuijizu1.jpg");
            btnTab3.BackgroundImage = Image.FromFile(root.FullName + "/Resources/shuibeng1.jpg");
            btnTab4.BackgroundImage = Image.FromFile(root.FullName + "/Resources/lengqueta1.jpg");
            btnTab5.BackgroundImage = Image.FromFile(root.FullName + "/Resources/changguijieguo1.jpg");
            btnTab6.BackgroundImage = Image.FromFile(root.FullName + "/Resources/youhua1.jpg");
            btnTab7.BackgroundImage = Image.FromFile(root.FullName + "/Resources/jisuan1.jpg");
            btnTab8.BackgroundImage = Image.FromFile(root.FullName + "/Resources/baobiao2.jpg");
        }

       




        /// <summary>
        /// 添加按钮上鼠标 移入、移出、按下、弹起 事件
        /// </summary>
        /// <param name="btn">操作的按钮</param>
        //private void AddBtnEvent(Button btn)
        //{
        //    btn.MouseEnter += delegate(object sender, EventArgs e)
        //    {
        //        ((Button)sender).BackgroundImage = Resources.btnMove;
        //    };
        //    btn.MouseLeave += delegate(object sender, EventArgs e)
        //    {
        //        ((Button)sender).BackgroundImage = Resources.btnNormal;
        //    };
        //    btn.MouseDown += delegate(object sender, MouseEventArgs e)
        //    {
        //        ((Button)sender).BackgroundImage = Resources.btnClick;
        //    };
        //    btn.MouseUp += delegate(object sender, MouseEventArgs e)
        //    {
        //        ((Button)sender).BackgroundImage = Resources;
        //    };
        //}




    }
}




