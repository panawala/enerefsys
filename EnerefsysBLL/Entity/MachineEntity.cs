using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BLL
{
    public class MachineEntity
    {
        public string Type { get; set; }
        public double Value { get; set; }
        public MachineEntity(string type, double value)
        {
            Type = type;
            Value = value;
        }
    }
}
