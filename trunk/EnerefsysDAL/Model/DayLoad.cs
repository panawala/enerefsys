using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnerefsysDAL.Model
{
    public class DayLoad
    {
        public int DayLoadID { get; set; }
        public string BeginTime {get;set;}
        public string EndTime {get;set;}
        public double Load {get;set;}
        public double Ratio {get;set;}
        public string BuildingType { get; set; }
    }
}
