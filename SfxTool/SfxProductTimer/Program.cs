using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace SfxProductTimer
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
            bool bNotRun = true;
            System.Threading.Mutex mux = new System.Threading.Mutex(true, "SfxProductTimer", out bNotRun);
            if (!bNotRun)
            {
                MessageBox.Show("程序已经运行！");
                Environment.Exit(0);
            }
            Application.Run(new Form1());
        }
    }
}
