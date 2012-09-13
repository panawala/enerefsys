using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using EnerefsysBLL;
using Microsoft.Win32;
using System.IO;
using EnerefsysBLL.Entity;
using EnerefsysBLL.Manager;
using EnerefsysBLL.Utility;

namespace ControlApp
{
    /// <summary>
    /// Interaction logic for EngineInputPage.xaml
    /// </summary>
    public partial class EngineInputPage : Page
    {
        public EngineInputPage()
        {
            InitializeComponent();
        }
        //页面加载效果
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            //淡入效果
            this.Opacity = 0;
            DoubleAnimation OpercityAnimation =
            new DoubleAnimation(0.01, 1.00, new Duration(TimeSpan.FromSeconds(1)));
            this.BeginAnimation(Page.OpacityProperty, OpercityAnimation);

            List<string> results = PumpManager.GetPumpTypes();
            foreach (var result in results)
            {
                PumpType.Items.Add(result);
            }

        }
        
        

        private void Temperature_TextChanged(object sender, TextChangedEventArgs e)
        {
            TextBox textBox = sender as TextBox;
            TextChange[] change = new TextChange[e.Changes.Count];
            e.Changes.CopyTo(change, 0);

            int offset = change[0].Offset;
            if (change[0].AddedLength > 0)
            {
                double num = 0;
                if (!Double.TryParse(textBox.Text, out num))
                {
                    textBox.Text = textBox.Text.Remove(offset, change[0].AddedLength);
                    textBox.Select(offset, 0);
                }
            }
        }

        private void Temperature_KeyDown(object sender, KeyEventArgs e)
        {
            TextBox txt = sender as TextBox;

            if ((e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || e.Key == Key.Decimal)
            {
                if (txt.Text.Contains(".") && e.Key == Key.Decimal)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else if (((e.Key >= Key.D0 && e.Key <= Key.D9) || e.Key == Key.OemPeriod) && e.KeyboardDevice.Modifiers != ModifierKeys.Shift)
            {
                if (txt.Text.Contains(".") && e.Key == Key.OemPeriod)
                {
                    e.Handled = true;
                    return;
                }
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        //得到一定温度下，一台主机的能耗的函数
        private List<double> getFormulaByEntity(string type, double load, double temperature)
        {
            //得到一定温度下，特定类型和特定负荷下的主机能耗关于流量的函数的二次系数
            List<double> results = EngineManager.GetParamsByType(temperature, load, type);
            return results;
        }
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
            //string pumpType = PumpType.Text;
            int engineCount = mes.Count;

            /***********************************************************************************/
            //冷却水泵的计算公式
            /***********************************************************************************/
            if (CoolingType.Equals("并联"))
            {
                double constantNumber = 240 * engineCount;
                double constantNumber2 = constantNumber * constantNumber;
                //从数据库得到二次项系数
                List<double> doubleParams = PumpManager.GetParamsByType("1");
                //对水泵公式中的自变量进行变换，影响到二次方程的a,b,c
                a += doubleParams[0] * constantNumber2;
                b += doubleParams[1] * constantNumber;
                c += doubleParams[2];
            }
            if (CoolingType.Equals("一对一"))
            {
                //从数据库得到二次项系数
                List<double> doubleParams = PumpManager.GetParamsByType("1");
                //对水泵公式中的自变量进行变换，影响到二次方程的a,b,c
                a += doubleParams[0] * 240 * 240 * engineCount;
                b += doubleParams[1] * 240 * engineCount;
                c += doubleParams[2] * engineCount;
            }


            //求出使得能耗最低的解，即流量的百分比
            double solute = Utility.GetMinSolute(a, b, c);
            double result = a * solute * solute + b * solute + c;
            return new SoluteResult(result, solute);
        }
        public double TemperRange { get; set; }
        //冷却类型
        public string CoolingType { get; set; }
        //冷冻类型
        public string FreezeType { get; set; }
        private List<MachineEntity> meMin = new List<MachineEntity>();
        private double minResult = double.MaxValue;
        private double minSolute = double.MaxValue;
        private double percentValue = 0;
        /// <summary>
        /// 冷冻功率
        /// </summary>
        public double freezePumpPower = 0;
        /// <summary>
        /// 主机功率
        /// </summary>
        public double EnginePower = 0;
        /// <summary>
        /// 冷却塔功率
        /// </summary>
        public double coolingPower = 0;
        private List<MachineEntity> machineList = new List<MachineEntity>();
        public bool IsSwap { get; set; }
        public bool IsNormal { get; set; }
        private void Image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            meMin.Clear();
            minResult = double.MaxValue;
            minSolute = double.MaxValue;
            percentValue = 0;
            machineList.Clear();
            
            if (!string.IsNullOrEmpty(textBox1.Text))
            {
                MachineEntity machine = new MachineEntity(comBox1.Text, Convert.ToDouble(textBox1.Text));
                machineList.Add(machine);
            }
            if (!string.IsNullOrEmpty(textBox2.Text))
            {
                MachineEntity machine = new MachineEntity(comBox2.Text, Convert.ToDouble(textBox2.Text));
                machineList.Add(machine);
            }
            if (!string.IsNullOrEmpty(textBox3.Text))
            {
                MachineEntity machine = new MachineEntity(comBox3.Text, Convert.ToDouble(textBox3.Text));
                machineList.Add(machine);
            }
            if (!string.IsNullOrEmpty(textBox4.Text))
            {
                MachineEntity machine = new MachineEntity(comBox4.Text, Convert.ToDouble(textBox4.Text));
                machineList.Add(machine);
            }
            if (!string.IsNullOrEmpty(textBox5.Text))
            {
                MachineEntity machine = new MachineEntity(comBox5.Text, Convert.ToDouble(textBox5.Text));
                machineList.Add(machine);
            }
            if (!string.IsNullOrEmpty(textBox6.Text))
            {
                MachineEntity machine = new MachineEntity(comBox6.Text, Convert.ToDouble(textBox6.Text));
                machineList.Add(machine);
            }
            if (!string.IsNullOrEmpty(textBox7.Text))
            {
                MachineEntity machine = new MachineEntity(comBox7.Text, Convert.ToDouble(textBox7.Text));
                machineList.Add(machine);
            }
            if (!string.IsNullOrEmpty(textBox8.Text))
            {
                MachineEntity machine = new MachineEntity(comBox8.Text, Convert.ToDouble(textBox8.Text));
                machineList.Add(machine);
            }
            if (!string.IsNullOrEmpty(textBox9.Text))
            {
                MachineEntity machine = new MachineEntity(comBox9.Text, Convert.ToDouble(textBox9.Text));
                machineList.Add(machine);
            }
            if (!string.IsNullOrEmpty(textBox10.Text))
            {
                MachineEntity machine = new MachineEntity(comBox10.Text, Convert.ToDouble(textBox10.Text));
                machineList.Add(machine);
            }
            IsNormal = false ;
            IsSwap = false;
            FreezeType = "一对一";
            CoolingType = "并联";

            TemperRange = 7;

            /*****************************************************************************/
            //根据界面的温度信息，得到温度
            /*****************************************************************************/
            double temperature = (int)(Convert.ToDouble(Temperature.Text) + 0.5);

            /*****************************************************************************/
            //根据界面的板换信息，得到最终的总负荷
            /*****************************************************************************/
            double swapPower = 0;
            int swapCount = 0;
            //此处为总负荷，由界面录入，
            double load = Convert.ToDouble(Load.Text);
            if (IsSwap)
                load = load - swapCount * swapPower;



            DealWithCalculate(temperature, load, machineList);



            addStrToBox("主机组合如下：", Result);
            addStrToBox("--------", Result);
            foreach(var me in meMin)
            {
                string machineResult = "类型：" + me.Type + ";"; 
                addStrToBox(machineResult, Result);
                machineResult= me.Value + "KW * " + String.Format("{0:F}", percentValue * 100) + "%=" + String.Format("{0:F}", me.Value * percentValue) + "KW.";
                addStrToBox(machineResult, Result);
            }
            addStrToBox("--------", Result);
            string minPowerStr = "系统最低功率为：" + String.Format("{0:F}", minResult) + "KW.";
            addStrToBox(minPowerStr, Result);
            addStrToBox("此时流量为：" + String.Format("{0:F}", minSolute * 100)+"%.", Result);
            addStrToBox("各主机负荷率为：" + String.Format("{0:F}", percentValue * 100) + "%.", Result);
            
        }
        
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
                minResult += doubleParamsCool[0] * 240 * 240 * engineCount * engineCount
                    + doubleParamsCool[1] * 240 * engineCount 
                    + doubleParamsCool[2] * engineCount;
                //冷冻水泵
                List<double> doubleParamsFreeze = PumpManager.GetParamsByType("2");
                minResult += doubleParamsFreeze[0] * 125 * 125 * engineCount * engineCount
                    + doubleParamsFreeze[1] * 125 * engineCount
                    + doubleParamsFreeze[2] * engineCount;
               
                minSolute=1.0;
                return;
            }

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
                    double minValue=double.MaxValue;
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
                    List<double> doubleParams = PumpManager.GetParamsByType("2");
                    foreach (var me in machineResult)
                    {
                        double curflow = me.Value / (4.187 * TemperRange);
                        double curPower = doubleParams[0] * curflow * curflow + doubleParams[1] * curflow + doubleParams[2];
                        freezePumpPower += curPower;
                    }
                }
                if (FreezeType.Equals("并联"))
                {
                    if (4.187 * TemperRange * 125 *machineResult.Count< load)
                    {
                        MessageBox.Show("总负荷过大，所提供冷冻水泵不足");
                        return;
                    }

                    //从数据库得到二次项系数
                    List<double> doubleParams = PumpManager.GetParamsByType("2");
                    double curflow = load / (4.187 * TemperRange * machineResult.Count);
                    double curPower = doubleParams[0] * curflow * curflow + doubleParams[1] * curflow + doubleParams[2];
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



            /***********************************************************************************/
            //主机功率的计算公式
            /***********************************************************************************/
            double aa = 0;
            double ab = 0;
            double ac = 0;
            double asumLoad2 = 0;
            foreach (var me in meMin)
            {
                asumLoad2 += me.Value;
            }
            //求得每台主机的负荷率，每台主机运行的负荷除以总负荷相等
            double apercentValue1 = load / asumLoad2;

            foreach (var me in meMin)
            {
                //得到每台特定类型的主机在一定温度，一定负荷下的关于流量的二次项系数
                List<double> results = getFormulaByEntity(me.Type, me.Value * apercentValue1, temperature);
                aa += results[0];
                ab += results[1];
                ac += results[2];
            }
            //得到主机的功率
            EnginePower = aa * minSolute * minSolute + ab * minSolute + ac;


            double sumLoad1 = 0;
            foreach (var me in meMin)
            {
                sumLoad1 += me.Value;
            }
            //求得每台主机的负荷率，每台主机运行的负荷除以总负荷相等
            percentValue = load / sumLoad1;
        }

        //常规算法：
        //根据总负荷，按照从vsd到csd的顺序，按照从大到小的顺序选择主机，流量100%，计算能耗
        public List<MachineEntity> OrderMachiList(List<MachineEntity> list, double load)
        {
            List<MachineEntity> tmplist = new List<MachineEntity>();
            Sort(ref list);
            int index=0;
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


        private void Image_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "保存信息";
            saveFileDialog.Filter = "文本文件(*.txt)|*.txt";
            saveFileDialog.FileName = "最优解";
            if (saveFileDialog.ShowDialog() == true)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog.FileName);

                sw.WriteLine("录入主机情况：");
                foreach (var me in machineList)
                {
                    sw.WriteLine("类型：" + me.Type+"，负荷："+me.Value+"KW.");
                }
                sw.WriteLine("水泵类型：" + PumpType.Text+".");
                sw.WriteLine("当前温度：" + Temperature.Text + "摄氏度.");
                sw.WriteLine("总负荷：" + Load.Text + "KW.");
                sw.WriteLine("----------------------------------");
                sw.WriteLine("根据系统优化结果如下：");
                
                sw.WriteLine("主机组合如下：");
                sw.WriteLine("---------------------");
                foreach (var me in meMin)
                {
                    string machineResult = "类型：" + me.Type + ";   实际功率为：" + me.Value + "KW * " + String.Format("{0:F}", percentValue * 100) + "%=" + String.Format("{0:F}", me.Value * percentValue) + "KW.";
                    sw.WriteLine(machineResult);
                }
                sw.WriteLine("---------------------");
                string minPowerStr = "系统最低功率为：" + String.Format("{0:F}", minResult) + "KW.";
                sw.WriteLine(minPowerStr);
                sw.WriteLine("此时流量为：" + String.Format("{0:F}", minSolute * 100) + "%.");
                sw.WriteLine("各主机负荷率为：" + String.Format("{0:F}", percentValue * 100) + "%.");

                sw.Close();
            }
        }

        private void addStrToBox(string str,RichTextBox rtbox)
        {
            Paragraph paragraph = new Paragraph();
            paragraph.Inlines.Add(str);
            paragraph.FontSize = 20;
            rtbox.Document.Blocks.Add(paragraph);
        }






    }
}
