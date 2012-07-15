using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Enerefsys
{
    public class PumpWinFormEventArgs
    {
        public PumpWinFormEventArgs()
        {
 
        }
        public PumpWinFormEventArgs(string type)
        {
            PumpType = type;
        }
        public string PumpType { get; set; }
    }
}
