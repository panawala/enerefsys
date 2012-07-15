using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnerefsysDAL.Model
{
    public class Electronic
    {
        public int ElectronicID { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public double ElectronicPrice { get; set; }
        public double DefaultValue { get; set; }
    }
}
