using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnerefsysBLL.Entity
{
    public class SoluteResult
    {
        public double Result { get; set; }
        public double Solute { get; set; }
        public SoluteResult(double result,double solute)
        {
            Result = result;
            Solute = solute;
        }
    }
}
