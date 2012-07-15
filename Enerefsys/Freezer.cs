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
    public partial class Freezer : Form
    {
        //添加一个委托
        public delegate void PassDataBetweenFormHandler(object sender, EngineWinFormEventArgs e);
        //添加一个PassDataBetweenFormHandler类型的事件
        public event PassDataBetweenFormHandler PassDataBetweenForm;


        private List<Label> label_list;
        private List<SubFreezer> subFreezer_list;
        public List<MachineEntity> meList { get; set; }//获得冷冻值列表（类型和冷量）
        private int labelFlag = 0;//判断是否要显示labe用
        private int reponseCount
        {
            get;
            set;
        }
        public Freezer()
        {
            InitializeComponent();
            init_label_list();
            conceal_Label(label_list);
            reponseCount = 0;
            Freezer_Load(null, null);
        }


        private void Freezer_Load(object sender, EventArgs e)
        {

            this.dataGridView1.Columns.Add("JanWin", "编号");

            this.dataGridView1.Columns.Add("JanLoss", "机器名称和型号");

            this.dataGridView1.Columns.Add("FebWin", "数量");

            this.dataGridView1.Columns.Add("FebLoss", "TR");

            this.dataGridView1.Columns.Add("MarWin", "流量V3");

            this.dataGridView1.Columns.Add("MarLoss1", "回水温度℃");

            this.dataGridView1.Columns.Add("MarLoss", "供水温度℃");

            this.dataGridView1.Columns.Add("pk1", "压降kpa");

            this.dataGridView1.Columns.Add("pk2", "流量V3");

            this.dataGridView1.Columns.Add("pk3", "进口温度℃");

            this.dataGridView1.Columns.Add("pk4", "出口温度℃");

            this.dataGridView1.Columns.Add("pk5", "压降kpa");

            for (int j = 0; j < this.dataGridView1.ColumnCount; j++)
            {
                if (j == 1)
                    this.dataGridView1.Columns[j].Width = 130;
                else
                    this.dataGridView1.Columns[j].Width = 90;

            }

            this.dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.EnableResizing;

            this.dataGridView1.ColumnHeadersHeight = this.dataGridView1.ColumnHeadersHeight * 2;

            this.dataGridView1.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.BottomCenter;

            this.dataGridView1.CellPainting += new DataGridViewCellPaintingEventHandler(dataGridView1_CellPainting);

            this.dataGridView1.Paint += new PaintEventHandler(dataGridView1_Paint);

        }
        void dataGridView1_Paint(object sender, PaintEventArgs e)
        {

            string[] monthes = { "单台制冷量", "蒸发器", "冷凝器" };

            for (int j = 3; j < 6; )
            {
                Rectangle r1 = this.dataGridView1.GetCellDisplayRectangle(j, -1, true); //get the column header cell
                r1.X += 1;

                r1.Y += 1;
                if (j == 3)
                {
                    r1.Width = r1.Width - 2;
                }
                else if (j == 4)
                {
                    r1.Width = r1.Width * 4 - 2;
                }
                else if (j == 5)
                {
                    r1.X += 270;
                    r1.Width = r1.Width * 4;
                }

                r1.Height = r1.Height / 2 - 2;

                e.Graphics.FillRectangle(new SolidBrush(this.dataGridView1.ColumnHeadersDefaultCellStyle.BackColor), r1);

                StringFormat format = new StringFormat();

                format.Alignment = StringAlignment.Center;

                format.LineAlignment = StringAlignment.Center;

                e.Graphics.DrawString(monthes[j - 3],

                    this.dataGridView1.ColumnHeadersDefaultCellStyle.Font,

                    new SolidBrush(this.dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor),

                    r1,

                    format);

                j += 1;
            }

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
            int freezerCount=0;
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
                else
                {
                    labelFlag += 1;
                    conceal_Label(label_list);
                    return;
                }
            }
            catch (Exception e1)
            {
                Console.Write(""+e1.Message);
                MessageBox.Show("请输入正确的数据类型！");
                return;
               
            }
            create_Freezer_Num(freezerCount);
            set_Freezer_Panel(subFreezer_list);
        }
        //隐藏标签
        private void conceal_Label(List<Label> label_list)
        {
            foreach(Label label in label_list){
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
        private List<Label> add_Label_List(Label type_Label,Label cooling_Capacity_Label,Label brand_Label,Label model_Label,Label is_Frequency_Conversion_Label,Label performance_Data_Label)
        {
            List<Label> temp_label_list=new List<Label>();
            temp_label_list.Add(type_Label);
            temp_label_list.Add(cooling_Capacity_Label);
            temp_label_list.Add(brand_Label);
            temp_label_list.Add(model_Label);
            temp_label_list.Add(is_Frequency_Conversion_Label);
            temp_label_list.Add(performance_Data_Label);
            return temp_label_list;
        }
        //初始化标签列表
        private void init_label_list()
        {
            label_list = add_Label_List(type_Label, cooling_Capacity_Label, brand_Label, model_Label, is_Frequency_Conversion_Label, performance_Data_Label);
           
        }
        //产生冷冻机数量
        private void create_Freezer_Num(int freezerCount)
        {
            subFreezer_list = new List<SubFreezer>();
            for (int i = 1; i <= freezerCount;i++)
            {
                subFreezer_list.Add(new SubFreezer(i));
            }
        }
        //动态显示冷冻机
        private void set_Freezer_Panel(List<SubFreezer> subFreezer_list)
        {
            foreach(SubFreezer sub_freezer in subFreezer_list){
                SubFreezer temp_SubFreezer = (SubFreezer)sub_freezer;
                freezer_Panel.Controls.Add(temp_SubFreezer.freazer);
                freezer_Panel.Controls.Add(temp_SubFreezer.type_box);
                freezer_Panel.Controls.Add(temp_SubFreezer.cooling_comboBox);
                freezer_Panel.Controls.Add(temp_SubFreezer.brand_comboBox);
                freezer_Panel.Controls.Add(temp_SubFreezer.model_box);
                freezer_Panel.Controls.Add(temp_SubFreezer.is_Frequency_Conversion_checkBox);
                freezer_Panel.Controls.Add(temp_SubFreezer.performance_data_box);
            }
        }
        //清楚panel中冷冻机组件
        private void clear_Panel()
        {
            freezer_Panel.Controls.Clear();
        }

        //返回冷冻机类型和冷量
        private List<MachineEntity> getFreezerTypeAndCooling(List<SubFreezer> subFreezer_list)
        {
            int tempFreezerNum;
            List<MachineEntity> machineList=new List<MachineEntity>();
            if (null != freezerNum.Text.ToString().Trim() && "" != freezerNum.Text.ToString().Trim()&&0<subFreezer_list.Count)
            {
                tempFreezerNum = Int32.Parse(freezerNum.Text.ToString().Trim());
               
                for (int i = 1; i <= tempFreezerNum; i++)
                {
                    SubFreezer sub_Freezer =(SubFreezer)subFreezer_list.ElementAt(i-1);
                    if (null != sub_Freezer.cooling_comboBox.Text.ToString().Trim() && "" != sub_Freezer.cooling_comboBox.Text.ToString().Trim())
                    {
                        MachineEntity me = new MachineEntity("冷冻机" + i, sub_Freezer.type_box.Text.ToString(), Convert.ToDouble(sub_Freezer.cooling_comboBox.Text.ToString().Trim()));
                        machineList.Add(me);
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

        private void btn_ok_Click(object sender, EventArgs e)
        {
            if (null != subFreezer_list && 0 < subFreezer_list.Count)
            {
                meList = getFreezerTypeAndCooling(subFreezer_list);
                EngineWinFormEventArgs ewfe = new EngineWinFormEventArgs(meList);
                PassDataBetweenForm(this, ewfe);
                this.Close();
                //this.Visible = false;
            }
            else
                MessageBox.Show("请输入数据");
        }

        private void btnLoadData_Click(object sender, EventArgs e)
        {


        }

        
    }

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
        public CheckBox is_Frequency_Conversion_checkBox
        {
            get;
            set;
        }
        public ComboBox performance_data_box
        {
            get;
            set;
        }
        public SubFreezer(int i)
        {
            freazer = new Label();
            type_box = new ComboBox();
            cooling_comboBox = new TextBox();
            brand_comboBox = new ComboBox();
            model_box = new TextBox();
            is_Frequency_Conversion_checkBox = new CheckBox();
            performance_data_box = new ComboBox();
            setComponentAttribute(i);
            setComponetLocation(i);
        }

        public void setComponentAttribute(int i)
        {
            freazer.Name = "freezer_label" + i;
            type_box.Name = "type_box" + i;
            type_box.Items.Add("CSD");
            type_box.Items.Add("VSD");
            cooling_comboBox.Name = "cooling_comboBox" + i;
            brand_comboBox.Name = "brand_comboBox" + i;
            model_box.Name = "model_box" + i;
            is_Frequency_Conversion_checkBox.Name = "is_Frequency_Conversion_checkBox" + i;
            performance_data_box.Name = "performance_data_box" + i;
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
            is_Frequency_Conversion_checkBox.Location = new Point(669, 20 + (i - 1) * 39);
            performance_data_box.Location = new Point(773, 17 + (i - 1) * 39);
        }
    }
}
