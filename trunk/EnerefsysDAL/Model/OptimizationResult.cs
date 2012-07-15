using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnerefsysDAL.Model
{
    public class OptimizationResult
    {
        public int OptimizationResultID { get; set; }
        public string Day { get; set; }
        public string Time { get; set; }
        public double Temperature { get; set; }
        public double Load { get; set; }
        public string EngineType { set; get; }
        public double EngineValue { get; set; }
        public double EngineLoadRatio { get; set; }
        public double EnginePower { get; set; }
        public double Flow { get; set; }
        public double SystemMinPower { get; set; }
        public double FreezePumpPower { get; set; }
        public double CoolingPumpPower { get; set; }
        public double CoolingPower { get; set; }
    }
}
