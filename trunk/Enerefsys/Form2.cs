using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace Enerefsys
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string xml = "xml.xml";
            //插入元素
            XmlHelper.Insert(xml, "/Root", "Studio", "", "");
            //插入元素/属性
            XmlHelper.Insert(xml, "/Root/Studio", "Site", "Name", "小路工作室");
            XmlHelper.Insert(xml, "/Root/Studio", "Site", "Name", "丁香鱼工作室");
            XmlHelper.Insert(xml, "/Root/Studio", "Site", "Name", "五月软件");
            XmlHelper.Insert(xml, "/Root/Studio/Site[@Name='五月软件]", "Master", "", "五月");
            //插入属性
            XmlHelper.Insert(xml, "/Root/Studio/Site[@Name='小路工作室']", "", "Url", "http://www.wzlu.com/");
            XmlHelper.Insert(xml, "/Root/Studio/Site[@Name='丁香鱼工作室']", "", "Url", "http://www.luckfish.net/");
            XmlHelper.Insert(xml, "/Root/Studio/Site[@Name='五月软件]", "", "Url", "http://www.vs2005.com.cn/");
            //修改元素值
            XmlHelper.Update(xml, "/Root/Studio/Site[@Name='五月软件]/Master", "", "Wuyue");
            //修改属性值
            XmlHelper.Update(xml, "/Root/Studio/Site[@Name='五月软件]", "Url", "http://www.vs2005.com.cn/");
            XmlHelper.Update(xml, "/Root/Studio/Site[@Name='五月软件]", "Name", "MaySoft");
            //读取元素值
            string xmlString = XmlHelper.Read(xml, "/Root/Studio/Site/Master", "");
            //读取属性值
            xmlString = XmlHelper.Read(xml, "/Root/Studio/Site", "Url");
            //读取特定属性值
            xmlString = XmlHelper.Read(xml, "/Root/Studio/Site[@Name='丁香鱼工作室']", "Url");
            //删除属性
            //XmlHelper.Delete(xml, "/Root/Studio/Site[@Name='小路工作室']", "Url");
            //删除元素
            //XmlHelper.Delete(xml, "/Root/Studio", "");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string xml = "prj.xml";
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
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/BuildingCooling", "BuildingType", "", "hello");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/BuildingCooling", "BuildingArea", "", "this is a value");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/BuildingCooling", "PeakLoad", "", "this is");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/BuildingCooling", "CoolingStart", "", "this is");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/BuildingCooling", "CoolingEnd", "", "this is");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/BuildingCooling", "HoursPerDay", "", "this is");
            //建筑物制冷设计
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable", "CoolingDesign", "", "");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign", "Freeze", "", "");
            //冷冻
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign/Freeze", "InTemperature", "", "this is a value");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign/Freeze", "BackTemperature", "", "this is a value");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign/Freeze", "TotalFlow", "", "this is a value");
            //冷却日温度
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign", "CoolingDay", "", "");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign/CoolingDay", "InTemperature", "", "this is a value");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign/CoolingDay", "OutTemperature", "", "this is a value");
            //冷却夜温度
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign", "CoolingNight", "", "");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/CoolingDesign/CoolingNight", "InTemperature", "", "this is");



            //电价配置
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable", "ProperPrice", "", "");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/ProperPrice", "ProjectPlace", "", "hello");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/ProperPrice", "PriceType", "", "hello");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/ProperPrice", "PowerLevel", "", "hello");


            //外部环境配置
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable", "OuterEnvironment", "", "");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/OuterEnvironment", "WetBallTemperature", "", "hello");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/OuterEnvironment", "DayDryBallTemperature", "", "hello");
            XmlHelper.Insert(xml, "/Project/EnvironmentVariable/OuterEnvironment", "NightDryBallTemperature", "", "hello");

            /**********************************************************************************************/
            //插入环境变量配置
            /**********************************************************************************************/
            XmlHelper.Insert(xml, "/Project", "FreezeConfiguration", "", "");
            for (int i = 0; i < 3; i++)
            {
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration", "FreezeMachine", "id", i.ToString());
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id="+i.ToString()+"]", "Type", "", "this is type");
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "FreezePower", "", "this is type");
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "Brand", "", "this is type");
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "Model", "", "this is type");
                XmlHelper.Insert(xml, "/Project/FreezeConfiguration/FreezeMachine[@id=" + i.ToString() + "]", "Frequency", "", "this is type");
            }


            /**********************************************************************************************/
            //插入水泵配置
            /**********************************************************************************************/
            XmlHelper.Insert(xml, "/Project", "PumpConfiguration", "", "");
            XmlHelper.Insert(xml, "/Project/PumpConfiguration", "FreezePumpCount", "", "3");
            XmlHelper.Insert(xml, "/Project/PumpConfiguration", "CoolingPumpCount", "", "3");
            for (int i = 0; i < 3; i++)
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
            for (int i = 0; i < 3; i++)
            {
                XmlHelper.Insert(xml, "/Project/PumpConfiguration", "CoolingPump", "id", i.ToString());
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Brand", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Flow", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Distance", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Power", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Model", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Type", "", "this is type");
                XmlHelper.Insert(xml, "/Project/PumpConfiguration/CoolingPump[@id=" + i.ToString() + "]", "Frequency", "", "this is type");
            }

            /**********************************************************************************************/
            //插入冷却配置
            /**********************************************************************************************/
            XmlHelper.Insert(xml, "/Project", "CoolingConfiguration", "", "");
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration", "CoolingTower", "", "");
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "Flow", "", "this is flow");
            XmlHelper.Insert(xml, "/Project/CoolingConfiguration/CoolingTower", "Frequency", "", "this is frequency");

            /**********************************************************************************************/
            //插入结果配置
            /**********************************************************************************************/
            XmlHelper.Insert(xml, "/Project", "Result", "", "");
            XmlHelper.Insert(xml, "/Project/Result", "RoomTemperature", "", "this is temperature");
            XmlHelper.Insert(xml, "/Project/Result", "SystemLoad", "", "this is system");
            

            ////插入元素/属性
            //XmlHelper.Insert(xml, "/Root/Studio", "Site", "Name", "小路工作室");
            //XmlHelper.Insert(xml, "/Root/Studio", "Site", "Name", "丁香鱼工作室");
            //XmlHelper.Insert(xml, "/Root/Studio", "Site", "Name", "五月软件");
            //XmlHelper.Insert(xml, "/Root/Studio/Site[@Name='五月软件]", "Master", "", "五月");
            ////插入属性
            //XmlHelper.Insert(xml, "/Root/Studio/Site[@Name='小路工作室']", "", "Url", "http://www.wzlu.com/");
            //XmlHelper.Insert(xml, "/Root/Studio/Site[@Name='丁香鱼工作室']", "", "Url", "http://www.luckfish.net/");
            //XmlHelper.Insert(xml, "/Root/Studio/Site[@Name='五月软件]", "", "Url", "http://www.vs2005.com.cn/");
            
        }
    }
}
