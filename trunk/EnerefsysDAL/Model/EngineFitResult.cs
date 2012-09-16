using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnerefsysDAL.Model
{
    public class EngineFitResult
    {
        public int EngineFitResultID { get; set; }
        public float B1 { get; set; }
        public float B2 { get; set; }
        public float B3 { get; set; }
        public float B4 { get; set; }
        public float B5 { get; set; }
        public float B6 { get; set; }
        public float B7 { get; set; }
        public double Temperature { get; set; }
        public string Type { set; get; }
    }
}
