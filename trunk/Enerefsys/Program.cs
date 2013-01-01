using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Enerefsys
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            int days = (System.DateTime.Now - Convert.ToDateTime("2013-01-10")).Days;
            if (days > 0)
                System.Environment.Exit(0);

            Application.Run(new Enerefsys());

           
            //Application.Run(new ReportForm());
            //Application.Run(new SystemRealTime());
        }
    }
}
