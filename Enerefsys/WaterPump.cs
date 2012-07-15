using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using BLL;

namespace WindowsFormsApplication1
{
    public partial class WaterPump : Form
    {
        private List<Label> label_list;
        private List<SubFreezer> subFreezer_list;
        private int labelFlag = 0;//判断是否要显示labe用
        private int reponseCount
        {
            get;
            set;
        }
        public WaterPump()
        {
            InitializeComponent();
            init_label_list();
            conceal_Label(label_list);
            reponseCount = 0;

            List<string> results = PumpManager.GetPumpTypes();
            foreach (var result in results)
            {
                comboBox_PumpType.Items.Add(result);
            }
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
        private List<Label> add_Label_List(Label brand_Label,Label flow_label, Label lift_Label, Label power_Label, Label model_Label, Label is_Frequency_Conversion_Label, Label performance_Data_Label)
        {
            List<Label> temp_label_list = new List<Label>();
            temp_label_list.Add(brand_Label);
            temp_label_list.Add(flow_label);
            temp_label_list.Add(lift_Label);
            temp_label_list.Add(power_Label);
            temp_label_list.Add(model_Label);
            temp_label_list.Add(is_Frequency_Conversion_Label);
            temp_label_list.Add(performance_Data_Label);
            return temp_label_list;
        }
        //初始化标签列表
        private void init_label_list()
        {
            label_list = add_Label_List(brand_Label, flow_Label, lift_Label, power_Label, model_Label, is_Frequency_Conversion_Label, performance_Data_Label);

        }
        //产生冷冻或冷却水泵数量
        private void create_Freezer_Num(int freezerCount)
        {
            subFreezer_list = new List<SubFreezer>();
            for (int i = 1; i <= freezerCount; i++)
            {
                subFreezer_list.Add(new SubFreezer(i));
            }
        }
        //动态显示冷冻机
        private void set_Freezer_Panel(List<SubFreezer> subFreezer_list,Panel tempPanel)
        {
            int j= 0;
            foreach (SubFreezer sub_freezer in subFreezer_list)
            {
                j++;
                SubFreezer temp_SubFreezer = (SubFreezer)sub_freezer;
                if (tempPanel.Name.Equals("freezingPanel"))
                    temp_SubFreezer.freazerAndcooler.Text = "冷冻水泵" + j;
                else
                {
                    temp_SubFreezer.freazerAndcooler.Text = "冷却水泵" +j;
                }
                tempPanel.Controls.Add(temp_SubFreezer.freazerAndcooler);
                tempPanel.Controls.Add(temp_SubFreezer.brand_comboBox);
                tempPanel.Controls.Add(temp_SubFreezer.flow_comboBox);
                tempPanel.Controls.Add(temp_SubFreezer.lift_comboBox);
                tempPanel.Controls.Add(temp_SubFreezer.power_comboBox);
                tempPanel.Controls.Add(temp_SubFreezer.model_comboBox);
                tempPanel.Controls.Add(temp_SubFreezer.type_comboBox);
                tempPanel.Controls.Add(temp_SubFreezer.is_Frequency_Conversion_checkBox);
               
                if(j<4&&tempPanel.Name.Equals("freezingPanel"))
                tempPanel.Controls.Add(temp_SubFreezer.performance_data_button);
                
            }
        }
        //清楚panel中冷冻水泵和冷却水泵组件
        private void clear_Panel(Panel tempPanel)
        {
            tempPanel.Controls.Clear();
        }

        //冷冻水泵和冷却水泵
        class SubFreezer
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
            public ComboBox model_comboBox
            {
                get;
                set;
            }
            public ComboBox flow_comboBox
            {
                get;
                set;
            }
            public ComboBox lift_comboBox
            {
                get;
                set;
            }
            public ComboBox power_comboBox
            {
                get;
                set;
            }
            public ComboBox type_comboBox
            {
                get;
                set;
            }
            public CheckBox is_Frequency_Conversion_checkBox
            {
                get;
                set;
            }
            public Button performance_data_button
            {
                get;
                set;
            }
            public SubFreezer(int i)
            {
                freazerAndcooler = new Label();
                brand_comboBox = new ComboBox();
                model_comboBox = new ComboBox();
                is_Frequency_Conversion_checkBox = new CheckBox();
                performance_data_button = new Button();
                flow_comboBox = new ComboBox();
                lift_comboBox = new ComboBox();
                power_comboBox = new ComboBox();
                type_comboBox = new ComboBox();
                setComponentAttribute(i);
                setComponetLocation(i);
            }

            public void setComponentAttribute(int i)
            {
                freazerAndcooler.Name = "freazerAndcooler" + i;
                brand_comboBox.Name = "brand_comboBox" + i;
                model_comboBox.Name = "model_comboBox" + i;
                is_Frequency_Conversion_checkBox.Name = "is_Frequency_Conversion_checkBox" + i;
                performance_data_button.Name = "performance_data_button" + i;
                flow_comboBox.Name = "flow_comboBox" + i;
                lift_comboBox.Name = "lift_comboBox" + i;
                power_comboBox.Name = "power_comboBox" + i;
                type_comboBox.Name = "type_comboBox" + i;
                type_comboBox.Items.Add("VSD");
                type_comboBox.Items.Add("CSD");
            }

            public void setComponetLocation(int i)
            {
                freazerAndcooler.Location = new Point(15, 17 + (i - 1) * 35);
                freazerAndcooler.Width = 68;
                freazerAndcooler.Height = 12;
                brand_comboBox.Location = new Point(82, 15 + (i - 1) * 35);
                brand_comboBox.Width = 76;
                brand_comboBox.Height = 20;
                flow_comboBox.Location = new Point(174,15+(i-1)*35);
                flow_comboBox.Width = 76;
                flow_comboBox.Height = 20;
                lift_comboBox.Location = new Point(269, 15 + (i - 1) * 35);
                lift_comboBox.Width = 76;
                lift_comboBox.Height = 20;
                power_comboBox.Location = new Point(363, 15 + (i - 1) * 35);
                power_comboBox.Width = 76;
                power_comboBox.Height = 20;
                model_comboBox.Location = new Point(458, 15 + (i - 1) * 35);
                model_comboBox.Width = 76;
                model_comboBox.Height = 20;
                type_comboBox.Location = new Point(552,15+(i-1)*35);
                type_comboBox.Width = 76;
                type_comboBox.Height = 20;
                is_Frequency_Conversion_checkBox.Location = new Point(660, 17 + (i - 1) * 35);
                is_Frequency_Conversion_checkBox.Width = 15;
                is_Frequency_Conversion_checkBox.Height = 14;
                performance_data_button.Location = new Point(700, 15 + (i - 1) * 35);
                performance_data_button.Width = 60;
                performance_data_button.Height = 23;
                performance_data_button.Text = "调用";
            }
        }

        private void freezingNum_TextChanged(object sender, EventArgs e)
        {
            int freezerCount = 0;
            reponseCount += 1;
            if (reponseCount == 1 || labelFlag == 1)
                appear_Label(label_list);
            labelFlag = 0;
            clear_Panel(freezingPanel);
            try
            {
                if (null != freezingNum.Text.ToString().Trim() && "" != freezingNum.Text.ToString().Trim())
                {
                    freezerCount = Int32.Parse(freezingNum.Text.ToString());
                }
                else
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
            set_Freezer_Panel(subFreezer_list,freezingPanel);
        }

        private void coolingNum_TextChanged(object sender, EventArgs e)
        {
            int freezerCount = 0;
            reponseCount += 1;
            if (reponseCount == 1 || labelFlag == 1)
                appear_Label(label_list);
            labelFlag = 0;
            clear_Panel(coolingPanel);
            try
            {
                if (null != coolingNum.Text.ToString().Trim() && "" != coolingNum.Text.ToString().Trim())
                {
                    freezerCount = Int32.Parse(coolingNum.Text.ToString());
                }
                else
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
            set_Freezer_Panel(subFreezer_list, coolingPanel);
        }


        //添加一个委托
        public delegate void PassDataBetweenFormHandler(object sender, PumpWinFormEventArgs e);
        //添加一个PassDataBetweenFormHandler类型的事件
        public event PassDataBetweenFormHandler PassDataBetweenForm;
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            //传递给父窗口选择类型
            PumpWinFormEventArgs pwfa = new PumpWinFormEventArgs(comboBox_PumpType.Text);
            PassDataBetweenForm(this, pwfa);
            this.Dispose();
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {

        }
       
    }
}
