using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WindowsFormsApplication1
{
    public class EngineWinFormEventArgs : EventArgs
    {

        public EngineWinFormEventArgs()
        {
            //
        }
        public EngineWinFormEventArgs(List<MachineEntity> melist)
        {
            meList = melist;
        }
        public List<MachineEntity> meList { get; set; }
       
    }
}
