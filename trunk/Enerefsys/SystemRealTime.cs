using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using EnerefsysBLL;
using EnerefsysBLL.Entity;
using EnerefsysBLL.Utility;
using EnerefsysBLL.Manager;

namespace Enerefsys
{
    public partial class SystemRealTime : Form
    {
        public List<MachineEntity> meList = new List<MachineEntity>();//获得冷冻值列表（类型和冷量）
        public SystemRealTime()
        {
            InitializeComponent();
        }

        private void btnEnvironmemt_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
        }

        private void btnEngine_Click(object sender, EventArgs e)
        {
            Freezer freezer = new Freezer();
            freezer.PassDataBetweenForm += new Freezer.PassDataBetweenFormHandler(freezer_PassDataBetweenForm);
            //frmChild.PassDataBetweenForm += new FrmChild.PassDataBetweenFormHandler(FrmChild_PassDataBetweenForm);
            freezer.Show();
        }
        //事件
        void freezer_PassDataBetweenForm(object sender, EngineWinFormEventArgs e)
        {
            meList = e.meList;
        }

        private void btnPump_Click(object sender, EventArgs e)
        {
            WaterPump wp = new WaterPump();
            wp.PassDataBetweenForm += new WaterPump.PassDataBetweenFormHandler(wp_PassDataBetweenForm);
            wp.Show();
        }

        void wp_PassDataBetweenForm(object sender, PumpWinFormEventArgs e)
        {
            pumpType = e.PumpType;
        }
        public string pumpType { get; set; }

        private void btnCoolTower_Click(object sender, EventArgs e)
        {
            CoolTower ct = new CoolTower();
            ct.Show();
        }

        private void btnShow_Click(object sender, EventArgs e)
        {

            System.IO.DirectoryInfo directory = new System.IO.DirectoryInfo(Application.StartupPath);
            System.IO.DirectoryInfo root = directory.Parent.Parent;

            pictureBox_Result.ImageLocation = root.FullName + "/Resources/result.jpg";
            //pictureBox_Result.Image = Image.FromFile("../Resources/result.jpg");
            //GetOptimizationResult(meList, pumpType, Convert.ToDouble(textBox_Load.Text), Convert.ToDouble(textBox_Temperature.Text));
            //double minre = minResult;
            //double mins = minSolute;
            //double pre = percentValue;

            CoolingType = "一对一";
            FreezeType = "一对一";
            TemperRange = 10;
            IsNormal = false;
            DealWithCalculate(Convert.ToDouble(textBox_Temperature.Text), Convert.ToDouble(textBox_Load.Text), meList);

            showMessage();

         
            
            //for (int i = 0; i < 24; i++)
            //{
            //    double time = i + 0.5;
            //    string timeStr = i + ":00-" + (i + 1) + ":00";
            //    double load = EnerefsysDAL.DayLoadData.GetItemByTime("2012-05-09", time.ToString());
            //    DealWithCalculate(28, load, meList);
            //    //GetOptimizationResult(meList, pumpType, load, Convert.ToDouble(textBox_Temperature.Text));
            //    double coolingPumpPower = minResult - EnginePower - freezePumpPower - coolingPower;
            //    foreach (var memin in meMin)
            //    {
            //        //EnerefsysDAL.OptimizationResultData.Insert("2012-05-09", timeStr, Convert.ToDouble(textBox_Temperature.Text), load, memin.Type, memin.Value
            //        //    , percentValue, memin.Value * percentValue, minSolute, minResult, freezePumpPower, coolingPumpPower, coolingPower);
            //        EnerefsysDAL.OptimizationResultData.Insert("2012-05-09", timeStr, Convert.ToDouble(textBox_Temperature.Text), 
            //            load, memin.Type, EnginePower, percentValue, 
            //            memin.Value * percentValue, minSolute, minResult, freezePumpPower, coolingPumpPower, coolingPower);
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
            addStrToBox("主机功率为：" + String.Format("{0:F}", EnginePower) + "KW.", textBox_Message);
            addStrToBox("冷却水泵功率为："+ String.Format("{0:F}", freezePumpPower) + "KW.",textBox_Message);
            addStrToBox("冷冻水泵功率为：" + String.Format("{0:F}", minResult - freezePumpPower - EnginePower - coolingPower) + "KW.", textBox_Message);
            addStrToBox("冷却塔功率为：" + String.Format("{0:F}", coolingPower) + "KW.", textBox_Message);

        }
        private void addStrToBox(string str, TextBox rtbox)
        {
            rtbox.Text += str;
            rtbox.Text += "\r\n";
        }

        /// <summary>
        /// 传入参数为主机组合，水泵类型，给定负荷，给定温度
        /// </summary>
        /// <param name="machineList"></param>
        /// <param name="pumpType"></param>
        /// <param name="load"></param>
        /// <param name="temperature"></param>
        private void GetOptimizationResult(List<MachineEntity> machineList, string pumpType, double load, double temperature)
        {
            List<double> doubleList = new List<double>();
            for (int i = 0; i < machineList.Count; i++)
            {
                doubleList.Add(machineList[i].Value);
            }
            //double temperature = Convert.ToDouble(Temperature.Text);

            //判断总负荷是否成立
            double sumLoad = 0;
            foreach (var me in machineList)
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
                    machineResult.Add(machineList[val]);
                }
                //以上得到一个组合，接下来对其求最小值

                SoluteResult sr = getMinByConsist(machineResult, temperature, load, pumpType);
                //判断某个组合的的最小功率是不是在所有组合中最小
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
        
        
        
        
        
        
        //分别保存当前最佳组合，当前最优结果，和当前每台主机的负荷率
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

        //在一定温度下得到一个主机组合的最低能耗，及对应的流量
        private SoluteResult getMinByConsist(List<MachineEntity> mes, double temperature, double load, string pumpType)
        {
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
            double constantNumber = 230.6 * engineCount;
            double constantNumber2 = constantNumber * constantNumber;


            //从数据库得到二次项系数
            List<double> doubleParams = PumpManager.GetParamsByType(pumpType);

            //对水泵公式中的自变量进行变换，影响到二次方程的a,b,c
            a += doubleParams[0] * constantNumber2;
            b += doubleParams[1] * constantNumber;
            c += doubleParams[2];

            //求出使得能耗最低的解，即流量的百分比
            double solute = Utility.GetMinSolute(a, b, c);
            double result = a * solute * solute + b * solute + c;
            return new SoluteResult(result, solute);

        }

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




        //是否板换
        public bool IsSwap { get; set; }
        /// <summary>
        /// 是否常规
        /// </summary>
        public bool IsNormal { get; set; }
        public double TemperRange { get; set; }
        //冷却类型
        public string CoolingType { get; set; }
        //冷冻类型
        public string FreezeType { get; set; }
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
                tempa = doubleParams[0] * constantNumber2;
                tempb = doubleParams[1] * constantNumber;
                tempc = doubleParams[2];
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

            EnginePower = tempa * solute * solute + b * solute + c;

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
                minResult += doubleParamsCool[0] * 240 * 240 * engineCount * engineCount
                    + doubleParamsCool[1] * 240 * engineCount
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
                    if (4.187 * TemperRange * 125 * machineResult.Count < load)
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


            double sumLoad1 = 0;
            foreach (var me in meMin)
            {
                sumLoad1 += me.Value;
            }
            //求得每台主机的负荷率，每台主机运行的负荷除以总负荷相等
            percentValue = load / sumLoad1;



            /***********************************************************************************/
            //主机功率的计算公式
            ///***********************************************************************************/
            //double aa = 0;
            //double ab = 0;
            //double ac = 0;

            //foreach (var me in meMin)
            //{
            //    //得到每台特定类型的主机在一定温度，一定负荷下的关于流量的二次项系数
            //    List<double> results = getFormulaByEntity(me.Type, me.Value * percentValue, temperature);
            //    aa += results[0];
            //    ab += results[1];
            //    ac += results[2];
            //}
            ////得到主机的功率
            //EnginePower = aa * minSolute * minSolute + ab * minSolute + ac;


           
        }



    }
}
