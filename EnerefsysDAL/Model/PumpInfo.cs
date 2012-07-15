using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnerefsysDAL.Model
{
    public class PumpInfo
    {
        public int PumpInfoID { get; set; }
        public double PumpFlow { get; set; }
        public int PumpCount { get; set; }
        public string PumpType { get; set; }
        public double PumpDesignFlow { get; set; }
    }
}
