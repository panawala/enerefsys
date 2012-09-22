using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnerefsysDAL.Model
{
    public class StandardLoad
    {
        public int StandardLoadID { get; set; }
        /// <summary>
        /// 时间，月
        /// </summary>
        public int Month { get; set; }
        /// <summary>
        /// 时间，日
        /// </summary>
        public int Day { get; set; }
        /// <summary>
        /// 时间，时
        /// </summary>
        public int Hour { get; set; }
        /// <summary>
        /// 干球温度
        /// </summary>
        public double DryTemperature { get; set; }
        /// <summary>
        /// 湿球温度
        /// </summary>
        public double WetTemperature { get; set; }
        /// <summary>
        /// 冷负荷
        /// </summary>
        public double Load { get; set; }
        /// <summary>
        /// 冷却水进口温度
        /// </summary>
        public double EnterTemperature { get; set; }
        /// <summary>
        /// 电价
        /// </summary>
        public double ElectronicPrice { get; set; }
    }
}
