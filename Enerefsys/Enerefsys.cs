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

namespace Enerefsys
{
    public partial class Enerefsys : Form
    {

        public Enerefsys()
        {
            InitializeComponent();

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

            List<string> results = PumpManager.GetPumpTypes();
            foreach (var result in results)
            {
                comboBox_PumpType.Items.Add(result);
            }
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
        public List<MachineEntity> meList { get; set; }//获得冷冻值列表（类型和冷量）
        public List<MachineEntity> bhList { get; set; }//获得版换值列表（类型和冷量）
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
                else if (null == boarderNum.Text.ToString().Trim() || "" == boarderNum.Text.ToString().Trim())
                {
                    labelFlag += 1;
                    conceal_Label(label_list);
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
            foreach (SubFreezer sub_freezer in subFreezer_list)
            {
                SubFreezer temp_SubFreezer = (SubFreezer)sub_freezer;
                freezer_Panel.Controls.Add(temp_SubFreezer.freazer);
                freezer_Panel.Controls.Add(temp_SubFreezer.type_box);
                freezer_Panel.Controls.Add(temp_SubFreezer.cooling_comboBox);
                freezer_Panel.Controls.Add(temp_SubFreezer.brand_comboBox);
                freezer_Panel.Controls.Add(temp_SubFreezer.model_box);
                freezer_Panel.Controls.Add(temp_SubFreezer.amount_textBox);
                //freezer_Panel.Controls.Add(temp_SubFreezer.performance_data_box);
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
        private void boarderNum_TextChanged(object sender, EventArgs e)
        {
            int boarderCount = 0;
            reponseCount += 1;
            if (reponseCount == 1 || labelFlag == 1)
                appear_Label(label_list);
            labelFlag = 0;
            clear_Panel2();
            try
            {
                if (null != boarderNum.Text.ToString().Trim() && "" != boarderNum.Text.ToString().Trim())
                {
                    boarderCount = Int32.Parse(boarderNum.Text.ToString());
                }
                else if (null == freezerNum.Text.ToString().Trim() || "" == freezerNum.Text.ToString().Trim())
                {
                    labelFlag += 1;
                    conceal_Label(label_list);
                    return;
                }
            }
            catch (Exception e1)
            {
                Console.Write("" + e1.Message);
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
                boarder_Panel.Controls.Add(temp_SubBoarder.type_box);
                boarder_Panel.Controls.Add(temp_SubBoarder.cooling_comboBox);
                boarder_Panel.Controls.Add(temp_SubBoarder.brand_comboBox);
                boarder_Panel.Controls.Add(temp_SubBoarder.model_box);
                boarder_Panel.Controls.Add(temp_SubBoarder.amount_textBox);
                //freezer_Panel.Controls.Add(temp_SubFreezer.performance_data_box);
            }
        }

        private List<MachineEntity> getBoarderTypeAndCooling(List<SubBoarder> subBoarder_list)
        {
            int tempBoarderNum;
            List<MachineEntity> machineList = new List<MachineEntity>();
            if (null != boarderNum.Text.ToString().Trim() && "" != boarderNum.Text.ToString().Trim() && 0 < subBoarder_list.Count)
            {
                tempBoarderNum = Int32.Parse(boarderNum.Text.ToString().Trim());
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
                                MachineEntity me = new MachineEntity("板换" + i, sub_Boarder.addition.Text.ToString() + sub_Boarder.type_box.Text.ToString(), Convert.ToDouble(sub_Boarder.cooling_comboBox.Text.ToString().Trim()));
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
        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (null != subFreezer_list && 0 < subFreezer_list.Count)
            {
                meList = getFreezerTypeAndCooling(subFreezer_list);
                bhList = getBoarderTypeAndCooling(subBoarder_list);
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
                tempPanel.Controls.Add(waterPump_SubFreezer.brand_comboBox);
                tempPanel.Controls.Add(waterPump_SubFreezer.flow_textBox);
                tempPanel.Controls.Add(waterPump_SubFreezer.lift_textBox);
                tempPanel.Controls.Add(waterPump_SubFreezer.power_textBox);
                tempPanel.Controls.Add(waterPump_SubFreezer.model_textBox);
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
                WaterFreezer temp_SubFreezer = (WaterFreezer)sub_freezer;
                if (tempPanel.Name.Equals("freezingPanel"))
                    temp_SubFreezer.freazerAndcooler.Text = "冷冻水泵" + j;
                else
                {
                    temp_SubFreezer.freazerAndcooler.Text = "冷却水泵" + j;
                }
                tempPanel.Controls.Add(temp_SubFreezer.freazerAndcooler);
                tempPanel.Controls.Add(temp_SubFreezer.brand_comboBox);
                tempPanel.Controls.Add(temp_SubFreezer.flow_textBox);
                tempPanel.Controls.Add(temp_SubFreezer.lift_textBox);
                tempPanel.Controls.Add(temp_SubFreezer.power_textBox);
                tempPanel.Controls.Add(temp_SubFreezer.model_textBox);
                tempPanel.Controls.Add(temp_SubFreezer.amount_textBox);

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
                else
                {
                    waterPump_labelFlag += 1;
                    waterPump_conceal_Label(label_list);
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
                else
                {
                    waterPump_labelFlag += 1;
                    waterPump_conceal_Label(label_list);
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
                                Convert.ToInt32(waterPump_subFreezer_list[ix].flow_textBox.Text);
                                Convert.ToInt32(waterPump_subFreezer_list[ix].lift_textBox.Text);
                                Convert.ToInt32(waterPump_subFreezer_list[ix].power_textBox.Text);
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
                                Convert.ToInt32(waterPump_subCooler_list[ix].flow_textBox.Text);
                                Convert.ToInt32(waterPump_subCooler_list[ix].lift_textBox.Text);
                                Convert.ToInt32(waterPump_subCooler_list[ix].power_textBox.Text);
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


        ////得到一定温度下，一台主机的能耗的函数
        //private List<double> getFormulaByEntity(string type, double load, double temperature)
        //{
        //    //得到一定温度下，特定类型和特定负荷下的主机能耗关于流量的函数的二次系数
        //    List<double> results = EngineManager.GetParamsByType(temperature, load, type);
        //    return results;
        //}
        ////在一定温度下得到一个主机组合的最低能耗，及对应的流量
        //private SoluteResult getMinByConsist(List<MachineEntity> mes, double temperature, double load)
        //{
        //    /***********************************************************************************/
        //    //主机功率的计算公式
        //    /***********************************************************************************/
        //    double a = 0;
        //    double b = 0;
        //    double c = 0;
        //    double sumLoad2 = 0;
        //    foreach (var me in mes)
        //    {
        //        sumLoad2 += me.Value;
        //    }
        //    //求得每台主机的负荷率，每台主机运行的负荷除以总负荷相等
        //    double percentValue1 = load / sumLoad2;

        //    foreach (var me in mes)
        //    {
        //        //得到每台特定类型的主机在一定温度，一定负荷下的关于流量的二次项系数
        //        List<double> results = getFormulaByEntity(me.Type, me.Value * percentValue1, temperature);
        //        a += results[0];
        //        b += results[1];
        //        c += results[2];
        //    }
        //    //string pumpType = PumpType.Text;
        //    int engineCount = mes.Count;

        //    /***********************************************************************************/
        //    //冷却水泵的计算公式
        //    /***********************************************************************************/
        //    if (CoolingType.Equals("并联"))
        //    {
        //        double constantNumber = fullFlow * engineCount;
        //        double constantNumber2 = constantNumber * constantNumber;
        //        //从数据库得到二次项系数
        //        List<double> doubleParams = PumpManager.GetParamsByType("1");
        //        //对水泵公式中的自变量进行变换，影响到二次方程的a,b,c
        //        a += doubleParams[0] * constantNumber2;
        //        b += doubleParams[1] * constantNumber;
        //        c += doubleParams[2];
        //    }
        //    if (CoolingType.Equals("一对一"))
        //    {
        //        //从数据库得到二次项系数
        //        List<double> doubleParams = PumpManager.GetParamsByType("1");
        //        //对水泵公式中的自变量进行变换，影响到二次方程的a,b,c
        //        a += doubleParams[0] * fullFlow * fullFlow * engineCount;
        //        b += doubleParams[1] * fullFlow * engineCount;
        //        c += doubleParams[2] * engineCount;
        //    }


        //    //求出使得能耗最低的解，即流量的百分比
        //    double solute = Utility.GetMinSolute(a, b, c);
        //    double result = a * solute * solute + b * solute + c;
        //    return new SoluteResult(result, solute);
        //}
        public double TemperRange { get; set; }
        //冷却类型
        public string CoolingType { get; set; }
        //冷冻类型
        public string FreezeType { get; set; }
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
        //private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        //{
        //    meMin.Clear();
        //    minResult = double.MaxValue;
        //    minSolute = double.MaxValue;
        //    percentValue = 0;
        //    machineList.Clear();

        //    if (!string.IsNullOrEmpty(textBox1.Text))
        //    {
        //        MachineEntity machine = new MachineEntity(comBox1.Text, Convert.ToDouble(textBox1.Text));
        //        machineList.Add(machine);
        //    }
        //    if (!string.IsNullOrEmpty(textBox2.Text))
        //    {
        //        MachineEntity machine = new MachineEntity(comBox2.Text, Convert.ToDouble(textBox2.Text));
        //        machineList.Add(machine);
        //    }
        //    if (!string.IsNullOrEmpty(textBox3.Text))
        //    {
        //        MachineEntity machine = new MachineEntity(comBox3.Text, Convert.ToDouble(textBox3.Text));
        //        machineList.Add(machine);
        //    }
        //    if (!string.IsNullOrEmpty(textBox4.Text))
        //    {
        //        MachineEntity machine = new MachineEntity(comBox4.Text, Convert.ToDouble(textBox4.Text));
        //        machineList.Add(machine);
        //    }
        //    if (!string.IsNullOrEmpty(textBox5.Text))
        //    {
        //        MachineEntity machine = new MachineEntity(comBox5.Text, Convert.ToDouble(textBox5.Text));
        //        machineList.Add(machine);
        //    }
        //    if (!string.IsNullOrEmpty(textBox6.Text))
        //    {
        //        MachineEntity machine = new MachineEntity(comBox6.Text, Convert.ToDouble(textBox6.Text));
        //        machineList.Add(machine);
        //    }
        //    if (!string.IsNullOrEmpty(textBox7.Text))
        //    {
        //        MachineEntity machine = new MachineEntity(comBox7.Text, Convert.ToDouble(textBox7.Text));
        //        machineList.Add(machine);
        //    }
        //    if (!string.IsNullOrEmpty(textBox8.Text))
        //    {
        //        MachineEntity machine = new MachineEntity(comBox8.Text, Convert.ToDouble(textBox8.Text));
        //        machineList.Add(machine);
        //    }
        //    if (!string.IsNullOrEmpty(textBox9.Text))
        //    {
        //        MachineEntity machine = new MachineEntity(comBox9.Text, Convert.ToDouble(textBox9.Text));
        //        machineList.Add(machine);
        //    }
        //    if (!string.IsNullOrEmpty(textBox10.Text))
        //    {
        //        MachineEntity machine = new MachineEntity(comBox10.Text, Convert.ToDouble(textBox10.Text));
        //        machineList.Add(machine);
        //    }
        //    IsNormal = true;
        //    IsSwap = false;
        //    FreezeType = "一对一";
        //    CoolingType = "并联";

        //    TemperRange = 7;

        //    /*****************************************************************************/
        //    //根据界面的温度信息，得到温度
        //    /*****************************************************************************/
        //    double temperature = (int)(Convert.ToDouble(Temperature.Text) + 0.5);

        //    /*****************************************************************************/
        //    //根据界面的板换信息，得到最终的总负荷
        //    /*****************************************************************************/
        //    double swapPower = 0;
        //    int swapCount = 0;
        //    //此处为总负荷，由界面录入，
        //    double load = Convert.ToDouble(Load.Text);
        //    if (IsSwap)
        //        load = load - swapCount * swapPower;



        //    DealWithCalculate(temperature, load, machineList);



        //    addStrToBox("主机组合如下：", Result);
        //    addStrToBox("--------", Result);
        //    foreach (var me in meMin)
        //    {
        //        string machineResult = "类型：" + me.Type + ";";
        //        addStrToBox(machineResult, Result);
        //        machineResult = me.Value + "KW * " + String.Format("{0:F}", percentValue * 100) + "%=" + String.Format("{0:F}", me.Value * percentValue) + "KW.";
        //        addStrToBox(machineResult, Result);
        //    }
        //    addStrToBox("--------", Result);
        //    string minPowerStr = "系统最低功率为：" + String.Format("{0:F}", minResult) + "KW.";
        //    addStrToBox(minPowerStr, Result);
        //    addStrToBox("此时流量为：" + String.Format("{0:F}", minSolute * 100) + "%.", Result);
        //    addStrToBox("各主机负荷率为：" + String.Format("{0:F}", percentValue * 100) + "%.", Result);

        //}

        //public void DealWithCalculate(double temperature, double load, List<MachineEntity> machineEntities)
        //{
        //    /*****************************************************************************/
        //    //根据界面的板换信息，得到最终的总负荷
        //    /*****************************************************************************/
        //    double swapPower = 0;
        //    int swapCount = 0;
        //    //此处为总负荷，由界面录入，
        //    if (IsSwap)
        //        load = load - swapCount * swapPower;

        //    /*****************************************************************************/
        //    //如果是常规算法。则机器按照从vsd到csd的顺序，按照从大到小的顺序选择主机
        //    /*****************************************************************************/
        //    if (IsNormal)
        //    {
        //        minResult = 0;
        //        // 加工machineResult, 排序...
        //        machineEntities = OrderMachiList(machineEntities, load);
        //        meMin = machineEntities;

        //        double a = 0, b = 0, c = 0;
        //        double sumLoad2 = 0;
        //        foreach (var me in machineEntities)
        //        {
        //            sumLoad2 += me.Value;
        //        }
        //        //求得每台主机的负荷率，每台主机运行的负荷除以总负荷相等
        //        double percentValue1 = load / sumLoad2;
        //        foreach (var me in machineEntities)
        //        {
        //            //得到每台特定类型的主机在一定温度，一定负荷下的关于流量的二次项系数
        //            List<double> results = getFormulaByEntity(me.Type, me.Value * percentValue1, temperature);
        //            a += results[0];
        //            b += results[1];
        //            c += results[2];
        //        }
        //        percentValue = percentValue1;
        //        minResult += a + b + c;

        //        int engineCount = machineEntities.Count;
        //        //冷却水泵,按照并联处理
        //        List<double> doubleParamsCool = PumpManager.GetParamsByType("1");
        //        minResult += doubleParamsCool[0] * fullFlow * fullFlow * engineCount * engineCount
        //            + doubleParamsCool[1] * fullFlow * engineCount
        //            + doubleParamsCool[2] * engineCount;
        //        //冷冻水泵
        //        List<double> doubleParamsFreeze = PumpManager.GetParamsByType("2");
        //        minResult += doubleParamsFreeze[0] * 125 * 125 * engineCount * engineCount
        //            + doubleParamsFreeze[1] * 125 * engineCount
        //            + doubleParamsFreeze[2] * engineCount;

        //        minSolute = 1.0;
        //        return;
        //    }

        //    List<double> doubleList = new List<double>();
        //    for (int i = 0; i < machineEntities.Count; i++)
        //    {
        //        doubleList.Add(machineEntities[i].Value);
        //    }

        //    //判断总负荷是否成立
        //    double sumLoad = 0;
        //    foreach (var me in machineEntities)
        //    {
        //        sumLoad += me.Value;
        //    }
        //    if (sumLoad < load)
        //    {
        //        MessageBox.Show("总负荷过大，所提供主机不足");
        //        return;
        //    }

        //    //根据数量得到最终组合
        //    List<List<int>> consist = Utility.GetConsist(doubleList, load);
        //    foreach (var con in consist)
        //    {
        //        //申请一个组合的列表
        //        List<MachineEntity> machineResult = new List<MachineEntity>();

        //        //对一个组合中的数字进行轮询
        //        foreach (var val in con)
        //        {
        //            //将每一个脚码添加到结果里面
        //            machineResult.Add(machineEntities[val]);
        //        }
        //        //以上得到一个组合，接下来对其求最小值
        //        SoluteResult sr = getMinByConsist(machineResult, temperature, load);
        //        /***********************************************************************************/
        //        //冷冻水泵的计算公式
        //        /***********************************************************************************/
        //        double freezePumpPower = 0;
        //        if (FreezeType.Equals("一对一"))
        //        {
        //            double minValue = double.MaxValue;
        //            foreach (var me in machineResult)
        //            {
        //                if (minValue > me.Value)
        //                    minValue = me.Value;
        //            }
        //            if (4.187 * TemperRange * 125 < minValue)
        //            {
        //                MessageBox.Show("总负荷过大，所提供冷冻水泵不足");
        //                return;
        //            }

        //            //从数据库得到二次项系数
        //            List<double> doubleParams = PumpManager.GetParamsByType("2");
        //            foreach (var me in machineResult)
        //            {
        //                double curflow = me.Value / (4.187 * TemperRange);
        //                double curPower = doubleParams[0] * curflow * curflow + doubleParams[1] * curflow + doubleParams[2];
        //                freezePumpPower += curPower;
        //            }
        //        }
        //        if (FreezeType.Equals("并联"))
        //        {
        //            if (4.187 * TemperRange * 125 * machineResult.Count < load)
        //            {
        //                MessageBox.Show("总负荷过大，所提供冷冻水泵不足");
        //                return;
        //            }

        //            //从数据库得到二次项系数
        //            List<double> doubleParams = PumpManager.GetParamsByType("2");
        //            double curflow = load / (4.187 * TemperRange * machineResult.Count);
        //            double curPower = doubleParams[0] * curflow * curflow + doubleParams[1] * curflow + doubleParams[2];
        //            freezePumpPower += curPower;
        //        }
        //        //得到最终结果,并且加上冷却塔功率
        //        double coolingPower = 0;
        //        sr = new SoluteResult(sr.Result + freezePumpPower + coolingPower, sr.Solute);

        //        /***********************************************************************************/
        //        //判断某个组合的的最小功率是不是在所有组合中最小
        //        /***********************************************************************************/
        //        if (sr.Result < minResult)
        //        {
        //            //如果是最小的，则将最小值赋值为当前组合的最小值
        //            minResult = sr.Result;
        //            //保存取得最小能耗时的流量
        //            minSolute = sr.Solute;
        //            //此处保存最小的主机组合
        //            meMin = machineResult;
        //        }
        //    }
        //    //循环结束得到最小的主机组合，及最小值

        //    double sumLoad1 = 0;
        //    foreach (var me in meMin)
        //    {
        //        sumLoad1 += me.Value;
        //    }
        //    //求得每台主机的负荷率，每台主机运行的负荷除以总负荷相等
        //    percentValue = load / sumLoad1;
        //}

        //常规算法：
        //根据总负荷，按照从vsd到csd的顺序，按照从大到小的顺序选择主机，流量100%，计算能耗
        //public List<MachineEntity> OrderMachiList(List<MachineEntity> list, double load)
        //{
        //    List<MachineEntity> tmplist = new List<MachineEntity>();
        //    Sort(ref list);
        //    int index = 0;
        //    double total = 0.0;
        //    while ((total < load) && (index < list.Count))
        //    {
        //        tmplist.Add(list[index]);
        //        total += list[index++].Value;
        //    }
        //    return tmplist;
        //}

        //private void Sort(ref List<MachineEntity> list)
        //{
        //    MachineEntity tmp;
        //    for (int i = 0; i < list.Count; i++)
        //    {
        //        for (int j = i; j < list.Count; j++)
        //        {
        //            if (Small(list[j], list[i]))
        //            {
        //                tmp = list[i];
        //                list[i] = list[j];
        //                list[j] = tmp;
        //            }
        //        }
        //    }
        //}
        //private bool Small(MachineEntity obj1, MachineEntity obj2)
        //{
        //    if (obj1.Type == obj2.Type)
        //        return obj1.Value > obj2.Value ? true : false;
        //    else
        //    {
        //        if (obj1.Type == "VSD")
        //            return true;
        //        else
        //            return false;
        //    }
        //}


        private void btnShow_Click(object sender, EventArgs e)
        {
            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            pictureBox_Result.ImageLocation = root.FullName + "/Resources/result.jpg";

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
            if (!string.IsNullOrEmpty(textBox_TemperRange.Text))
                TemperRange = Convert.ToInt32(textBox_TemperRange.Text);
            coolingPower = Convert.ToInt32(textBox_CoolingPower.Text.ToString());
            GetOptimizationResult(meList, Convert.ToDouble(textBox_Load.Text), Convert.ToDouble(textBox_Temperature.Text));

            //如果是板换，则散热塔的功率等于板换数量
            if (IsBoard)
                coolingPower = coolingPower * BoardCount;
            else
            {
                coolingPower = coolingPower * meMin.Count;
            }
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
            addStrToBox("冷却水泵能耗为：" + String.Format("{0:F}", minResult - freezePumpPower - enginePower - coolingPower) + "KW.", textBox_Message);
            addStrToBox("冷却塔能耗为：" + String.Format("{0:F}", coolingPower) + "KW.", textBox_Message);
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
        //在一定温度下得到一个主机组合的最低能耗，及对应的流量
        private SoluteResult getMinByConsist(List<MachineEntity> mes, double temperature, double load)
        {
            /***********************************************************************************/
            //主机功率的计算公式
            /***********************************************************************************/
            double a = 0;
            double b = 0;
            double c = 0;
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
                a += results[0];
                b += results[1];
                c += results[2];
            }

            double tempa = a;
            double tempb = b;
            double tempc = c;

            //string pumpType = PumpType.Text;
            int engineCount = mes.Count;

            //如果使用板换，去除主机,主机数量当成板换数量
            if (IsBoard)
            {
                tempa = 0d;
                tempb = 0d;
                tempc = 0d;
                engineCount = BoardCount;
            }

            double threeOption = 0d;
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
            //if (solute < 0.45)
            // solute = 0.45;
            double result = a * solute * solute + b * solute + c;

            enginePower = tempa * solute * solute + tempb * solute + tempc;

            return new SoluteResult(result, solute);
        }
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
                minResult += a + b + c;

                int engineCount = machineEntities.Count;
                //冷却水泵,按照并联处理
                List<double> doubleParamsCool = PumpManager.GetParamsByType("1");
                minResult += doubleParamsCool[0] * fullFlow * fullFlow * engineCount * engineCount
                    + doubleParamsCool[1] * fullFlow * engineCount
                    + doubleParamsCool[2] * engineCount;
                //冷冻水泵
                List<double> doubleParamsFreeze = PumpManager.GetParamsByType("2");
                minResult += doubleParamsFreeze[0] * 125 * 125 * engineCount * engineCount
                    + doubleParamsFreeze[1] * 125 * engineCount
                    + doubleParamsFreeze[2] * engineCount;

                minSolute = 1.0;
                return;
            }

            /*****************************************************************************/
            //如果是优化算法。根据主机的选择和冷却水泵和冷却水泵的并联和一对一进行
            /*****************************************************************************/
            List<double> doubleList = new List<double>();
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
                MessageBox.Show("总负荷过大，所提供主机不足");
                return;
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
                        MessageBox.Show("总负荷过大，所提供冷冻水泵不足");
                        return;
                    }

                    //从数据库得到二次项系数
                    //一对一：一台水泵对应一台主机
                    List<double> doubleParams = PumpManager.GetParamsByType("2");
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
                type_box.DropDownStyle = ComboBoxStyle.DropDownList;
                cooling_comboBox.Name = "freezer_cooling_comboBox" + i;
                brand_comboBox.Name = "freezer_brand_comboBox" + i;
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
                boarder.Text = "板换" + i;
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
                brand_comboBox.Location = new Point(82, 15 + (i - 1) * 35);
                brand_comboBox.Width = 76;
                brand_comboBox.Height = 20;
                flow_textBox.Location = new Point(174, 15 + (i - 1) * 35);
                flow_textBox.Width = 76;
                flow_textBox.Height = 20;
                lift_textBox.Location = new Point(269, 15 + (i - 1) * 35);
                lift_textBox.Width = 76;
                lift_textBox.Height = 20;
                power_textBox.Location = new Point(363, 15 + (i - 1) * 35);
                power_textBox.Width = 76;
                power_textBox.Height = 20;
                model_textBox.Location = new Point(458, 15 + (i - 1) * 35);
                model_textBox.Width = 76;
                model_textBox.Height = 20;
                amount_textBox.Location = new Point(553, 15 + (i - 1) * 35);
                amount_textBox.Width = 20;
                amount_textBox.Height = 20;
                //performance_data_button.Location = new Point(700, 15 + (i - 1) * 35);
                //performance_data_button.Width = 60;
                //performance_data_button.Height = 23;
                //performance_data_button.Text = "调用";
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
                string strIs_Frequency_Conversion_checkBox = "is_Frequency_Conversion_checkBox" + (i + 1);
                CheckBox temp_is_Frequency_Conversion_checkBox = (CheckBox)GetControl(strIs_Frequency_Conversion_checkBox, freezer_Panel);
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "Frequency", "", temp_is_Frequency_Conversion_checkBox.Text);
                string strPerformance_data_box = "performance_data_box" + (i + 1);
                //ComboBox temp_performance_data_box = (ComboBox)GetControl(strPerformance_data_box, freezer_Panel);
                //MessageBox.Show(temp_performance_data_box.Name);
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
                ComboBox temp_flow_comboBox = (ComboBox)GetControl(strflow_comboBox, freezingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Flow", "", temp_flow_comboBox.Text);

                string strlift_comboBox = "lift_comboBox" + (i + 1);
                ComboBox temp_lift_comboBox = (ComboBox)GetControl(strlift_comboBox, freezingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Distance", "", temp_lift_comboBox.Text);

                string strpower_comboBox = "power_comboBox" + (i + 1);
                ComboBox temp_power_comboBox = (ComboBox)GetControl(strpower_comboBox, freezingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Power", "", temp_power_comboBox.Text);

                string strmodel_comboBox = "model_comboBox" + (i + 1);
                ComboBox temp_model_comboBox = (ComboBox)GetControl(strmodel_comboBox, freezingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Model", "", temp_model_comboBox.Text);


                string strtype_comboBox = "type_comboBox" + (i + 1);
                ComboBox temp_type_comboBox = (ComboBox)GetControl(strtype_comboBox, freezingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Type", "", temp_type_comboBox.Text);


                string stris_Frequency_Conversion_checkBox = "is_Frequency_Conversion_checkBox" + (i + 1);
                CheckBox temp_is_Frequency_Conversion_checkBox = (CheckBox)GetControl(stris_Frequency_Conversion_checkBox, freezingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Frequency", "", temp_brand_comboBox.Text);
            }

            for (int i = 0; i < Convert.ToInt32(coolingNum.Text); i++)
            {
                XmlHelper.Insert(xml, "/Project/PumpConfiguration", "CoolingPump", "id", i.ToString());
                string strbrand_comboBox = "brand_comboBox" + (i + 1);
                ComboBox temp_brand_comboBox = (ComboBox)GetControl(strbrand_comboBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Brand", "", temp_brand_comboBox.Text);


                string strflow_comboBox = "flow_comboBox" + (i + 1);
                ComboBox temp_flow_comboBox = (ComboBox)GetControl(strflow_comboBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Flow", "", temp_flow_comboBox.Text);

                string strlift_comboBox = "lift_comboBox" + (i + 1);
                ComboBox temp_lift_comboBox = (ComboBox)GetControl(strlift_comboBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Distance", "", temp_lift_comboBox.Text);

                string strpower_comboBox = "power_comboBox" + (i + 1);
                ComboBox temp_power_comboBox = (ComboBox)GetControl(strpower_comboBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Power", "", temp_power_comboBox.Text);

                string strmodel_comboBox = "model_comboBox" + (i + 1);
                ComboBox temp_model_comboBox = (ComboBox)GetControl(strmodel_comboBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Model", "", temp_model_comboBox.Text);


                string strtype_comboBox = "type_comboBox" + (i + 1);
                ComboBox temp_type_comboBox = (ComboBox)GetControl(strtype_comboBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Type", "", temp_type_comboBox.Text);


                string stris_Frequency_Conversion_checkBox = "is_Frequency_Conversion_checkBox" + (i + 1);
                CheckBox temp_is_Frequency_Conversion_checkBox = (CheckBox)GetControl(stris_Frequency_Conversion_checkBox, coolingPanel);
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Frequency", "", temp_brand_comboBox.Text);
            }

            /*XmlHelper.Insert(xml, "/Project", "PumpConfiguration", "", "");
            XmlHelper.Insert(xml, "/Project/PumpConfiguration", "FreezePumpCount", "", Convert.ToInt32(freezingNum.Text));
            XmlHelper.Insert(xml, "/Project/PumpConfiguration", "CoolingPumpCount", "", Convert.ToInt32(coolingNum.Text));
            for (int i = 0; i < Convert.ToInt32(freezingNum.Text); i++)
            {
                XmlHelper.Insert(xml, "/Project/PumpConfiguration", "FreezePump", "id", i.ToString());
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Brand", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Flow", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Distance", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Power", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Model", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Type", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]", "Frequency", "", "this is type");
            }
            for (int i = 0; i < Convert.ToInt32(coolingNum.Text); i++)
            {
                XmlHelper.Insert(xml, "/Project/PumpConfiguration", "CoolingPump", "id", i.ToString());
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Brand", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Flow", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Distance", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Power", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Model", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Type", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Frequency", "", "this is type");
            }*/

            /**********************************************************************************************/
            //插入冷却配置
            /**********************************************************************************************/
            XmlHelper.Insert(xml, "/Project", "CoolingConfiguration", "", "");
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration", "CoolingTower", "", "");
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "Flow", "", coolTower_tb1.Text);
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "Frequency", "", coolTower_cb.Text);

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
                string strIs_Frequency_Conversion_checkBox = "is_Frequency_Conversion_checkBox" + (i + 1);
                CheckBox temp_is_Frequency_Conversion_checkBox = (CheckBox)GetControl(strIs_Frequency_Conversion_checkBox, freezer_Panel);
                temp_is_Frequency_Conversion_checkBox.Text = XmlHelper.Read(path, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]/Frequency", "");
                string strPerformance_data_box = "performance_data_box" + (i + 1);
                ComboBox temp_performance_data_box = (ComboBox)GetControl(strPerformance_data_box, freezer_Panel); ;
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
                ComboBox temp_flow_comboBox = (ComboBox)GetControl(strflow_comboBox, freezingPanel);
                temp_flow_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Flow", "");

                string strlift_comboBox = "lift_comboBox" + (i + 1);
                ComboBox temp_lift_comboBox = (ComboBox)GetControl(strlift_comboBox, freezingPanel);
                temp_lift_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Distance", "");

                string strpower_comboBox = "power_comboBox" + (i + 1);
                ComboBox temp_power_comboBox = (ComboBox)GetControl(strpower_comboBox, freezingPanel);
                temp_power_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Power", "");

                string strmodel_comboBox = "model_comboBox" + (i + 1);
                ComboBox temp_model_comboBox = (ComboBox)GetControl(strmodel_comboBox, freezingPanel);
                temp_model_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Model", "");


                string strtype_comboBox = "type_comboBox" + (i + 1);
                ComboBox temp_type_comboBox = (ComboBox)GetControl(strtype_comboBox, freezingPanel);
                temp_type_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Type", "");


                string stris_Frequency_Conversion_checkBox = "is_Frequency_Conversion_checkBox" + (i + 1);
                CheckBox temp_is_Frequency_Conversion_checkBox = (CheckBox)GetControl(stris_Frequency_Conversion_checkBox, freezingPanel);
                temp_brand_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Frequency", "");
            }
            for (int i = 0; i < Convert.ToInt32(coolingNum.Text); i++)
            {
                string strbrand_comboBox = "brand_comboBox" + (i + 1);
                ComboBox temp_brand_comboBox = (ComboBox)GetControl(strbrand_comboBox, coolingPanel);
                temp_brand_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Brand", "");


                string strflow_comboBox = "flow_comboBox" + (i + 1);
                ComboBox temp_flow_comboBox = (ComboBox)GetControl(strflow_comboBox, coolingPanel);
                temp_flow_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Flow", "");

                string strlift_comboBox = "lift_comboBox" + (i + 1);
                ComboBox temp_lift_comboBox = (ComboBox)GetControl(strlift_comboBox, coolingPanel);
                temp_lift_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Distance", "");

                string strpower_comboBox = "power_comboBox" + (i + 1);
                ComboBox temp_power_comboBox = (ComboBox)GetControl(strpower_comboBox, coolingPanel);
                temp_power_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Power", "");

                string strmodel_comboBox = "model_comboBox" + (i + 1);
                ComboBox temp_model_comboBox = (ComboBox)GetControl(strmodel_comboBox, coolingPanel);
                temp_model_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Model", "");


                string strtype_comboBox = "type_comboBox" + (i + 1);
                ComboBox temp_type_comboBox = (ComboBox)GetControl(strtype_comboBox, coolingPanel);
                temp_type_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Type", "");


                string stris_Frequency_Conversion_checkBox = "is_Frequency_Conversion_checkBox" + (i + 1);
                CheckBox temp_is_Frequency_Conversion_checkBox = (CheckBox)GetControl(stris_Frequency_Conversion_checkBox, coolingPanel);
                temp_brand_comboBox.Text = XmlHelper.Read(path, "/Project/PumpConfiguration/FreezePump[@id=" + i.ToString() + "]/Frequency", "");
            }


            /**********************************************************************************************/
            //插入冷却配置
            /**********************************************************************************************/
            coolTower_tb1.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/Flow", "");
            coolTower_cb.Text = XmlHelper.Read(path, "/Project/CoolingConfiguration/CoolingTower/Frequency", "");

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
            this.reportViewer1.RefreshReport();
        }

        private void btnViewReport_Click(object sender, EventArgs e)
        {
            var resultSet = OptimizationResultData.GetAll();
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "Enerefsys.Report1.rdlc";
            //ReportParameter rp = new ReportParameter("content", this.textBox1.Text);
            //this.reportViewer1.LocalReport.SetParameters(new ReportParameter[] { rp });
            this.reportViewer1.LocalReport.DataSources.Add(new Microsoft.Reporting.WinForms.ReportDataSource("DS_Result", resultSet));
            this.reportViewer1.RefreshReport();
        }


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

                EngineParam ep = new EngineParam(progressBar, comboBox_EngineType.Text, openFileDialog.FileName);

                DealData(ep);
            }
        }


        private void DealData(EngineParam engineParam)
        {
            BackgroundWorker mWorker = new BackgroundWorker();
            mWorker.WorkerReportsProgress = true;
            mWorker.WorkerSupportsCancellation = true;
            if (engineParam.ProgressBar == progressBar)
            {
                mWorker.DoWork += new DoWorkEventHandler(mWorker_DoWork);
                mWorker.ProgressChanged += new ProgressChangedEventHandler(mWorker_ProgressChanged);
                mWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(mWorker_RunWorkerCompleted);
                mWorker.RunWorkerAsync(engineParam);
                completed.Text = "处理中...";
            }
            else if (engineParam.ProgressBar == progressBarPump)
            {
                mWorker.DoWork += new DoWorkEventHandler(mWorker_DoWorkPump);
                mWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(worker_ProgressChangedPump);
                mWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(worker_RunWorkerCompletedPump);
                mWorker.RunWorkerAsync(engineParam);
                completedPump.Text = "处理中...";
            }


        }



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
                BackgroundWorker worker = (BackgroundWorker)sender;
                EngineParam ep = (EngineParam)e.Argument;
                string fileName = ep.FileName;

                //得到excel中的所有sheet名字，然后循环得到数据模拟表达式,sheet名代表水泵类型
                List<string> sheets = Utility.GetSheetNames(fileName);
                PumpManager.DeletePump();

                int iStep = 0;
                foreach (var sheet in sheets)
                {
                    Fit.Test test = new Fit.Test();

                    //根据dll得到excel中的数据，并插入数据库
                    MathWorks.MATLAB.NET.Arrays.MWArray mwArray = test.GetFND(fileName, sheet);
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
                    MathWorks.MATLAB.NET.Arrays.MWArray mArray = svpf.SingleVariablePolyFit(fileName, sheet);
                    MathWorks.MATLAB.NET.Arrays.MWNumericArray mmArray = mArray as MathWorks.MATLAB.NET.Arrays.MWNumericArray;
                    Array array = mmArray.ToArray();
                    int ret = PumpManager.Insert((array.GetValue(0, 0)), array.GetValue(0, 1), array.GetValue(0, 2), array.GetValue(0, 3), sheet);
                    if (ret == 1)
                    {
                        iStep++;
                        worker.ReportProgress(iStep);
                    }
                }
            }
            catch (Exception ee)
            {
            }

        }



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
            BackgroundWorker worker = (BackgroundWorker)sender;
            EngineParam ep = (EngineParam)e.Argument;
            string fileName = ep.FileName;
            ProgressBar progressBar = ep.ProgressBar;
            string engineType = ep.EngineType;

            //得到excel中的所有sheet名字，然后循环得到数据模拟表达式
            List<string> sheets = Utility.GetSheetNames(fileName);
            //删除所有数据
            int rc = EngineManager.DeleteByType(engineType);

            int iStep = 0;
            foreach (var sheet in sheets)
            {
                Fit.Test test = new Fit.Test();
                MathWorks.MATLAB.NET.Arrays.MWArray mArray = test.MultiPolyfit(fileName, sheet);
                MathWorks.MATLAB.NET.Arrays.MWNumericArray mmArray = mArray as MathWorks.MATLAB.NET.Arrays.MWNumericArray;
                Array array = mmArray.ToArray();
                int ret = EngineManager.Insert((array.GetValue(0, 0)), array.GetValue(1, 0), array.GetValue(2, 0), array.GetValue(3, 0), array.GetValue(4, 0), array.GetValue(5, 0), sheet, engineType);
                if (ret == 1)
                {
                    iStep++;
                    worker.ReportProgress(iStep);
                }
            }
        }

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
                EngineParam ep = new EngineParam(progressBarPump, "", openFileDialog.FileName);

                DealData(ep);
            }
        }

        String s1 = @"../../image/1.jpg";
        String s2 = @"../../image/2.jpg";
        String s3 = @"../../image/1.jpg";
        String s4 = @"../../image/4.jpg";
        String s5 = @"../../image/5.jpg";
        int count = 1;

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (count == 6)
                count = 1;
            switch (count)
            {
                case 1:
                    pictureBox1.Image = Image.FromFile(s1);
                    pictureBox2.Image = Image.FromFile(s2);
                    count++;
                    break;
                case 2:
                    pictureBox1.Image = Image.FromFile(s3);
                    pictureBox2.Image = Image.FromFile(s4);
                    count++;
                    break;
                case 3:
                    pictureBox1.Image = Image.FromFile(s1);
                    pictureBox2.Image = Image.FromFile(s5);
                    count++;
                    break;
                case 4:
                    pictureBox1.Image = Image.FromFile(s2);
                    pictureBox2.Image = Image.FromFile(s1);
                    count++;
                    break;
                case 5:
                    pictureBox1.Image = Image.FromFile(s4);
                    pictureBox1.Image = Image.FromFile(s5);
                    count++;
                    break;
            }
        }

        private void comboBox7_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox7.Text.Equals("大系统"))
            {
                fullFlow = 1332;
                //coolingPower = 94.0d;
            }
            else if (comboBox7.Text.Equals("小系统"))
            {
                fullFlow = 320;
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

}




