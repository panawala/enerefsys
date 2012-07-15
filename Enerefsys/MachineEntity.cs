using System;
using System.Collections.Generic;
using System.Text;

namespace Enerefsys
{
   public class MachineEntity
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public double Value { get; set; }
        public MachineEntity(string name,string type, double value)
        {
            Name = name;
            Type = type;
            Value = value;
        }

    }
}
